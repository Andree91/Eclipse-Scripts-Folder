using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        //Rigidbody playerRigidbody;

        protected override void Awake() 
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            //playerRigidbody = GetComponent<Rigidbody>();
        }

        #region  Attack Actions
        

        // void PerformLTWeaponArt(bool isTwoHanding)
        // {
        //     if (playerManager.isInteracting) { return; }

        //     if (isTwoHanding)
        //     {
        //         //if twohanding, perform righthand weapon art
        //     }
        //     else
        //     {
        //         //perform weapon art fo left hand
        //         playerAnimatorManager.PlayTargetAnimation(weapon_art, true);
        //     }

        // }


        #endregion

        #region  Defence Actions
        


        #endregion


        public override void DrainStaminaBasedOnAttack()
        {
            if (player.isUsingRightHand)
            {
                if (currentAttackType == AttackType.LightAttack01 || currentAttackType == AttackType.LightAttack02)
                {
                    player.playerStatsManager.DetuctStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.HeavyAttack01 || currentAttackType == AttackType.HeavyAttack02)
                {
                    player.playerStatsManager.DetuctStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.heavyAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.RunningAttack || currentAttackType == AttackType.JumpingAttack || currentAttackType == AttackType.PlungingAttack)
                {
                    player.playerStatsManager.DetuctStamina(player.playerInventoryManager.rightWeapon.baseStaminaCost * player.playerInventoryManager.rightWeapon.jumpingAttackStaminaMultiplier);
                }
            }
            else if (player.isUsingLeftHand)
            {
                if (currentAttackType == AttackType.LightAttack01 || currentAttackType == AttackType.LightAttack02)
                {
                    player.playerStatsManager.DetuctStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.lightAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.HeavyAttack01 || currentAttackType == AttackType.HeavyAttack02)
                {
                    player.playerStatsManager.DetuctStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.heavyAttackStaminaMultiplier);
                }
                else if (currentAttackType == AttackType.RunningAttack || currentAttackType == AttackType.JumpingAttack || currentAttackType == AttackType.PlungingAttack)
                {
                    player.playerStatsManager.DetuctStamina(player.playerInventoryManager.leftWeapon.baseStaminaCost * player.playerInventoryManager.leftWeapon.jumpingAttackStaminaMultiplier);
                }
            }
        }

        public override void AttemptToBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, float magicDamage, float lightningDamage, float holyDamage, string blockAnimation)
        {
            base.AttemptToBlock(attackingWeapon, physicalDamage, fireDamage, magicDamage, lightningDamage, holyDamage, blockAnimation);
            player.playerStatsManager.staminaBar.SetCurrentStamina(Mathf.RoundToInt(player.playerStatsManager.currentStamina));
        }

        public void AttackWhileRiding()
        {
            if (player.isRiding)
            {
                if (player.playerStatsManager.currentStamina > 0)
                {
                    if (player.isUsingLeftHand)
                    {
                        StartCoroutine(AttackCoolDown("OH_L_Light_Attack_01"));
                    }
                    else
                    {
                        StartCoroutine(AttackCoolDown("OH_R_Light_Attack_01"));
                    }
                }
            }
        }

        IEnumerator AttackCoolDown(string attackAnimation)
        {
            yield return new WaitForSeconds(1f);
            player.playerAnimatorManager.PlayTargetAnimation(attackAnimation, true);
        }
    }
}
