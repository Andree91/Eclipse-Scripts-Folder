using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Consumables/Buff Item")]
    public class BuffConsumableItem : ConsumableItem
    {
        [Header("Effect")]
        [SerializeField] WeaponBuffEffect weaponBuffEffect;

        [Header("Buff SFX")]
        [SerializeField] AudioClip buffTriggerSound;
        [SerializeField] float triggerSoundVolume = 0.3f;

        // [Header("Buff FX")]
        // public GameObject buffFX;

        // [Header("Buff Type")]
        // public bool isFireBuff;
        // public bool isLightnigBuff;

        public override void AttempToConsumeItem(PlayerManager player)
        {
            // THIS IS THE CURRENT SYSTEM

            // If character can't use this item, return without doing anything
            if (!CanIUseThisItem(player))
            {
                player.playerAnimatorManager.PlayTargetAnimation("Shrug", true); // Delete Later, just for testing
                return;
            }

            if (player.playerAnimatorManager.canUseConsumeItem)
            {
                player.playerAnimatorManager.EraseHandIKForWeapon();
                player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
            }

            // THIS IS THE OLD WEAPON BUFF SYSTEM
            // I MODIFIED IT TO BE MORE SIMPLE FOR THE MULTIPLAYING PURPOSE
            //     if (player.playerAnimatorManager.canUseConsumeItem && !player.hasWeaponBuff)
            //     {
            //         if (currentItemAmount > 0)
            //         {
            //             player.playerAnimatorManager.EraseHandIKForWeapon();

            //             if (isFireBuff)
            //             {
            //                 if (!player.isUsingLeftHand)
            //                 {
            //                     if (player.playerInventoryManager.rightWeapon.canBeBuffed)
            //                     {
            //                         player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting);
            //                         player.playerInventoryManager.rightWeapon.weaponBuffType = DamageType.Fire;
            //                         player.playerEffectsManager.ActivateWeaponBuffFX(false);
            //                         currentItemAmount -= 1;
            //                     }
            //                 }
            //                 else
            //                 {
            //                     if (player.playerInventoryManager.leftWeapon.canBeBuffed)
            //                     {
            //                         player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, false, true);
            //                         player.playerInventoryManager.leftWeapon.weaponBuffType = DamageType.Fire;
            //                         player.playerEffectsManager.ActivateWeaponBuffFX(true);
            //                         currentItemAmount -= 1;
            //                     }

            //                 }
            //             }
            //             else if (isLightnigBuff)
            //             {
            //                 if (!player.isUsingLeftHand)
            //                 {
            //                     if (player.playerInventoryManager.rightWeapon.canBeBuffed)
            //                     {
            //                         player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting);
            //                         player.playerInventoryManager.rightWeapon.weaponBuffType = DamageType.Lightning;
            //                         player.playerEffectsManager.ActivateWeaponBuffFX(false);
            //                         currentItemAmount -= 1;
            //                     }
            //                 }
            //                 else
            //                 {
            //                     if (player.playerInventoryManager.leftWeapon.canBeBuffed)
            //                     {
            //                         player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, false, true);
            //                         player.playerInventoryManager.leftWeapon.weaponBuffType = DamageType.Lightning;
            //                         player.playerEffectsManager.ActivateWeaponBuffFX(true);
            //                         currentItemAmount -= 1;
            //                     }

            //                 }
            //             }

            //             //playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
            //             player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(this);
            //         }
            //         else
            //         {
            //             player.playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            //         }
            //     }
            //     // else
            //     // {
            //     //     playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            //     // }
            // }
        }

        public override void SuccesfullyConsumeItem(PlayerManager player)
        {
            base.SuccesfullyConsumeItem(player);

            player.characterSoundFXManager.PlaySoundFX(buffTriggerSound, triggerSoundVolume);
            WeaponBuffEffect weaponBuff = Instantiate(weaponBuffEffect);
            weaponBuff.isRightHandBuff = true;
            player.playerEffectsManager.rightWeaponBuffEffect = weaponBuff;
            player.playerEffectsManager.ProcessWeaponBuffs();
        }

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (player.playerInventoryManager.currentConsumable.currentItemAmount <= 0)
            {
                return false;
            }

            MeleeWeaponItem meleeWeapon = player.playerInventoryManager.rightWeapon as MeleeWeaponItem;

            if (meleeWeapon != null && meleeWeapon.canBeBuffed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}