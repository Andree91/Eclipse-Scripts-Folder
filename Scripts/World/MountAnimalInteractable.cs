using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MountAnimalInteractable : Interactable
    {
        public Transform playerStandingPosition;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            //Rotate player towards animal
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            //Lock his transform in front of animal
            playerManager.MountToAnimalInteraction();
        }

        public void PetThisAnimal(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            //Rotate player towards animal
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            //Lock his transform in front of animal
            playerManager.PetAnimal();
        }
    }
}
