using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem.Wrappers;

namespace AG
{
    public class DialogueInteractable : Interactable
    {
        public Transform playerStandingPosition;
        public DialogueSystemTrigger dialogueSystemTrigger;
        public GameObject mouthNpc;
        Animator animator;
        EnemyManager aiCharacter;
        bool tempPatrol;
        // public bool isBottom;
        // public bool isTop;

        protected override void Awake() 
        {
            aiCharacter = GetComponentInParent<EnemyManager>();
            tempPatrol = aiCharacter.isPatrolling;
            animator = mouthNpc.GetComponent<Animator>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (!aiCharacter.isTalking && !aiCharacter.isInteracting)
            {
                //Rotate player towards ladder
                Vector3 rotationDirection = transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                //Lock his transform in front of ladder
                playerManager.TalkInteraction(playerStandingPosition, this);
                dialogueSystemTrigger.enabled = true;
                aiCharacter.isPatrolling = false;
                aiCharacter.isTalking = true;
                aiCharacter.isTurning = true;
                aiCharacter.dialogueSystemTrigger.enabled = false;
                StandardBarkUI barkUI = aiCharacter.GetComponentInChildren<StandardBarkUI>();
                barkUI.ToggleBarkUI(true);
                aiCharacter.playerTalking = playerManager;
                playerManager.npcTalking = aiCharacter;
                aiCharacter.characterAnimatorManager.PlayTargetAnimation("Turn Right", true);
                mouthNpc.GetComponent<MeshRenderer>().enabled = true;
                animator.Play("Mouth_Animation");
                animator.SetBool("isTalking", true);
                //dialogueSystemTrigger.OnUse()
            }
        }

        // void OnTriggerExit(Collider other) 
        // {
        //     if (other.gameObject.tag == "Player")
        //     {
        //         if (tempPatrol)
        //         {
        //             aiCharacter.isPatrolling = tempPatrol; // true
        //         }
        //     }
        // }

        public void ToggleAICharacterBools() // THIS IS CALLED WHEN DIALOGUE ENDS
        {
            if (tempPatrol)
            {
                aiCharacter.isPatrolling = true;
            }

            if (aiCharacter.isTalking)
            {
                aiCharacter.isTalking = false;
                aiCharacter.isTurning = false;
                mouthNpc.GetComponent<MeshRenderer>().enabled = false;
                animator.SetBool("isTalking", false);
                //gameObject.SetActive(true);
                aiCharacter.playerTalking.TogglePlayerTalkingBools();
                //dialogueSystemTrigger.maxConversationDistance = 0;
                //dialogueSystemTrigger.maxConversationDistance = 5;
                //aiCharacter.characterHead.transform.localPosition = aiCharacter.characterHeadOriginalPosition;
                //Vector3 targetPosition = Vector3.Lerp(aiCharacter.characterHead.transform.localPosition, aiCharacter.characterHeadOriginalPosition, Time.deltaTime * aiCharacter.headReturnSpeed);
                //StartCoroutine(RotateAIWhileTalking(targetPosition));
                //aiCharacter.headRig.weight = Mathf.Lerp(aiCharacter.headRig.weight, aiCharacter.rigWeight, Time.deltaTime * 2);
            }
        }

        // IEnumerator RotateAIWhileTalking(Vector3 targetPosition)
        // {
        //     float tempTime = Time.deltaTime;
        //     float timer = 0;

        //     while (timer <= tempTime + 1f)
        //     {
        //         timer += Time.deltaTime;
        //         aiCharacter.characterHead.transform.localPosition = targetPosition;
        //         aiCharacter.headRig.weight = Mathf.Lerp(aiCharacter.headRig.weight, aiCharacter.rigWeight, Time.deltaTime * 2);
        //         yield return new WaitForEndOfFrame();
        //     }
        // }

    }
}
