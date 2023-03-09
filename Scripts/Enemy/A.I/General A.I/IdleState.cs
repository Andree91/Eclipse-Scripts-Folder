using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class IdleState : State
    {
        public PatrolState patrolState;
        public PursueTargetState pursueTargetState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        [SerializeField] float maxTimeToWaitUntilContinuePatrol = 6f;
        [SerializeField] float minimunTimeToWaitUntilContinuePatrol = 3f;

        float timer = 0;
        float delay = 0;

        public override State Tick(EnemyManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            
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

            #region  Handle To Switching To Next State
            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }
            else if (aiCharacter.isPatrolling)
            {
                if (delay < minimunTimeToWaitUntilContinuePatrol)
                {
                    delay = Random.Range(minimunTimeToWaitUntilContinuePatrol, maxTimeToWaitUntilContinuePatrol);
                }
                timer = timer + Time.deltaTime;
                if (delay <= timer)
                {
                    delay = 0;
                    timer = 0;
                    return patrolState;
                }
                return this;
            }

            else
            {
                return this;
            }
            #endregion
        }
    }
}
