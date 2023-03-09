using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class NpcStateAgro : State
    {
        public PursueTargetStateHumanoid pursueTargetState;
        public CompanionStateFollowHost followHostState;
        public NpcStatePatrol patrolState;
        public NpcStateIdle npcStateIdle;

        public LayerMask detectionLayer;
        public LayerMask layersThatBlockLineOfSight;

        //[SerializeField] float maxTimeToWaitUntilContinuePatrol = 6f;
        //[SerializeField] float minimunTimeToWaitUntilContinuePatrol = 3f;

        //float timer = 0;
        //float delay = 0;

        public override State Tick(EnemyManager aiCharacter)
        {
            aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

            HandleNpcAgro(aiCharacter);

            return npcStateIdle;
        }

        public void HandleNpcAgro(EnemyManager aiCharacter)
        {
            if (aiCharacter.characterStatsManager.currentHealth <= aiCharacter.characterStatsManager.maxHealth * 0.7f || aiCharacter.hitCounter >= 40)
            {
                aiCharacter.characterStatsManager.teamIDNumeber = 1;
                aiCharacter.hitCounter = 0;
                aiCharacter.characterInventoryManager.leftWeapon = aiCharacter.characterInventoryManager.weaponsInLeftHandSlots[0];
                aiCharacter.characterInventoryManager.rightWeapon = aiCharacter.characterInventoryManager.weaponsInRightHandSlots[0];
                aiCharacter.characterWeaponSlotManager.LoadBothWeaponsOnSlot();
                aiCharacter.currentTarget = FindObjectOfType<PlayerManager>();
            }
        }


    }
}
