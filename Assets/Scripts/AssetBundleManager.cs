using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// AssetBundle管理クラス
/// </summary>
public class AssetBundleManager
{
    /// <summary>
    /// ホスト
    /// </summary>
    private static readonly string Host = "https://simple-social-game.s3.ap-northeast-3.amazonaws.com/";

    /// <summary>
    /// Manifest
    /// </summary>
    private AssetBundleManifest Manifest = null;

    /// <summary>
    /// AssetBundleの辞書
    /// </summary>
    private Dictionary<string, AssetBundle> BundleDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// ダウンロード
    /// </summary>
    /// <param name="Name">AssetBundleを識別するための名前</param>
    /// <param name="OnSuccess">成功コールバック</param>
    /// <param name="OnFail">失敗コールバック</param>
    public IEnumerator Download(string Name, Action<AssetBundle> OnSuccess, Action OnFail = null)
    {
        if (BundleDic.ContainsKey(Name))
        {
            OnSuccess?.Invoke(BundleDic[Name]);
            BundleDic[Name].Unload(false);
            yield break;
        }
        if (Manifest == null)
        {
            // 依存関係解決の為、先にManifestを落とす
            string ManifestUrl = Host + "Windows";
            yield return DownloadFile(ManifestUrl, (Bundle) =>
            {
                Manifest = Bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                Bundle.Unload(false);
            }, OnFail);
        }

        var Depends = Manifest.GetAllDependencies(Name);
        List<AssetBundle> DependBundles = new List<AssetBundle>();
        foreach (var Depend in Depends)
        {
            string DependUrl = Host + Depend;
            yield return DownloadFile(DependUrl, (Bundle) =>
            {
                BundleDic.Add(Depend, Bundle);
                DependBundles.Add(Bundle);
            }, OnFail);
        }

        string Url = Host + Name;
        yield return DownloadFile(Url, (Bundle) =>
        {
            BundleDic.Add(Name, Bundle);
            OnSuccess?.Invoke(Bundle);
            Bundle.Unload(false);
        }, OnFail);

        foreach (var Bundle in DependBundles)
        {
            Bundle.Unload(false);
        }
    }

    /// <summary>
    /// ファイルをダウンロード
    /// </summary>
    /// <param name="Url">URL</param>
    private IEnumerator DownloadFile(string Url, Action<AssetBundle> OnSuccess, Action OnFail)
    {
        using (var Request = UnityWebRequestAssetBundle.GetAssetBundle(Url))
        {
            yield return Request.SendWebRequest();

            if (Request.isHttpError || Request.isNetworkError)
            {
                OnFail?.Invoke();
                yield break;
            }

            var Handle = Request.downloadHandler as DownloadHandlerAssetBundle;
            var Bundle = Handle.assetBundle;
            OnSuccess?.Invoke(Bundle);
        }
    }

    #region Singleton
    public static AssetBundleManager Instance { get { return _Instance; } }
    private static AssetBundleManager _Instance = new AssetBundleManager();
    #endregion
}
