using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PatrolStateHumanoid : State
    {
        [SerializeField] IdleStateHumanoid idleState;
        [SerializeField] TurnAroundTowardsSoundHumanoidAI turnAroundTowardsSoundHumanoid;
        [SerializeField] PursueTargetStateHumanoid pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public bool patrolComplete;
        public bool repeatPatrol;
        public bool hasRandomPatrolRoute;

        // How long before next patrol
        [Header("Patrol Rest Time")]
        [SerializeField] float endOfPatrolRestTime;
        [SerializeField] float endOfPatrolTimer;

        [Header("Patrol Position")]
        [SerializeField] int patrolDestinationIndex;
        [SerializeField] int nextPatrolDestinationIndex; // Used only if random Patrol Route
        [SerializeField] bool hasPatrolDestination;
        public Transform currentPatrolDestination;
        [SerializeField] float distanceFromCurrentPatrolPoint;
        [SerializeField] List<Transform> listOfPatrolDestinations = new List<Transform>();

        public override State Tick(EnemyManager aiCharacter)
        {
            SearchForTargerWhilePatrolling(aiCharacter);

            // If aiCharacter is doing somekind of action , stop all momevent and return
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this; 
            }

            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }

            if (aiCharacter.noiseTarget != null)
            {
                return turnAroundTowardsSoundHumanoid;
            }

            // If we completed our patrol and want to repeat patrol route, we do so
            if (patrolComplete && repeatPatrol)
            {
                // We count up our patrol rest time and reset all our patrol flags
                if (endOfPatrolRestTime > endOfPatrolTimer)
                {
                    aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                    endOfPatrolTimer += Time.deltaTime;
                    return this;
                }
                else if (endOfPatrolTimer >= endOfPatrolRestTime)
                {
                    patrolDestinationIndex = -1;
                    hasPatrolDestination = false;
                    currentPatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if (patrolComplete && !repeatPatrol)
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                // aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (hasPatrolDestination)
            {
                if (currentPatrolDestination != null)
                {
                    distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.position);

                    if (distanceFromCurrentPatrolPoint > 1f)
                    {
                        aiCharacter.navMeshAgent.enabled = true;
                        aiCharacter.navMeshAgent.destination = currentPatrolDestination.position;
                        Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
                        aiCharacter.transform.rotation = targetRotation;
                        aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                    }
                    else
                    {
                        currentPatrolDestination = null;
                        hasPatrolDestination = false;
                    }
                }
            }
            else if (!hasPatrolDestination)
            {
                if (!hasRandomPatrolRoute)
                {
                    patrolDestinationIndex = patrolDestinationIndex + 1;
                }
                else
                {
                    // Re roll if next patrol index is same as current patrol index
                    if (patrolDestinationIndex == nextPatrolDestinationIndex)
                    {
                        nextPatrolDestinationIndex = Random.Range(0, listOfPatrolDestinations.Count);
                        patrolDestinationIndex = nextPatrolDestinationIndex;
                        nextPatrolDestinationIndex = -1;
                    }
                    else
                    {
                        nextPatrolDestinationIndex = Random.Range(0, listOfPatrolDestinations.Count);
                        patrolDestinationIndex = nextPatrolDestinationIndex;
                        nextPatrolDestinationIndex = -1;
                    }
                }

                if (patrolDestinationIndex > listOfPatrolDestinations.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }

                currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
                hasPatrolDestination = true;
            }

            return this;
        }

        void SearchForTargerWhilePatrolling(EnemyManager aiCharacter)
        {
            #region  Handle Enemy Target Detection

            //Searches for a potential target within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //If a potential target is found, that is not on the same team as the A.I we proceed to the next step
                if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != aiCharacter.enemyStatsManager.teamIDNumeber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //If a potential targer is found, it has to be standing infront of the A.I's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                    {
                        //If the A.I's potential target has an obstruction in between itself and the A.I, we don't set it as our current target
                        if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
                        {
                            return;
                        }
                        else
                        {
                            aiCharacter.currentTarget = targetCharacter;
                        }
                    }
                    else if (Vector3.Distance(aiCharacter.transform.position, targetCharacter.transform.position) < aiCharacter.noiseDetectionRadius)
                    {
                        PlayerManager player = targetCharacter as PlayerManager;
                        if (!targetCharacter.isCrouching && player.inputHandler.moveAmount > 0.5f)
                        {
                            aiCharacter.noiseTarget = targetCharacter;
                            aiCharacter.lastHeardPosition = targetCharacter.transform.position;
                        }
                    }
                }
            }
            #endregion

            #region  Handle To Switching To Next State
            if (aiCharacter.currentTarget != null)
            {
                return;
            }
            else if (aiCharacter.noiseTarget != null)
            {
                return;
            }
            else
            {
                return;
            }
            #endregion
        }
    }
}
