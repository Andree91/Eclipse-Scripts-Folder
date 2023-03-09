using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerAnimatorManager : AnimatorManager
    {
        public PlayerManager player;
        

        int vertical;
        int horizontal;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UptadeAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            player.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            player.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void EnableCollision()
        {
            player.playerMovement.characterCollider.enabled = true;
            player.playerMovement.characterCollisionBlockerCollider.enabled = true;
        }

        public void DisableCollison()
        {
            player.playerMovement.characterCollider.enabled = false;
            player.playerMovement.characterCollisionBlockerCollider.enabled = false;
        }

        public override void EnableRollingCollider()
        {
            if (player.isInGodMode) { return; }
            character.GetComponentInChildren<SphereCollider>().enabled = true;
        }

        public override void DisableRollingCollider()
        {
            if (player.isInGodMode) { return; }
            character.GetComponentInChildren<SphereCollider>().enabled = false;
        }

        public void EnableUseConsumeItem()
        {
            canUseConsumeItem = true;
        }

        public void DisableUseConsumeItem()
        {
            canUseConsumeItem = false;
        }

        public void SuccessfullyUseCurrentConsumable()
        {
            if (character.characterInventoryManager.currentConsumable != null)
            {
                character.characterInventoryManager.currentConsumable.SuccesfullyConsumeItem(player);
            }
        }

        public void EnableIsInterActing()
        {
            player.animator.SetBool("isInteracting", true);
        }

        public void AwardSoulsOnDeath()
        {
            // if
            // PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            // SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            // if (playerStats != null)
            // {
            //     playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

            //     if (soulCountBar != null)
            //     {
            //         soulCountBar.SetSoulCountText(playerStats.soulCount);
            //     }
            // }
        }

        void OnAnimatorMove() 
        {
            if (character.isInteracting == false) { return; }

            float delta = Time.deltaTime;
            player.playerMovement.rb.drag = 0;
            Vector3 deltaPosition = player.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            player.playerMovement.rb.velocity = velocity;

        }

    }
}
