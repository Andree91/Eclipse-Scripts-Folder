using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PixelCrushers.DialogueSystem.Wrappers;

namespace AG
{
    public class EnemyManager : CharacterManager
    {
        public EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        public EnemyBossManager enemyBossManager;
        public EnemyEffectsManager enemyEffectsManager;

        public State currentState;
        public CharacterManager currentTarget;
        public CharacterManager noiseTarget;
        public Vector3 lastHeardPosition;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;
        public GameObject dialogueInteractable;
        public DialogueSystemTrigger dialogueSystemTrigger;
        public BarkOnIdle barkOnIdle;
        //public SphereCollider dialogueCollider;

        public bool isPerformingAction;
        public float maximumAggroRadius = 1.5f;
        public float rotationSpeed = 25;
        public float maximunAttackRange = 1.5f; //Different Idea of stopping
        public float stoppingDistance = 1.2f; //How close we will get to our target before stopping infront of them, or haulting forward movement

        // public EnemyAttackAction[] enemyAttacks;
        // public EnemyAttackAction currentAttack;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1.0f;
        //[SerializeField] float groundDirectionRayDistance = 0.2f;
        [SerializeField] float maxDistanceToGround = 1f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask ignoreLayer;
        public float inAirTimer;

        [Header("Colliders")]
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        [Header("Loot")]
        [SerializeField] float[] lootTable = { 60, 30, 8, 2 };
        [SerializeField] float totalWeight;
        [SerializeField] float rollNumber;
        public List<Item> lootItems;
        public int lootItemIndex;
        Item lootItem;
        GameObject itemPickUp;
        public GameObject weaponPickUp;
        public GameObject consumableItemPickUp;
        public GameObject helmetItemPickUp;
        public GameObject bodyItemPickUp;
        public GameObject handItemPickUp;
        public GameObject legItemPickUp;
        public GameObject ammoItemPickUp;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float noiseDetectionRadius = 10f;
        public float currentRecoveryTime = 0;
        public bool isPatrolling;
        public bool isTurning;
        public bool isNPC;
        public PlayerManager playerTalking;

        //These Settings only effect A.I with the humanoid states
        [Header("ADVANCED A.I SETTINGS")]
        public bool allowAIToPerformBlock;
        public int blockLikelyHood = 50;    //Number from 0-100. 100% will generate a block EVERY TIME, 0% will generate a block 0% of the time.
        public bool allowAIToPerformDodge;
        public int dodgeLikelyHood = 50;
        public bool allowAIToPerformParry;
        public int parryLikelyHood = 50;
        public bool allowAIToPerformRiposte;
        public int riposteLikelyHood = 50;

        [Header("A.I Combat Settings")]
        public bool isStatic;
        public bool allowAIToPerformCombos;
        public bool isPhaseShifting;
        public float comboLikelyHood;
        public AICombatStyle combatStyle;

        [Header("A.I Archery Settings")]
        public bool isStationaryArcher;
        public float minimunTimeToAimAtTarget = 3;
        public float maximunTimeToAimAtTarget = 6;

        [Header("A.I Companion Settings")]
        public float maxDistanceFromCompanion; // Max distance we can go from our companion
        public float minimumDistanceFromCompanion; // Minium distance we have to be from our companion
        public float returnDistanceFromCompanion = 2; // How close we get to our companion whwn we return to them
        public float distanceFromCompanion;
        public CharacterManager companion;

        [Header("A.I Target Information")]
        public float distanceFromTarget;
        public float distanceFromNoiseTarget;
        public Vector3 targetDirection;
        public float viewableAngle;
        public float hearableAngle;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
            dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
            //dialogueCollider = GetComponentInChildren<SphereCollider>();
            barkOnIdle = GetComponent<BarkOnIdle>();
        }

        protected override void Start()
        {
            base.Start();
            enemyRigidbody.isKinematic = false;
            isGrounded = true;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            //enemyEffectsManager.HandleAllBuildUpEffects();
        }

        protected override void Update()
        {
            base.Update();
            if (enemyRigidbody.useGravity == false && !isStatic)
            {
                HandleFallingAndLanding();
            }
            HandleRecoveryTimer();

            if (!isDead)
            {
                HandleStateMachine();
            }

            if (isTalking)
            {
                HandleAICharacterDuringDialogue();
            }

            // if (!isTalking)
            // {
            //     if (isNPC)
            //     {
            //         if (headRig.weight != 0)
            //         {
            //             characterHead.transform.localPosition = Vector3.Lerp(characterHead.transform.position, characterHeadOriginalPosition, Time.deltaTime * headReturnSpeed);
            //             headRig.weight = Mathf.Lerp(headRig.weight, rigWeight, Time.deltaTime * 2);
            //         }
            //     }
            // }

            if (isNPC)
            {
                HandleDialogueChanges();

                if (!isDead && !isDodging)// && !isInteracting)// && !isInCombat)
                {
                    HandlePointOfInterest();
                }
                else
                {
                    HandleAIDeath();
                }

            }

            isInteracting = animator.GetBool("isInteracting");
            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isInVulnerable = animator.GetBool("isInVulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isDead", isDead);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isTwoHanding", isTwoHanding);
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isSwimming", isSwimming);
            animator.SetBool("leftHandIsShield", leftHandIsShield);


            if (currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                Vector3 targetDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            }

            if (noiseTarget != null)
            {
                distanceFromNoiseTarget = Vector3.Distance(noiseTarget.transform.position, transform.position);
                Vector3 noiseTargetDirection = noiseTarget.transform.position - transform.position;
                hearableAngle = Vector3.SignedAngle(noiseTargetDirection, transform.forward, Vector3.up);
            }

            if (companion != null)
            {
                distanceFromCompanion = Vector3.Distance(companion.transform.position, transform.position);
            }
        }

        void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (!isDead && currentTarget != null)
            {
                if (!currentTarget.isInCombat)
                {
                    currentTarget.isInCombat = true;
                }
            }
            // else
            // {
            //     if (currentTarget.isInCombat)
            //     {
            //         currentTarget.isInCombat = false;
            //     }
            // }

            // if (lockOnUI != null)
            // {
            //     lockOnUI.transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized);
            // }
        }

        void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        void SwitchToNextState(State state)
        {
            currentState = state;
        }

        void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        void HandleAIDeath()
        {
            //characterHead.GetComponentInParent<CapsuleCollider>().enabled = false;
            //dialogueCollider.radius = 1f;
            //dialogueCollider.radius = 3f;
            dialogueSystemTrigger.enabled = false;
            //barkOnIdle.enabled = true;
            //StartCoroutine(DelayBarkDialogue(8f));
            dialogueInteractable.SetActive(false);
            GetComponentInChildren<PointOfInterest>().isPlayer = true;
        }

        IEnumerator DelayBarkDialogue(float delay)
        {
            yield return new WaitForSeconds(delay);
            barkOnIdle.enabled = false;
        }

        public void HandleFallingAndLanding()
        {
            if (isDead) { return; }

            if (isOnSandGlider) { return; }

            if (isSwimming) { return; }

            if (isUnderWater) { return; }

            if (isClimbing) { return; }

            if (isRiding) { return; }

            RaycastHit hit;
            Vector3 rayCastOrigin = transform.position;
            Vector3 targetPosition;
            rayCastOrigin.y = rayCastOrigin.y + groundDetectionRayStartPoint;
            targetPosition = transform.position;

            if (!isGrounded)// && !isJumping)
            {
                inAirTimer = inAirTimer + Time.deltaTime;
                enemyRigidbody.AddForce(transform.forward * 1f); //1f is enemys leaping velocity
                enemyRigidbody.AddForce(-Vector3.up * 500 * inAirTimer);

                if (!isInteracting && inAirTimer > 0.8f)
                {
                    enemyAnimatorManager.PlayTargetAnimation("Falling", true);
                }
                else if (inAirTimer > 5f)
                {
                    Debug.Log("Air timer should be over 5f");
                    Debug.Log("Name of enemy is " + gameObject.name);
                    enemyAnimatorManager.AwardSoulsOnDeath();
                    Destroy(this.gameObject);
                }
            }
            if (!isTakingScreenShots) Debug.DrawRay(rayCastOrigin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistanceToGround, groundLayer))
            {
                if (!isGrounded && isInteracting)
                {
                    if (inAirTimer > 0.8f)
                    {
                        enemyAnimatorManager.PlayTargetAnimation("Land", true);
                        // this.enabled = false;
                        // StartCoroutine(Enablemovement());

                    }
                    else
                    {
                        enemyAnimatorManager.PlayTargetAnimation("Empty", false);
                    }
                }

                Vector3 rayCastHitPoint = hit.point;
                targetPosition.y = rayCastHitPoint.y;
                inAirTimer = 0;
                isGrounded = true;
                //player.isInteracting = false;
            }
            else
            {
                isGrounded = false;
            }

            if (isGrounded)// && !isJumping)
            {
                if (isInteracting || enemyRigidbody.velocity.x > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        void HandleAICharacterDuringDialogue()
        {
            if (playerTalking == null)
            {
                playerTalking = FindObjectOfType<PlayerManager>();
            }

            if (isTurning)
            {
                Vector3 rotationDirection = playerTalking.transform.position - transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * 2f);
                //transform.rotation = targetRotation;
                StartCoroutine(RotateAIWhileTalking(targetRotation));
            }
            // else
            // {
            //     if (characterHead != null)
            //     {
            //         characterHead.transform.position = playerTalking.lockOnTransform.position + new Vector3(0, 0, 0.1f);
            //         rigWeight = 0.9f;
            //     }
            // }
        }

        IEnumerator RotateAIWhileTalking(Quaternion targetRotation)
        {
            float tempTime = Time.deltaTime;
            float timer = 0;

            while (timer <= tempTime + 2f)
            {
                timer += Time.deltaTime;
                transform.rotation = targetRotation;
                yield return new WaitForEndOfFrame();
            }
            isTurning = false;
        }

        void HandleDialogueChanges()
        {
            if (hasAgroed && !isDead)
            {
                hasAgroed = false;
                dialogueSystemTrigger.enabled = false;
                SphereCollider dialogueCollider = GetComponent<SphereCollider>();
                dialogueCollider.enabled = false;
                StandardBarkUI barkUI = GetComponentInChildren<StandardBarkUI>();
                barkUI.ToggleBarkUI(false);
                //dialogueCollider.radius = 4f;
                barkOnIdle.enabled = true;
                barkOnIdle.conversation = "New Conversation 3";
                StartCoroutine(DelayBarkDialogue(9f));
            }
            else if (isDead && !hasAgroed)
            {
                hasAgroed = true;
                dialogueSystemTrigger.enabled = false;
                //dialogueSystemTrigger.barkConversation = "New Conversation 4";
                barkOnIdle.enabled = true;
                if (gameObject.name == "Mage_NPC_01")
                {
                    barkOnIdle.conversation = "Mage Death";
                }
                else if (gameObject.name == "Male_NPC_01")
                {
                    barkOnIdle.conversation = "New Conversation 4";
                }
                else
                {
                    barkOnIdle.conversation = "New Conversation 4";
                }
                StartCoroutine(DelayBarkDialogue(9f));
            }
        }

        #region "Collisions"
        // Actions while colliding with enviroment

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == "Fog Entrance" && characterStatsManager.teamIDNumeber == 0)
            {
                Debug.Log("Here should be collision");
                PassThroughFogWallInteraction(other.transform);
            }
            else if (other.gameObject.tag == "Breakable Object")
            {
                Debug.Log("Should start breaking");
                BreakDestructibleObjects();
            }
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            //Make sure we are facing Fog Wall firs

            enemyRigidbody.velocity = Vector3.zero; //Stop player from "ice skating"
            Vector3 rotationDirection = fogWallEntrance.transform.right * -1; //transform.forward should be direction
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            //Rotate over time (TODO later)

            //aiCharacter.characterAnimatorManager.EraseHandIKForWeapon();
            characterAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }

        public void BreakDestructibleObjects()
        {
            CombatStanceStateHumanoid combatStanceStateHumanoid = GetComponentInChildren<CombatStanceStateHumanoid>();
            if (combatStanceStateHumanoid != null)
            {
                combatStanceStateHumanoid.enemyAttacks[1].PerformAttackAction(this);
            }
            else
            {
                CombatStanceState combatStanceState = GetComponentInChildren<CombatStanceState>();
                enemyAnimatorManager.PlayTargetAnimation(combatStanceState.enemyAttacks[1].actionAnimation, true);
                enemyAnimatorManager.PlayWeaponTrailFX();
            }

        }

        #endregion

        public void RollForLootItemAndDropIt()
        {
            foreach (var item in lootTable)
            {
                totalWeight += item;
            }

            rollNumber = Random.Range(0f, totalWeight);

            for (int i = 0; i < lootTable.Length; i++)
            {
                if (rollNumber <= lootTable[i]) // 40
                {
                    Debug.Log($"Roll {i} number is = {rollNumber}");
                    lootItemIndex = i;

                    if (lootItemIndex != 0)
                    {
                        InstantiateLoot();
                    }

                    return;

                }
                else
                {
                    rollNumber -= lootTable[i];
                    Debug.Log(("Round number is " + i));
                }
            }
        }

        void InstantiateLoot()
        {
            if (lootItems.Count > 1)
            {
                lootItem = lootItems[lootItemIndex];

                if (lootItem as WeaponItem)
                {
                    itemPickUp = weaponPickUp;
                    itemPickUp.GetComponent<WeaponPickUp>().weapon = lootItem as WeaponItem;
                }
                else if (lootItem as ConsumableItem)
                {
                    itemPickUp = consumableItemPickUp;
                    itemPickUp.GetComponent<ConsumableItemPickUp>().item = lootItem as ConsumableItem;
                    itemPickUp.GetComponent<ConsumableItemPickUp>().amount = Random.Range(1, 6);
                }
                else if (lootItem as HelmetEquipment)
                {
                    itemPickUp = helmetItemPickUp;
                    itemPickUp.GetComponent<HelmetItemPickUp>().item = lootItem as HelmetEquipment;
                }
                else if (lootItem as TorsoEquipment)
                {
                    itemPickUp = bodyItemPickUp;
                    itemPickUp.GetComponent<BodyItemPickUp>().item = lootItem as TorsoEquipment;
                }
                else if (lootItem as HandEquipment)
                {
                    itemPickUp = handItemPickUp;
                    itemPickUp.GetComponent<HandItemPickUp>().item = lootItem as HandEquipment;
                }
                else if (lootItem as LegEquipment)
                {
                    itemPickUp = legItemPickUp;
                    itemPickUp.GetComponent<LegItemPickUp>().item = lootItem as LegEquipment;
                }
                else if (lootItem as RangedAmmoItem)
                {
                    itemPickUp = ammoItemPickUp;
                    itemPickUp.GetComponent<RangedAmmoItemPickUp>().item = lootItem as RangedAmmoItem;
                    itemPickUp.GetComponent<RangedAmmoItemPickUp>().amount = Random.Range(4, 12);
                }

                GameObject itemPickUpLive = Instantiate(itemPickUp, transform.position, Quaternion.identity);
                itemPickUpLive.GetComponent<Interactable>().isLootItem = true;
            }
        }

        void OnDrawGizmosSelected()
        {
            Vector3 fovLine1 = Quaternion.AngleAxis(maximumDetectionAngle, transform.up) * transform.forward * detectionRadius;

            Vector3 fovLine2 = Quaternion.AngleAxis(minimumDetectionAngle, transform.up) * transform.forward * detectionRadius;

            Gizmos.color = Color.blue;

            Gizmos.DrawRay(transform.position, fovLine1);

            Gizmos.DrawRay(transform.position, fovLine2);
        }
    }
}
