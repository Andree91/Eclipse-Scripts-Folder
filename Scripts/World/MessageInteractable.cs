using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MessageInteractable : Interactable
    {
        [Header("Message information")]
        [SerializeField] int messageWorldID; // This is unique  ID for this message in the game world, each message you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenRated;
        [SerializeField] string messageText= null;

        public override void Interact(PlayerManager player)
        {
            //Rotate player towards chest
            Vector3 rotationDirection = transform.position - player.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 300 * Time.deltaTime);
            player.transform.rotation = targetRotation;

            //Lock player transform
            player.ReadMessageInteraction(GetMessageText());
        }

        public string GetMessageText()
        {
            return messageText;
        }

        public void SetMessageText(string message)
        {
            messageText = message;
        }
    }
}