using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WaterSurface : MonoBehaviour
    {
        public Vector3 swimmingLevel;
        public WaterInteractable waterInteractable;
        public PlayerManager player;
        EnemyManager enemy;

        void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Animal") { return; }

            player = other.transform.GetComponent<PlayerManager>();
            if (player != null && !player.isRiding)
            {
                player.isSwimming = true;
                player.isCrouching = false;
                player.inputHandler.crouchFlag = false;
                player.playerMovement.inAirTimer = 0;
                player.characterSoundFXManager.PlayWaterSplashSoundFX();
                player.playerEffectsManager.PlayWaterTrailFX(player.waterTrailTrasform);
                player.playerWeaponSlotManager.rightHandSlot.UnloadWeaponAndDestroy();
                player.playerWeaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
                Destroy(player.playerEffectsManager.currentUnderWaterParticleFX);
                Vector3 originalPos = player.transform.position;
                if (!player.isUnderWater)
                {
                    Vector3 swimmingPos = originalPos + swimmingLevel;
                    player.transform.position = swimmingPos;
                }
                else
                {
                    Vector3 swimmingPos = originalPos - swimmingLevel - swimmingLevel;
                    player.transform.position = swimmingPos;
                }
            }
            else
            {
                enemy = other.transform.GetComponent<EnemyManager>();
                if (enemy != null)
                {
                    enemy.isSwimming = true;
                    //enemy.animator.SetBool("isSwimming", true);
                    Vector3 originalPos = enemy.transform.position;
                    Vector3 swimmingPos = originalPos + swimmingLevel;
                    enemy.transform.position = swimmingPos;
                    //enemy.enemyRigidbody.useGravity = false;
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            //Maybe Do some more logic later
            if (other.gameObject.tag == "Animal") { return; }

            player = other.transform.GetComponent<PlayerManager>();
            if (player != null && !player.isRiding)
            {
                player.isSwimming = true;
                waterInteractable.gameObject.SetActive(true);
                player.isUnderWater = false;
            }
            else
            {
                enemy = other.transform.GetComponent<EnemyManager>();
                if (enemy != null)
                {
                    enemy.isSwimming = true;
                    //enemy.animator.SetBool("isSwimming", false);
                    //enemy.enemyRigidbody.useGravity = true;
                }
            }
        }

        void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.tag == "Animal") { return; }

            player = other.transform.GetComponent<PlayerManager>();
            if (player != null && !player.isRiding)
            {
                Destroy(player.playerEffectsManager.currentWaterParticleFX);
                player.isSwimming = false;
                if (!player.isUnderWater)
                {
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    if (player.playerWeaponSlotManager.backSlot.currentWeaponModel == null && player.playerWeaponSlotManager.shieldBackSlot.currentWeaponModel == null)
                    {
                        player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    }
                }
                waterInteractable.gameObject.SetActive(false);
            }
            else
            {
                enemy = other.transform.GetComponent<EnemyManager>();
                if (enemy != null)
                {
                    enemy.isSwimming = false;
                    //enemy.animator.SetBool("isSwimming", false);
                    //enemy.enemyRigidbody.useGravity = true;
                    Debug.Log(enemy.name + " is getting og the water");
                }
            }
        }
    }
}
