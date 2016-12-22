using UnityEngine;
using System.Collections.Generic;
using javitechnologies.ballwar.model;

namespace javitechnologies.levelgenerator.data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level Generation/Level Generator Setup", order = 1)]
    public class LevelGeneratorData : ScriptableObject
    {
        [SerializeField]
        public List<LevelData> levels;
    }
}