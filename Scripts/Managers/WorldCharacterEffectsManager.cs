using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [Header("POISON")]
        public PoisonBuildUpEffect poisonBuildUpEffect;
        public PoisonedEffect poisonedEffect;
        public GameObject poisonFX;
        public AudioClip poisonSFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(instance);
        }
    }
}