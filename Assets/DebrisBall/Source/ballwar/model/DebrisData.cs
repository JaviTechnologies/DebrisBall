using UnityEngine;
using System.Collections;

namespace javitechnologies.ballwar.model
{
    [System.Serializable]
    public class DebrisData
    {
        [SerializeField]
        public string name;

        [SerializeField]
        public Vector3 position;

        [SerializeField]
        public float density;

        [SerializeField]
        public Vector3 scale;

        [SerializeField]
        public Transform prefab;
    }
}