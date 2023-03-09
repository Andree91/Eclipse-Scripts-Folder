using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TurnAroundTowardsSoundHumanoidAI : State
    {
        public CombatStanceStateHumanoid combatStanceState;
        public LookForTheNoiseTargetHumanoid lookForTheNoiseTarget;

        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);

            // Vector3 noiseTargetDirection = enemy.noiseTarget.transform.position - enemy.transform.position;
            // float hearableAngle = Vector3.SignedAngle(noiseTargetDirection, enemy.transform.forward, Vector3.up);

            if (enemy.isInteracting) { return this; } //When entering the state we will still be interacting from the attack animation, so we pause until it has finished

            if (enemy.hearableAngle >= 100 && enemy.hearableAngle <= 180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return lookForTheNoiseTarget;
            }
            else if (enemy.hearableAngle <= -101 && enemy.hearableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return lookForTheNoiseTarget;
            }
            else if (enemy.hearableAngle <= -45 && enemy.hearableAngle >= -100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return lookForTheNoiseTarget;
            }
            else if (enemy.hearableAngle >= 45 && enemy.hearableAngle <= 100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return lookForTheNoiseTarget;
            }

            return lookForTheNoiseTarget;
        }
    }
}
