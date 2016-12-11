using UnityEngine;
using System.Collections;

public class CubeDebris : MonoBehaviour, IDebris
{
    private float density = 0.01f;
    public float volume;
    public float mass;

    public float Volume
    {
        get { return volume; }
    }

    public float Mass
    {
        get { return mass; }
    }

    void Start()
    {
        Vector3 localScale = transform.localScale;
        volume = localScale.x * localScale.y * localScale.z;

        mass = density * volume;

        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.mass = mass;
    }
}
