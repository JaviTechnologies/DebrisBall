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
        public static LevelGeneratorData Create()
        {
            LevelGeneratorData asset = ScriptableObject.CreateInstance<LevelGeneratorData>();

            asset.levels = new System.Collections.Generic.List<LevelData>();

            AssetDatabase.CreateAsset(asset, "Assets/NewLevelGeneratorSetup.asset");
            AssetDatabase.SaveAssets();

            return asset;
        }
    }
}