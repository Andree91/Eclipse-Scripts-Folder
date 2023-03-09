using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
    public class ItemBasedAttackAction : ScriptableObject
    {
        [Header("Attack Type")]
        public AIAttackActionType actionAttackType = AIAttackActionType.meleeAttackAction;
        public AttackType attackType = AttackType.LightAttack01;

        [Header("Action Combo Sttetings")]
        public bool actionCanCombo = false;

        [Header("Right Hand Or Left Hand Action")]
        bool isRightHandedAction = true; //Maybe chance later for eneums if some enemy has like 6 different arms etc...

        [Header("Action Settings")]
        public int attackScore = 3;
        public float recorveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;

        public void PerformAttackAction(EnemyManager enemy)
        {
            if (isRightHandedAction)
            {
                enemy.UpdateWhichHandCharacterIsUsing(true);
                PerformRightHandActionBasedOnAttackType(enemy);
            }
            else
            {
                enemy.UpdateWhichHandCharacterIsUsing(false);
                PerformLeftHandActionBasedOnAttackType(enemy);
            }
        }

        //DECIDE WHICH HAND PERFORMS ACTION
        private void PerformRightHandActionBasedOnAttackType(EnemyManager enemy)
        {
            if (actionAttackType == AIAttackActionType.meleeAttackAction)
            {
                //PERFORM RIGHT HAND MELEE ACTION
                PerformRightHandMeleeAction(enemy);
            }
            else if (actionAttackType == AIAttackActionType.magicAttackAction)
            {
                PerformRightHandMagicAction(enemy);
            }
            else if (actionAttackType == AIAttackActionType.rangedAttackAction)
            {
                //PERFORM RIGHT HAND RANGE ACTION
            }
        }

        private void PerformLeftHandActionBasedOnAttackType(EnemyManager enemy)
        {
            if (actionAttackType == AIAttackActionType.meleeAttackAction)
            {
                //PERFORM LEFT HAND MELEE ACTION
                PerformLeftHandMeleeAction(enemy);
            }
            else if (actionAttackType == AIAttackActionType.magicAttackAction)
            {
                //PERFORM LEFT HAND MAGIC ACTION
            }
            else if (actionAttackType == AIAttackActionType.rangedAttackAction)
            {
                //PERFORM LEFT HAND RANGE ACTION
            }
        }

        //RIGHT HAND ACTION
        private void PerformRightHandMeleeAction(EnemyManager enemy)
        {
            if (enemy.isTwoHanding)
            {
                if (attackType == AttackType.LightAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_RB_action.PerformAction(enemy);
                }
                else if (attackType == AttackType.HeavyAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_RT_action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.LightAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.tap_RB_action.PerformAction(enemy);
                }
                else if (attackType == AttackType.HeavyAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.tap_RT_action.PerformAction(enemy);
                }
            }
        }

        private void PerformRightHandMagicAction(EnemyManager enemy)
        {
            if (enemy.isTwoHanding)
            {
                if (attackType == AttackType.LightAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_RB_action.PerformAction(enemy);
                }
                else if (attackType == AttackType.HeavyAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_RT_action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.LightAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.tap_RB_action.PerformAction(enemy);
                }
                else if (attackType == AttackType.HeavyAttack01)
                {
                    enemy.characterInventoryManager.rightWeapon.tap_RT_action.PerformAction(enemy);
                }
            }
        }

        //LEFT HAND ACTION
        private void PerformLeftHandMeleeAction(EnemyManager enemy)
        {
            if (enemy.isTwoHanding)
            {
                if (attackType == AttackType.LightAttack01)
                {
                    enemy.characterInventoryManager.leftWeapon.th_tap_RB_action.PerformAction(enemy);
                }
                else if (attackType == AttackType.HeavyAttack01)
                {
                    enemy.characterInventoryManager.leftWeapon.th_tap_RT_action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.LightAttack01)
                {
                    enemy.characterInventoryManager.leftWeapon.tap_RB_action.PerformAction(enemy);
                }
                else if (attackType == AttackType.HeavyAttack01)
                {
                    enemy.characterInventoryManager.leftWeapon.tap_RT_action.PerformAction(enemy);
                }
            }
        }
    }
}
