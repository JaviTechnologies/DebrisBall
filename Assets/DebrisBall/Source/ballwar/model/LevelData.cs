using UnityEngine;
using System.Collections;
using javitechnologies.ballwar.levelgenerator.spawner;
using System.Collections.Generic;

namespace javitechnologies.ballwar.model
{
    [CreateAssetMenu(fileName = "LevelSetup", menuName = "Level Generation/Level Setup", order = 1)]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        public long levelId;

        [SerializeField]
        public string levelName;

        [SerializeField]
        public List<DebrisData> objects = new List<DebrisData>();

        public void Clone (LevelData levelData)
        {
            // id
            levelId = levelData.levelId;

            // name
            levelName = levelData.levelName;

            // objects
            objects.Clear();
            if (levelData != null)
            {
                for (int i = 0; i < levelData.objects.Count; i++)
                {
                    objects.Add(levelData.objects[i]);
                }
            }
        }
    }
}