using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (character.isInteracting) { return; }

            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();

            //Create the Live arrow instantiate loacation
            ArrowInstantiateLocation arrowInstantiateLocation;
            arrowInstantiateLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiateLocation>();

            //Animate the bow firing arrow
            Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_Object_Fire");

            //Destroy loaded arrow model fx at player hand
            Destroy(character.characterEffectsManager.instantiatedFXModel);

            //Reset the player holding arrow flag
            Debug.Log("Here is fire animation");
            character.characterAnimatorManager.PlayTargetAnimation("Bow_Fire_Arrow", true);
            character.animator.SetBool("isHoldingArrow", false);
            character.characterInventoryManager.currentAmmo01.currentAmmo -= 1;

            //FIRE THE ARROW AS A PLAYER CHARACTER
            if (player != null)
            {
                player.inputHandler.fireFlagRB = false;

                //Create and Fire The live Arrow
                GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo01.liveAmmomodel,
                                                    arrowInstantiateLocation.transform.position,
                                                    player.cameraHandler.cameraPivotTransform.rotation);
                Rigidbody rigidbodyArrow = liveArrow.GetComponent<Rigidbody>();

                player.uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(player.playerInventoryManager.currentAmmo01, true);

                if (player.isAiming)
                {
                    Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hitPoint;

                    if (Physics.Raycast(ray, out hitPoint, 100.0f))
                    {
                        liveArrow.transform.LookAt(hitPoint.point);
                    }
                    else
                    {
                        liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                    }
                }
                else
                {
                    //Give Ammo Velocity
                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        //Since while locked we are always facing our our target, we can copy our facing direction to our arrows facing direction when fired
                        Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                        liveArrow.transform.rotation = arrowRotation;
                    }
                    else
                    {
                        liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                    }
                }

                rigidbodyArrow.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo01.forwardVelocity);
                rigidbodyArrow.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo01.upwardVelocity);
                rigidbodyArrow.useGravity = player.playerInventoryManager.currentAmmo01.useGravity;
                rigidbodyArrow.mass = player.playerInventoryManager.currentAmmo01.ammoMass;
                liveArrow.transform.parent = null;
                RangeProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangeProjectileDamageCollider>();

                //Set Live arrow damage collider
                damageCollider.character = player;
                damageCollider.ammoItem = player.playerInventoryManager.currentAmmo01;
                damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo01.physicalDamage;
                damageCollider.teamIDNumeber = player.playerStatsManager.teamIDNumeber;
            }
            
            //FIRE THE ARROW AS AN A.I CHARACTER
            else
            {
                EnemyManager enemy = character as EnemyManager;
                //Create and Fire The live Arrow
                GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo01.liveAmmomodel,
                                                    arrowInstantiateLocation.transform.position,
                                                    Quaternion.identity);

                Rigidbody rigidbodyArrow = liveArrow.GetComponent<Rigidbody>();

                //Give Ammo Velocity
                if (enemy.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }

                rigidbodyArrow.AddForce(liveArrow.transform.forward * enemy.characterInventoryManager.currentAmmo01.forwardVelocity);
                rigidbodyArrow.AddForce(liveArrow.transform.up * enemy.characterInventoryManager.currentAmmo01.upwardVelocity);
                rigidbodyArrow.useGravity = enemy.characterInventoryManager.currentAmmo01.useGravity;
                rigidbodyArrow.mass = enemy.characterInventoryManager.currentAmmo01.ammoMass;
                liveArrow.transform.parent = null;
                RangeProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangeProjectileDamageCollider>();

                //Set Live arrow damage collider
                damageCollider.character = enemy;
                damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo01;
                damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo01.physicalDamage;
                damageCollider.teamIDNumeber = character.characterStatsManager.teamIDNumeber;
            }
        }

        public void FireArrowActionOH(CharacterManager character)
        {
            PerformAction(character);
        }
    }
}
