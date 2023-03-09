using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LookForTheNoiseTargetHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceStateHumanoid;
        public IdleStateHumanoid idleState;
        public GoBackToStartingPositionHumanoidState goBackToStartingPositionHumanoidState;
        public PursueTargetStateHumanoid pursueTargetState;
        public NpcStateIdle npcStateIdle;
        //public RotateTowardsTargetState rotateTowardsTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public override State Tick(EnemyManager enemy)
        {
            #region  Handle Enemy Target Detection

            //Searches for a potential target within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //If a potential target is found, that is not on the sam team as the A.I we proceed to the next step
                if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != enemy.enemyStatsManager.teamIDNumeber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //If a potential targer is found, it has to be standing infront of the A.I's field of view
                    if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                    {
                        if (Physics.Linecast(enemy.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                        {
                            return this;
                        }
                        else
                        {
                            enemy.currentTarget = targetCharacter;
                        }
                    }
                }
            }
            #endregion

            //Vector3 targetDirection = currentWaypoint.position - aiCharacter.transform.position;
            float distanceFromTarget = Vector3.Distance(enemy.lastHeardPosition, enemy.transform.position);

            HandleRotateTowardsTarget(enemy);

            #region  Handle To Switching To Next State
            if (enemy.currentTarget != null)
            {
                ResetNoiseTarget(enemy);
                return pursueTargetState;
            }
            else if (distanceFromTarget <= enemy.navMeshAgent.stoppingDistance)
            {
                ResetNoiseTarget(enemy);
                return goBackToStartingPositionHumanoidState;
            }
            else if (distanceFromTarget > enemy.navMeshAgent.stoppingDistance)
            {
                enemy.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
                return this;
            }
            else
            {
                return this;
            }
            #endregion
        }

        void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.lastHeardPosition == null) { return; }

            //Rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.lastHeardPosition - enemyManager.transform.position; //Maybe delete enemymanager.?
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
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targerVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.lastHeardPosition);
                enemyManager.enemyRigidbody.velocity = targerVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed * Time.deltaTime);
            }
        }

        void ResetNoiseTarget(EnemyManager enemy)
        {
            enemy.noiseTarget = null;
        }
    }
}
