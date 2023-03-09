using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class BonfireInteractable : Interactable
    {
        //Location of the Bonfire (For teleporting) (Not sure if I'm going to allow teleport at all...)
        [Header("Bonfire Teleport Transform")]
        [SerializeField] Transform bonfireTeleportTransform;

        [Header("Activation Status")]
        public bool hasBeenActivated = false;

        //Bonfire Unique ID (For saving which Bonfires you have activated)

        [Header("Bonfire FX")]
        public ParticleSystem activationFX;
        public ParticleSystem fireFX;
        public AudioClip bonfireActivationSoundFX;

        AudioSource audioSource;

        protected override void Awake() 
        {
            //If Bonfire Has Already Activated, Play "Fire FX" When Bonfire Is Loaded To Scene
            if (hasBeenActivated)
            {
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                interactableText = "Rest";
            }
            else
            {
                interactableText = "Light Bonfire";
            }

            audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager player)
        {
            Debug.Log("BONFIRE INTERACTED");

            if (hasBeenActivated)
            {
                //Open Bonfire Menu
                base.Interact(player);
                //player.playerEffectsManager.ResetAllBuildUpEffects();
                player.uIManager.levelUpWindow.SetActive(true);
                player.SitAtCampFireInteraction(bonfireTeleportTransform);
            }
            else
            {
                //Activate Bonfire
                player.playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
                player.uIManager.ActivateBonfirePopUp();
                hasBeenActivated = true;
                interactableText = "Rest";
                activationFX.gameObject.SetActive(true);
                activationFX.Play();
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                audioSource.PlayOneShot(bonfireActivationSoundFX, 0.4f);
            }
        }
    }
}
