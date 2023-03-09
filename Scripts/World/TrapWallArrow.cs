using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TrapWallArrow : MonoBehaviour
    {
        public GameObject wallArrow;
        public Transform[] arrowInstantiateLocations;
        public bool isEnabled = true;
        Animator animator;
        Rigidbody arrowRigidbody;

        void Awake() 
        {
            animator = GetComponentInParent<Animator>();
        }

        void OnTriggerEnter(Collider other) 
        {
            if (isEnabled)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    animator.Play("Trap_Trigger_Down");

                    foreach (Transform arrowInstantiateLocation in arrowInstantiateLocations)
                    {
                        GameObject wallArrowLiveModel = Instantiate(wallArrow, arrowInstantiateLocation.position, arrowInstantiateLocation.transform.rotation);
                        wallArrowLiveModel.transform.parent = null;

                        arrowRigidbody = wallArrowLiveModel.GetComponent<Rigidbody>();

                        Debug.Log("before add force");
                        arrowRigidbody.isKinematic = false;
                        arrowRigidbody.useGravity = false;
                        arrowRigidbody.AddForce(wallArrowLiveModel.transform.forward * 800);
                        arrowRigidbody.AddForce(wallArrowLiveModel.transform.up * 1);
                        arrowRigidbody.transform.parent = null;

                        RangeProjectileDamageCollider damageCollider = wallArrowLiveModel.GetComponent<RangeProjectileDamageCollider>();

                        //Set Live arrow damage collider
                        CharacterManager enemy = other.GetComponent<CharacterManager>();
                        damageCollider.character = enemy;
                        damageCollider.isTrapArrow = true;
                        // damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo;
                        // damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo.physicalDamage;
                        damageCollider.teamIDNumeber = 10;
                    }
                }
            }
        }
    }
}
