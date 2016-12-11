using UnityEngine;
using System.Collections;

namespace javitechnologies.katamari
{
    public class BallMovement : MonoBehaviour
    {
        public float speed;

        private Transform cameraTransform;
        Rigidbody body;
        Vector3 force;

        void Awake()
        {
            cameraTransform = Camera.main.transform;
            force = Vector3.zero;
            body = gameObject.GetComponent<Rigidbody>();
        }

        void LateUpdate()
        {
            force.x = Input.GetAxis("Horizontal");
            force.z = Input.GetAxis("Vertical");

            if (force.x != 0 || force.z != 0)
            {
                // correct the force to face the camera
                // so the force is always from the perspective of the user
                force = cameraTransform.TransformDirection(force);
                force.y = 0f;

                body.AddForce(force * speed);
            }
        }
    }
}