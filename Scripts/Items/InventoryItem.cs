using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Inventory Items")]
    public class InventoryItem : Item
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animations")]
        public string consumeAnimation;
        public bool isInteracting;

        // public virtual void AttempToConsumeItem(PlayerAnimatorManager playerAnimatorManager,
        //                                         PlayerWeaponSlotManager playerWeaponSlotManager,
        //                                         PlayerEffectsManager playerEffectsManager)
        // {
        //     if (currentItemAmount > 0)
        //     {
        //         playerAnimatorManager.EraseHandIKForWeapon();
        //         playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
        //         //playerAnimatorManager.animator.CrossFade("Both Arms Empty", 0.2f);
        //     }
        //     else
        //     {
        //         playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        //     }
        // }

    }
}
