using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DisablerProp : MonoBehaviour
    {
        public List<Collider> colliders = new List<Collider>();
        public List<Rigidbody> rigidBodies = new List<Rigidbody>();

        bool isDisabled = false;
        [SerializeField] float timer = 3f;

        void Start() 
        {
            // transform.GetChild(0).GetComponentsInChildren(colliders);
            // transform.GetChild(0).GetComponentsInChildren(rigidBodies);

            StartCoroutine(ToggleColliders());
        }


        IEnumerator ToggleColliders()
        {
            yield return new WaitForSeconds(timer);

            foreach (var collider in colliders)
            {
                collider.enabled = isDisabled;
            }
            foreach (var rigidBody in rigidBodies)
            {
                rigidBody.detectCollisions = isDisabled;
                rigidBody.isKinematic = !isDisabled;
            }
        }
    }
}
