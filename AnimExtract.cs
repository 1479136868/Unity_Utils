using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 动画脱壳
/// </summary>
public class AnimExtract : AssetPostprocessor
{
    /// <summary>
    /// 方法名不能随意更改
    /// </summary>
    /// <param name="obj"></param>
    public void OnPostprocessModel(GameObject obj)
    {

        if (!assetImporter.assetPath.Contains("Models"))
        {
            EditorApplication.delayCall += RemoveFBX;
        }
    }
    private void RemoveFBX()
    {
        EditorApplication.delayCall -= RemoveFBX;

        ModelImporter importer = assetImporter as ModelImporter;
        var assets = AssetDatabase.LoadAllAssetsAtPath(importer.assetPath);
        foreach (var clipItem in assets)
        {
            if (clipItem is AnimationClip && !clipItem.name.Contains("__preview__"))
            {
                string outPutPath = Path.GetDirectoryName(importer.assetPath) + Path.DirectorySeparatorChar + clipItem.name + ".anim";
                var currentAsset = AssetDatabase.LoadAssetAtPath<AnimationClip>(outPutPath);
                if (currentAsset != null)
                {
                    EditorUtility.CopySerialized(clipItem, currentAsset);
                    EditorUtility.SetDirty(currentAsset);
                }
                else
                {
                    var newAnim = new AnimationClip();
                    EditorUtility.CopySerialized(clipItem, newAnim);
                    AssetDatabase.CreateAsset(newAnim, outPutPath);
                }

            }
        }
        AssetDatabase.DeleteAsset(assetImporter.assetPath);
    }
}
