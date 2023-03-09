using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace AG
{
    public class NpcStateIdle : State
    {
        public PursueTargetStateHumanoid pursueTargetState;
        public CompanionStateFollowHost followHostState;
        public NpcStatePatrol patrolState;
        public NpcStateAgro npcStateAgro;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        [SerializeField] float maxTimeToWaitUntilContinuePatrol = 6f;
        [SerializeField] float minimunTimeToWaitUntilContinuePatrol = 3f;

        float timer = 0;
        float delay = 0;

        public override State Tick(EnemyManager aiCharacter)
        {
            // If aiCharacter is doing somekind of action , stop all momevent and return
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }
            
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

            HandleNpcAgro(aiCharacter);

            // if (aiCharacter.isTalking)
            // {
            //     HandleAICharacterDuringDialogue(aiCharacter);
            // }

            // if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            // {
            //     return followHostState;
            // }

            #region  Handle AI Target Detection

            //Searches for a potential target within the detection radius
            // Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

            // for (int i = 0; i < colliders.Length; i++)
            // {
            //     CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            //     //If a potential target is found, that is not on the same team as the A.I we proceed to the next step
            //     if (targetCharacter != null && targetCharacter.characterStatsManager.teamIDNumeber != aiCharacter.enemyStatsManager.teamIDNumeber && !targetCharacter.isDead)
            //     {
            //         Vector3 targetDirection = targetCharacter.transform.position - transform.position;
            //         float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            //         //If a potential targer is found, it has to be standing infront of the A.I's field of view
            //         if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
            //         {
            //             //If the A.I's potential target has an obstruction in between itself and the A.I, we don't set it as our current target
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
            #endregion

            #region  Handle To Switching To Next State
            if (aiCharacter.currentTarget != null)
            {
                return pursueTargetState;
            }
            else if (aiCharacter.isPatrolling)
            {
                if (delay == 0)
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

        public void HandleNpcAgro(EnemyManager aiCharacter)
        {
            if (aiCharacter.characterStatsManager.currentHealth <= aiCharacter.characterStatsManager.maxHealth * 0.7f || aiCharacter.hitCounter >= 3)
            {
                aiCharacter.characterStatsManager.teamIDNumeber = 1;
                aiCharacter.hitCounter = 0;
                aiCharacter.characterInventoryManager.leftWeapon = aiCharacter.characterInventoryManager.weaponsInLeftHandSlots[0];
                aiCharacter.characterInventoryManager.rightWeapon = aiCharacter.characterInventoryManager.weaponsInRightHandSlots[0];
                Debug.Log("Before loading weapons");
                aiCharacter.characterWeaponSlotManager.LoadBothWeaponsOnSlot();
                Debug.Log("After loading weapons");
                aiCharacter.dialogueInteractable.GetComponent<DialogueInteractable>().ToggleAICharacterBools();
                DialogueManager.StopConversation();
                // StandardDialogueUI dialogueCanvas = FindObjectOfType<StandardDialogueUI>();
                // dialogueCanvas.enabled = false;
                // dialogueCanvas.enabled = true;
                aiCharacter.hasAgroed = true;
                //aiCharacter.isInCombat = true;
                aiCharacter.dialogueInteractable.SetActive(false);
                if (!aiCharacter.isBeingBackStabbed)
                {
                    aiCharacter.currentTarget = FindObjectOfType<PlayerManager>();

                }
            }
        }

        
    }
}
