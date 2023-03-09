using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        public CharacterManager character;

        [Header("Current FX")]
        public GameObject instantiatedFXModel;

        // Instant Effects (Taking damage and build up)

        // Static Effects (Effects from Amulets, some armors and weapons)
        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticCharacterEffects;

        // Status Effects/Timed Effects (Effects from weapon buffs, consumables, potions, spells)
        [Header("Timed Effects")]
        public List<CharacterEffect> timedEffects;
        [SerializeField] float effectTickTimer = 0;
        [SerializeField] public float buildUpDecayAmount = 1f;
        [SerializeField] public float effectDecayAmount = 1f;

        [Header("Timed Effect Visual FX")]
        public List<GameObject> timedEffectParticles;

        [Header("Damage FX")]
        public GameObject bloodSplatterFX;
        public GameObject dustCloudFX;

        [Header("Weapon Managers")]
        public WeaponManager rightWeaponManager;
        public WeaponManager leftWeaponManager;

        // FOR ENEMY FOR NOW, DELETE LATER
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;

        [Header("Weapon Buffs")]
        public WeaponBuffEffect rightWeaponBuffEffect;
        public WeaponBuffEffect leftWeaponBuffEffect;
        // float timerBuff;
        // [SerializeField] float buffActiveTime = 30f;

        [Header("Poison")]
        // public GameObject defaultPoisonParticleFX;
        // public GameObject currentPoisonParticleFX;
        public Transform buildUpTransform; //The location where build up particles FX Instantiate

        // public float defaultPoisonAmount = 100; //The default amount of poison a player has to process once they become poisened
        // public float poisonTimer = 2; //The amount of time between enach damage ticks
        // public int poisonDamage = 1;
        // float timerPoison;

        [Header("Water")]
        public GameObject defaultWaterParticleFX;
        public GameObject defaultUnderWaterParticleFX;
        public GameObject defaultWaterTrailParticleFX;
        public GameObject currentWaterParticleFX;
        public GameObject currentUnderWaterParticleFX;
        public Transform waterFXTransform;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        // JUST FOR TESTING
        protected virtual void Start() 
        {
            // JUST FOR TESTING
            //CheckForStaticEffects();
        }

        public virtual void ProcessAllTimedEffects()
        {
            effectTickTimer += Time.deltaTime;

            if (effectTickTimer >= 1)
            {
                effectTickTimer = 0;
                ProcessWeaponBuffs();

                // Pricess all active effects over game time
                for (int i = timedEffects.Count - 1; i > -1; i--)
                {
                    timedEffects[i].ProcessEffect(character);
                }

                // Decays build up efffects over game time
                ProcessBuildUpDecay();
            }
        }

        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            StaticCharacterEffect staticEffect;

            // Check the list so we don't add a dublicate effect
            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] != null)
                {
                    if (staticCharacterEffects[i].effectID == effect.effectID)
                    {
                        staticEffect = staticCharacterEffects[i];
                        // We remove the actual effect from our character
                        staticEffect.RemoveStaticEffect(character);
                        // We then remove the effect from our active effects list
                        staticCharacterEffects.Remove(staticEffect);
                    }
                }
            }

            // We add the effect to our list of active static effects
            staticCharacterEffects.Add(effect);
            // We add the actual effect to our character
            effect.AddStaticEffect(character);

            // Check the list for NULL items and remove them
            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] == null)
                {
                    staticCharacterEffects.RemoveAt(i);
                }
            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            StaticCharacterEffect staticEffect;

            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] != null)
                {
                    if (staticCharacterEffects[i].effectID == effectID)
                    {
                        staticEffect = staticCharacterEffects[i];
                        // We remove the actual effect from our character
                        staticEffect.RemoveStaticEffect(character);
                        // We then remove the effect from our active effects list
                        staticCharacterEffects.Remove(staticEffect);
                    }
                }
            }



            // Check the list for NULL items and remove them
            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] == null)
                {
                    staticCharacterEffects.RemoveAt(i);
                }
            }

        }

        public virtual int GetStaticEffectsCount()
        {
            return staticCharacterEffects.Count;
        }

        public virtual void CheckForStaticEffects()
        {
            foreach (var effect in staticCharacterEffects)
            {
                effect.AddStaticEffect(character);
            }
        }

        public void ProcessWeaponBuffs()
        {
            if (rightWeaponBuffEffect != null)
            {
                rightWeaponBuffEffect.ProcessEffect(character);
            }
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if (!isLeft)
            {
                //Play fx on right weapon
                if (rightWeaponManager != null)
                {
                    rightWeaponManager.PlayWeaponTrailFX();
                }
            }
            else
            {
                if (leftWeaponManager != null)
                {
                    leftWeaponManager.PlayWeaponTrailFX();
                }
            }
        }

        // public virtual void PlayWeaponFX(bool isLeft)
        // {
        //     if (!isLeft)
        //     {
        //         //Play fx on right weapon
        //         if (rightWeaponFX != null)
        //         {
        //             if (!character.hasWeaponBuff)
        //             {
        //                 rightWeaponFX.PlayWeaponFX();
        //             }
        //             // else if (character.characterInventoryManager.rightWeapon.weaponBuffType == DamageType.Fire)
        //             // {
        //             //     rightWeaponFX.PlayFireWeaponTrailFX();
        //             // }
                    
        //         }
        //     }
        //     else
        //     {
        //         if (leftWeaponFX != null)
        //         {
        //             if (!character.hasWeaponBuff)
        //             {
        //                 leftWeaponFX.PlayWeaponFX();
        //             }
        //         //     else if (character.characterInventoryManager.leftWeapon.weaponBuffType == DamageType.Fire)
        //         //     {
        //         //         leftWeaponFX.PlayFireWeaponTrailFX();
        //         //     }
        //         }
        //     }
        // }

        public virtual void PlayWeaponSparkFX(bool isLeft)
        {
            if (!isLeft)
            {
                if (rightWeaponManager != null)
                {
                    rightWeaponManager.PlayWeaponSparkFX();
                }
            }
            else
            {
                if (leftWeaponManager != null)
                {
                    leftWeaponManager.PlayWeaponSparkFX();
                }
            }
        }

        // public virtual void PlayFireWeaponFX(bool isLeft)
        // {
        //     if (!isLeft)
        //     {
        //         //Play Fire Trail fx on right weapon
        //         if (rightWeaponManager != null)
        //         {
        //             rightWeaponManager.PlayFireWeaponTrailFX();
        //         }
        //     }
        //     else
        //     {
        //         if (leftWeaponManager != null)
        //         {
        //             leftWeaponManager.PlayFireWeaponTrailFX();
        //         }
        //     }
        // }

        public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
        {
            if (bloodSplatterLocation == null) { return; }
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
        }

        public virtual void PlayWaterTrailFX(Transform trailLocation)
        {
            currentWaterParticleFX = Instantiate(defaultWaterTrailParticleFX, trailLocation);
        }

        public virtual void PlayUnderWaterBubbleFX(Transform bubbleLocation)
        {
            currentUnderWaterParticleFX = Instantiate(defaultUnderWaterParticleFX, bubbleLocation);
        }

        public virtual void PlayDustCloudFX(Transform dustLocation)
        {
            GameObject dust = Instantiate(dustCloudFX, dustLocation);
        }

        // public virtual void ActivateWeaponBuffFX(bool isLeft)
        // {
        //     if (!isLeft)
        //     {
        //         if (rightWeaponFX != null)
        //         {
        //             if (character.characterInventoryManager.rightWeapon.weaponBuffType == DamageType.Fire)
        //             {
        //                 rightWeaponFX.ActivateWeaponFireBuffFX();
        //                 character.characterInventoryManager.rightWeapon.fireDamage += 25;
        //                 Debug.Log("Now fire damage should be going up");
        //             }
        //             else if (character.characterInventoryManager.rightWeapon.weaponBuffType == DamageType.Lightning)
        //             {
        //                 rightWeaponFX.ActivateWeaponLightningBuffFX();
        //                 character.characterInventoryManager.rightWeapon.lightingDamage += 25;
        //             }
        //         }
        //         character.characterWeaponSlotManager.LoadRightHandWeaponDamageCollider();
                
        //     }
        //     else
        //     {
        //         if (leftWeaponFX != null)
        //         {
        //             if (character.characterInventoryManager.leftWeapon.weaponBuffType == DamageType.Fire)
        //             {
        //                 leftWeaponFX.ActivateWeaponFireBuffFX();
        //                 character.characterInventoryManager.leftWeapon.fireDamage += 25;
        //             }
        //             else if (character.characterInventoryManager.leftWeapon.weaponBuffType == DamageType.Lightning)
        //             {
        //                 leftWeaponFX.ActivateWeaponLightningBuffFX();
        //                 character.characterInventoryManager.leftWeapon.lightingDamage += 25;
        //             }
        //         }
        //         character.characterWeaponSlotManager.LoadLeftHandWeaponDamageCollider();
        //     }

        //     character.hasWeaponBuff = true;
        // }

        // public virtual void DeActivateWeaponBuffFX(bool isLeft)
        // {
        //     if (!isLeft)
        //     {
        //         if (rightWeaponFX != null)
        //         {
        //             if (character.characterInventoryManager.rightWeapon.weaponBuffType == DamageType.Fire)
        //             {
        //                 rightWeaponFX.DeActivateWeaponFireBuffFX();
        //                 character.characterInventoryManager.rightWeapon.fireDamage -= 25;
        //             }
        //             else if (character.characterInventoryManager.rightWeapon.weaponBuffType == DamageType.Lightning)
        //             {
        //                 rightWeaponFX.DeActivateWeaponLightningBuffFX();
        //                 character.characterInventoryManager.rightWeapon.lightingDamage -= 25;
        //             }
        //         }
        //         character.characterWeaponSlotManager.LoadRightHandWeaponDamageCollider();
        //     }
        //     else
        //     {
        //         if (leftWeaponFX != null)
        //         {
        //             if (character.characterInventoryManager.leftWeapon.weaponBuffType == DamageType.Fire)
        //             {
        //                 leftWeaponFX.DeActivateWeaponFireBuffFX();
        //                 character.characterInventoryManager.leftWeapon.fireDamage -= 25;
        //             }
        //             else if (character.characterInventoryManager.leftWeapon.weaponBuffType == DamageType.Lightning)
        //             {
        //                 leftWeaponFX.DeActivateWeaponLightningBuffFX();
        //                 character.characterInventoryManager.leftWeapon.lightingDamage -= 25;
        //             }
        //         }
        //         character.characterWeaponSlotManager.LoadLeftHandWeaponDamageCollider();
        //     }

        //     character.hasWeaponBuff = false;
        // }

        // public virtual void HandleAllBuildUpEffects()
        // {
        //     if (character.isDead) 
        //     {
        //         ResetAllBuildUpEffects();
        //         return;
        //     }

        //     HandlePoisonBuildUp();
        //     HandleIsPoisonedEffect();
        // }

        // public virtual void HandleAllWeaponBuffs()
        // {
        //     if (character.isDead)
        //     {
        //         ResetAllWeaponBuffs();
        //         return;
        //     }

        //     HandleConsumableItemWeaponBuff();
        // }

        // public virtual void ResetAllBuildUpEffects()
        // {
        //     //Poison Effects
        //     isPoisoned = false;
        //     poisonAmount = defaultPoisonAmount;
        //     poisonBuildUp = 0;
        //     Destroy(currentPoisonParticleFX);
        // }

        // protected virtual void HandlePoisonBuildUp()
        // {
        //     if (isPoisoned) { return; }

        //     if (poisonBuildUp > 0 && poisonBuildUp < 100)
        //     {
        //         poisonBuildUp = poisonBuildUp - 1 * Time.deltaTime;
        //         character.isInCombat = true;
        //     }
        //     else if (poisonBuildUp >= 100)
        //     {
        //         isPoisoned = true;
        //         poisonBuildUp = 0;

        //         if (buildUpTransform != null)
        //         {
        //             currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
        //         }
        //         else
        //         {
        //             currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, character.transform);
        //         }
        //     }
        // }

        // protected virtual void HandleIsPoisonedEffect()
        // {
        //     if (isPoisoned)
        //     {
        //         character.isInCombat = true;
        //         if (poisonAmount > 0)
        //         {
        //             timerPoison += Time.deltaTime;

        //             //Damage player over time
        //             if (timerPoison >= poisonTimer)
        //             {
        //                 character.characterStatsManager.TakePoisonDamage(poisonDamage);
        //                 timerPoison = 0;
        //             }

        //             //Play poisoned VF on player/Enemy
        //             poisonAmount = poisonAmount - 1 * Time.deltaTime;
        //         }
        //         else
        //         {
                    
        //             isPoisoned = false;
        //             character.isInCombat = false;
        //             poisonAmount = defaultPoisonAmount;
        //             Destroy(currentPoisonParticleFX);
        //         }
        //     }
        // }

        // public virtual void ResetAllWeaponBuffs()
        // {
        //     character.hasWeaponBuff = false;
        //     timerBuff = 0;
        // }

        // protected virtual void HandleConsumableItemWeaponBuff()
        // {
        //     if (character.hasWeaponBuff)
        //     {
        //         timerBuff += Time.deltaTime;

        //         if (timerBuff >= buffActiveTime)
        //         {
        //             DeActivateWeaponBuffFX(character.isUsingLeftHand);
        //         }
        //     }
        //     else
        //     {
        //         ResetAllWeaponBuffs();
        //     }
        // }

        public virtual void InterruptEffect()
        {
            //Can be used to destroy effects models (Drinking Estus, Having arrow draw etc...)
            if (instantiatedFXModel != null)
            {
                Destroy(instantiatedFXModel);
            }
            //Fires the characters bow and removes the arrow if they are currently holding an arrow
            if (character.isHoldingArrow)
            {
                character.animator.SetBool("isHoldingArrow", false);
                Animator rangedWeaponAnimator = character.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();

                if (rangedWeaponAnimator != null)
                {
                    rangedWeaponAnimator.SetBool("isDrawn", false);
                    rangedWeaponAnimator.Play("Bow_Object_Fire");
                }
            }

            //Removes player from aiming state if they are currently aiming
            if (character.isAiming)
            {
                character.animator.SetBool("isAiming", false);
            }
        }

        protected virtual void ProcessBuildUpDecay()
        {
            if (character.characterStatsManager.poisonBuildUp > 0)
            {
                character.characterStatsManager.poisonBuildUp -= buildUpDecayAmount;
            }
        }

        public virtual void AddTimedEffectParticle(GameObject effect)
        {
            GameObject effectGameObject = Instantiate(effect, buildUpTransform);
            timedEffectParticles.Add(effectGameObject);
        }

        public virtual void RemoveTimedEffectParticle(EffectParticleType effectType)
        {
            for (int i = timedEffectParticles.Count - 1; i > -1; i--)
            {
                if (timedEffectParticles[i].GetComponent<EffectParticle>().effectType == effectType)
                {
                    Destroy(timedEffectParticles[i]);
                    timedEffectParticles.RemoveAt(i);
                }
            }
        }

    }
}