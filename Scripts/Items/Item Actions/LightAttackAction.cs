using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0) { return; }

            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();
            character.characterEffectsManager.PlayWeaponFX(false);

            if (character.isRiding)
            {
                HandleRidingAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.LightAttack01;
                return;
            }

            //If we can do running attack, do that, if not continue
            if (character.isSprinting)
            {
                HandleRunningAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.RunningAttack;
                return;
            }

            if (character.isDodging)
            {
                Debug.Log("Here should be backstep attack");
                HandleDodgingAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.LightAttack01;
                return;
            }

            //If we can do combo
            if (character.canDoCombo)
            {
                //character.inputHandler.comboFlag = true;
                HandleLightWeaponCombo(character);
                character.characterCombatManager.currentAttackType = AttackType.LightAttack02;
                //character.inputHandler.comboFlag = false;
            }
            //If not, perform regular attack
            else
            {
                if (character.isInteracting) { return; }
                if (character.canDoCombo) { return; }

                HandleLightAttack(character);
                character.characterCombatManager.currentAttackType = AttackType.LightAttack01;
            }
        }

        void HandleLightAttack(CharacterManager character)
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
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
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
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
                }
            }
        }

        void HandleRunningAttack(CharacterManager character)
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
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_running_attack_01, true, false, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_running_attack_01;
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
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_running_attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_running_attack_01;
                }
            }
        }

        void HandleDodgingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_back_step_attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_back_step_attack_01;
            }
            else if (character.isUsingRightHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_back_step_attack_01, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_back_step_attack_01;
            }
        }

        void HandleRidingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_L_light_riding_attack_01, false);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_L_light_riding_attack_01;
            }
            else if (character.isUsingRightHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_R_light_riding_attack_01, false);
                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_R_light_riding_attack_01;
            }
        }

        void HandleLightWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_01, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_02)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_03, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_03;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_01, true, false, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
                        }
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_light_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_light_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_light_attack_02)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_03, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_03;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_heavy_attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_02;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_light_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_light_attack_01;
                        }
                    }
                }  
            }
        }
        
    }
}
