using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponWallCollider : MonoBehaviour
    {
        CharacterManager character;

        void Start() 
        {
            character = GetComponentInParent<CharacterManager>();
        }

        void OnTriggerEnter(Collider collision) 
        {
            //Debug.Log("Trigger Enter");
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment") || collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                Debug.Log("Now we should call spark function");
                character.characterEffectsManager.PlayWeaponSparkFX(character.isUsingLeftHand);
                character.characterSoundFXManager.PlayRandomWallHitSoundFX();
                // PlayerManager player = character as PlayerManager;
                // player.playerEffectsManager.PlayWeaponSparkFX(player.isUsingLeftHand);
            }
        }
        
    }
}
