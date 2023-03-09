using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Explosive Damage & Radius")]
        public float explosiveRadius = 1.5f;
        public int explosionDamage;
        public int explosionSplashDamage;
        //magicExplosiveDamage
        //lightningExplosiveDamage etc...

        public Rigidbody bombRigidbody;
        bool hasCollied = false;
        public GameObject impactParticles;

        protected override void Awake() 
        {
            damageCollider = GetComponent<Collider>();
            bombRigidbody = GetComponent<Rigidbody>();
        }

        void OnCollisionEnter(Collision collision) 
        {
            if (!hasCollied)
            {
                hasCollied = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                //impactParticles.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                Explode();

                CharacterStatsManager character =  collision.transform.GetComponent<CharacterStatsManager>();

                if (character != null && character.teamIDNumeber != teamIDNumeber)
                {
                    //Check for friendly fire, team id
                    character.TakeDamage(0, explosionDamage, 0, currentDamageAnimation, base.character);
                    attackTriggerAnimal.gameObject.SetActive(true);
                }

                Destroy(impactParticles, 5f);
                Destroy(transform.parent.parent.gameObject);
            }

            if (collision.gameObject.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }

        void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

            foreach (Collider objectInExplosive in characters)
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
