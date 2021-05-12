using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 他のAssetBundleと依存関係のあるPrefabを生成するオブジェクト
/// </summary>
public class DependPrefabSpawner : MonoBehaviour
{
    /// <summary>
    /// CanvasのTransform
    /// </summary>
    [SerializeField]
    private Transform CanvasTransform = null;

    /// <summary>
    /// ダウンロード中？
    /// </summary>
    private bool bIsDownloading = false;

    /// <summary>
    /// ダウンロード開始
    /// </summary>
    public void BeginDownload()
    {
        StartCoroutine("DownloadAssetBundle");
    }

    /// <summary>
    /// AssetBundleをダウンロード
    /// </summary>
    private IEnumerator DownloadAssetBundle()
    {
        if (bIsDownloading) { yield break; }
        bIsDownloading = true;
        using (var Request = UnityWebRequestAssetBundle.GetAssetBundle("https://simple-social-game.s3.ap-northeast-3.amazonaws.com/prefabpack"))
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
            var Prefab = Bundle.LoadAsset<GameObject>("ImageObject");
            var Obj = GameObject.Instantiate(Prefab);
            Obj.transform.SetParent(CanvasTransform);
            Obj.transform.localPosition = Vector3.zero;
        }
        bIsDownloading = false;
    }
}
