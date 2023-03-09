using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Blocking Action")]
    public class BlockingAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting) { return; }

            if (character.isBlocking) { return; }

            if (character.characterInventoryManager.leftWeapon.weaponType == WeaponType.Shield && !character.isTwoHanding)
            {
                character.characterAnimatorManager.PlayTargetAnimation("Block Start", false, true);
                character.animator.runtimeAnimatorController = character.characterInventoryManager.leftWeapon.weaponController;
                character.leftHandIsShield = true;
            }
            else
            {
                character.leftHandIsShield = false;
            }

            character.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapons();

            character.isBlocking = true;
        }
    }
}
