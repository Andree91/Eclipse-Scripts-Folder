using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager player;
        BombConsumeableItem fireBombItem;

        protected override void Awake() 
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftHandWeaponDamageCollider();
                    if (player.hasWeaponBuff && weaponItem.canBeBuffed)
                    {
                        // if (weaponItem.weaponBuffType == DamageType.Fire)
                        // {
                        //     player.playerEffectsManager.leftWeaponFX.ActivateWeaponFireBuffFX();
                        // }
                        // else if (weaponItem.weaponBuffType == DamageType.Lightning)
                        // {
                        //     player.playerEffectsManager.leftWeaponFX.ActivateWeaponLightningBuffFX();
                        // }
                    }

                    if (player.uIManager.quickSlotsUI != null)
                    {
                        if (leftHandSlot.currentWeapon.weaponType == WeaponType.Bow)
                        {
                            if (!player.uIManager.ammoQuickSlotsParent.gameObject.activeInHierarchy)
                            {
                                // Move slots to the left of the quickslot hud
                                player.uIManager.ammoRT.position = new Vector3(287.9f, player.uIManager.ammoRT.position.y, player.uIManager.ammoRT.position.z);
                                player.uIManager.ammoQuickSlotsParent.SetActive(true);
                            }
                        }
                        else if (rightHandSlot.currentWeapon.weaponType != WeaponType.Bow)
                        {
                            player.uIManager.ammoQuickSlotsParent.SetActive(false);
                        }

                        player.uIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    }
                    //player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (player.inputHandler.twoHandFlag)
                    {
                        if (leftHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                        {
                            shieldBackSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                            //animator.CrossFade("TH_Idle_01", 0.2f);
                        }
                        else
                        {
                            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            if (backSlot.currentWeaponModel.name == "Torch_Weapon(Clone)")
                            {
                                backSlot.currentWeaponModel.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
                                backSlot.currentWeaponModel.GetComponentInChildren<Light>().gameObject.SetActive(false);
                            }
                            leftHandSlot.UnloadWeaponAndDestroy();
                            player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                            //animator.CrossFade("TH_Idle_01", 0.2f);
                        }
                    }
                    else
                    {
                        //animator.CrossFade("Both Arms Empty", 0.2f);
                        backSlot.UnloadWeaponAndDestroy();
                        shieldBackSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightHandWeaponDamageCollider();
                    if (character.hasWeaponBuff && weaponItem.canBeBuffed)
                    {
                        // if (weaponItem.weaponBuffType == DamageType.Fire)
                        // {
                        //     player.playerEffectsManager.rightWeaponFX.ActivateWeaponFireBuffFX();
                        // }
                        // else if (weaponItem.weaponBuffType == DamageType.Lightning)
                        // {
                        //     player.playerEffectsManager.rightWeaponFX.ActivateWeaponLightningBuffFX();
                        // }
                    }
                    if (leftHandSlot.currentWeapon == null)
                    {
                        leftHandSlot.currentWeapon = unarmedWeapon;
                    }
                    if (player.uIManager.quickSlotsUI != null)
                    {
                        if (rightHandSlot.currentWeapon.weaponType == WeaponType.Bow)
                        {
                            // Move slots to the right of the quickslot hud
                            player.uIManager.ammoRT.position = new Vector3(player.uIManager.ammoRTOriginalX, player.uIManager.ammoRT.position.y, player.uIManager.ammoRT.position.z);
                            player.uIManager.ammoQuickSlotsParent.SetActive(true);
                        }
                        else if (leftHandSlot.currentWeapon.weaponType == WeaponType.Bow)
                        {
                            player.uIManager.ammoRT.position = new Vector3(287.9f, player.uIManager.ammoRT.position.y, player.uIManager.ammoRT.position.z);
                            player.uIManager.ammoQuickSlotsParent.SetActive(true);
                        }
                        else
                        {
                            player.uIManager.ammoQuickSlotsParent.SetActive(false);
                        }
                        player.uIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    }
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    player.playerInventoryManager.leftWeapon = weaponItem;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftHandWeaponDamageCollider();
                    player.uIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    //player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    player.playerInventoryManager.rightWeapon = weaponItem;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightHandWeaponDamageCollider();
                    player.uIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        public void SuccessfullyThrowFireBomb()
        {
            Destroy(player.playerEffectsManager.instantiatedFXModel);
            if (!player.uIManager.usingThroughInventory)
            {
                fireBombItem = player.playerInventoryManager.currentConsumable as BombConsumeableItem;
            }
            else if (player.uIManager.usingThroughInventory)
            {
                fireBombItem = player.uIManager.inventoryConsumableItemBeingUsed as BombConsumeableItem;
            }
    
            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
            activeModelBomb.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            damageCollider.explosionDamage = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumeber = player.playerStatsManager.teamIDNumeber;
            LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
            player.uIManager.inventoryConsumableItemBeingUsed = null;
        }

        #region Handle Weapon's Stamina Drainage

        // public void DrainStaminaLightAttack()
        // {
        //     WeaponItem currentWeaponBeingUsed = player.characterInventoryManager.currentItemBeingUsed as WeaponItem;
        //     player.playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaMultiplier));
        // }

        // public void DrainStaminaHeavyAttack()
        // {
        //     WeaponItem currentWeaponBeingUsed = player.characterInventoryManager.currentItemBeingUsed as WeaponItem;
        //     player.playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaMultiplier));
        // }

        #endregion

    }
}
