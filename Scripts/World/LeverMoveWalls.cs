using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LeverMoveWalls : Interactable
    {
        public WallMover wallMover;
        public bool isDisabled;
        public Transform playerStandingPosition;
        Animator animator;

        protected override void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            // if (isAtRightPosition)
            // {
            //     playerManager.ShowCantCurrentlyUsePopUp();
            // }

            if (!isDisabled)
            {
                //Rotate player towards Lever
                Vector3 rotationDirection = transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                //Lock his transform in front of lever
                playerManager.PullLeverInteraction(playerStandingPosition);

                //Animate Lever Moving
                animator.Play("Lever_Pull");
                isDisabled = true;

                //Move walls to the new position
                wallMover.MoveWalls();

            }
        }
    }
}
