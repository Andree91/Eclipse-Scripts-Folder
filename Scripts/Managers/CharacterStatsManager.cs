using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public CharacterManager character;

        [Header("NAME")]
        public string characterName = "Nameless";
        public string className;

        [Header("Team I.D")]
        public int teamIDNumeber = 0;

        [Header("CHARACTER LEVEL")]
        public int playerLevel = 1;

        [Header("STAT LEVELS")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int poiseLevel = 10;
        public int strenghtLevel = 10;
        public int dexterityLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;
        public int arcaneLevel = 10;

        [Header("Health Stats")]
        public int maxHealth;
        public int currentHealth;

        [Header("Stamina Stats")]
        public float maxStamina;
        public float currentStamina;
        public float staminaRegenerationAmount = 20;
        public float staminaRegenerationAmountWhileBlocking = 2;
        public float staminaRegenerationTimer = 0;

        [Header("Focus Point Stats")]
        public float maxFocusPoints;
        public float currentFocusPoints;

        [Header("Poise")]
        public float totalPoiseDefence; //The TOTAL poise after damage calculation
        public float offensivePoiseBonus; //The Poise you gain during an attack woth weapon (better with heavy weapons)
        public float armorPoiseBonus; //The Poise you'll gain from your equipment, rings, talismans etc
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        [Header("Armor Absorptions")]
        //Physical Absorption
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionHands;
        public float physicalDamageAbsorptionLegs;

        //Fire Absorption
        public float fireDamageAbsorptionHead;
        public float fireDamageAbsorptionBody;
        public float fireDamageAbsorptionHands;
        public float fireDamageAbsorptionLegs;

        //Lightning Absorption
        public float lightningDamageAbsorptionHead;
        public float lightningDamageAbsorptionBody;
        public float lightningDamageAbsorptionHands;
        public float lightningDamageAbsorptionLegs;

        // //Magic Absorption
        // public float magicDamageAbsorptionHead;
        // public float magicDamageAbsorptionBody;
        // public float magicDamageAbsorptionHands;
        // public float magicDamageAbsorptionLegs;

        // //Frost Absorption
        // public float frostDamageAbsorptionHead;
        // public float frostDamageAbsorptionBody;
        // public float frostDamageAbsorptionHands;
        // public float frostDamageAbsorptionLegs;

        // //Blood Absorption
        // public float bloodDamageAbsorptionHead;
        // public float bloodDamageAbsorptionBody;
        // public float bloodDamageAbsorptionHands;
        // public float bloodDamageAbsorptionLegs;

        [Header("Resistances")]
        public float poisonResistance;
        public float bleedResistance;
        public float frostResistance;
        public float curseResistance;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalDamageAbsorption;
        public float blockingFireDamageAbsorption;
        public float blockingMagicDamageAbsorption;
        public float blockingLightningDamageAbsorption;
        //public float blockingFrostDamageAbsorption;
        //public float blockingBloodDamageAbsorption;
        public float blockingHolyDamageAbsorption;
        //public float blockingDarkDamageAbsorption;
        public float blockingStabilityRating;

        // Any damage done by this player is modified by these type modifiers amount
        [Header("Damage Type Modifiers")]
        public float physicalDamagePercentageModifier = 100.0f;
        public float fireDamagePercentageModifier = 100.0f;
        public float lightningDamagePercentageModifier = 100.0f;
        public float frostDamagePercentageModifier = 100.0f;
        public float bloodDamagePercentageModifier = 100.0f;
        public float holyDamagePercentageModifier = 100.0f;
        public float darkDamagePercentageModifier = 100.0f;
        public float magicDamagePercentageModifier = 100.0f;

        // Incoming damage AFTER armor calculations is modified by these amount
        [Header("Damage Absorption Modifiers")]
        public float physicalAbsorptionPercentageModifier = 0.0f;
        public float fireAbsorptionPercentageModifier = 0.0f;
        public float lightningAbsorptionPercentageModifier = 0.0f;
        public float frostAbsorptionPercentageModifier = 0.0f;
        public float bloodAbsorptionPercentageModifier = 0.0f;
        public float holyAbsorptionPercentageModifier = 0.0f;
        public float darkAbsorptionPercentageModifier = 0.0f;
        public float magicAbsorptionPercentageModifier = 0.0f;

        [Header("Guard Break Modifier")]
        public float guardBreakModifier;

        [Header("Poison")]
        public bool isPoisoned;
        public float poisonBuildUp = 0; //The builp up over time, when it hit 100% you'll be poisoned
        public float poisonAmount = 100; //The amount of poison the player has to process until poison is cleared

        [Header("SOULS")]
        public int soulsAwardedOnDeath = 50;
        public int currentSoulCount = 0;

        protected virtual void Awake() 
        {
            character = GetComponent<CharacterManager>();
        }

        void Start() 
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        protected virtual void Uptade()
        {
            Debug.Log("Uptade");
            HandlePoiseResetTimer();
        }

        public virtual void TakeDamage(int physicalDamage, int fireDamage, int lightningDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead) { return; }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            //Debug.Log($"Damage at start of function is {physicalDamage+fireDamage+lightningDamage}");

            if (enemyCharacterDamagingMe != null)
            {
                // Before calculating damage defense, we check attacking character damage modifiers
                physicalDamage = Mathf.RoundToInt(physicalDamage * (enemyCharacterDamagingMe.characterStatsManager.physicalDamagePercentageModifier / 100.0f));
                fireDamage = Mathf.RoundToInt(fireDamage * (enemyCharacterDamagingMe.characterStatsManager.fireDamagePercentageModifier / 100.0f));
                lightningDamage = Mathf.RoundToInt(lightningDamage * (enemyCharacterDamagingMe.characterStatsManager.lightningDamagePercentageModifier / 100.0f));
                // frostDamage = Mathf.RoundToInt(frostDamage * (enemyCharacterDamagingMe.characterStatsManager.frostDamagePercentageModifier / 100.0f));
                // bloodDamage = Mathf.RoundToInt(bloodDamage * (enemyCharacterDamagingMe.characterStatsManager.bloodDamagePercentageModifier / 100.0f));
                // holyDamage = Mathf.RoundToInt(holyDamage * (enemyCharacterDamagingMe.characterStatsManager.holyDamagePercentageModifier / 100.0f));
                // darkDamage = Mathf.RoundToInt(darkDamage * (enemyCharacterDamagingMe.characterStatsManager.darkDamagePercentageModifier / 100.0f));
                // magicDamage = Mathf.RoundToInt(magicDamage * (enemyCharacterDamagingMe.characterStatsManager.magicDamagePercentageModifier / 100.0f));
            }

            //Debug.Log($"Damage before adding modifiers is {physicalDamage + fireDamage + lightningDamage}");

            float totalPhysicalDamageAbsorption = 1 - 
            (1 - physicalDamageAbsorptionHead / 100) * 
            (1 - physicalDamageAbsorptionBody / 100) * 
            (1 - physicalDamageAbsorptionHands /100) * 
            (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            physicalDamage -= Mathf.RoundToInt(physicalDamage * (physicalAbsorptionPercentageModifier / 100.0f));

            float totalFireDamageAbsorption = 1 - 
            (1 - fireDamageAbsorptionHead / 100) * 
            (1 - fireDamageAbsorptionBody / 100) * 
            (1 - fireDamageAbsorptionHands /100) * 
            (1 - fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            fireDamage -= Mathf.RoundToInt(fireDamage * (fireAbsorptionPercentageModifier / 100.0f));

            float totalLightningDamageAbsorption = 1 -
            (1 - lightningDamageAbsorptionHead / 100) *
            (1 - lightningDamageAbsorptionBody / 100) *
            (1 - lightningDamageAbsorptionHands / 100) *
            (1 - lightningDamageAbsorptionLegs / 100);

            lightningDamage = Mathf.RoundToInt(lightningDamage - (lightningDamage * totalLightningDamageAbsorption));

            lightningDamage -= Mathf.RoundToInt(lightningDamage * (lightningAbsorptionPercentageModifier / 100.0f));


            float finalDamage = physicalDamage + fireDamage + lightningDamage; //+ magicDamage etc...

            //Debug.Log("Fire damage is " + fireDamage);

            if (enemyCharacterDamagingMe != null)
            {
                if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
                {
                    finalDamage = finalDamage * 1.5f;
                }
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);
            character.hitCounter += 1;

            Debug.Log($"Total damage deal it {finalDamage}");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }

            character.characterSoundFXManager.PlayRandomDamageSoundFX();
            //Just for testing, delete later
            character.isInteracting = true;
        }

        public virtual void TakeDamageAfterBlock(int physicalDamage, int fireDamage, int lightningDamage,CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead) { return; }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            physicalDamage = Mathf.RoundToInt(physicalDamage * (enemyCharacterDamagingMe.characterStatsManager.physicalDamagePercentageModifier / 100.0f));
            fireDamage = Mathf.RoundToInt(fireDamage * (enemyCharacterDamagingMe.characterStatsManager.fireDamagePercentageModifier / 100.0f));
            lightningDamage = Mathf.RoundToInt(fireDamage * (enemyCharacterDamagingMe.characterStatsManager.lightningDamagePercentageModifier / 100.0f));

            float totalPhysicalDamageAbsorption = 1 -
            (1 - physicalDamageAbsorptionHead / 100) *
            (1 - physicalDamageAbsorptionBody / 100) *
            (1 - physicalDamageAbsorptionHands / 100) *
            (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            physicalDamage -= Mathf.RoundToInt(physicalDamage * (physicalAbsorptionPercentageModifier / 100.0f));

            float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsorptionHead / 100) *
            (1 - fireDamageAbsorptionBody / 100) *
            (1 - fireDamageAbsorptionHands / 100) *
            (1 - fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            fireDamage -= Mathf.RoundToInt(fireDamage * (fireAbsorptionPercentageModifier / 100.0f));

            float totalLightningDamageAbsorption = 1 -
            (1 - lightningDamageAbsorptionHead / 100) *
            (1 - lightningDamageAbsorptionBody / 100) *
            (1 - lightningDamageAbsorptionHands / 100) *
            (1 - lightningDamageAbsorptionLegs / 100);

            lightningDamage = Mathf.RoundToInt(lightningDamage - (lightningDamage * totalLightningDamageAbsorption));

            lightningDamage -= Mathf.RoundToInt(lightningDamage * (lightningAbsorptionPercentageModifier / 100.0f));

            float finalDamage = physicalDamage + fireDamage + lightningDamage; //+ magicDamage etc...

            if (enemyCharacterDamagingMe != null)
            {
                if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
                {
                    finalDamage = finalDamage * 1.5f;
                }
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log($"Total damage deal after blocking is {finalDamage}");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }

            character.characterSoundFXManager.PlayRandomShieldHitSoundFX();
        }


        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int lightningDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (character.isDead) { return; }

            if (enemyCharacterDamagingMe != null)
            {
                physicalDamage = Mathf.RoundToInt(physicalDamage * (enemyCharacterDamagingMe.characterStatsManager.physicalDamagePercentageModifier / 100.0f));
                fireDamage = Mathf.RoundToInt(fireDamage * (enemyCharacterDamagingMe.characterStatsManager.fireDamagePercentageModifier / 100.0f));
                lightningDamage = Mathf.RoundToInt(fireDamage * (enemyCharacterDamagingMe.characterStatsManager.lightningDamagePercentageModifier / 100.0f));
            }

            float totalPhysicalDamageAbsorption = 1 -
            (1 - physicalDamageAbsorptionHead / 100) *
            (1 - physicalDamageAbsorptionBody / 100) *
            (1 - physicalDamageAbsorptionHands / 100) *
            (1 - physicalDamageAbsorptionLegs / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            physicalDamage -= Mathf.RoundToInt(physicalDamage * (physicalAbsorptionPercentageModifier / 100.0f));

            float totalFireDamageAbsorption = 1 -
            (1 - fireDamageAbsorptionHead / 100) *
            (1 - fireDamageAbsorptionBody / 100) *
            (1 - fireDamageAbsorptionHands / 100) *
            (1 - fireDamageAbsorptionLegs / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            fireDamage -= Mathf.RoundToInt(fireDamage * (fireAbsorptionPercentageModifier / 100.0f));

            float totalLightningDamageAbsorption = 1 -
            (1 - lightningDamageAbsorptionHead / 100) *
            (1 - lightningDamageAbsorptionBody / 100) *
            (1 - lightningDamageAbsorptionHands / 100) *
            (1 - lightningDamageAbsorptionLegs / 100);

            lightningDamage = Mathf.RoundToInt(lightningDamage - (lightningDamage * totalLightningDamageAbsorption));

            lightningDamage -= Mathf.RoundToInt(lightningDamage * (lightningAbsorptionPercentageModifier / 100.0f));

            float finalDamage = physicalDamage + fireDamage + lightningDamage; //+ magicDamage etc...

            if (enemyCharacterDamagingMe != null)
            {
                if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
                {
                    finalDamage = finalDamage * 1.5f;
                }
            }

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log($"Total damage deal it {finalDamage}");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void HealCharacter(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

        public virtual void RestoreCharacterMana(int focusPointsAmount)
        {
            currentFocusPoints = currentFocusPoints + focusPointsAmount;

            if (currentFocusPoints > maxFocusPoints)
            {
                currentFocusPoints = maxFocusPoints;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            Debug.Log(" Character Poise reset timer is" + poiseResetTimer);
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public virtual void HandleTogglingRagDoll(float timeToEnableRagDoll)
        {
            StartCoroutine(RagDollTimer(timeToEnableRagDoll));
        }

        IEnumerator RagDollTimer(float timeToEnableRagDoll)
        {
            yield return new WaitForSeconds(timeToEnableRagDoll);
            character.ragDollManager.ToggleRagDoll(character.isRagdoll);
        }

        public virtual void DetuctStamina(float staminaToDetuct)
        {
            currentStamina -= staminaToDetuct;
        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }
    }
}
