using UnityEngine;
using System.Collections;

namespace javitechnologies.katamari
{
    public class SmoothFollow : MonoBehaviour
    {
        // target to follow
        public Transform target;

        // my transform
        private Transform myTransform;
        // keep track of the target's last position to know where is forward and what is backwards
        private Vector3 lastPosition;
        // keep the original offset values
        private float originalHorizontalOffset, originalVerticalOffset;
        // keep the actual offset value
        private Vector3 relativeOffset;
        private Vector3 relativeDisplacement;

        public float smoothTime = 0.3F;
        private Vector3 velocity = Vector3.zero;

        void Start()
        {
            myTransform = transform;
            Vector3 offset = myTransform.position - target.position;
            originalHorizontalOffset = new Vector3(offset.x, 0f, offset.z).magnitude;
            originalVerticalOffset = offset.y;
            relativeOffset = offset;
        }

        void LateUpdate()
        {
            relativeDisplacement = (lastPosition - target.position).normalized;
            lastPosition = target.position;

            if (relativeDisplacement.x != 0 || relativeDisplacement.z != 0)
            {
                relativeOffset = relativeDisplacement * originalHorizontalOffset;
            }
            relativeOffset.y = originalVerticalOffset;
            
            myTransform.position = Vector3.SmoothDamp(myTransform.position, target.position + relativeOffset, ref velocity, smoothTime);
        }
    }
}