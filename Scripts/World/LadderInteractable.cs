using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LadderInteractable : Interactable
    {
        public Transform playerStandingPosition;
        public Collider ladderCollider;
        // public bool isBottom;
        // public bool isTop;

        public override void Interact(PlayerManager playerManager)
        {
            //Rotate player towards ladder
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            //Lock his transform in front of ladder
            playerManager.LadderInteraction(playerStandingPosition);
            ladderCollider.enabled = false;
        }

    }
}
