using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("Buff FX")]
        [SerializeField] GameObject physicalBuffFX;
        [SerializeField] GameObject fireBuffFX;
        [SerializeField] GameObject lightningBuffFX;
        [SerializeField] GameObject poiseBuffFX;

        [Header("Trail FX")]
        [SerializeField] ParticleSystem defaultTrailFX;
        [SerializeField] ParticleSystem fireTrailFX;
        [SerializeField] ParticleSystem lightningTrailFX;

        [Header("Buffed Blades")]
        [SerializeField] GameObject fireBuffBlade;

        [Header("Sparks FX")]
        [SerializeField] ParticleSystem normalWeaponSpark;

        bool weaponIsBuffed;
        BuffClass weaponBuffClass;

        [HideInInspector] public MeleeWeaponDamageCollider damageCollider;
        public AudioSource audioSource;

        void Awake()
        {
            damageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void BuffWeapon(BuffClass buffClass, float physicalAddedBuffDamage, float fireAddedBuffDamage, float lightningAddedBuffDamage, float poiseAddedBuffDamage)
        {
            // Reset any active buff, but only weapon buffs
            DeBuffWeapon();
            weaponIsBuffed = true;
            weaponBuffClass = buffClass;
            audioSource.Play();

            switch (weaponBuffClass)
            {
                case BuffClass.Physical:
                    physicalBuffFX.SetActive(true);
                    //damageCollider.physicalBuffDamage = physicalAddedBuffDamage;
                    break;
                case BuffClass.Fire:
                    fireBuffFX.SetActive(true);
                    fireBuffBlade.SetActive(true);
                    break;
                case BuffClass.Lightning:
                    lightningBuffFX.SetActive(true);
                    break;
                case BuffClass.Poise:
                    if (poiseBuffFX == null)
                        return;
                    poiseBuffFX.SetActive(true);
                    break;
                default:
                    break;
            }

            damageCollider.physicalBuffDamage = physicalAddedBuffDamage;
            damageCollider.fireBuffDamage = fireAddedBuffDamage;
            damageCollider.lightningBuffDamage = lightningAddedBuffDamage;
            damageCollider.poiseBuffDamage = poiseAddedBuffDamage;
        }

        public void DeBuffWeapon()
        {
            weaponIsBuffed = false;
            audioSource.Stop();

            if (physicalBuffFX != null)
            {
                physicalBuffFX.SetActive(false);
            }
            if (fireBuffFX != null)
            {
                fireBuffFX.SetActive(false);
                fireBuffBlade.SetActive(false);
            }
            if (lightningBuffFX != null)
            {
                lightningBuffFX.SetActive(false);
            }

            damageCollider.physicalBuffDamage = 0;
            damageCollider.fireBuffDamage = 0;
            damageCollider.lightningBuffDamage = 0;
            damageCollider.poiseBuffDamage = 0;
        }

        public void PlayWeaponTrailFX()
        {
            if (weaponIsBuffed)
            {
                switch (weaponBuffClass)
                {
                    // IF weapon is physically buffed, play just defaultWeaponTrail
                    case BuffClass.Physical:
                        if (defaultTrailFX == null)
                            return;
                        defaultTrailFX.Play();
                        break;
                    case BuffClass.Fire:
                        if (fireTrailFX == null)
                            return;
                        fireTrailFX.Play();
                        break;
                    case BuffClass.Lightning:
                        if (lightningTrailFX = null)
                            return;
                        lightningTrailFX.Play();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                defaultTrailFX.Stop();

                if (defaultTrailFX.isStopped)
                {
                    defaultTrailFX.Play();
                }
            }
        }

        public void PlayWeaponSparkFX()
        {
            normalWeaponSpark.Stop();

            if (normalWeaponSpark.isStopped)
            {
                normalWeaponSpark.Play();
                ParticleSystem[] extraSparks = normalWeaponSpark.GetComponentsInChildren<ParticleSystem>();
                if (extraSparks.Length > 0)
                {
                    foreach (ParticleSystem spark in extraSparks)
                    {
                        spark.Play();
                    }
                }
            }
        }
    }
}