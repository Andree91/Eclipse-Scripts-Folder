using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class NpcStatePatrol : State
    {
        public NpcStateIdle idleState;
        public PursueTargetStateHumanoid pursueTargetState;
        public NpcStateAgro npcStateAgro;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;
        public Transform[] patrolArea;
        public Transform currentWaypoint;
        public Transform nextWaypoint;
        int nextWaypointIndex;

        [SerializeField] float stoppingDistanceCorrection = 1.5f;

        public override State Tick(EnemyManager aiCharacter)
        {
            // #region  Handle Enemy Target Detection

            // //Searches for a potential target within the detection radius
            // Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            // for (int i = 0; i < colliders.Length; i++)
            // {
            //     CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            //     //If a potential target is found, that is not on the sam team as the A.I we proceed to the next step
            //     if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != aiCharacter.enemyStatsManager.teamIDNumeber)
            //     {
            //         Vector3 targetDirection = targetCharacter.transform.position - transform.position;
            //         float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            //         //If a potential targer is found, it has to be standing infront of the A.I's field of view
            //         if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
            //         {
            //             if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
            //             {
            //                 return this;
            //             }
            //             else
            //             {
            //                 aiCharacter.currentTarget = targetCharacter;
            //             }
            //         }
            //     }
            // }
            // #endregion

            //TODO; Have to find better solution for NPC pathfinding. Ecspecialy when player tries to get on NPC's way on purpose

            // If aiCharacter is doing somekind of action , stop all momevent and return
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //Vector3 targetDirection = currentWaypoint.position - aiCharacter.transform.position;
            float distanceFromTarget = Vector3.Distance(currentWaypoint.position, aiCharacter.transform.position);

            idleState.HandleNpcAgro(aiCharacter);

            HandleRotateTowardsTarget(aiCharacter);
            //Patrol(aiCharacter, distanceFromTarget);

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
            else if (distanceFromTarget > aiCharacter.navMeshAgent.stoppingDistance - stoppingDistanceCorrection && !aiCharacter.isTalking)
            {
                aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
                return this;
            }
            else if (aiCharacter.isTalking)
            {
                return idleState;
            }
            else
            {
                return this;
            }
            #endregion

        }

        protected State Patrol(EnemyManager aiCharacter, float distanceFromTarget)
        {
            // if (currentWaypoint == null)
            // {
            //     SetNextWaypoint();
            //     currentWaypoint = nextWaypoint;
            // }

            if (currentWaypoint != null)
            {
                //Go to next way point
                if (distanceFromTarget > aiCharacter.navMeshAgent.stoppingDistance - stoppingDistanceCorrection)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
                    return this;
                }

                if (distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance - stoppingDistanceCorrection)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    // //Invoke("SetNextWaypoint", (Random.Range(minimunTimeToWaitUntilContinuePatrol, maxTimeToWaitUntilContinuePatrol))); //SetNextWaypoint();
                    // StartCoroutine(SetNextWayPointDelay(Random.Range(minimunTimeToWaitUntilContinuePatrol, maxTimeToWaitUntilContinuePatrol)));
                    SetNextWaypoint();
                    Debug.Log("Before return to idle state");
                    
                }
            }
            else
            {
                //SetNextWaypoint();
                return idleState;
            }
            return this;
        }

        void SetNextWaypoint()
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

        IEnumerator SetNextWayPointDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetNextWaypoint();
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
