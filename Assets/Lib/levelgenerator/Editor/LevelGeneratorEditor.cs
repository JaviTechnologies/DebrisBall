using UnityEngine;
using System.Collections;
using UnityEditor;
using javitechnologies.levelgenerator.view;
using javitechnologies.levelgenerator.data;

namespace javitechnologies.levelgenerator.editor
{
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : Editor
    {
        LevelGenerator levelGenerator;
        LevelGenerator CurrentLevelGenerator
        {
            get
            {
                if (levelGenerator == null)
                {
                    levelGenerator = (LevelGenerator)target;
                }
                return levelGenerator;
            }
        }

        private LevelGroupData localData;

        void OnEnable ()
        {
            Debug.Log("LevelGeneratorEditor.OnEnable");
            localData = null;
            if (CurrentLevelGenerator != null)
            {
                // update localData
                localData = CurrentLevelGenerator.CurrentLevelGroupData;
                Debug.Log("LevelGeneratorEditor.OnEnable:: Updating local data to: " + localData);

                CurrentLevelGenerator.WantRepaint += WantRepaint;
            }
        }

        private void WantRepaint ()
        {
//            Debug.LogWarning("REEEPAINTING YEEEAAAAH!!!!");
//            this.Repaint();
        }

        public override void OnInspectorGUI()
        {
            if (CurrentLevelGenerator != null)
            {
                localData = EditorGUILayout.ObjectField(
                    "Config",
                    CurrentLevelGenerator.CurrentLevelGroupData,
                    typeof(LevelGroupData),
                    false) as LevelGroupData;
                EditorGUILayout.LabelField("Current Level", CurrentLevelGenerator.CurrentLevel != null ? CurrentLevelGenerator.CurrentLevel.levelName : "Null");
            }

            if (GUI.changed)
            {
                CurrentLevelGenerator.SetLevelGroupData(localData);
            }
        }
    }
}