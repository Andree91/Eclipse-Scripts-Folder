using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CompanionStateFollowHost : State
    {
        public CompanionStateIdle idleState;

        // void Awake() 
        // {
        //     idleState = GetComponent<CompanionStateIdle>(); Maybe I will set this on awake
        // }

        public override State Tick(EnemyManager aiCharacter)
        {
            if (aiCharacter.currentTarget != null)
            {
                if (aiCharacter.currentTarget.isDead)
                {
                    //ResetStateFlags();
                    aiCharacter.companion = null;
                    aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    return idleState;
                }
            }

            if (aiCharacter.isInteracting) { return this; }

            if (aiCharacter.isPerformingAction)
            {
                //Debug.Log("Enemy is performing action on pursuetarget state");
                aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                aiCharacter.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }


            if (aiCharacter.distanceFromCompanion <= aiCharacter.returnDistanceFromCompanion)
            {
                aiCharacter.currentTarget = null; //HAVE TO FIND BETTER PLACE FOR CLEARING CURRENT TARGET
                return idleState;
            }
            else
            {
                return this;
            }
        }

        void HandleRotateTowardsTarget(EnemyManager aiCharacter)
        {
            if (aiCharacter.companion == null) { return; }

            if (aiCharacter.companion.isDead) { return; }

            //Rotate manually
            if (aiCharacter.isPerformingAction)
            {
                Vector3 direction = aiCharacter.companion.transform.position - aiCharacter.transform.position; //Maybe delete enemymanager.?
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed * Time.deltaTime); //Should be * not / ?
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(aiCharacter.navMeshAgent.desiredVelocity);
                Vector3 targerVelocity = aiCharacter.enemyRigidbody.velocity;

                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.navMeshAgent.SetDestination(aiCharacter.companion.transform.position);
                aiCharacter.enemyRigidbody.velocity = targerVelocity;
                aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, aiCharacter.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
