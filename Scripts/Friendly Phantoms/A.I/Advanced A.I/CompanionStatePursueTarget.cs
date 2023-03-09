using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CompanionStatePursueTarget : State
    {
        public CompanionStateCombatStance combatStanceStateHumanoid;
        public CompanionStateIdle idleState;
        public CompanionStateFollowHost followHostState;
        //public RotateTowardsTargetState rotateTowardsTargetState;

        public override State Tick(EnemyManager aiCharacter)
        {
            // If we are too far away from our companion, we want to return to them
            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion * 2f)
            {
                return followHostState;
            }

            if (aiCharacter.combatStyle == AICombatStyle.SwordAndShield)
            {
                if (aiCharacter.currentTarget != null)
                {
                    return ProcessSwordAndShieldCombatStyle(aiCharacter);
                }
                else
                {
                    //ResetStateFlags();
                    return idleState;
                }
            }

            else if (aiCharacter.combatStyle == AICombatStyle.Archer)
            {
                if (aiCharacter.currentTarget != null)
                {
                    return ProcessArcherCombatStyle(aiCharacter);
                }
                else
                {
                    //ResetStateFlags();
                    return idleState;
                }

            }
            else
            {
                return this;
            }
        }

        protected State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {
            // Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
            // float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

            if (enemy.currentTarget != null)
            {
                if (enemy.currentTarget.isDead)
                {
                    //ResetStateFlags();
                    enemy.currentTarget = null;
                    enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemy.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    return idleState;
                }
            }

            HandleRotateTowardsTarget(enemy);

            // if (viewableAngle > 65 || viewableAngle <= -65) Different Ideas Again...
            // {
            //     return rotateTowardsTargetState;
            // }

            if (enemy.isInteracting) { return this; }

            if (enemy.isPerformingAction)
            {
                Debug.Log("Enemy is performing action on pursuetarget state");
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }


            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
            {
                return combatStanceStateHumanoid;
            }
            else
            {
                return this;
            }
        }

        protected State ProcessArcherCombatStyle(EnemyManager enemy)
        {
            // Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
            // float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

            if (enemy.currentTarget != null)
            {
                if (enemy.currentTarget.isDead)
                {
                    //ResetStateFlags();
                    enemy.currentTarget = null;
                    enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    return idleState;
                }
            }

            HandleRotateTowardsTarget(enemy);

            // if (viewableAngle > 65 || viewableAngle <= -65) Different Ideas Again...
            // {
            //     return rotateTowardsTargetState;
            // }

            if (enemy.isInteracting) { return this; }

            if (enemy.isPerformingAction)
            {
                Debug.Log("Enemy is performing action on pursuetarget state");
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }


            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                if (!enemy.isStationaryArcher)
                {
                    enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
            {
                return combatStanceStateHumanoid;
            }
            else
            {
                return this;
            }
        }

        void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.currentTarget == null) { return; }

            if (enemyManager.currentTarget.isDead) { return; }

            //Rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position; //Maybe delete enemymanager.?
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



                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targerVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targerVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed * Time.deltaTime);
            }

        }
    }
}
