using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PatrolState : State
    {
        public IdleState idleState;
        public PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;
        public bool hasRandomPatrolRoute;
        public Transform[] patrolArea;
        public Transform currentWaypoint;
        public Transform nextWaypoint;
        int nextWaypointIndex;

        [SerializeField] float stoppingDistanceCorrection = 1.5f;

        public override State Tick(EnemyManager aiCharacter)
        {
            #region  Handle Enemy Target Detection

            //Searches for a potential target within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //If a potential target is found, that is not on the sam team as the A.I we proceed to the next step
                if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != aiCharacter.enemyStatsManager.teamIDNumeber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //If a potential targer is found, it has to be standing infront of the A.I's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                    {
                        if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                        {
                            return this;
                        }
                        else
                        {
                            aiCharacter.currentTarget = targetCharacter;
                        }
                    }
                }
            }
            #endregion

            //Vector3 targetDirection = currentWaypoint.position - aiCharacter.transform.position;
            float distanceFromTarget = Vector3.Distance(currentWaypoint.position, aiCharacter.transform.position);

            HandleRotateTowardsTarget(aiCharacter);

            #region  Handle To Switching To Next State
            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }
            else if (distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance - stoppingDistanceCorrection)
            {
                SetNextWaypoint();
                return idleState;
            }
            else if (distanceFromTarget > aiCharacter.navMeshAgent.stoppingDistance - stoppingDistanceCorrection)
            {
                aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
                return this;
            }
            else
            {
                return this;
            }
            #endregion

            // void Patrol(EnemyManager aiCharacter)
            // {
            //     // if (currentWaypoint == null)
            //     // {
            //     //     SetNextWaypoint();
            //     //     currentWaypoint = nextWaypoint;
            //     // }

            //     if (currentWaypoint != null)
            //     {
            //         //Go to next way point
            //         if (distanceFromTarget > aiCharacter.navMeshAgent.stoppingDistance)
            //         {
            //             aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
            //         }

            //         if (distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
            //         {
            //             SetNextWaypoint();
            //         }
                    
            //     }
            //     else
            //     {
            //         SetNextWaypoint();
            //     }

            // }

            void SetNextWaypoint()
            {
                if (hasRandomPatrolRoute)
                {
                    if (nextWaypoint != null)
                    {
                        if (currentWaypoint == nextWaypoint)
                        {
                            nextWaypointIndex = Random.Range(0, patrolArea.Length);
                            nextWaypoint = patrolArea[nextWaypointIndex];
                            currentWaypoint = nextWaypoint;
                            nextWaypoint = null;
                        }
                    }
                    else
                    {
                        nextWaypointIndex = Random.Range(0, patrolArea.Length);
                        nextWaypoint = patrolArea[nextWaypointIndex];
                        currentWaypoint = nextWaypoint;
                        nextWaypoint = null;
                    }
                }
                else
                {
                    nextWaypointIndex = nextWaypointIndex + 1;

                    if (nextWaypointIndex > patrolArea.Length - 1)
                    {
                        nextWaypointIndex = 0;
                        nextWaypoint = patrolArea[nextWaypointIndex];
                        currentWaypoint = nextWaypoint;
                        nextWaypoint = null;
                    }
                    else
                    {
                        nextWaypoint = patrolArea[nextWaypointIndex];
                        currentWaypoint = nextWaypoint;
                        nextWaypoint = null;
                    }
                        
                }
            }

            void HandleRotateTowardsTarget(EnemyManager aiCharacter)
            {
                //Rotate with pathfinding (navmesh)

                Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
                Vector3 targerVelocity = aiCharacter.enemyRigidbody.velocity;

                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.navMeshAgent.SetDestination(currentWaypoint.position);
                aiCharacter.enemyRigidbody.velocity = targerVelocity;
                aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
