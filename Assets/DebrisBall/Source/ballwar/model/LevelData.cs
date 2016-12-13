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
        public List<AbstractSpawnerSetup> spawners = new List<AbstractSpawnerSetup>();
    }
}