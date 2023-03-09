using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        FocusPointBar focusPointBar;

        protected override void Awake() 
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            //healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !player.isInteracting)
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        

        public override void TakeDamage(int physicalDamage, int fireDamage, int lightingDamage,string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (player.isInVulnerable) { return; }

            if (player.isUsingShieldSpell) { return; }

            if (player.isDead) { return; }

            base.TakeDamage(physicalDamage, fireDamage, lightingDamage,damageAnimation, enemyCharacterDamagingMe);
            player.uIManager.ShowHUD();
            healthBar.SetCurrentHealth(currentHealth);
            player.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
                player.isDead = true;
                player.playerRigidbody.isKinematic = true;
                //TODO Handle Player Death
                player.HandlePlayerDeath();
            }
        }

        public override void TakeDamageAfterBlock(int physicalDamage, int fireDamage, int lightningDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (player.isDead) { return; }

            base.TakeDamageAfterBlock(physicalDamage, fireDamage, lightningDamage, enemyCharacterDamagingMe);
            player.uIManager.ShowHUD();
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
                player.isDead = true;
                player.playerRigidbody.isKinematic = true;
                //TODO Handle Player Death
                player.HandlePlayerDeath();
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int lightningDamage, CharacterManager enemyCharacterDamagingMe)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage, lightningDamage, enemyCharacterDamagingMe);
            player.uIManager.ShowHUD();
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //player.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
                player.isDead = true;
                player.playerRigidbody.isKinematic = true;
                //TODO Handle Player Death
                player.HandlePlayerDeath();
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (player.isDead) { return; }
            
            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death_01", true);
                player.isDead = true;
                player.playerRigidbody.isKinematic = true;
                //TODO Handle Player Death
                player.HandlePlayerDeath();
            }
        }

        //OLD STAMINA SYSTEM
        // public void TakeStaminaDamage(int staminaDamage)
        // {
        //     currentStamina = currentStamina - staminaDamage;
        //     staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        // }

        public override void DetuctStamina(float staminaToDetuct)
        {
            if (player.isInCombat)
            {
                base.DetuctStamina(staminaToDetuct);
                staminaBar.isDetucing = true;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }

        public void RegenerateStamina()
        {
            if (player.isInteracting || player.canJumpAttack)
            {
                staminaRegenerationTimer = 0;
            }

            else if (player.isRiding)
            {
                if (currentStamina < maxStamina && staminaRegenerationTimer > 1)
                {
                    staminaRegenerationTimer += Time.deltaTime;

                    if (player.isRiderAttack)
                    {
                        currentStamina -= staminaRegenerationAmount * 0.5f * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        StartCoroutine(StaminaRegenerateCoolDown());
                    }
                    else if (!player.isRiderAttack)
                    {
                        currentStamina += staminaRegenerationAmount * 0.5f * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                }

                if (currentStamina >= maxStamina)
                {
                    staminaRegenerationTimer = 5;
                }
            }
            else
            {
                staminaRegenerationTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    if (player.isBlocking)
                    {
                        currentStamina += staminaRegenerationAmountWhileBlocking * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else
                    {
                        currentStamina += staminaRegenerationAmount * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                }

                if (currentStamina >= maxStamina)
                {
                    staminaRegenerationTimer = 0;
                }
            }
        }

        IEnumerator StaminaRegenerateCoolDown()
        {
            yield return new WaitForSeconds(1f);
            player.isRiderAttack = false;
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoints(Mathf.RoundToInt(currentFocusPoints));
        }

        public override void HealCharacter(int healAmount)
        {
            base.HealCharacter(healAmount);

            healthBar.SetCurrentHealth(currentHealth);
        }

        public override void RestoreCharacterMana(int focusPointsAmount)
        {
            base.RestoreCharacterMana(focusPointsAmount);

            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }

    }
}
