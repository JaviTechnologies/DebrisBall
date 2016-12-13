using UnityEngine;
using System.Collections;

namespace javitechnologies.ballwar.levelgenerator.spawner
{
    [System.Serializable]
    public class RandomSpawnerSetup : AbstractSpawnerSetup
    {
        [SerializeField]
        public Rect range;

        [SerializeField]
        public int quantity;

        [SerializeField]
        public Vector3 [] scaleRange;

        [SerializeField]
        public float rate;
    }
}