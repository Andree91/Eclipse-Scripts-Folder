using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MountSandGlider : Interactable
    {
        MountSandGlider mountSandGlider;

        public Transform playerStandingPosition;
        public Transform playerSittingPosition;

        protected override void Awake()
        {
            mountSandGlider = GetComponent<MountSandGlider>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (!playerManager.isOnSandGlider)
            {
                //Rotate player towards Sand Glider
                Vector3 rotationDirection = transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                playerManager.MountSandGliderInteraction(playerStandingPosition, playerSittingPosition);

                //Destroy(mountSandGlider, 3f);
            }
            else
            {
                //Rotate player towards Sand Glider
                Vector3 rotationDirection = transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                playerManager.GetOffOnSandGliderInteraction(playerStandingPosition);
            }
        }
    }
}
