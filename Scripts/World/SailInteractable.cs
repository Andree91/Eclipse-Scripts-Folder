using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class SailInteractable : Interactable
    {
        public Transform playerStandingPosition;
        

        public override void Interact(PlayerManager player)
        {
            if (!player.isOnShip)
            {
                //Rotate player towards Ship Wheel
                Vector3 rotationDirection = transform.position - player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 300 * Time.deltaTime);
                player.transform.rotation = targetRotation;

                player.MountShipWheelInteraction(playerStandingPosition);

                //Destroy(mountSandGlider, 3f);
            }
            else
            {
                //Rotate player towards Sand Glider
                Vector3 rotationDirection = transform.position - player.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 300 * Time.deltaTime);
                player.transform.rotation = targetRotation;

                player.GetOffOnShipWheelInteraction(playerStandingPosition);
            }
        }
    }
}
