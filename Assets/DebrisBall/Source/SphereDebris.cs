using UnityEngine;
using System.Collections;

public class SphereDebris : MonoBehaviour, IDebris
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
        volume = Mathf.PI * Mathf.Pow(transform.localScale.magnitude, 3) / 6;

        mass = density * volume;

        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.mass = mass;
    }
}
