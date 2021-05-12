using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// AssetBundleビルドクラス
/// </summary>
public class AssetBundleBuilder
{
    /// <summary>
    /// AssetBundleのビルド
    /// </summary>
    [MenuItem("AssetBundle/Build")]
    public static void Build()
    {
        Debug.Log("Build AssetBundle");
    }
}
