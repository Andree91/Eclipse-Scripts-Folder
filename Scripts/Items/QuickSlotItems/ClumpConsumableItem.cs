using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
    public class ClumpConsumableItem : ConsumableItem
    {
        [Header("Recovery FX")]
        public GameObject clumpConsumeFX;

        [Header("Cure FX")]
        public bool curePoison;
        //cure bleed
        //cure frost
        //death

        public override void AttempToConsumeItem(PlayerManager player)
        {
            if (player.playerAnimatorManager.canUseConsumeItem)
            {
                base.AttempToConsumeItem(player);
                if (currentItemAmount > 0)
                {
                    GameObject clump = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
                    player.playerEffectsManager.currentParticleFX = clumpConsumeFX;
                    player.playerEffectsManager.instantiatedFXModel = clump;

                    if (curePoison)
                    {
                        player.playerStatsManager.poisonBuildUp = 0;
                        //player.playerStatsManager.poisonAmount = player.playerEffectsManager.defaultPoisonAmount;
                        player.playerStatsManager.isPoisoned = false;
                        player.isInCombat = false;

                        //if (player.playerEffectsManager.currentPoisonParticleFX != null)
                        {
                            //Destroy(player.playerEffectsManager.currentPoisonParticleFX);
                        }
                    }

                    player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
                    currentItemAmount -= 1;
                    player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(this);
                }
            }
        }
    }
}
