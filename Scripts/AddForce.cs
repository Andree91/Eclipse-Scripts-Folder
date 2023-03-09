using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AddForce : MonoBehaviour
    {
        Rigidbody rb;

        void Awake() 
        {
            rb = GetComponent<Rigidbody>();
            // rb.AddForce(Vector3.right * 150);
            // rb.AddForce(Vector3.forward * 75);
            rb.AddExplosionForce(10, transform.position, 1f, 0.2f, ForceMode.Impulse);
            Debug.Log(rb.velocity);
        }


    }
}
