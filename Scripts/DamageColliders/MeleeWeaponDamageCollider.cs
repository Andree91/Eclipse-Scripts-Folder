using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Weapon Buff Damage")]
        public float physicalBuffDamage;
        public float fireBuffDamage;
        public float lightningBuffDamage;
        public float poiseBuffDamage;

        protected override void DealDamage(CharacterStatsManager enemyStats)
        {
            //Get Attack Type from character
            //Apply Damage multipliers
            //Pass damage

            float finalPhysicalDamage = physicalDamage + physicalBuffDamage;
            float finalFireDamage = fireDamage + fireBuffDamage;
            float finalLightningDamage = lightningDamage + lightningBuffDamage;
            float attackDamageModifier = 0f;
            //float finalDamage = 0f;

            //If character is using right hand
            if (character.isUsingRightHand)
            {
                switch (character.characterCombatManager.currentAttackType)
                {
                    case AttackType.LightAttack01:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.lightAttack01DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.LightAttack02:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.lightAttack02DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.HeavyAttack01:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.heavyAttack01DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.HeavyAttack02:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.heavyAttack02DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.RunningAttack:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.runningAttackDamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.JumpingAttack:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.jumpingAttackDamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.PlungingAttack:
                        attackDamageModifier = character.characterInventoryManager.rightWeapon.plungingAttackDamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                }
            //     if (character.characterCombatManager.currentAttackType == AttackType.LightAttack01)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.lightAttack01DamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            //     else if (character.characterCombatManager.currentAttackType == AttackType.LightAttack02)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.lightAttack02DamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            //     else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack01)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.heavyAttack01DamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            //     else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack02)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.heavyAttack02DamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            //     else if (character.characterCombatManager.currentAttackType == AttackType.RunningAttack)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.runningAttackDamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            //     else if (character.characterCombatManager.currentAttackType == AttackType.JumpingAttack)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.jumpingAttackDamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            //     else if (character.characterCombatManager.currentAttackType == AttackType.PlungingAttack)
            //     {
            //         attackDamageModifier = character.characterInventoryManager.rightWeapon.plungingAttackDamageModifier;
            //         finalDamage = finalPhysicalDamage * attackDamageModifier;
            //         finalDamage += finalFireDamage * attackDamageModifier;
            //         finalDamage += finalLightningDamage * attackDamageModifier;
            //     }
            }
            //If character is using left hand
            else
            {
                switch (character.characterCombatManager.currentAttackType)
                {
                    case AttackType.LightAttack01:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.lightAttack01DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.LightAttack02:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.lightAttack02DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.HeavyAttack01:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.heavyAttack01DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.HeavyAttack02:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.heavyAttack02DamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.RunningAttack:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.runningAttackDamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.JumpingAttack:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.jumpingAttackDamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                    case AttackType.PlungingAttack:
                        attackDamageModifier = character.characterInventoryManager.leftWeapon.plungingAttackDamageModifier;
                        //finalDamage = finalPhysicalDamage;
                        finalPhysicalDamage *= attackDamageModifier;
                        //finalDamage += finalFireDamage;
                        finalFireDamage *= attackDamageModifier;
                        //finalDamage += finalLightningDamage;
                        finalLightningDamage *= attackDamageModifier;
                        //finalDamage *= attackDamageModifier;
                        break;
                }
                // if (character.characterCombatManager.currentAttackType == AttackType.LightAttack01)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.lightAttack01DamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
                // else if (character.characterCombatManager.currentAttackType == AttackType.LightAttack02)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.lightAttack02DamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
                // else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack01)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.heavyAttack01DamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
                // else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack02)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.heavyAttack02DamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
                // else if (character.characterCombatManager.currentAttackType == AttackType.RunningAttack)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.runningAttackDamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
                // else if (character.characterCombatManager.currentAttackType == AttackType.JumpingAttack)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.jumpingAttackDamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
                // else if (character.characterCombatManager.currentAttackType == AttackType.PlungingAttack)
                // {
                //     attackDamageModifier = character.characterInventoryManager.leftWeapon.plungingAttackDamageModifier;
                //     finalDamage = finalPhysicalDamage * attackDamageModifier;
                //     finalDamage += finalFireDamage * attackDamageModifier;
                //     finalDamage += finalLightningDamage * attackDamageModifier;
                // }
            }


            //Deal Modified Final Amount Of Damage
            if (enemyStats.totalPoiseDefence > poiseBreak)
            {
                enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), Mathf.RoundToInt(finalLightningDamage), character);
            }
            else
            {
                if (finalPhysicalDamage != 0)
                {
                    //Debug.Log("Now we are dealing physical damage");
                    enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), Mathf.RoundToInt(finalLightningDamage), currentDamageAnimation, character);
                }
                else if (fireDamage != 0)
                {
                    //Debug.Log("Now we are dealing fire damage");
                    enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), Mathf.RoundToInt(finalLightningDamage), currentDamageAnimation, character);
                }
                else if (enemyStats.character.characterWeaponSlotManager.rightHandDamageCollider != null)
                {
                    enemyStats.character.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                }
            }
        }
    }
}