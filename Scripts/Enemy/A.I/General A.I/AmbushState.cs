using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public bool hideWeapon;
        public bool isStatic;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;
        public AttackState attackState;
        public Rigidbody enemyRigidbody;
        public GameObject enemyMesh;
        public GameObject enemyHealthBar;

        CapsuleCollider enemyCollider;

        void Awake() 
        {
            if (isStatic)
            {
                enemyRigidbody = GetComponentInParent<Rigidbody>();
                enemyCollider = GetComponentInParent<CapsuleCollider>();
            }
        }

        public override State Tick(EnemyManager enemy)
        {
            if (hideWeapon)
            {
                enemy.isStatic = isStatic;
                enemy.characterWeaponSlotManager.rightHandSlot.UnloadWeapon();
            }

            if (isSleeping && enemy.isInteracting == false)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }

            #region  Handle Target Detection

            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter != null)
                {
                    Vector3 targetDirection = targetCharacter.transform.position - enemy.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                    if (viewableAngle > enemy.minimumDetectionAngle && 
                        viewableAngle < enemy.maximumDetectionAngle)
                    {
                        enemy.currentTarget = targetCharacter;
                        if (isSleeping)
                        {
                            enemy.enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                        }
                        isSleeping = false;
                    }
                }
            }

            #endregion

            #region Handle State Change

            if (enemy.currentTarget != null)
            {
                if (hideWeapon)
                {
                    enemyCollider.enabled = true;
                    enemyMesh.SetActive(true);
                    enemy.characterWeaponSlotManager.LoadWeaponOnSlot(enemy.characterInventoryManager.rightWeapon, false);
                    return attackState;
                }
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
