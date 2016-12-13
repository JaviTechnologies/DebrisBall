using UnityEngine;
using System.Collections;

namespace javitechnologies.ballwar.levelgenerator.spawner
{
    [System.Serializable]
    public class FixedSpawnerSetup : AbstractSpawnerSetup
    {
        [SerializeField]
        public Vector3 placementPosition;

        [SerializeField]
        public Vector3 localScale;
    }
}