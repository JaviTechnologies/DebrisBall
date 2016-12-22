using UnityEngine;
using System.Collections;
using UnityEditor;
using javitechnologies.levelgenerator.data;
using javitechnologies.ballwar.model;

namespace javitechnologies.levelgenerator.editor
{
    public class LevelGeneratorSetupFactory
    {
        [MenuItem("Assets/Create/Level Generator Setup")]
        public static LevelGeneratorData Create(string path)
        {
            try
            {
                LevelGeneratorData asset = ScriptableObject.CreateInstance<LevelGeneratorData>();

                asset.levels = new System.Collections.Generic.List<LevelData>();

                AssetDatabase.CreateAsset(asset, FileUtil.GetProjectRelativePath(path));
                AssetDatabase.SaveAssets();

                return asset;
            }
            catch (System.Exception e)
            {
                Debug.Log("LevelGeneratorSetupFactory.Create: " + e.Message);
            }
            return null;
        }
    }
}