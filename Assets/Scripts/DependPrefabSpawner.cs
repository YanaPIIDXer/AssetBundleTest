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

        yield return AssetBundleManager.Instance.Download("prefabpack", (Bundle) =>
        {
            var Prefab = Bundle.LoadAsset<GameObject>("ImageObject");
            var Obj = GameObject.Instantiate(Prefab);
            Obj.transform.SetParent(CanvasTransform);
            Obj.transform.localPosition = Vector3.zero;
        }, () => Debug.Log("Download Failed."));
        bIsDownloading = false;
    }
}
