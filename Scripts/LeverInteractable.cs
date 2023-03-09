using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LeverInteractable : Interactable
    {
        public Elevator elevator;
        public bool isAtRightPosition;
        public bool isDisabled;
        Animator animator;

        public Transform playerStandingPosition;

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

            if (!isDisabled && !isAtRightPosition)
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

                //Moves Elevator to the right position
                //if (!isAtRightPosition)
                {
                    elevator.MoveElevator();
                    isAtRightPosition = true;
                }
            }
        }
    }
}
