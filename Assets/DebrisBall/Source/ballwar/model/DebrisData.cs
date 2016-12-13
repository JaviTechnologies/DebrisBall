using UnityEngine;
using System.Collections;

namespace javitechnologies.ballwar.model
{
    [System.Serializable]
    public struct DebrisData
    {
        [SerializeField]
        public string name;

        [SerializeField]
        public Vector3 position;

        [SerializeField]
        public Vector3 scale;

        [SerializeField]
        public Object prefab;
    }
}