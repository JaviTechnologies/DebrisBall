using UnityEngine;
using System.Collections;
using javitechnologies.ballwar.levelgenerator;
using javitechnologies.levelgenerator.data;

namespace javitechnologies.levelgenerator.view
{
    [System.Serializable]
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        public LevelGeneratorData levelGeneratorSetup;
    }
}