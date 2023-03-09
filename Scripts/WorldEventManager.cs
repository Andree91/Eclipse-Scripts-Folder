using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;
        public List<FogWall> warriorFogWalls;
        public UIBossHealthBar bossPigHealthBar;
        public UIBossHealthBar bossWarriorHealthBar;
        public GameObject timeLine;
        public GameObject bossMesh;
        public GameObject bossWeaponMesh;
        public bool isPigBoss;
        EnemyBossManager enemyBossManager;

        public bool bossFightIsActive; //Currently fighting the boss
        public bool bossHasBeenAwaked; //Woke boss//Watched cutscene//Died during fight
        public bool bossHasBeenDefeated; //Player has defeated the boss

        void Awake()
        {
            //bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyBossManager = FindObjectOfType<EnemyBossManager>();
        }

        void Start()
        {
            if (timeLine != null)
            {
                bossWeaponMesh.SetActive(false);
            }
        }

        public void ActivateBossFight()
        {
            if (!bossFightIsActive)
            {
                if (timeLine != null)
                {
                    timeLine.SetActive(true);
                    StartCoroutine(DestroyTimeline());
                }
            }

            bossFightIsActive = true;
            bossHasBeenAwaked = true;

            if (!bossHasBeenDefeated)
            {
                //Activate Fog Wall(s)
                if (isPigBoss)
                {
                    foreach (FogWall fogWall in fogWalls)
                    {
                        fogWall.ActivateFogWall();
                    }
                }
                else
                {
                    foreach (FogWall fogWall in warriorFogWalls)
                    {
                        fogWall.ActivateFogWall();
                    }
                }

                if (isPigBoss)
                {
                    bossPigHealthBar.SetUIHealthBarToActive();
                }
                else
                {
                    bossWarriorHealthBar.SetUIHealthBarToActive();
                }
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;

            //Deactive Fog Walls
            if (isPigBoss)
            {
                foreach (FogWall fogWall in fogWalls)
                {
                    fogWall.DeactivateFogWall();
                }
            }
            else
            {
                foreach (FogWall fogWall in warriorFogWalls)
                {
                    fogWall.DeactivateFogWall();
                }
            }

            if (isPigBoss)
            {
                bossPigHealthBar.SetHealthBarToInactive();
            }
            else
            {
                bossWarriorHealthBar.SetHealthBarToInactive();
            }
        }

        IEnumerator DestroyTimeline()
        {
            yield return new WaitForSeconds(1.2f);
            Destroy(timeLine.gameObject);
            bossMesh.gameObject.SetActive(true);
            bossWeaponMesh.gameObject.SetActive(true);
            enemyBossManager.enemy.characterEffectsManager.PlayDustCloudFX(enemyBossManager.transform);
        }
    }
}
