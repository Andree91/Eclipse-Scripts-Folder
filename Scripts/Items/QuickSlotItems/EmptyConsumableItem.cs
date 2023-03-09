using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Consumables/Empty")]
    public class EmptyConsumableItem : ConsumableItem
    {
        
        public override void AttempToConsumeItem(PlayerManager player)
        {
            //return;
            // if (playerAnimatorManager.canUseConsumeItem)
            // {
            //     base.AttempToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerEffectsManager);
            //     if (currentItemAmount > 0)
            //     {
            //         GameObject clump = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform);
            //         playerEffectsManager.currentParticleFX = clumpConsumeFX;
            //         playerEffectsManager.instantiatedFXModel = clump;

            //         if (curePoison)
            //         {
            //             playerEffectsManager.poisonBuildUp = 0;
            //             playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
            //             playerEffectsManager.isPoisoned = false;

            //             if (playerEffectsManager.currentPoisonParticleFX != null)
            //             {
            //                 Destroy(playerEffectsManager.currentPoisonParticleFX);
            //             }
            //         }

            //         playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
            //         currentItemAmount -= 1;
            //         playerAnimatorManager.player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(this);
            //     }
            // }
        }
    }
}
