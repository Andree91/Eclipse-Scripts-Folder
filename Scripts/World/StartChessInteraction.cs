using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class StartChessInteraction : Interactable
    {
        [SerializeField] GameManager gameManager;
        [SerializeField] Transform playerStandingPosition;

        
        public override void Interact(PlayerManager playerManager)
        {
            //Rotate player towards chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            //Lock player transform
            playerManager.StartChessInteraction(playerStandingPosition);
            gameManager.LoadNewScene("Chess Game");
        }
    }
}