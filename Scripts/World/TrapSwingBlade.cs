using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TrapSwingBlade : MonoBehaviour
    {
        public float angle = 70f;
        public float speed = 2f;
        public float startTime = 0f;

        Quaternion startQuaterion, endQuaterion;

        void Start()
        {
            startQuaterion = PendulumRotation(angle);
            endQuaterion = PendulumRotation(-angle);
        }

        void FixedUpdate() 
        {
            startTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startQuaterion, endQuaterion, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1f) / 2f);
        }

        void ResetTimer()
        {
            startTime = 0f;
        }
        
        Quaternion PendulumRotation(float angle)
        {
            var pendulumRotation = transform.rotation;
            var angleZ = pendulumRotation.eulerAngles.z + angle;

            if (angleZ > 180)
            {
                angleZ -= 360;
            }
            else if (angleZ < -180)
            {
                angleZ += 360;
            }

            pendulumRotation.eulerAngles = new Vector3 (pendulumRotation.eulerAngles.x, pendulumRotation.eulerAngles.y, angleZ);
            return pendulumRotation;
        }
    }
}
