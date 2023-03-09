using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LadderExit : MonoBehaviour
    {
        public Collider ladderCollider;

        PlayerManager player;

        void OnTriggerEnter(Collider other) 
        {
            player = other.transform.GetComponent<PlayerManager>();
            if (player != null && !player.isJumping && player.isClimbing)
            {
                //DOESN'T WORK WHILE JUMPING, HAVE TO MAKE BETTER CHECKING SYSTEM
                player.isClimbing = false;
                player.playerRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                if (player.playerWeaponSlotManager.backSlot.currentWeaponModel == null && player.playerWeaponSlotManager.shieldBackSlot.currentWeaponModel == null)
                {
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                }
                ladderCollider.enabled = true;
                Debug.Log("isClimbing is " + player.isClimbing);
            }
        }
    }
}
