using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class ConsumableItem : Item
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        public int maxStoredAmount;
        public int currentStoredItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animations")]
        public string consumeAnimation;
        public bool isInteracting;

        public bool canUseMultipleAtSameTime;
        public bool isEmpty;

        public virtual void AttempToConsumeItem(PlayerManager player)
        {
            if (currentItemAmount > 0)
            {
                player.playerAnimatorManager.EraseHandIKForWeapon();
                player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
                //playerAnimatorManager.animator.CrossFade("Both Arms Empty", 0.2f);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
        }

        public virtual void SuccesfullyConsumeItem(PlayerManager player)
        {
            currentItemAmount -= 1;
            player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(this);
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }
    }
}
