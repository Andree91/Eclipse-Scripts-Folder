using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager character;
        public GameObject attackTriggerAnimal;
        protected Collider damageCollider;
        protected CapsuleCollider wallCollider;
        public bool enabledDamageColliderOnStartUp = false;
        public bool isTrapArrow;

        [Header("Team I.D")]
        public int teamIDNumeber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int physicalDamage = 25;
        public int fireDamage;
        public int magicDamage;
        public int lightningDamage;
        public int holyDamage; //Could be dark holy and Light holy

        [Header("Guard Break Modifier")]
        public float guardBreakModifier = 1;

        [Header("Flags")]
        protected bool shieldHasBeenHit;
        protected bool hasBeenParried;

        [Header("Damage Animations")]
        protected string currentDamageAnimation;

        public List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();

        protected virtual void Awake() 
        {
            damageCollider = GetComponent<Collider>();
            wallCollider = GetComponentInChildren<CapsuleCollider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStartUp;
            if (wallCollider != null)
            {
                wallCollider.enabled = enabledDamageColliderOnStartUp;
            }
        }

        public void EnableDamageCollider()
        {
            damageCollider.gameObject.SetLayer(13);
            damageCollider.enabled = true;
            if (wallCollider != null)
            {
                wallCollider.enabled = true;
            }
            character.canBeParried = true; //CHANGE FOR OWN ANIMATION EVENT LATER
            attackTriggerAnimal.SetActive(true);
            //Debug.Log(damageCollider.gameObject.layer);
        }

        public void DisableDamageCollider()
        {
            if (charactersDamagedDuringThisCalculation.Count > 0)
            {
                charactersDamagedDuringThisCalculation.Clear();
            }

            damageCollider.enabled = false;
            if (wallCollider != null)
            {
                wallCollider.enabled = false;
            }
            
            character.canBeParried = false; //CHANGE FOR OWN ANIMATION EVENTS LATER
            if (attackTriggerAnimal != null)
            {
                attackTriggerAnimal.SetActive(false);
            }
        }

        public void ChangeCurrentWeaponDamage(float weaponDamageMultiplier)
        {
            physicalDamage = Mathf.RoundToInt(physicalDamage * weaponDamageMultiplier);
        }

        protected virtual void OnTriggerEnter(Collider collision) 
        {
            if (character != null && character.isDead) { return; }

            // if (character != null)
            // {
            //     if (character.isRiding)
            //     {
            //         //attackTriggerAnimal.SetActive(false);
            //     }
            // }
            if (collision.gameObject.layer == 17 && teamIDNumeber == 0) // So the player won't hurt they own Animal
            { 
                attackTriggerAnimal.SetActive(false);
                return; 
            }

            if (collision.tag == "Animal" && teamIDNumeber == 0) // So the player won't hurt they own Animal
            {
                Debug.Log(collision.tag);
                attackTriggerAnimal.SetActive(false);
                return;
            }

            //if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))                                //if (collision.tag == "Character")
            if (collision.tag == "Damageable Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                // Collision Debub
                //Debug.Log($"Here is collision with {collision.name}");

                //CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemy = collision.GetComponentInParent<CharacterManager>();    //CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                //CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();

                if (enemy != null)
                {
                    EnemyManager aiCharacter = enemy as EnemyManager;

                    if (charactersDamagedDuringThisCalculation.Contains(enemy)) { return; }
                    
                    charactersDamagedDuringThisCalculation.Add(enemy);

                    if (enemy.characterStatsManager.teamIDNumeber == teamIDNumeber) { return; }

                    CheckForParry(enemy);
                    CheckForBlocking(enemy);

                    if (hasBeenParried) { return; }

                    if (shieldHasBeenHit) { return; }

                    enemy.characterStatsManager.poiseResetTimer = enemy.characterStatsManager.totalPoiseResetTime;
                    enemy.characterStatsManager.totalPoiseDefence = enemy.characterStatsManager.totalPoiseDefence - poiseBreak;
                    //Debug.Log($"Players poise is currently {playerStats.totalPoiseDefence}");

                    //Detects where on the collider our weapon first make contact
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(character.transform.forward, enemy.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    if (physicalDamage != 0)
                    {
                        enemy.characterEffectsManager.PlayBloodSplatterFX(contactPoint); //JUST TEMP FIX, HAVE TO FIND OUT WHY THERE IS STILL DAMAGE ANIMATION AND BLOOD SPLATTER SOMETIMES
                    }
                    enemy.characterEffectsManager.InterruptEffect();

                    //Deals Damage
                    DealDamage(enemy.characterStatsManager);
                    
                    if (aiCharacter != null && !aiCharacter.isNPC)
                    {
                        // If Target is A.I, Receives new damage, Target is one who is doing damage
                        aiCharacter.currentTarget = character;
                    }

                }

            }

            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                character.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlocking(CharacterManager enemyManager)
        {
            CharacterStatsManager enemyShield = enemyManager.characterStatsManager;
            Vector3 directionFromPlayerToEnemy = new Vector3(0.0f, 0.0f, 0.0f);
            if (character != null)
            {
                directionFromPlayerToEnemy = (character.transform.position - enemyManager.transform.position);
            }
            if (isTrapArrow)
            {
                directionFromPlayerToEnemy = (transform.position - enemyManager.transform.position);
            }
            //Debug.Log("Direction from player to enemy is " + directionFromPlayerToEnemy);
            float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

            if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
            {
                shieldHasBeenHit = true;
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * enemyShield.blockingPhysicalDamageAbsorption) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * enemyShield.blockingFireDamageAbsorption) / 100;
                float magicDamageAfterBlock = magicDamage - (magicDamage * enemyShield.blockingMagicDamageAbsorption) / 100;
                float lightningDamageAfterBlock = lightningDamage -(lightningDamage * enemyShield.blockingLightningDamageAbsorption) / 100;
                float holyDamageAfterBlock = holyDamage - (holyDamage * enemyShield.blockingHolyDamageAbsorption) / 100;

                //Attemp to block the attack (CHECK FOR GUARD BREAK)
                enemyManager.characterCombatManager.AttemptToBlock(this, physicalDamage, fireDamage, magicDamage, 
                                                                    lightningDamage, holyDamage, "Block Guard");

                enemyShield.TakeDamageAfterBlock(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), Mathf.RoundToInt(lightningDamageAfterBlock),character);
            }
        }

        protected virtual void DealDamage(CharacterStatsManager enemyStats)
        {
            //Get Attack Type from character
            //Apply Damage multipliers
            //Pass damage

            //float finalDamage;
            float finalPhysicalDamage = physicalDamage;
            float finalFireDamage = fireDamage;
            float finalLightningDamage = lightningDamage;
            //If character is using right hand
            if (character.isUsingRightHand)
            {
                if (character.characterCombatManager.currentAttackType == AttackType.LightAttack01)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.lightAttack01DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.LightAttack02)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.lightAttack02DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack01)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.heavyAttack01DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack02)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.heavyAttack02DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.RunningAttack)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.runningAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.JumpingAttack)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.jumpingAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.PlungingAttack)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.rightWeapon.plungingAttackDamageModifier;
                }
            }
            //If character is using left hand
            else
            {
                if (character.characterCombatManager.currentAttackType == AttackType.LightAttack01)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.lightAttack01DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.LightAttack02)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.lightAttack02DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack01)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.heavyAttack01DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.HeavyAttack02)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.heavyAttack02DamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.RunningAttack)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.runningAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.JumpingAttack)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.jumpingAttackDamageModifier;
                }
                else if (character.characterCombatManager.currentAttackType == AttackType.PlungingAttack)
                {
                    finalPhysicalDamage = finalPhysicalDamage * character.characterInventoryManager.leftWeapon.plungingAttackDamageModifier;
                }
            }


            //Deal Modified Final Amount Of Damage
            if (enemyStats.totalPoiseDefence > poiseBreak)
            {
                enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), Mathf.RoundToInt(finalLightningDamage), character);
            }
            else
            {
                if (finalPhysicalDamage != 0)
                {
                    //Debug.Log("Now we are dealing physical damage");
                    enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), Mathf.RoundToInt(finalLightningDamage), currentDamageAnimation, character);
                }
                else if (fireDamage != 0)
                {
                    //Debug.Log("Now we are dealing fire damage");
                    enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), Mathf.RoundToInt(finalFireDamage), Mathf.RoundToInt(finalLightningDamage), currentDamageAnimation, character);
                }
                else if (enemyStats.character.characterWeaponSlotManager.rightHandDamageCollider != null)
                {
                    enemyStats.character.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                }
            }
        }

        //Check this function, need more animations and testing
        protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
        {
            //Debug.Log($"Attack direction was {direction} angle");

            if (direction >= 145 && direction <= 180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if (direction <= -145 && direction >= -180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if (direction >= -45 && direction <= 45)
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            else if (direction >= 144 && direction <= -45)
            {
                currentDamageAnimation = "Damage_Right_01";
            }
            else if (direction >= 45 && direction <= 144)
            {
                currentDamageAnimation = "Damage_Left_01";
            }
            else
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
        }
    }
}
