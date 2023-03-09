using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Spells/Staff Magic Spell")]
    public class StaffMagicSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;
        public float fireDamage;
        public float magicDamage;

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
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.leftHandSlot.currentWeaponModel.transform);
                instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
                instantiatedWarmUpSpellFX.transform.localPosition += new Vector3 (0.277999997f, -0.225999996f, -0.744000018f);
                if (!character.isRiding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
                }
                else if (character.isRiding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation("OH_L_Magic_Attack_01", false, false, character.isUsingLeftHand);
                }
            }
            else
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.transform);
                instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
                instantiatedWarmUpSpellFX.transform.localPosition += new Vector3(0.277999997f, -0.225999996f, -0.744000018f);
                if (!character.isRiding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
                }
                else if (character.isRiding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation("OH_R_Magic_Attack_01", false, false, character.isUsingLeftHand);
                }
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
                                                            player.playerWeaponSlotManager.leftHandSlot.currentWeaponModel.transform.position,
                                                            player.cameraHandler.cameraPivotTransform.rotation); //Maybe delete transform on cameraPivotTransfor.
                    //instantiatedSpellFX.transform.position += new Vector3 (0, 0.4f, 0);
                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponentInChildren<SpellDamageCollider>();
                    spellDamageCollider.character = player;
                    spellDamageCollider.teamIDNumeber = player.playerStatsManager.teamIDNumeber;

                    rb = instantiatedSpellFX.GetComponentInChildren<Rigidbody>();
                    //And make intiation location, if using staff spell have to laucnh at the tip of the staff

                    //spelldDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>(); (later), now only flat damage

                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        Vector3 spellLockOnTarget = player.cameraHandler.currentLockOnTarget.transform.position + new Vector3 (0, 1f, 0);
                        //instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                        instantiatedSpellFX.transform.LookAt(spellLockOnTarget);
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
                    Destroy(instantiatedSpellFX, 10f);
                }
                else
                {
                    GameObject instantiatedSpellFX = Instantiate(spellCastFX,
                                                            player.playerWeaponSlotManager.rightHandSlot.transform.position,
                                                            player.cameraHandler.cameraPivotTransform.rotation); //Maybe delete transform on cameraPivotTransfor.
                    //instantiatedSpellFX.transform.position += new Vector3(0, 0.4f, 0);
                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponentInChildren<SpellDamageCollider>();
                    spellDamageCollider.character = player;
                    spellDamageCollider.teamIDNumeber = player.playerStatsManager.teamIDNumeber;

                    rb = instantiatedSpellFX.GetComponentInChildren<Rigidbody>();

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
                    Destroy(instantiatedSpellFX, 10f);
                }
            }

            //HANDLE THE PROCESS IF THE CASTER IS AN A.I
            else
            {
                EnemyManager enemy = character as EnemyManager;
                GameObject instantiatedSpellFX = Instantiate(spellCastFX,
                                                            character.characterWeaponSlotManager.rightHandSlot.transform.position,
                                                            Quaternion.identity); //Maybe delete transform on cameraPivotTransfor.
                                                                                                                 //instantiatedSpellFX.transform.position += new Vector3(0, 0.4f, 0);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponentInChildren<SpellDamageCollider>();
                spellDamageCollider.character = enemy;
                spellDamageCollider.teamIDNumeber = enemy.characterStatsManager.teamIDNumeber;

                rb = instantiatedSpellFX.GetComponentInChildren<Rigidbody>();

                //spelldDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>(); (later), now only flat damage

                if (enemy.currentTarget != null)
                {
                    Quaternion spellRotation = Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - instantiatedSpellFX.gameObject.transform.position);
                    instantiatedSpellFX.transform.rotation = spellRotation;
                }

                rb.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                rb.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                rb.useGravity = isEffectedByGravity;
                rb.mass = projectileMass;
                instantiatedSpellFX.transform.parent = null;
                Destroy(instantiatedSpellFX, 10f);
            }
        }

    }

}
