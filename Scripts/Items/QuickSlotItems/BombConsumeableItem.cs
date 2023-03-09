using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Consumables/Bomb Item")]
    public class BombConsumeableItem : ConsumableItem
    {
        [Header("Velocity")]
        public int upwardVelocity = 50;
        public int forwardVelocity = 50;
        public int bombMass = 1;

        [Header("Live Bomb Model")]
        public GameObject liveBombModel;

        [Header("Base Damage")]
        public int baseDamage = 200;
        public int explosiveDamage = 75;

        public override void AttempToConsumeItem(PlayerManager player)
        {
            if (currentItemAmount > 0)
            {
                if (player.playerAnimatorManager.canUseConsumeItem)
                {
                    player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
                    player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true);
                    GameObject bombModel = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform.position, 
                                                        Quaternion.identity, player.playerWeaponSlotManager.rightHandSlot.transform);

                    player.playerEffectsManager.instantiatedFXModel = bombModel;
                    currentItemAmount -= 1;
                    if (!player.uIManager.usingThroughInventory)
                    {
                        player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(this);
                    }
                }
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }

    }
}
