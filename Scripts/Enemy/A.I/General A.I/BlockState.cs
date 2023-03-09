using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class BlockState : State
    {
        public LayerMask detectionLayer;
        public CharacterManager character;

        public override State Tick(EnemyManager enemy)
        {
            #region  Handle Enemy Target Detection

            Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != enemy.enemyStatsManager.teamIDNumeber)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                    {
                        enemy.currentTarget = targetCharacter;
                    }
                }
            }
            #endregion

            character.isBlocking = true;
            character.isUsingLeftHand = true;
            if (character.isUsingLeftHand)
            {
                character.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapons();
            }
            character.isUsingLeftHand = false;
            character.characterAnimatorManager.PlayTargetAnimation("Block Loop", true);

           return this;
        }
    }
}
