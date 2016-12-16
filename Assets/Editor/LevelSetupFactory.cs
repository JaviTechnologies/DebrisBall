using UnityEngine;
using System.Collections;
using UnityEditor;
using javitechnologies.ballwar.model;

namespace javitechnologies.levelgenerator.editor
{
    public class LevelSetupFactory
    {
        public static LevelData Create(string path)
        {
            LevelData asset = ScriptableObject.CreateInstance<LevelData>();

            AssetDatabase.CreateAsset(asset, FileUtil.GetProjectRelativePath(path));
            asset.levelName = asset.name;

            AssetDatabase.SaveAssets();

            return asset;
        }
    }
}