using UnityEngine;
using System.Collections.Generic;
using javitechnologies.ballwar.model;

namespace javitechnologies.levelgenerator.data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "LevelGroupData", menuName = "Level Generation/Level Group Data", order = 1)]
    public class LevelGroupData : ScriptableObject
    {
        [SerializeField]
        public List<LevelData> levels;
    }
}