using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public EnemyAttackAction currentAttack;
        public AmbushState ambushState;

        public bool isStatic;
        [SerializeField] float timeBetweenAttacks = 3f;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemy)
        {
            #region Attacks

            //float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

            RotateTowardsTargetWhileAttacking(enemy);

            if (isStatic)
            {
                ambushState.enemyRigidbody.isKinematic = true;
                //enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                enemy.UpdateWhichHandCharacterIsUsing(true);
                enemy.isTwoHanding = true;
                enemy.enemyAnimatorManager.PlayTargetAnimation("TH_Heavy_Attack_02", true);
                enemy.enemyAnimatorManager.PlayWeaponTrailFX();
                enemy.currentRecoveryTime = timeBetweenAttacks;
                hasPerformedAttack = true;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                if (isStatic)
                {
                    ResetStateFlags();
                    return ambushState;
                }
                else
                {
                    ResetStateFlags();
                    return pursueTargetState;
                }
                
            }

            // if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget > enemyManager.maximunAttackRange)

            // {

            //     return pursueTargetState;

            // }

            // if (distanceFromTarget > enemyManager.maximunAttackRange)
            // {
            //     return pursueTargetState;
            // }

            if (enemy.canBeRiposted) 
            {
                ResetStateFlags();
                return combatStanceState;
            }

            if (willDoComboOnNextAttack && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemy);
                RollForComboChance(enemy);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; //Goes back up and perform combo
            }

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

        void AttackTarget(EnemyManager enemy)
        {
            // enemyAnimatorManager.animator.SetBool("isUsingRightHand", currentAttack.isRightHandAction);
            // enemyAnimatorManager.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandAction);
            enemy.isUsingRightHand = currentAttack.isRightHandAction;
            enemy.isUsingLeftHand = !currentAttack.isRightHandAction;
            enemy.UpdateWhichHandCharacterIsUsing(true);
            enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemy.enemyAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recorveryTime;
            hasPerformedAttack = true;
        }

        void AttackTargetWithCombo(EnemyManager enemy)
        {
            // enemyAnimatorManager.animator.SetBool("isUsingRightHand", currentAttack.isRightHandAction);
            // enemyAnimatorManager.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandAction);
            enemy.isUsingRightHand = currentAttack.isRightHandAction;
            enemy.isUsingLeftHand = !currentAttack.isRightHandAction;
            enemy.UpdateWhichHandCharacterIsUsing(true);
            willDoComboOnNextAttack = false;
            enemy.enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemy.enemyAnimatorManager.PlayWeaponTrailFX();
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
                if (currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }

        void ResetStateFlags()
        {
            willDoComboOnNextAttack = false;
            hasPerformedAttack = false;
        }

    }
}
