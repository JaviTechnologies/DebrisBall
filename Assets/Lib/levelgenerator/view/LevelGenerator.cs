using UnityEngine;
using System.Collections;
using javitechnologies.ballwar.levelgenerator;
using javitechnologies.levelgenerator.data;
using javitechnologies.ballwar.model;

namespace javitechnologies.levelgenerator.view
{
//    [ExecuteInEditMode]
//    [System.Serializable]
    public class LevelGenerator : MonoBehaviour
    {
        // Declare the event to which editor code will hook itself.
        public System.Action WantRepaint;

        [SerializeField]
        private LevelGroupData data;


        public LevelGroupData CurrentLevelGroupData
        {
            get
            {
                return data;
            }
        }

        [SerializeField]
        public LevelData CurrentLevel { get; private set; }

        void OnEnable ()
        {
            if (data != null && data.levels.Count > 0)
                LoadLevel(data.levels[0]);
            else
                LoadLevel(null);
        }

        private void LoadLevel (LevelData levelData)
        {
            if (levelData == CurrentLevel)
            {
                Debug.Log("NOT Loading Level ");
                return;
            }

            // save as current level
            CurrentLevel = levelData;

            // clear objects
            ClearLevelScene();

            if (CurrentLevel != null)
            {
                // load spawners

                // load objects
                if (CurrentLevel.objects != null)
                {
                    for (int i = 0; i < CurrentLevel.objects.Count; i++)
                    {
                        Instantiate(CurrentLevel.objects[i].prefab, CurrentLevel.objects[i].position, Quaternion.identity, transform);
                    }
                }
            }
        }

        private void ClearLevelScene ()
        {
            Debris[] children = gameObject.GetComponentsInChildren<Debris>();

            if (children != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    DestroyImmediate(children[i].gameObject);
                }
            }
        }

        public void SetLevelGroupData (LevelGroupData levelGroupData, bool repaintInspector = false)
        {
            data = levelGroupData;

            if (data == null)
                return;

            if (data.levels.Count > 0)
                LoadLevel(data.levels[0]);
            else
                LoadLevel(null);

            if (repaintInspector && WantRepaint != null)
            {
                WantRepaint();
            }
        }
    }
}