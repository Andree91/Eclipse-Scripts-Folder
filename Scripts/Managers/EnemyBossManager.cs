using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

namespace AG
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;

        public EnemyManager enemy;
        public UIBossHealthBar bossHealthBar;
        public PlayableDirector bossTimeline;
        BossCombatStanceState bossCombatStanceState;
        WeaponHolderSlot weaponHolderSlot;

        [Header("Second Phase FX")]
        public GameObject particleFX;

        //Handle Switching Phase
        //Handle Switching Attack Patterns

        void Awake() 
        {
            enemy = GetComponent<EnemyManager>();
            //bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
            weaponHolderSlot = GetComponentInChildren<WeaponHolderSlot>();
        }

        void Start() 
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemy.enemyStatsManager.maxHealth);
        }

        public void UptadeBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                ShiftToSecondPhase();
                bossCombatStanceState.hasPhaseShifted = true;
            }

            if (currentHealth <= 0)
            {
                WorldEventManager worldEventManager = FindObjectOfType<WorldEventManager>();
                worldEventManager.BossHasBeenDefeated();
            }
        }

        public void ShiftToSecondPhase()
        {
            //Play Animation//Animation event for vfx buffs (particles, weapon change etc)
            Debug.Log("Start of Second Phase");
            enemy.animator.SetBool("isInVulnerable", true);
            enemy.animator.SetBool("isPhaseShifting", true);
            enemy.enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);

            //Switch Attack patters, actions
            bossCombatStanceState.hasPhaseShifted = true;
            //damageCollider.currentWeaponDamage = Mathf.RoundToInt(damageCollider.currentWeaponDamage * 1.5f);
            DamageCollider damageCollider = weaponHolderSlot.GetComponentInChildren<DamageCollider>();
            Debug.Log("current damage collider is" + damageCollider.gameObject.name);
            damageCollider.ChangeCurrentWeaponDamage(1.5f);
            

        }

        // public void PlayBossCutScene()
        // {
        //     bossTimeline.Play();
        //     StartCoroutine(DestroyTimeline());
        // }
    }
}
