using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        public float explosiveRadius = 0.5f;
        public int explosionSplashDamage = 10;

        bool hasCollied = false;

        CharacterManager spellTarget;
        Rigidbody rb;

        Vector3 impactNormal; //Used to rotate impact particles

        protected override void Awake() 
        {
            rb = GetComponent<Rigidbody>();
            physicalDamage = 1;
        }

        void Start() 
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;

            if (muzzleParticles != null)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f); //How long muzzle particles last
            }
        }

        void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collision" + other.gameObject.name);

            if (other.collider.isTrigger) { return; }

            if (!hasCollied)
            {
                spellTarget = other.transform.GetComponentInParent<CharacterManager>();

                if (spellTarget != null)
                {
                    EnemyManager aiCharacter = spellTarget as EnemyManager;

                    if (charactersDamagedDuringThisCalculation.Contains(spellTarget)) { return; }

                    charactersDamagedDuringThisCalculation.Add(spellTarget);

                    if (spellTarget.characterStatsManager.teamIDNumeber == teamIDNumeber)
                    {
                        return;
                    }
                    else if (spellTarget.characterStatsManager.teamIDNumeber != teamIDNumeber)
                    {
                        //CheckForParry(spellTarget);
                        CheckForBlocking(spellTarget);

                        if (shieldHasBeenHit) { return; }

                        spellTarget.characterStatsManager.poiseResetTimer = spellTarget.characterStatsManager.totalPoiseResetTime;
                        spellTarget.characterStatsManager.totalPoiseDefence = spellTarget.characterStatsManager.totalPoiseDefence - poiseBreak;
                        //Debug.Log($"Players poise is currently {playerStats.totalPoiseDefence}");

                        //Detects where on the collider our weapon first make contact
                        Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                        float directionHitFrom = (Vector3.SignedAngle(character.transform.forward, spellTarget.transform.forward, Vector3.up));
                        ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                        if (physicalDamage != 0)
                        {
                            spellTarget.characterEffectsManager.PlayBloodSplatterFX(contactPoint); //JUST TEMP FIX, HAVE TO FIND OUT WHY THERE IS STILL DAMAGE ANIMATION AND BLOOD SPLATTER SOMETIMES
                        }
                        spellTarget.characterEffectsManager.InterruptEffect();

                        //Deals Damage
                        DealDamage(spellTarget.characterStatsManager);

                        if (aiCharacter != null && !aiCharacter.isNPC)
                        {
                            // If Target is A.I, Receives new damage, Target is one who is doing damage
                            aiCharacter.currentTarget = character;
                        }

                        //spellTarget.characterStatsManager.TakeDamage(0, fireDamage, currentDamageAnimation, character);
                    }
                }

                if (other.gameObject.tag == "Illusionary Wall")
                {
                    IllusionaryWall illusionaryWall = other.transform.GetComponent<IllusionaryWall>();

                    illusionaryWall.wallHasBeenHit = true;
                }

                hasCollied = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 2f);
                Destroy(gameObject, 0.2f);

            }
        }

        protected override void OnTriggerEnter(Collider other) 
        {
            if (!hasCollied)
            {
                if (other.gameObject.tag == "Weapon") { return; }

                spellTarget = other.transform.GetComponentInParent<CharacterManager>();

                if (spellTarget != null)
                {
                    EnemyManager aiCharacter = spellTarget as EnemyManager;
                    
                    if (charactersDamagedDuringThisCalculation.Contains(spellTarget)) { return; }

                    charactersDamagedDuringThisCalculation.Add(spellTarget);

                    if (spellTarget.characterStatsManager.teamIDNumeber == teamIDNumeber)
                    {
                        return;
                    }
                    else if (spellTarget.characterStatsManager.teamIDNumeber != teamIDNumeber)
                    {
                        //CheckForParry(spellTarget);
                        CheckForBlocking(spellTarget);

                        if (shieldHasBeenHit) { return; }

                        spellTarget.characterStatsManager.poiseResetTimer = spellTarget.characterStatsManager.totalPoiseResetTime;
                        spellTarget.characterStatsManager.totalPoiseDefence = spellTarget.characterStatsManager.totalPoiseDefence - poiseBreak;
                        //Debug.Log($"Players poise is currently {playerStats.totalPoiseDefence}");

                        //Detects where on the collider our weapon first make contact
                        Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                        float directionHitFrom = (Vector3.SignedAngle(character.transform.forward, spellTarget.transform.forward, Vector3.up));
                        ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                        if (physicalDamage != 0)
                        {
                            spellTarget.characterEffectsManager.PlayBloodSplatterFX(contactPoint); //JUST TEMP FIX, HAVE TO FIND OUT WHY THERE IS STILL DAMAGE ANIMATION AND BLOOD SPLATTER SOMETIMES
                        }
                        spellTarget.characterEffectsManager.InterruptEffect();

                        //Deals Damage
                        DealDamage(spellTarget.characterStatsManager);

                        if (aiCharacter != null && !aiCharacter.isNPC)
                        {
                            // If Target is A.I, Receives new damage, Target is one who is doing damage
                            aiCharacter.currentTarget = character;
                        }
                    }
                }

                if (other.gameObject.tag == "Illusionary Wall")
                {
                    IllusionaryWall illusionaryWall = other.transform.GetComponent<IllusionaryWall>();

                    illusionaryWall.wallHasBeenHit = true;
                }

                
                hasCollied = true;
                Explode();
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 2f);
                Destroy(gameObject, 0.2f);
            }
        }

        void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

            foreach (Collider objectInExplosive in characters)
            {
                if (!objectInExplosive.isTrigger)
                {
                    CharacterStatsManager character = objectInExplosive.GetComponent<CharacterStatsManager>();

                    if (character != null && character.teamIDNumeber != teamIDNumeber)
                    {
                        character.TakeDamage(0, explosionSplashDamage, 0, currentDamageAnimation, base.character);
                    }
                }
            }
        }
    }
}
