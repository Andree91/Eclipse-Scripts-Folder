using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TakeDamageEffect : CharacterEffect
    {
        // If damage is caused by character, they are listed here
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage Types")]
        public float physicalDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float frostDamage = 0;
        public float bloodDamage = 0;
        public float holyDamage = 0;
        public float darkDamage = 0;
        public float magicDamage = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animations")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation = null;

        [Header("SFX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSFX = null;     // Extra damage sound. which is played when receiving elemental damage (Fire, Magic, Darkness etc)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom = 0;
        public Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager character)
        {
            if (character.isDead) { return; }

            if (character.isInVulnerable) { return; }


            CalculateDamage(character);
            CheckWhichDirectionDamageCameFrom(character);
            PlayDamageAnimation(character);
            PlayDamageSoundFX(character);
            PlayBloodSplatterFX(character);
            // If damage is A.I, Assing them the damaging character as a target
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (character.isDead) { return; }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            //Debug.Log($"Damage at start of function is {physicalDamage+fireDamage+lightningDamage}");

            if (characterCausingDamage != null)
            {
                // Before calculating damage defense, we check attacking character damage modifiers
                physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100.0f));
                fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100.0f));
                lightningDamage = Mathf.RoundToInt(lightningDamage * (characterCausingDamage.characterStatsManager.lightningDamagePercentageModifier / 100.0f));
                // frostDamage = Mathf.RoundToInt(frostDamage * (enemyCharacterDamagingMe.characterStatsManager.frostDamagePercentageModifier / 100.0f));
                // bloodDamage = Mathf.RoundToInt(bloodDamage * (enemyCharacterDamagingMe.characterStatsManager.bloodDamagePercentageModifier / 100.0f));
                // holyDamage = Mathf.RoundToInt(holyDamage * (enemyCharacterDamagingMe.characterStatsManager.holyDamagePercentageModifier / 100.0f));
                // darkDamage = Mathf.RoundToInt(darkDamage * (enemyCharacterDamagingMe.characterStatsManager.darkDamagePercentageModifier / 100.0f));
                // magicDamage = Mathf.RoundToInt(magicDamage * (enemyCharacterDamagingMe.characterStatsManager.magicDamagePercentageModifier / 100.0f));
            }

            //Debug.Log($"Damage before adding modifiers is {physicalDamage + fireDamage + lightningDamage}");

            float totalPhysicalDamageAbsorption = 1 -
            (1 - character.characterStatsManager.physicalDamageAbsorptionHead / 100) *
            (1 - character.characterStatsManager.physicalDamageAbsorptionBody / 100) *
            (1 - character.characterStatsManager.physicalDamageAbsorptionHands / 100) *
            (1 - character.characterStatsManager.physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            physicalDamage -= Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100.0f));

            float totalFireDamageAbsorption = 1 -
            (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
            (1 - character.characterStatsManager.fireDamageAbsorptionBody / 100) *
            (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100) *
            (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            fireDamage -= Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100.0f));

            float totalLightningDamageAbsorption = 1 -
            (1 - character.characterStatsManager.lightningDamageAbsorptionHead / 100) *
            (1 - character.characterStatsManager.lightningDamageAbsorptionBody / 100) *
            (1 - character.characterStatsManager.lightningDamageAbsorptionHands / 100) *
            (1 - character.characterStatsManager.lightningDamageAbsorptionLegs / 100);

            lightningDamage = Mathf.RoundToInt(lightningDamage - (lightningDamage * totalLightningDamageAbsorption));

            lightningDamage -= Mathf.RoundToInt(lightningDamage * (character.characterStatsManager.lightningAbsorptionPercentageModifier / 100.0f));


            float finalDamage = physicalDamage + fireDamage + lightningDamage; //+ magicDamage etc...

            //Debug.Log("Fire damage is " + fireDamage);

            if (characterCausingDamage != null)
            {
                if (characterCausingDamage.isPerformingFullyChargedAttack) // Change this to melee weapon only
                {
                    finalDamage = finalDamage * 1.5f;
                }
            }

            character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);
            character.hitCounter += 1;

            Debug.Log($"Total damage deal it {finalDamage}");

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }

            //character.characterSoundFXManager.PlayRandomDamageSoundFX();
            //Just for testing, delete later
            //character.isInteracting = true;

        }

        private void CheckWhichDirectionDamageCameFrom(CharacterManager character)
        {
            if (manuallySelectDamageAnimation) { return; }
            float angleHitFrom = (Vector3.SignedAngle(character.transform.forward, characterCausingDamage.transform.forward, Vector3.up));
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                ChooseDamageAnimationBackward(character);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                ChooseDamageAnimationLeft(character);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                ChooseDamageAnimationRight(character);
            }
            else
            {
                ChooseDamageAnimationForward(character);
            }
        }

        private void ChooseDamageAnimationForward(CharacterManager character)
        {
            // Poise Bracket < 25      SMALL
            // Poise Bracket > 25 < 50 MEDIUM
            // Poise Bracket > 50 < 75 LARGE
            // Poise Bracket > 75      COLOSSAL

            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Forward_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Forward_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Forward_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Forward_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Forward_Large);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Forward_Large);
                    return;
                }
            }
            else if (poiseDamage >= 75)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Forward_Colossal);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Forward_Colossal);
                    return;
                }
            }
        }

        private void ChooseDamageAnimationBackward(CharacterManager character)
        {
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Back_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Back_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Back_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Back_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Back_Large);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Back_Large);
                    return;
                }
            }
            else if (poiseDamage >= 75)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Back_Colossal);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Back_Colossal);
                    return;
                }
            }
        }

        private void ChooseDamageAnimationLeft(CharacterManager character)
        {
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Left_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Left_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Left_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Left_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Left_Large);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Left_Large);
                    return;
                }
            }
            else if (poiseDamage >= 75)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Left_Colossal);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Left_Colossal);
                    return;
                }
            }
        }

        private void ChooseDamageAnimationRight(CharacterManager character)
        {
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Right_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Right_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Right_Medium);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Right_Medium);
                    return;
                }
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Right_Large);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Right_Large);
                    return;
                }
            }
            else if (poiseDamage >= 75)
            {
                if (character.isTwoHanding)
                {
                    // Play Two Handed Damage Animation
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.TH_Damage_Animations_Right_Colossal);
                    return;
                }
                else
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomDamageAnimationFromList(character.characterAnimatorManager.OH_Damage_Animations_Right_Colossal);
                    return;
                }
            }
        }

        private void PlayDamageSoundFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayRandomDamageSoundFX();

            if (fireDamage > 0)
            {
                character.characterSoundFXManager.PlaySoundFX(elementalDamageSFX, 0.3f);
            }
        }

        private void PlayDamageAnimation(CharacterManager character)
        {
            /* If we are currently playing a damage animation that is heavy and light attack hit us
               We don't want to play light damage animation (so it won't override the current heavy damage animation)
            */
            if (character.isInteracting && character.characterCombatManager.previousPoiseDamageTaken > poiseDamage)
            {
                // If character is interacting && previous poise damage is greater than current poise damage, return,
                // so we don't change the damage animation to lighter damage animation
                return;
            }

            if (character.isDead)
            {
                character.characterWeaponSlotManager.CloseDamageCollider();
                character.characterAnimatorManager.PlayTargetAnimation("Death_01", true);
                return;
            }

            if (!poiseIsBroken)
            {
                // If poise is not borken, character won't play any damage animation
                return;
            }
            else
            {
                // Enable/Disable Stun Lock
                if (playDamageAnimation)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(damageAnimation, true);
                }
            }
        }

        private void PlayBloodSplatterFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterFX(contactPoint);
        }

        private void AssingNewAITarget(CharacterManager character)
        {
            EnemyManager aiCharacter = character as EnemyManager;

            if (aiCharacter != null && characterCausingDamage != null)
            {
                aiCharacter.currentTarget = characterCausingDamage;
            }
        }

    }
}