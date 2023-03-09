using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool manaFlask;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFx;

        public override void AttempToConsumeItem(PlayerManager player)
        {
            if (player.playerAnimatorManager.canUseConsumeItem)
            {
                base.AttempToConsumeItem(player);
                if (currentItemAmount > 0)
                {
                    GameObject flask = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
                    player.playerEffectsManager.currentParticleFX = recoveryFx;

                    if (estusFlask)
                    {
                        player.playerEffectsManager.amountToBeHealed = healthRecoverAmount;
                    }
                    else if (manaFlask)
                    {
                        player.playerEffectsManager.amountToBeRestoredMana = focusPointRecoverAmount;
                    }

                    player.playerEffectsManager.instantiatedFXModel = flask;
                    player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
                    currentItemAmount -= 1;
                    if (!player.uIManager.usingThroughInventory)
                    {
                        player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(this);
                    }
                    player.uIManager.inventoryConsumableItemBeingUsed = null;

                }
            }

            //Add health or focus
            //Instantiate flask in player hand
            //Play recovery fx if successfully used flask
        }
    }
}
