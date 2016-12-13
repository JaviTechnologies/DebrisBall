using UnityEngine;
using System.Collections;

namespace javitechnologies.debrisball.data
{
    enum DebrisType
    {
        Sphere, Box
    }

    [CreateAssetMenu(fileName = "DebrisData", menuName = "Inventory/DebrisData", order = 1)]
    public class CreateDebrisPrefabData : ScriptableObject
    {
        [SerializeField]
        private string type;

        [SerializeField]
        private Transform prefab;
    }
}