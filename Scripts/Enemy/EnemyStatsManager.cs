using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        //public HealthBar healthBar;
        EnemyManager enemy;
        public UIBossHealthBar bossHealthBar;
        public UIEnemyHealthBar enemyHealthBar;
        public bool isBoss;

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
        }

        public override void HandlePoiseResetTimer()
        {
            Debug.Log("Poise reset timer is" + poiseResetTimer);
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !enemy.isInteracting)
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        // private int SetMaxHealthFromHealthLevel()
        // {
        //     maxHealth = healthLevel * 10;
        //     return maxHealth;
        // }

        private float SetMaxStaminaFromStaminahLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, int lightingDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
        {
            if (enemy.isInVulnerable) { return; }

            if (enemy.isUsingShieldSpell) { return; }

            if (enemy.isDead) { return; }

            base.TakeDamage(physicalDamage, fireDamage, lightingDamage, damageAnimation, enemyCharacterDamagingMe);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemy != null)
            {
                enemy.enemyBossManager.UptadeBossHealthBar(currentHealth, maxHealth);
                bossHealthBar.ShowDealtDamage(physicalDamage, fireDamage);
            }

            enemy.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public override void TakeDamageAfterBlock(int physicalDamage, int fireDamage, int lightningDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (enemy.isDead) { return; }

            base.TakeDamageAfterBlock(physicalDamage, fireDamage, lightningDamage, enemyCharacterDamagingMe);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemy != null)
            {
                enemy.enemyBossManager.UptadeBossHealthBar(currentHealth, maxHealth);
                bossHealthBar.ShowDealtDamage(physicalDamage, fireDamage);
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        void HandleDeath()
        {
            currentHealth = 0;

            if (enemy.isBear)
            {
                enemy.animalLookAt.SetActive(false);
            }
            if (!enemy.isBeingBackStabbed)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
            }

            enemy.isDead = true;
            if (enemy.currentTarget != null)
            {
                enemy.currentTarget.isInCombat = false;
                enemy.currentTarget = null;
            }
            enemy.RollForLootItemAndDropIt();
            if (enemy.isRagdoll)
            {
                enemy.characterStatsManager.HandleTogglingRagDoll(3f);
                //enemy.ragDollManager.ToggleRagDoll(enemy.isRagdoll);
            }
            //scan every player in the scene and award souls to them
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage, int lightningDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (enemy.isInVulnerable) { return; }

            if (enemy.isDead) { return; }

            base.TakeDamageNoAnimation(physicalDamage, fireDamage, lightningDamage, enemyCharacterDamagingMe);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UptadeBossHealthBar(currentHealth, maxHealth);
                bossHealthBar.ShowDealtDamage(physicalDamage, fireDamage);
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (enemy.isDead) { return; }

            base.TakePoisonDamage(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UptadeBossHealthBar(currentHealth, maxHealth);
                bossHealthBar.ShowDealtDamage(damage, 0);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                enemy.enemyAnimatorManager.PlayTargetAnimation("Death_01", true);
                enemy.isDead = true;
            }
        }


        public void BreakGuard()
        {
            enemy.enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

        public void TakeStaminaDamage(int staminaDamage)
        {
            currentStamina = currentStamina - staminaDamage;
            //staminaBar.SetCurrentStamina(currentStamina);
        }
    }
}
