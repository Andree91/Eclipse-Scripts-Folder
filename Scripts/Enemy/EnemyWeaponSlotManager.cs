using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            //playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            //playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion

        public void EnableCombo()
        {
            //animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //animator.SetBool("canDoCombo", false);
        }

        #region  Handle Weapon's Poise Bonus

        public override void GrantWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.totalPoiseDefence + character.characterStatsManager.offensivePoiseBonus;
        }

        public override void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.armorPoiseBonus;
        }

        #endregion

    }
}
