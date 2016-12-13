using System;
using UnityEngine;

[System.Serializable]
[RequireComponent (typeof (Rigidbody))]
public abstract class Debris: MonoBehaviour, IDebris
{
    
    protected float density = 0.01f;
    private float volume;
    private float mass;

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
        volume = calculateVolume();

        mass = density * volume;

        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.mass = mass;
    }

    protected abstract float calculateVolume();
}

