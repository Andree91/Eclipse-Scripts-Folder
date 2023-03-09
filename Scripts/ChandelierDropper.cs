using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class ChandelierDropper : MonoBehaviour
    {
        public GameObject chandelierModel;
        public GameObject brokenModel;
        public Light chandelierLight;
        
        
        Rigidbody chandelierRigidbody;
        ParticleSystem chandelierFX;
        
        void Awake() 
        {
            chandelierRigidbody = GetComponentInParent<Rigidbody>();
            chandelierFX = GetComponentInParent<ParticleSystem>();
        }

        void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Weapon")
            {
                // Play sound and vfx;

                chandelierRigidbody.useGravity = true;
                chandelierRigidbody.isKinematic = false;

                StartCoroutine(ChangeMeshToBroken());
            }
        }

        IEnumerator ChangeMeshToBroken()
        {
            yield return new WaitForSeconds(2f);
            // Play crash sound and vfx;
            brokenModel.SetActive(true);
            chandelierModel.SetActive(false);
            chandelierLight.enabled = false;
            chandelierFX.Stop();
        }
    }
}
