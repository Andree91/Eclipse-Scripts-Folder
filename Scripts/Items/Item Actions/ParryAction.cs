using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting) { return; }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;

            //Check if parrying weapon is fast parry weapon or medium speed parry
            if (parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                //Do fast parry animation
                character.characterAnimatorManager.PlayTargetAnimation("Parry_02", true); //Change Later
                character.characterStatsManager.DetuctStamina(parryingWeapon.baseStaminaCost);
                //Bugler Parry
                //Caestus Parry
            }
            else if (parryingWeapon.weaponType == WeaponType.Shield)
            {
                //Do Regular Parry
                character.characterAnimatorManager.PlayTargetAnimation("Parry_01", true);
                character.characterStatsManager.DetuctStamina(parryingWeapon.baseStaminaCost);
            }
            // else
            // {
            //     maybe parrying dagger animation
            // }
        }
    }
}
