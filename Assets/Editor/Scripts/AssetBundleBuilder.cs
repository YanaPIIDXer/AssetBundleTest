using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// AssetBundleビルドクラス
/// </summary>
public class AssetBundleBuilder
{
    /// <summary>
    /// AssetBundleに突っ込むAssetを格納しているルートディレクトリ
    /// </summary>
    private static readonly string AssetBundleRootDirectory = "Assets/Editor/Assets/Bundle/";

    /// <summary>
    /// AssetBundleのビルド
    /// </summary>
    [MenuItem("AssetBundle/Build")]
    public static void Build()
    {
        // AssetBundleにするディレクトリを列挙
        string[] BundleDirs = Directory.GetFileSystemEntries(AssetBundleRootDirectory);
        foreach (var BundleDir in BundleDirs)
        {
            if (BundleDir.IndexOf(".meta") >= 0) { continue; }
            var BundleName = Path.GetFileNameWithoutExtension(BundleDir);
            // ディレクトリ内のAssetを列挙してAssetBudnleの名前を指定
            var Assets = Directory.GetFileSystemEntries(BundleDir);
            foreach (var Asset in Assets)
            {
                if (Asset.IndexOf(".meta") >= 0) { continue; }
                AssetImporter importer = AssetImporter.GetAtPath(Asset);
                if (importer == null)
                {
                    Debug.LogError("AssetImporter.GetAtPath Failed. Asset:" + Asset);
                    continue;
                }
                importer.SetAssetBundleNameAndVariant(BundleName, "");
            }
        }
    }
}
