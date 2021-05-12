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
    /// AssetBundleの辞書
    /// </summary>
    private Dictionary<string, AssetBundle> BundleDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// ダウンロード
    /// </summary>
    /// <param name="Name">AssetBundleを識別するための名前</param>
    /// <param name="Url">URL</param>
    /// <param name="OnSuccess">成功コールバック</param>
    /// <param name="OnFail">失敗コールバック</param>
    public IEnumerator Download(string Name, string Url, Action<AssetBundle> OnSuccess, Action OnFail = null)
    {
        if (BundleDic.ContainsKey(Name))
        {
            OnSuccess?.Invoke(BundleDic[Name]);
            yield break;
        }
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
            BundleDic.Add(Name, Bundle);
            OnSuccess?.Invoke(Bundle);
        }
    }

    #region Singleton
    public static AssetBundleManager Instance { get { return _Instance; } }
    private static AssetBundleManager _Instance = new AssetBundleManager();
    #endregion
}
