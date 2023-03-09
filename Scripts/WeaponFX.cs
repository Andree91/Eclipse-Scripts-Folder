using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem normalWeaponTrail;
        //other weapon trails
        //fireWeaponTrail
        public ParticleSystem fireWeaponBuff;
        public ParticleSystem fireWeaponTrail;
        //lightningWeaponTrail
        public ParticleSystem lightningWeaponBuff;
        //etc..
        public ParticleSystem normalWeaponSpark;

        public void PlayWeaponFX()
        {
            normalWeaponTrail.Stop();

            if (normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
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

        public void ActivateWeaponFireBuffFX()
        {
            if (fireWeaponBuff != null)
            {
                fireWeaponBuff.Stop();

                if (fireWeaponBuff.isStopped)
                {
                    fireWeaponBuff.Play();
                }
            }
        }

        public void ActivateWeaponLightningBuffFX()
        {
            if (lightningWeaponBuff != null)
            {
                lightningWeaponBuff.Stop();

                if (lightningWeaponBuff.isStopped)
                {
                    lightningWeaponBuff.Play();
                }
            }
        }

        public void DeActivateWeaponFireBuffFX()
        {
            if (fireWeaponBuff != null)
            {
                fireWeaponBuff.Stop();
            }
        }

        public void DeActivateWeaponLightningBuffFX()
        {
            if (fireWeaponBuff != null)
            {
                fireWeaponBuff.Stop();
            }
        }

        public void PlayFireWeaponTrailFX()
        {
            fireWeaponTrail.Stop();

            if (fireWeaponTrail.isStopped)
            {
                fireWeaponTrail.Play();
            }
        }
    }
}
