using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AttackStateHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceState;
        public PursueTargetStateHumanoid pursueTargetState;
        public RotateTowardsTargetStateHumanoid rotateTowardsTargetState;
        public IdleStateHumanoid idleState;
        public ItemBasedAttackAction currentAttack;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemy)
        {
            if (enemy.combatStyle == AICombatStyle.SwordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if (enemy.combatStyle == AICombatStyle.Archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {
            #region Attacks

            if (enemy.isInteracting) { return this; }

            //Still some problems with the transition to the idle state, enemy keep walking on place
            if (enemy.currentTarget != null)
            {
                if (enemy.currentTarget.isDead)
                {
                    ResetStateFlags();
                    enemy.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                    enemy.currentTarget = null;
                    enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemy.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    return idleState;
                }
                }

            //float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

            RotateTowardsTargetWhileAttacking(enemy);

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                enemy.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                return pursueTargetState;
            }

            // if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget > enemyManager.maximunAttackRange)

            // {

            //     return pursueTargetState;

            // }

            // if (distanceFromTarget > enemyManager.maximunAttackRange)
            // {
            //     return pursueTargetState;
            // }

            if (willDoComboOnNextAttack && enemy.canDoCombo)
            {
                enemy.animator.runtimeAnimatorController = enemy.characterInventoryManager.rightWeapon.weaponController;
                AttackTargetWithCombo(enemy);
            }

            if (!hasPerformedAttack)
            {
                enemy.animator.runtimeAnimatorController = enemy.characterInventoryManager.rightWeapon.weaponController;
                AttackTarget(enemy);
                RollForComboChance(enemy);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; //Goes back up and perform combo
            }

            ResetStateFlags();
            enemy.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
            return rotateTowardsTargetState;

            // if (enemyManager.isInteracting && enemyManager.canDoCombo == false) 
            // { 
            //     return this; 
            // }
            // else if (enemyManager.isInteracting && enemyManager.canDoCombo)
            // {
            //     if (willDoComboOnNextAttack)
            //     {
            //         willDoComboOnNextAttack = false;
            //         enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            //     }
            // }

            // Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            // float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            // float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            // HandleRotateTowardsTarget(enemyManager);

            // if (enemyManager.isPerformingAction) 
            // {
            //     return combatStanceState;
            // }

            // if (currentAttack != null)
            // {
            //     //If we are too close to the enemy to perform current attack, we get a new attack
            //     if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            //     {
            //         return this;
            //     }
            //     //If we are close enought to attack, then lets proceed
            //     else if (distanceFromTarget <= currentAttack.maximumDistanceNeededToAttack)
            //     {
            //         //If our enemy is within our attacks viewable angle, we attack
            //         if (viewableAngle <= currentAttack.maximumAttackAngle && 
            //             viewableAngle >= currentAttack.minimumAttackAngle)
            //         {
            //             if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
            //             {
            //                 enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            //                 enemyAnimatorHandler.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            //                 enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            //                 enemyManager.isPerformingAction = true;
            //                 RollForComboChance(enemyManager);

            //                 if (currentAttack.canCombo && willDoComboOnNextAttack)
            //                 {
            //                     currentAttack = currentAttack.comboAction;
            //                     return this;
            //                 }
            //                 else
            //                 {
            //                     enemyManager.currentRecoveryTime = currentAttack.recorveryTime;
            //                     currentAttack = null;
            //                     return combatStanceState;
            //                 }

            //             }
            //         }
            //     }
            // }
            // else
            // {
            //     GetNewAttack(enemyManager);
            // }

            // return combatStanceState;
        }

        private State ProcessArcherCombatStyle(EnemyManager enemy)
        {
            if (enemy.isInteracting) { return this;}

            if (!enemy.isHoldingArrow)
            {
                ResetStateFlags();
                return combatStanceState;
            }

            if (enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return idleState;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            if (!hasPerformedAttack)
            {
                //Fire Ammo
                FireAmmo(enemy);
            }

            ResetStateFlags();
            return rotateTowardsTargetState;
        }

        void AttackTarget(EnemyManager enemy)
        {
            if (currentAttack != null)
            {
                currentAttack.PerformAttackAction(enemy);
                enemy.currentRecoveryTime = currentAttack.recorveryTime;
                hasPerformedAttack = true;
            }
        }

        void AttackTargetWithCombo(EnemyManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            willDoComboOnNextAttack = false;
            enemy.currentRecoveryTime = currentAttack.recorveryTime;
            currentAttack = null;
        }

        #endregion

        void RotateTowardsTargetWhileAttacking(EnemyManager enemyManager)
        {
            //Rotate manually
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position; //Maybe add enemymanager.transform?
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed * Time.deltaTime); //Should be * not / ?
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                // enemyManager.navMeshAgent.enabled = true;

                // enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);



                // float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
                // float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                // if (distanceFromTarget > 5) enemyManager.navMeshAgent.angularSpeed = 500f;

                // else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30) enemyManager.navMeshAgent.angularSpeed = 50f;

                // else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30) enemyManager.navMeshAgent.angularSpeed = 500f;



                // Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;

                // Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);





                // if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)

                // {

                //     enemyManager.navMeshAgent.updateRotation = false;

                //     enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,

                //     Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);

                // }

                // else

                // {

                //     enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);

                // }



                // Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                // Vector3 targerVelocity = enemyManager.enemyRigidbody.velocity;

                // enemyManager.navMeshAgent.enabled = true;
                // enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                // enemyManager.enemyRigidbody.velocity = targerVelocity;
                // enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed * Time.deltaTime);
            }
        }

        void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 101);

            if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if (currentAttack != null)
                {
                    if (currentAttack.actionCanCombo)
                    {
                        willDoComboOnNextAttack = true;
                    }
                    else
                    {
                        willDoComboOnNextAttack = false;
                        currentAttack = null;
                    }
                }
            }
        }

        void FireAmmo(EnemyManager enemy)
        {
            if (enemy.isHoldingArrow)
            {
                hasPerformedAttack = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.tap_RB_action.PerformAction(enemy);
            }
        }

        void ResetStateFlags()
        {
            willDoComboOnNextAttack = false;
            hasPerformedAttack = false;
        }
    }
}
