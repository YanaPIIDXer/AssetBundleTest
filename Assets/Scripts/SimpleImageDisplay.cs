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
        using (var Request = UnityWebRequestAssetBundle.GetAssetBundle("https://simple-social-game.s3.ap-northeast-3.amazonaws.com/Windows/texpack0001"))
        {
            Debug.Log("AssetBundle Download Start.");
            yield return Request.SendWebRequest();

            if (Request.isHttpError || Request.isNetworkError)
            {
                Debug.LogError("AssetBundle Download Error.");
                bIsDownloading = false;
                yield break;
            }

            Debug.Log("AssetBundle Download Success.");

            var Handle = Request.downloadHandler as DownloadHandlerAssetBundle;
            var Bundle = Handle.assetBundle;
            var Img = Bundle.LoadAsset<Sprite>("f001");
            TargetImage.sprite = Img;
            TargetImage.SetNativeSize();
        }
        bIsDownloading = false;
    }
}
