using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        //public RotateTowardsTargetState rotateTowardsTargetState;

        public override State Tick(EnemyManager enemy)
        {
            // Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
            // float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

            HandleRotateTowardsTarget(enemy);

            // if (viewableAngle > 65 || viewableAngle <= -65) Different Ideas Again...
            // {
            //     return rotateTowardsTargetState;
            // }

            if (enemy.isInteracting) { return this; }

            if (enemy.isPerformingAction) 
            {
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this; 
            }


            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                if (!enemy.isStatic)
                {
                    enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
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
