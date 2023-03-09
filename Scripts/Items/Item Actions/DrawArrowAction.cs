using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting) { return; }

            if (character.isHoldingArrow) {return; }

            if (character.characterInventoryManager.currentAmmo01 != null)
            {
                if (character.characterInventoryManager.currentAmmo01.currentAmmo > 0)
                {
                    //Animate The Player
                    character.animator.SetBool("isHoldingArrow", true);
                    character.characterAnimatorManager.EraseHandIKForWeapon();
                    character.characterAnimatorManager.PlayTargetAnimation("Bow_Draw_Arrow", true);

                    //Instantiate The Arrow
                    GameObject loadedArrow = Instantiate(character.characterInventoryManager.currentAmmo01.loadedItemModel, character.characterWeaponSlotManager.leftHandSlot.transform);
                    character.characterEffectsManager.instantiatedFXModel = loadedArrow;

                    //Animate The Bow
                    Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                    bowAnimator.SetBool("isDrawn", true);
                    bowAnimator.Play("Bow_Object_Draw");
                }
                else
                {
                    //Otherwise play out of ammo animation
                    character.characterAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
            else
            {
                //Otherwise play out of ammo animation
                character.characterAnimatorManager.PlayTargetAnimation("Shrug", true);
            }
            
        }
    }
}
