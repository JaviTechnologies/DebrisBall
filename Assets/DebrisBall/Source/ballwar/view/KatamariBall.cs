using UnityEngine;
using System.Collections;

public class KatamariBall : MonoBehaviour
{
    private float currentHalfVolume = 0f;
    private Rigidbody body;
    private float density = 0.01f;

    void Start ()
    {
        body = gameObject.GetComponent<Rigidbody>();

        currentHalfVolume = Mathf.PI * Mathf.Pow(transform.localScale.magnitude,3) / 12;

        body.mass = 2 * currentHalfVolume * density;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Debris"))
            return;
        
        Debris debris = collision.gameObject.GetComponent<Debris>();
        if (debris.Volume < currentHalfVolume)
        {
            currentHalfVolume += (debris.Volume / 2);
            body.mass += debris.Mass;
            Destroy(debris);
            Destroy(collision.gameObject.GetComponent<Rigidbody>());
            collision.transform.SetParent(transform);
        }
    }
}
