using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class SummonPhantomInteractable : Interactable
    {
        Animator[] animators;
        public Transform playerStandingPosition;
        public GameObject friendlyPhantom;
        public GameObject[] summonFXs;

        bool isSummoning;
        public Renderer phantomRenderer;
        public float alpha;
        public float appearTimer = 2.5f;
        public AudioSource audioSource;
        public AudioClip illusionaryWallSound;


        // void Awake()
        // {
        //     animators = GetComponentsInChildren<Animator>();
        // }

        void Update() 
        {
            if (isSummoning)
            {
                FadeAppearPhantom();
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (playerManager.isInteracting) { return; }
            //Rotate player towards door handle
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;
            friendlyPhantom.gameObject.SetActive(true);
            playerManager.SummonPhantomInteraction(playerStandingPosition);
            isSummoning = true;

            //TODO Play VFX while slowly appering to the world
            StartCoroutine(StartSummonFXs());

            IEnumerator StartSummonFXs()
            {
                for (int i = 0; i < summonFXs.Length; i++)
                {
                    summonFXs[i].gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                }
            }

            //Open the lid and animate player to open it
            // for (int i = 0; i < animators.Length; i++)
            // {
            //     animators[i].Play("Door Open");
            // }

           Destroy(gameObject, 5f);
           GetComponent<SphereCollider>().enabled = false;
        }

        public void FadeAppearPhantom()
        {
            alpha = phantomRenderer.material.color.a; //alpha = illusionaryWallMaterial.color.a
            alpha = alpha + Time.deltaTime / appearTimer;
            Color phantomColor = new Color(1, 1, 1, alpha);
            phantomRenderer.material.color = phantomColor; //illusionaryWallMaterial.color = fadedWallColor

            if (alpha >= 0.8f)
            {
                isSummoning = false;
            }

            audioSource.PlayOneShot(illusionaryWallSound);
        }
    }
}
