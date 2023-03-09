using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Weapon Buff Effect")]
    public class WeaponBuffEffect : CharacterEffect
    {
        [Header("Buff Info")]
        [SerializeField] BuffClass buffClass;
        [SerializeField] float lengthOfBuff = 45f;
        public float timeRemainingOnBuff;
        [HideInInspector] public bool isRightHandBuff;
        [SerializeField] bool buffHasStarted;

        [Header("Buff SFX")]
        [SerializeField] AudioClip buffAmbientSound;
        [SerializeField] float ambientSoundVolume = 0.3f;

        [Header("Damage Info")]
        [SerializeField] float buffBaseDamagePercentageMultiplier = 15f; // How much % weapons base damage is multiplied, before any other effects

        [Header("Poise Buff")]
        [SerializeField] bool hasBuffPoiseDamage;
        [SerializeField] float buffBasePoiseDamagePercentageMultiplier = 15f;

        WeaponManager weaponManager;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (!buffHasStarted)
            {
                timeRemainingOnBuff = lengthOfBuff;
                buffHasStarted = true;

                weaponManager = character.characterWeaponSlotManager.rightHandDamageCollider.GetComponentInParent<WeaponManager>();
                weaponManager.audioSource.loop = true;
                weaponManager.audioSource.clip = buffAmbientSound;
                weaponManager.audioSource.volume = ambientSoundVolume;

                float baseWeaponDamage = 
                    weaponManager.damageCollider.physicalDamage +
                    weaponManager.damageCollider.fireDamage +
                    weaponManager.damageCollider.magicDamage +
                    weaponManager.damageCollider.lightningDamage +
                    weaponManager.damageCollider.holyDamage;

                float physicalBuffDamage = 0;
                float fireBuffDamage = 0;
                //float magicBuffDamage = 0;
                float lightingBuffDamage = 0;
                //float holyBuffDamage = 0;
                float poiseBuffDamage = 0;

                if (hasBuffPoiseDamage)
                {
                    poiseBuffDamage = weaponManager.damageCollider.poiseBreak * (buffBasePoiseDamagePercentageMultiplier / 100f);
                }

                switch (buffClass)
                {
                    case BuffClass.Physical:
                        physicalBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100f);
                        break;
                    case BuffClass.Fire:
                        fireBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100f);
                        break;
                    case BuffClass.Lightning:
                        lightingBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100f);
                        break;
                    case BuffClass.Frost:
                        break;
                    case BuffClass.Blood:
                        break;
                    case BuffClass.Holy:
                        //holyBuffDamage = baseWeaponDamage + (buffBaseDamagePercentageMultiplier / 100f);
                        break;
                    case BuffClass.Dark:
                        break;
                    case BuffClass.Magic:
                        //magicBuffDamage = baseWeaponDamage + (buffBaseDamagePercentageMultiplier / 100f);
                        break;
                    case BuffClass.Poise:
                        break;
                    default:
                        break;
                }

                weaponManager.BuffWeapon(buffClass, physicalBuffDamage, fireBuffDamage, lightingBuffDamage, poiseBuffDamage);

                if (buffHasStarted)
                {
                    timeRemainingOnBuff -= 1f;

                    if (timeRemainingOnBuff <= 0)
                    {
                        weaponManager.DeBuffWeapon();

                        if (isRightHandBuff)
                        {
                            character.characterEffectsManager.rightWeaponBuffEffect = null;
                        }
                    }
                }
            }
        }
    }
}