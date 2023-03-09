using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace AG
{
    public class CharacterManager : MonoBehaviour
    {
        public AnimatorManager characterAnimatorManager;
        public Animator animator;
        public CharacterWeaponSlotManager characterWeaponSlotManager;
        public CharacterStatsManager characterStatsManager;
        public CharacterInventoryManager characterInventoryManager;
        public CharacterEffectsManager characterEffectsManager;
        public CharacterSoundFXManager characterSoundFXManager;
        public CharacterCombatManager characterCombatManager;
        public RagDollManager ragDollManager;
        public GameObject characterHead;
        public Vector3 characterHeadOriginalPosition;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        public Transform waterTrailTrasform;

        [Header("Ray Casts")]
        public Transform criticalAttackRaycastStartingPoint;
        public Transform criticalAttackRayCastWhileCrouch;

        [Header("Combat Colliders")]

        [Header("Interactions")]
        public bool isInteracting;

        [Header("Point Of Interest")]
        public Rig headRig;
        public List<PointOfInterest> POIs;
        public PointOfInterest POI;
        public float sqrRadius;
        public float poiRadius = 10f;
        public float maxAngle = 90f;
        public float headReturnSpeed = 5f;
        public float rigWeight = 0;

        [Header("STATUS")]
        public bool isDead;
        public bool isRagdoll = false;
        public bool isTalking;
        public bool hasAgroed;
        public bool isInCombat;

        [Header("IMMUNITUES")]
        public bool isImmuneToPoison;
        public bool isImmuneToFire;
        public bool isImmuneToLightning;
        public bool isImmuneToBleed;
        public bool isImmuneToFrost;
        //etc...

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool canDoCombo;
        public bool canJumpAttack;
        public bool isParrying;
        public bool isBlocking;
        public bool isDodging;
        public bool isInVulnerable;
        public bool isUsingLeftHand;
        public bool isUsingRightHand;
        public bool isHoldingArrow = false;
        public bool isAiming;
        public bool isTwoHanding;
        public bool isPerformingFullyChargedAttack;
        public bool isAttacking;
        public bool isBeingBackStabbed;
        public bool isBeingRiposted;
        public bool isPerformingBackstab;
        public bool isPerformingRiposte;
        public bool hasWeaponBuff;
        public bool leftHandIsShield;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isJumping;
        public bool isInAir;
        public bool isGrounded;
        public bool isSwimming;
        public bool isUnderWater;
        public bool isClimbing;
        public bool isCrouching;
        public bool isMoving;
        public bool isRiding;
        public bool isSitting;
        public bool isOnSandGlider = false;
        public bool isOnShip = false;
        public bool isOnMiningCart = false;

        [Header("Spells")]
        public bool isFiringSpell;
        public bool isUsingShieldSpell;

        [Header("Animal Flags")]
        public bool isAnimal;
        public bool isBear;
        public GameObject animalLookAt;
        public bool canPet;

        [Header("Critical Attacks Parametrs")]
        //Damage will inflick durin an Animation Event
        //Use in Backstab and Riposte animations
        public Vector3 startingPosition;
        public int pendingCriticalDamage;

        [Header("Agro Hit COunter")]
        public int hitCounter = 0;

        [Header("Lock On UI Object")]
        public GameObject lockOnUI;

        [Header("Record Stuff")]
        public bool isTakingScreenShots;

        protected virtual void Awake()
        {
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterAnimatorManager = GetComponent<AnimatorManager>();
            animator = GetComponent<Animator>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();

            startingPosition = transform.position;

            //POIs = FindObjectsOfType<PointOfInterest>().ToList();
            //POIs.Add(GetComponentInChildren<PointOfInterest>());

            sqrRadius = poiRadius * poiRadius;

            if (characterHead != null)
            {
                characterHeadOriginalPosition = characterHead.transform.localPosition;
            }
            if (isRagdoll)
            {
                ragDollManager = GetComponent<RagDollManager>();
            }
        }

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (!isHoldingArrow && !isAnimal)
            {
                characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHanding);
            }

            if (isDead && animator.GetCurrentAnimatorStateInfo(5).IsName("Empty")) // returs bool in current anim state name is "Empty"
            {
                //Debug.Log("Iside death if");
                //animator.StopPlayback();
                animator.Play("Death_01");
                //characterAnimatorManager.PlayTargetAnimation("Death_01", true);
            }
        }

        protected virtual void Update()
        {
            characterEffectsManager.ProcessAllTimedEffects();
        }

        public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
        {
            if (usingRightHand)
            {
                isUsingRightHand = true;
                isUsingLeftHand = false;
            }
            else
            {
                isUsingLeftHand = true;
                isUsingRightHand = false;
            }
        }

        public virtual void HandlePointOfInterest()
        {
            Transform tracking = null;
            foreach (PointOfInterest poi in POIs)
            {
                if (poi != null)
                {
                    Vector3 delta = poi.transform.position - transform.position;
                    if (delta.sqrMagnitude < sqrRadius)
                    {
                        float angle = Vector3.Angle(transform.forward * 0.5f, delta);
                        if (angle < maxAngle)
                        {
                            if (poi.isPlayer)
                            {
                                tracking = poi.transform;
                                break;
                            }
                        }
                    }
                }
            }

            //float rigWeight = 0;
            Vector3 targetPos = transform.position + (transform.forward * 1f) + (transform.up * 1.3f);
            float timer = Time.deltaTime;
            if (timer % 2 == 0)
            {
                Debug.Log("Timer modula 2 is " + (timer % 2));
            }
            if (tracking != null && !isDodging && !isInCombat) //&& timer % 2 != 0)
            {
                targetPos = tracking.position + new Vector3(0, 0.2f, 0f);
                rigWeight = 1f;
            }
            else
            {
                targetPos = transform.position + (transform.forward * 2f) + (transform.up * 1.5f);
                rigWeight = 1f;
            }
            characterHead.transform.position = Vector3.Lerp(characterHead.transform.position, targetPos, Time.deltaTime * headReturnSpeed);
            headRig.weight = Mathf.Lerp(headRig.weight, rigWeight, Time.deltaTime * 2);
        }
    }
}
