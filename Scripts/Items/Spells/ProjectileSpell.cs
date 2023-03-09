using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;
        public float fireDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;
        Rigidbody rb;

        public override void AttempToCastSpell(CharacterManager character)
        {
            base.AttempToCastSpell(character);

            //Instansite spell in caster hand
            if (character.isUsingLeftHand)
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.leftHandSlot.transform);
                instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
            else
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.transform);
                instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
        }

        public override void SuccesfullyCastSpell(CharacterManager character)
        {
            base.SuccesfullyCastSpell(character);

            PlayerManager player = character as PlayerManager;

            //HANDLE THE PROCESS IF THE CASTER IS THE PLAYER
            if (player != null)
            {
                if (player.isUsingLeftHand)
                {
                    GameObject instantiatedSpellFX = Instantiate(spellCastFX,
                                                            player.playerWeaponSlotManager.leftHandSlot.transform.position,
                                                            player.cameraHandler.cameraPivotTransform.rotation); //Maybe delete transform on cameraPivotTransfor.
                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                    spellDamageCollider.character = player;
                    spellDamageCollider.teamIDNumeber = player.playerStatsManager.teamIDNumeber;

                    rb = instantiatedSpellFX.GetComponent<Rigidbody>();
                    //And make intiation location, if using staff spell have to laucnh at the tip of the staff

                    //spelldDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>(); (later), now only flat damage

                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                    }
                    else
                    {
                        instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x,
                                                                                    player.playerStatsManager.transform.eulerAngles.y, 0);
                    }

                    rb.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                    rb.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                    rb.useGravity = isEffectedByGravity;
                    rb.mass = projectileMass;
                    instantiatedSpellFX.transform.parent = null;
                }
                else
                {
                    GameObject instantiatedSpellFX = Instantiate(spellCastFX,
                                                            player.playerWeaponSlotManager.rightHandSlot.transform.position,
                                                            player.cameraHandler.cameraPivotTransform.rotation); //Maybe delete transform on cameraPivotTransfor.
                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                    spellDamageCollider.character = player;
                    spellDamageCollider.teamIDNumeber = player.playerStatsManager.teamIDNumeber;

                    rb = instantiatedSpellFX.GetComponent<Rigidbody>();

                    //spelldDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>(); (later), now only flat damage

                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                    }
                    else
                    {
                        instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x,
                                                                                    player.playerStatsManager.transform.eulerAngles.y, 0);
                    }

                    rb.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                    rb.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                    rb.useGravity = isEffectedByGravity;
                    rb.mass = projectileMass;
                    instantiatedSpellFX.transform.parent = null;
                }
            }
            
            //HANDLE THE PROCESS IF THE CASTER IS AN A.I
            else
            {

            }
        }

    }

}
