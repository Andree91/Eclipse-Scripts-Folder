using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInVulnerableBool = "isInVulnerable";
        public bool isInVulnerableStatus = false;

        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string isFiringSpellBool = "isFiringSpell";
        public bool isFiringSpellStatus = false;

        public string isRotatingWithRootMotionBool = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public string isMirroredbool = "isMirrored";
        public bool isMirroredStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterManager character = animator.GetComponent<CharacterManager>();

            character.isUsingLeftHand = false;
            character.isUsingRightHand = false;
            character.isAttacking = false;
            character.isBeingBackStabbed = false;
            character.isBeingRiposted = false;
            character.isPerformingBackstab = false;
            character.isPerformingRiposte = false;
            character.canBeParried = false;
            character.canBeRiposted = false;
            character.characterAnimatorManager.canUseConsumeItem = true;
            character.isBlocking = false;
            character.isDodging = false;
            character.isInVulnerable = false;

            // After the damage animation ends, reset characters previous poise damage
            character.characterCombatManager.previousPoiseDamageTaken = 0;

            PlayerManager player = character as PlayerManager;
            if (player != null)
            {
                player.uIManager.usingThroughInventory = false;
            }
            
            if (character.isUsingRightHand)
            {
                character.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
            }

            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
            animator.SetBool(isRotatingWithRootMotionBool, isRotatingWithRootMotionStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(isInVulnerableBool, isInVulnerableStatus);
            animator.SetBool(isMirroredbool, isMirroredStatus);
        }

    }
}
