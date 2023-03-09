using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class RangeProjectileDamageCollider : DamageCollider
    {
        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPeneratedSurface;
        float timeUntilDestroy = 20f;
        //protected GameObject penetratedProjectile;

        Rigidbody arrowRigidbody;
        CapsuleCollider arrowCapsuleCollider;

        //private List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();

        protected override void Awake()
        {
            //base.Awake();
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = false;
            damageCollider.enabled = enabledDamageColliderOnStartUp; //true
            arrowRigidbody = GetComponent<Rigidbody>();
            arrowCapsuleCollider = GetComponent<CapsuleCollider>();
        }

        void FixedUpdate()
        {
            if (arrowRigidbody.velocity != Vector3.zero)
            {
                //Debug.Log("Arrow Velocity is " + arrowRigidbody.velocity);
                arrowRigidbody.rotation = Quaternion.LookRotation(arrowRigidbody.velocity); //, Vector3.down); this doesn't work yet have to fix later!
                //Debug.Log(arrowRigidbody.rotation);
            }
        }

        // protected override void OnTriggerEnter(Collider collision) 
        // {
        //     if (collision.gameObject.layer == 16) 
        //     {
        //         Debug.Log(collision.gameObject.layer);
        //         return; 
        //     }

        //     if (collision.tag == "Interactable") { return; }

        //     //if (collision.tag == "Character")
        //     if (collision.tag == "Damageable Character") //if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
        //     {
        //         shieldHasBeenHit = false;
        //         hasBeenParried = false;

        //         //CharacterStatsManager enemyStats = collision.GetComponentInParent<CharacterStatsManager>();
        //         CharacterManager enemy = collision.GetComponentInParent<CharacterManager>();
        //         //CharacterEffectsManager enemyEffects = collision.GetComponentInParent<CharacterEffectsManager>();

        //         if (enemy != null)
        //         {
        //             if (charactersDamagedDuringThisCalculation.Contains(enemy)) { return; }

        //             charactersDamagedDuringThisCalculation.Add(enemy);

        //             if (enemy.characterStatsManager.teamIDNumeber == teamIDNumeber) { return; }

        //             CheckForParry(enemy);
        //             CheckForBlocking(enemy);
        //         }

        //         if (enemy.characterStatsManager != null)
        //         {
        //             if (enemy.characterStatsManager.teamIDNumeber == teamIDNumeber) { return; }

        //             if (hasBeenParried) { return; }

        //             if (shieldHasBeenHit) { return; }

        //             enemy.characterStatsManager.poiseResetTimer = enemy.characterStatsManager.totalPoiseResetTime;
        //             enemy.characterStatsManager.totalPoiseDefence = enemy.characterStatsManager.totalPoiseDefence - poiseBreak;
        //             //Debug.Log($"Players poise is currently {playerStats.totalPoiseDefence}");

        //             //Detects where on the collider our weapon first make contact
        //             Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //             float directionHitFrom = (Vector3.SignedAngle(character.transform.forward, enemy.transform.forward, Vector3.up));
        //             ChooseWhichDirectionDamageCameFrom(directionHitFrom);
        //             enemy.characterEffectsManager.PlayBloodSplatterFX(contactPoint);

        //             if (enemy.characterStatsManager.totalPoiseDefence > poiseBreak)
        //             {
        //                 enemy.characterStatsManager.TakeDamageNoAnimation(physicalDamage, fireDamage, character);
        //             }
        //             else
        //             {
        //                 enemy.characterStatsManager.TakeDamage(physicalDamage, fireDamage, currentDamageAnimation, character);
        //             }
        //         }
        //     }

        //     if (collision.tag == "Illusionary Wall")
        //     {
        //         IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

        //         illusionaryWall.wallHasBeenHit = true;
        //     }

        //     if (!hasAlreadyPeneratedSurface && penetratedProjectile == null)
        //     {
        //         hasAlreadyPeneratedSurface = true;
        //         Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //         GameObject penetratedArrow = Instantiate(ammoItem.penetratedModel, contactPoint, Quaternion.Euler(0, 0, 0));

        //         penetratedProjectile = penetratedArrow;
        //         penetratedArrow.transform.parent = collision.transform;
        //         penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward); 
        //     }

        //     Destroy(transform.root.gameObject);
        // }

        void OnCollisionEnter(Collision collision)
        {
            // if (collision.gameObject.layer == 16)
            // {
            //     Debug.Log(collision.gameObject.layer);
            //     return;
            // }

            // if (collision.gameObject.tag == "Interactable") { return; }

            Debug.Log("Name of collision tag is " + collision.gameObject.tag);

            //if (collision.tag == "Character")

            //if (collision.gameObject.tag == "Damageable Character") //if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
            //{
            shieldHasBeenHit = false;
            hasBeenParried = false;

            //CharacterStatsManager enemyStats = collision.GetComponentInParent<CharacterStatsManager>();
            CharacterManager enemy = collision.gameObject.GetComponent<CharacterManager>();
            //CharacterEffectsManager enemyEffects = collision.GetComponentInParent<CharacterEffectsManager>();

            if (enemy != null)
            {
                Debug.Log("Here is arrow colliding with enemy");
                EnemyManager aiCharacter = enemy as EnemyManager;

                if (charactersDamagedDuringThisCalculation.Contains(enemy)) { return; }

                charactersDamagedDuringThisCalculation.Add(enemy);

                if (enemy.characterStatsManager.teamIDNumeber == teamIDNumeber) { return; }

                CheckForParry(enemy);
                CheckForBlocking(enemy);

                if (hasBeenParried) { return; }

                if (shieldHasBeenHit)
                {
                    hasAlreadyPeneratedSurface = true;
                    arrowRigidbody.isKinematic = true;
                    arrowCapsuleCollider.enabled = false;

                    if (enemy.characterWeaponSlotManager.leftHandDamageCollider != null)
                    {
                        if (enemy.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                        {
                            gameObject.transform.position = enemy.characterWeaponSlotManager.leftHandDamageCollider.transform.position;//collision.GetContact(0).point;
                            gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
                            gameObject.transform.parent = enemy.characterWeaponSlotManager.leftHandDamageCollider.transform;//collision.collider.transform;
                        }
                        else
                        {
                            arrowRigidbody.isKinematic = false;
                            arrowCapsuleCollider.enabled = true;
                            hasAlreadyPeneratedSurface = false;
                            arrowRigidbody.AddForce(gameObject.transform.forward * -0.00005f);
                            arrowRigidbody.AddForce(gameObject.transform.up * -0.0001f);
                            gameObject.transform.LookAt(-gameObject.transform.forward);
                        }
                    }
                    else if (enemy.characterWeaponSlotManager.rightHandDamageCollider != null && enemy.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                    {
                        gameObject.transform.position = enemy.characterWeaponSlotManager.rightHandDamageCollider.transform.position;//collision.GetContact(0).point;
                        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
                        gameObject.transform.parent = enemy.characterWeaponSlotManager.rightHandDamageCollider.transform;//collision.collider.transform;
                    }
                    DestroyArrowModel(timeUntilDestroy);
                    return;
                }

                enemy.characterStatsManager.poiseResetTimer = enemy.characterStatsManager.totalPoiseResetTime;
                enemy.characterStatsManager.totalPoiseDefence = enemy.characterStatsManager.totalPoiseDefence - poiseBreak;
                //Debug.Log($"Players poise is currently {playerStats.totalPoiseDefence}");

                //Detects where on the collider our weapon first make contact
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                if (character != null)
                {
                    float directionHitFrom = (Vector3.SignedAngle(character.transform.forward, enemy.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                }
                enemy.characterEffectsManager.PlayBloodSplatterFX(contactPoint);

                if (enemy.characterStatsManager.totalPoiseDefence > poiseBreak)
                {
                    enemy.characterStatsManager.TakeDamageNoAnimation(physicalDamage, fireDamage, lightningDamage, character);
                }
                else
                {
                    enemy.characterStatsManager.TakeDamage(physicalDamage, fireDamage, lightningDamage, currentDamageAnimation, character);
                }

                if (aiCharacter != null && !aiCharacter.isNPC)
                {
                    // If Target is A.I, Receives new damage, Target is one who is doing damage
                    aiCharacter.currentTarget = character;
                }
            }
            //}

            if (collision.gameObject.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }

            if (!hasAlreadyPeneratedSurface)// && penetratedProjectile == null)
            {
                //CharacterManager enemy = collision.gameObject.GetComponent<CharacterManager>();

                if (enemy != null)
                {
                    //if (collision.gameObject.layer == 9) { return; }
                    if (enemy.characterStatsManager.teamIDNumeber != teamIDNumeber)
                    {
                        hasAlreadyPeneratedSurface = true;
                        arrowRigidbody.isKinematic = true;
                        arrowCapsuleCollider.enabled = false;

                        gameObject.transform.position = collision.GetContact(0).point;
                        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
                        gameObject.transform.parent = collision.collider.transform;
                        DestroyArrowModel(timeUntilDestroy);
                    }
                }
                else
                {
                    //if (collision.gameObject.layer == 9) { return; }
                    hasAlreadyPeneratedSurface = true;
                    arrowRigidbody.isKinematic = true;

                    if (collision.gameObject.tag != "Animal")
                    {
                        arrowCapsuleCollider.enabled = false;
                    }
                    else
                    {
                        attackTriggerAnimal.SetActive(true);
                        Invoke("DisableaAttackTriggerAnimal", 0.5f);
                    }

                    gameObject.transform.position = collision.GetContact(0).point;
                    gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
                    gameObject.transform.parent = collision.collider.transform;
                    DestroyArrowModel(timeUntilDestroy);
                }


                //Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                // GameObject penetratedArrow = Instantiate(ammoItem.penetratedModel, contactPoint, Quaternion.Euler(0, 0, 0));

                // penetratedProjectile = penetratedArrow;
                // penetratedArrow.transform.parent = collision.transform;
                // penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
            }

            //Destroy(transform.root.gameObject);
        }

        void DisableaAttackTriggerAnimal()
        {
            attackTriggerAnimal.SetActive(false);
        }

        void DestroyArrowModel(float timeUntilDestroy)
        {
            Destroy(transform.gameObject, timeUntilDestroy);
        }


    }
}
