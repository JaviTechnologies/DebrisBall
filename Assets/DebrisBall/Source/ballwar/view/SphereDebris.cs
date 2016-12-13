using UnityEngine;
using System.Collections;

public class SphereDebris : Debris
{
    protected override float calculateVolume()
    {
        return Mathf.PI * Mathf.Pow(transform.localScale.magnitude, 3) / 6;
    }
}
