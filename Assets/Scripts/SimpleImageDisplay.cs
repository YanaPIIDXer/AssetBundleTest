using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// 単純なImage表示
/// </summary>
public class SimpleImageDisplay : MonoBehaviour
{
    /// <summary>
    /// 表示用のImage
    /// </summary>
    [SerializeField]
    private Image TargetImage = null;

    private bool bIsDownloading = false;

    /// <summary>
    /// ダウンロード開始
    /// </summary>
    public void BeginDownload()
    {
        StartCoroutine("DownloadAssetBundle");
    }

    /// <summary>
    /// AssetBundleのダウンロード
    /// </summary>
    private IEnumerator DownloadAssetBundle()
    {
        if (bIsDownloading) { yield break; }

        bIsDownloading = true;
        yield return AssetBundleManager.Instance.Download("texpack0001", (Bundle) =>
        {
            var Img = Bundle.LoadAsset<Sprite>("f001");
            TargetImage.sprite = Img;
            TargetImage.SetNativeSize();
        }, () => Debug.Log("Download Failed."));
        bIsDownloading = false;
    }
}
