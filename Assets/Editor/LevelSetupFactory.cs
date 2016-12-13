using UnityEngine;
using System.Collections;
using UnityEditor;
using javitechnologies.ballwar.model;

namespace javitechnologies.levelgenerator.editor
{
    public class LevelSetupFactory
    {
        public static LevelData Create()
        {
            LevelData asset = ScriptableObject.CreateInstance<LevelData>();

            string name = "NewLevelSetupData";

            string assetPath = string.Format("{0}{1}.asset", LevelEditor.DataDirectory, name);

            // Get a name that doesn't exists
            string[] assetsFound = AssetDatabase.FindAssets(assetPath);
            int i = 1;
            while (assetsFound != null && assetsFound.Length > 0)
            {
                name = string.Format("NewLevelSetupData_copy_{0}.asset", i++);
                assetPath = string.Format("{0}{1}.asset", LevelEditor.DataDirectory, name);
                assetsFound = AssetDatabase.FindAssets(assetPath);
                return null;
            }

            asset.levelName = name;

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            return asset;
        }
    }
}