using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0) { return; }

            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();
            character.characterEffectsManager.PlayWeaponFX(false);

            //If we can do combo
            if (character.canDoCombo)
            {
                //character.inputHandler.comboFlag = true;
                HandleChargeWeaponCombo(character);
                character.characterCombatManager.currentAttackType = AttackType.HeavyAttack02;
                //character.inputHandler.comboFlag = false;
            }
            //If not, perform regular attack
            else
            {
                if (character.isInteracting) { return; }
                if (character.canDoCombo) { return; }

                HandleChargeAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.HeavyAttack01;
            }
        }

        void HandleChargeAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                }
            }
        }


        void HandleChargeWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_charge_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_01, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_02)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_03, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_03;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                        }
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_charge_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_charge_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_charge_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_charge_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_02;
                        }
                        // else if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_charge_attack_02)
                        // {
                        //     player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_charge_attack_03, true);
                        //     player.playerCombatManager.lastAttack = player.playerCombatManager.oh_charge_attack_03;
                        // }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_charge_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_charge_attack_01;
                        }
                    }
                }
            }
        }
    }
}

