using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterCombatManager : MonoBehaviour
    {
        public CharacterManager character;

        [Header("Combat Transforms")]
        public Transform backstabReceiverTransform;
        public Transform riposteReceiverTransform;

        public LayerMask characterLayer;
        public float criticalAttackRange = 0.5f;

        [Header("Last Amount Poise Damage Taken")]
        public int previousPoiseDamageTaken = 0; 

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Attack Animations")]
        public string oh_light_attack_01 = "OH_Light_Attack_01";
        public string oh_light_attack_02 = "OH_Light_Attack_02";
        public string oh_light_attack_03 = "OH_Light_Attack_03";
        public string oh_running_attack_01 = "OH_Running_Attack_01";
        public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";
        public string oh_back_step_attack_01 = "OH_Back_Step_Attack_01";
        public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
        public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
        public string oh_heavy_attack_03 = "OH_Heavy_Attack_03";

        public string oh_charge_attack_01 = "OH_Charging_Attack_Charge_01";
        public string oh_charge_attack_02 = "OH_Charging_Attack_Charge_02";
        public string oh_charge_attack_03 = "OH_Charging_Attack_Charge_03";

        public string th_light_attack_01 = "TH_Light_Attack_01";
        public string th_light_attack_02 = "TH_Light_Attack_02";
        public string th_light_attack_03 = "TH_Light_Attack_03";
        public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
        public string th_heavy_attack_03 = "TH_Heavy_Attack_03";

        public string th_charge_attack_01 = "TH_Charging_Attack_Charge_01";
        public string th_charge_attack_02 = "TH_Charging_Attack_Charge_02";
        public string th_charge_attack_03 = "TH_Charging_Attack_Charge_03";

        public string oh_R_light_riding_attack_01 = "OH_R_Light_Attack_01";
        public string oh_L_light_riding_attack_01 = "OH_L_Light_Attack_01";

        //string weapon_art = "Weapon_Art";
        public string weapon_art = "Weapon_Art";

        public string lastAttack;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetBlockingAbsorptionsFromBlockingWeapons()
        {
            if (character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingMagicDamageAbsorption = character.characterInventoryManager.rightWeapon.magicBlockingDamageAbsorption;
                character.characterStatsManager.blockingLightningDamageAbsorption = character.characterInventoryManager.rightWeapon.lightningBlockingDamageAbsorption;
                character.characterStatsManager.blockingHolyDamageAbsorption = character.characterInventoryManager.rightWeapon.holyBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
            }
            else if (character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingMagicDamageAbsorption = character.characterInventoryManager.leftWeapon.magicBlockingDamageAbsorption;
                character.characterStatsManager.blockingLightningDamageAbsorption = character.characterInventoryManager.leftWeapon.lightningBlockingDamageAbsorption;
                character.characterStatsManager.blockingHolyDamageAbsorption = character.characterInventoryManager.leftWeapon.holyBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            //If you want AI to lose stamina also, you could put modular code here on base class
            //But from now on only player character will use this function
        }

        //Not Working!! Stamina deduct is still a zero 0 0 0
        public virtual void AttemptToBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, 
                                            float magicDamage, float lightningDamage, float holyDamage, string blockAnimation)
        {
            float staminaDamageAbsoption = ((physicalDamage + fireDamage + magicDamage + lightningDamage + holyDamage) * attackingWeapon.character.characterInventoryManager.rightWeapon.guardBreakModifier)
                                            * (character.characterStatsManager.blockingStabilityRating / 100);

            float staminaDamage = ((physicalDamage + fireDamage + magicDamage + lightningDamage + holyDamage) * attackingWeapon.guardBreakModifier) 
                                    - staminaDamageAbsoption;

            Debug.Log("Stamina damage is " + staminaDamage);
            character.characterStatsManager.currentStamina = character.characterStatsManager.currentStamina - Mathf.Abs(staminaDamage);

            //Deduct stamina from blocking attack
            if (character.characterStatsManager.currentStamina <= 0)
            {
                //Guard Break
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Break Guard", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }
        }

        void SuccesfullyCastSpell()
        {
            character.characterInventoryManager.currentSpell.SuccesfullyCastSpell(character);
            character.animator.SetBool("isFiringSpell", true);
        }

        public void AttemptBackStabOrRiposte()
        {
            // There might be some issues with dialogue triggers...
            // Sometimes while backstabbing, wrong character get back stabbed
            Transform temp = character.criticalAttackRaycastStartingPoint;

            if (character.isInteracting) { return; }

            if (character.characterStatsManager.currentStamina <= 0) { return; }

            RaycastHit hit;

            if (character.isCrouching)
            {
                character.criticalAttackRaycastStartingPoint = character.criticalAttackRayCastWhileCrouch;
            }

            if (Physics.Raycast(character.criticalAttackRaycastStartingPoint.position, transform.TransformDirection(Vector3.forward), 
                                out hit, criticalAttackRange, characterLayer))
            {
                CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                if (Vector3.Distance(character.transform.position, enemyCharacter.transform.position) < 2)
                {
                    //enemyCharacter = enemyCharacter;
                }
                else
                {
                    enemyCharacter = null;
                    return;
                }
                Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
                float dotValue = Vector3.Dot (directionFromCharacterToEnemy, enemyCharacter.transform.forward);

                //Debug.Log($"CURRENT DOT VALUE IS {dotValue}");

                if (enemyCharacter.canBeRiposted)
                {
                    if (dotValue <= 1.2f && dotValue >= 0.6f) //Test and try to play around the float values to find better ones!
                    {
                        //ATTEMPT RIPOSTE
                        AttemptRiposte(hit, enemyCharacter);
                        return;
                    }
                }

                if (dotValue >= -0.7f && dotValue <= -0.6f)
                {
                    //ATTEMPT BACKSTAB
                    AttemptBackStab(hit, enemyCharacter);
                }
            }
            character.criticalAttackRaycastStartingPoint = temp;
        }

        void AttemptRiposte(RaycastHit hit, CharacterManager enemyCharacter)
        {
            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackStabbed || !enemyCharacter.isBeingRiposted)
                {
                    //We make is so the enemy cannot be damaged while being critically damaged
                    character.characterAnimatorManager.EnableIsInVulnerable();
                    character.isPerformingRiposte = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Riposte", true); //CHANGE LATER IF I WANT DIFFERENT RIPOSTE ANIMATION BASE ON CLASS, WEAPON ETC...

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMuiltiplier
                                            * (character.characterInventoryManager.rightWeapon.physicalDamage
                                            + character.characterInventoryManager.rightWeapon.fireDamage
                                            + character.characterInventoryManager.rightWeapon.lightingDamage
                                            + character.characterInventoryManager.rightWeapon.holyDamage
                                            + character.characterInventoryManager.rightWeapon.frostDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetRiposted(character);
                }
            }
        }

        void AttemptBackStab(RaycastHit hit, CharacterManager enemyCharacter)
        {
            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackStabbed || !enemyCharacter.isBeingRiposted)
                {
                    //We make is so the enemy cannot be damaged while being critically damaged
                    character.characterAnimatorManager.EnableIsInVulnerable();
                    character.isPerformingBackstab = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true); //CHANGE LATER IF I WANT DIFFERENT BACKSTAB ANIMATION BASE ON CLASS, WEAPON ETC...

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMuiltiplier 
                                            * (character.characterInventoryManager.rightWeapon.physicalDamage 
                                            + character.characterInventoryManager.rightWeapon.fireDamage 
                                            + character.characterInventoryManager.rightWeapon.lightingDamage 
                                            + character.characterInventoryManager.rightWeapon.holyDamage 
                                            + character.characterInventoryManager.rightWeapon.frostDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetBackStabbed(character);
                }
            }
        }

        public void GetRiposted(CharacterManager characterPerformingRiposte)
        {
            //PLAY SOUND FX
            //character.characterSoundFXManager.PlaySomethingsomethin();
            character.isBeingRiposted = true;

            //FORCE LOCK POSITION
            StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerformingRiposte));

            character.characterAnimatorManager.PlayTargetAnimation("Riposted", true);
            character.canBeRiposted = false;
        }

        IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerformingRiposte)
        {
            for (float timer = 0.05f; timer < 0.5f; timer += 0.05f)
            {
                Quaternion riposteRotation = Quaternion.LookRotation(-characterPerformingRiposte.transform.forward); //- for the rotation so the enemy will face the character performing the riposte
                transform.rotation = Quaternion.Slerp(transform.rotation, riposteRotation, 1);
                transform.parent = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform;
                transform.localPosition = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void GetBackStabbed(CharacterManager characterPerformingBackstab)
        {
            //PLAY SOUND FX
            //character.characterSoundFXManager.PlaySomethingsomethin();
            character.isBeingBackStabbed = true;
            Debug.Log("Here isBeingbackstabbeb is true");

            //FORCE LOCK POSITION
            StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerformingBackstab));

            character.characterAnimatorManager.PlayTargetAnimation("Back Stabbed", true);
            StartCoroutine(AssingTargetToCharacterWhoIsBackStabbed(characterPerformingBackstab, 5f));
        }

        IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerformingBackstab)
        {
            for (float timer = 0.05f; timer < 0.5f; timer += 0.05f)
            {
                Quaternion backstabRotation = Quaternion.LookRotation(characterPerformingBackstab.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
                transform.parent = characterPerformingBackstab.characterCombatManager.backstabReceiverTransform;
                transform.localPosition = characterPerformingBackstab.characterCombatManager.backstabReceiverTransform.localPosition;
                transform.parent = null;
                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator AssingTargetToCharacterWhoIsBackStabbed(CharacterManager characterPerformingBackstab, float timer)
        {
            yield return new WaitForSeconds(timer);
            EnemyManager enemy = character as EnemyManager;
            //if (!enemy.isNPC)
            {
                enemy.currentTarget = characterPerformingBackstab;
            }
        }

        //OLD BACKSTAB AND RIPOSTE FUNCTION
        // public void AttemptBackStabOrRiposte()
        // {
        //     if (character.characterStatsManager.currentStamina <= 0) { return; }

        //     RaycastHit hit;

        //     if (Physics.Raycast(character.criticalAttackRaycastStartingPoint.position, transform.TransformDirection(Vector3.forward), 
        //                         out hit, 0.5f, backStabLayer))
        //     {
        //         CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        //         DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;

        //         if (enemycharacterManager != null)
        //         {
        //             //Check for team I.D (No attacking on team mates)
        //             character.transform.position = enemycharacterManager.backStabCollider.criticalDamagerStandPosition.position;

        //             Vector3 rotationDirection = character.transform.root.eulerAngles;
        //             rotationDirection = hit.transform.position - character.transform.position;
        //             rotationDirection.y = 0;
        //             rotationDirection.Normalize();
        //             Quaternion tr = Quaternion.LookRotation(rotationDirection);
        //             Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
        //             character.transform.rotation = targetRotation;

        //             int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMuiltiplier * rightWeapon.physicalDamage;
        //             enemycharacterManager.pendingCriticalDamage = criticalDamage;

        //             //Change camera angle

        //             character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);

        //             enemycharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
        //             //Do damage
        //         }
        //     }
        //     else if (Physics.Raycast(character.criticalAttackRaycastStartingPoint.position, transform.TransformDirection(Vector3.forward), 
        //                             out hit, 0.7f, riposteLayer))
        //     {
        //         Debug.Log("after else if");
        //         //Check for team I.D (No attacking on team mates)
        //         CharacterManager enemycharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        //         DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;
        //         Debug.Log(enemycharacterManager.name);

        //         if (enemycharacterManager != null && enemycharacterManager.canBeRiposted)
        //         {
        //             Debug.Log("Start of riposte");
        //             character.transform.position = enemycharacterManager.riposteCollider.criticalDamagerStandPosition.position;

        //             Vector3 rotationDirection = character.transform.root.eulerAngles;
        //             rotationDirection = hit.transform.position - character.transform.position;
        //             rotationDirection.y = 0;
        //             rotationDirection.Normalize();
        //             Quaternion tr = Quaternion.LookRotation(rotationDirection);
        //             Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
        //             character.transform.rotation = targetRotation;

        //             int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMuiltiplier * rightWeapon.physicalDamage;
        //             enemycharacterManager.pendingCriticalDamage = criticalDamage;

        //             //Change camera angle
        //             Debug.Log("Before Riposte animation");
        //             character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);
        //             Debug.Log("After");

        //             Debug.Log("Before Riposted animation");
        //             enemycharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
        //             Debug.Log("After Riposte animation");
        //             //Do damage
        //         }

        //     }
        // }
    }
}
