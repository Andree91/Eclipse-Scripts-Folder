using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public Renderer wallRenderer; //If you want to reset Illusionary Wall at start
        public Material illusionaryWallMaterial; //If using like in Dark Souls
        public float alpha;
        public float fadeTimer = 2.5f;
        public BoxCollider wallCollider;

        public AudioSource audioSource;
        public AudioClip illusionaryWallSound;

        //public List<IllusionaryWall> illusionaryWalls = new List<IllusionaryWall>();

        // void Start() 
        // {
        //     PlayerPrefs.SetFloat("IllusionaryWallAlpha", 1);

        //     //ReAppearIllusionaryWalls();
        // }

        void Update() 
        {
            if (wallHasBeenHit)
            {
                FadeIllusionaryWall();
            //     if (PlayerPrefs.GetFloat("IllusionaryWallAlpha") <= 0)
            //     {
            //         Destroy(gameObject);
            //     }
            }
            // else if (illusionaryWalls.Contains(this))
            // {
            //     Destroy(gameObject);
            // }
        }

        void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Weapon")
            {
                wallHasBeenHit = true;
                //FadeIllusionaryWall();
            }
        }

        // StartCoroutine(FadeIlusionaryWallCoroutine());
        // IEnumerator FadeIlusionaryWallCoroutine()
        // {
        //     yield return new WaitForEndOfFrame();
        //     FadeIllusionaryWall();
        // }

        // void OnTriggerEnter(Collider other) 
        // {
        //     if (other.tag == "Player")
        //     {
        //         wallHasBeenHit = true;
        //     }
        // }

        public void FadeIllusionaryWall()
        {
            alpha = wallRenderer.material.color.a; //alpha = illusionaryWallMaterial.color.a
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            wallRenderer.material.color = fadedWallColor; //illusionaryWallMaterial.color = fadedWallColor

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(illusionaryWallSound);
            }

            if (alpha <= 0)
            {
                //PlayerPrefs.SetFloat("IllusionaryWallAlpha", alpha);
                //illusionaryWalls.Add(this);
                Destroy(gameObject);
            }
        }

        void ReAppearIllusionaryWalls()
        {
            Color regularWallColor = new Color(255, 255, 255, 255);
            illusionaryWallMaterial.color = regularWallColor;
        }

    }
}
