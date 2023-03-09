using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CompanionStateIdle : State
    {
        public CompanionStatePursueTarget pursueTargetState;
        public CompanionStateFollowHost followHostState;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        public override State Tick(EnemyManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return followHostState;
            }

            #region  Handle AI Target Detection

            //Searches for a potential target within the detection radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                //If a potential target is found, that is not on the same team as the A.I we proceed to the next step
                if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != aiCharacter.enemyStatsManager.teamIDNumeber && !targetCharacter.isDead)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //If a potential targer is found, it has to be standing infront of the A.I's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                    {
                        //If the A.I's potential target has an obstruction in between itself and the A.I, we don't set it as our current target
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
            else
            {
                return this;
            }
            #endregion
        }
    }
}
