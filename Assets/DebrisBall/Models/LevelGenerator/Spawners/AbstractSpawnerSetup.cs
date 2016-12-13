using UnityEngine;
using System.Collections;

namespace javitechnologies.ballwar.levelgenerator.spawner
{
    public enum SpawnerType
    {
        NONE, FIXED, RANDOM
    }

    [System.Serializable]
    public abstract class AbstractSpawnerSetup
    {
        [SerializeField]
        public SpawnerType type = SpawnerType.NONE;

        [SerializeField]
        public Transform prefab = null;
    }
}