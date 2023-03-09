using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0) { return; }

            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();
            character.characterEffectsManager.PlayWeaponFX(false);

            //If we can do jumping attack, do that, if not continue
            if (character.canJumpAttack)
            {
                character.isJumping = false;
                HandleJumpingAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.JumpingAttack;
                return;
            }

            //If we can do combo
            if (character.canDoCombo)
            {
                //character.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(character);
                character.characterCombatManager.currentAttackType = AttackType.HeavyAttack02;
                //character.inputHandler.comboFlag = false;
            }
            //If not, perform regular attack
            else
            {
                if (character.isInteracting) { return; }
                if (character.canDoCombo) { return; }

                HandleHeavyAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.HeavyAttack01;
            }
        }

        void HandleHeavyAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_01;
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_01;
                }
            }
        }

        void HandleJumpingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_jumping_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_jumping_attack_01;
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_01;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_jumping_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_jumping_attack_01;
                }
            }
        }

        void HandleHeavyWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_01, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_heavy_attack_02)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_03, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_03;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_01, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_01;
                        }
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_heavy_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_heavy_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_02;
                        }
                        // else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_heavy_attack_02)
                        // {
                        //     character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_03, true);
                        //     character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_03;
                        // }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_heavy_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_heavy_attack_01;
                        }
                    }
                }
            }
        }

    }
}
