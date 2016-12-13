using UnityEngine;
using System.Collections;

[System.Serializable]
public class CubeDebris : Debris
{
    protected override float calculateVolume()
    {
        Vector3 localScale = transform.localScale;
        return localScale.x * localScale.y * localScale.z;
    }
}
