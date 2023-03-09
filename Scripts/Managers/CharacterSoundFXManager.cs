using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        public AudioSource footStepsAudio;
        CharacterManager character;
        AudioSource audioSource;

        //Attacking Grunts

        //Taking Damage Grunts

        //Taking Damage Sounds
        [Header("Taking Damage Sounds")]
        public AudioClip[] takingDamageSounds;
        // private List<AudioClip> potentialDamageSounds;
        // private AudioClip lastDamageSoundPlayed;
        //Other Idea:
        int lastSound;

        [Header("Shield Hit Sounds")]
        public AudioClip[] shieldHitSounds;

        [Header("Enviroment Hit Sounds")]
        public AudioClip[] wallHitSounds;

        [Header("Weapon Whooshes")]
        private List<AudioClip> potentialWeaponWhooshes;
        private AudioClip lastWeaponWhoosh;

        //Different Foot Steps Sounds

        [Header("Water Sounds")]
        public AudioClip waterSplashSound;

        protected virtual void Awake() 
        {
            character = GetComponent<CharacterManager>();
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void PlaySoundFX(AudioClip soundFX, float audioVolume)
        {
            audioSource.volume = audioVolume;
            audioSource.PlayOneShot(soundFX);
        }

        // public virtual void PlayRandomDamageSoundFX()
        // {
        //     potentialDamageSounds = new List<AudioClip>();

        //     foreach (var damageSound in takingDamageSounds)
        //     {
        //         //If potential damage sound has not been played before, we add it as a potential (Stops repeating damage sounds)
        //         if (damageSound != lastDamageSoundPlayed)
        //         {
        //             potentialDamageSounds.Add(damageSound);
        //         }
        //     }

        //     int randomValue = Random.Range(0, potentialDamageSounds.Count);
        //     lastDamageSoundPlayed = takingDamageSounds[randomValue];
        //     audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.2f);
        // }

        //Other Idea:
        public virtual void PlayRandomDamageSoundFX()
        {
            int randomSound = Random.Range(0, takingDamageSounds.Length);

            if (takingDamageSounds.Length > 1)
            {
                if (randomSound == lastSound)
                {
                    PlayRandomDamageSoundFX();
                }
                else
                {
                    audioSource.PlayOneShot(takingDamageSounds[randomSound], 0.2f);
                    lastSound = randomSound;
                }
            }
        }

        public virtual void PlayRandomShieldHitSoundFX()
        {
            int randomSound = Random.Range(0, shieldHitSounds.Length);

            if (shieldHitSounds.Length > 1)
            {
                if (randomSound == lastSound)
                {
                    PlayRandomShieldHitSoundFX();
                }
                else
                {
                    audioSource.PlayOneShot(shieldHitSounds[randomSound], 0.2f);
                    lastSound = randomSound;
                }
            }
        }

        public virtual void PlayRandomWallHitSoundFX()
        {
            int randomSound = Random.Range(0, wallHitSounds.Length);

            if (wallHitSounds.Length > 1)
            {
                if (randomSound == lastSound)
                {
                    PlayRandomWallHitSoundFX();
                }
                else
                {
                    audioSource.PlayOneShot(wallHitSounds[randomSound], 0.1f);
                    lastSound = randomSound;
                }
            }
        }

        public virtual void PlayRandomWeaponWhoosh()
        {
            potentialWeaponWhooshes = new List<AudioClip>();

            if (character.isUsingRightHand && character.characterInventoryManager.rightWeapon.weaponWhooshes != null)
            {
                foreach (var whooshSound in character.characterInventoryManager.rightWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }
                }

                if (character.characterInventoryManager.rightWeapon.weaponWhooshes.Length > 1)
                {
                    int randomValue =Random.Range(0, potentialWeaponWhooshes.Count);
                    lastWeaponWhoosh = character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue];
                    audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue], 0.2f);
                }
            }
            else if (character.isUsingLeftHand && character.characterInventoryManager.leftWeapon.weaponWhooshes != null)
            {
                foreach (var whooshSound in character.characterInventoryManager.leftWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }
                }

                int randomValue =Random.Range(0, potentialWeaponWhooshes.Count);
                lastWeaponWhoosh = character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue];
                audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue], 0.2f);
            }
        }

        public void PlayWaterSplashSoundFX()
        {
            audioSource.PlayOneShot(waterSplashSound, 0.05f);
        }

        public void HandleFootStepsVolume()
        {
            footStepsAudio.volume = footStepsAudio.volume / 2;
        }
    }
}
