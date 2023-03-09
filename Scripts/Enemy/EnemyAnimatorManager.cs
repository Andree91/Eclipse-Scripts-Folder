using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemy;

        protected override void Awake() 
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
        }


        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemy.enemyStatsManager.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.currentSoulCount);
                }
            }
        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

            GameObject phaseFX = Instantiate(enemy.enemyBossManager.particleFX, bossFXTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            enemy.enemyEffectsManager.PlayWeaponFX(false);
        }

        public void EnableCollision()
        {
            enemy.characterCollider.enabled = true;
            enemy.characterCollisionBlockerCollider.enabled = true;
        }

        public void DisableCollison()
        {
            enemy.characterCollider.enabled = false;
            enemy.characterCollisionBlockerCollider.enabled = false;
        }

        void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemy.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = enemy.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemy.enemyRigidbody.velocity = velocity; //* enemyLocomotionManager.moveSpeed;

            if (enemy.isRotatingWithRootMotion)
            {
                enemy.transform.rotation *= enemy.animator.deltaRotation;
            }
        }

        
    }
}
