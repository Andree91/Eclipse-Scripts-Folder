using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem.Wrappers;
using Dropecho;
using UnityEngine.Playables;

namespace AG
{
    public class PlayerManager : CharacterManager
    {
        [Header("Camera")]
        public CameraHandler cameraHandler;

        [Header("Input")]
        public InputHandler inputHandler;

        [Header("UI")]
        public UIManager uIManager;

        [Header("Game Manager")]
        public GameManager gameManager;
        public WorldSaveGameManager saveGameManager;
        public Transform playerStartingPosition;
        public bool hasBloodStain;

        [Header("Player")]
        public PlayerStatsManager playerStatsManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerMovement playerMovement;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerEquipmentManager playerEquipmentManager;
        public bool isRiderAttack;
        public Vector3 lastSafePosition;
        public int lostSoulsCount;
        public bool isDiedByFalling;
        public bool isPlayingChess;

        [Header("Colliders")]
        public CapsuleCollider playerCollider;

        public Rigidbody playerRigidbody;
        public GameObject combatColliders;

        [Header("Interactables")]
        public InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        public GameObject interactableCannotBeUsedUIGameObject;
        public GameObject bloodstain;
        //public StandardDialogueUI dialogueCanvas;

        public SandGliderMovement sandGliderMovement;
        public RideMiningCart miningCart;
        public ShipMovement shipMovement;
        public bool isGroundedSandGlider;

        public WaterInteractable waterInteractable;
        bool lanterIsOn;

        public GameObject animal;
        public CharacterManager npcTalking;
        public FootStepAudio footStepAudio;

        public bool isInGodMode = false;

        public GameObject messageGameObject;

        [Header("Character Appereance")]
        public bool isInCharacterCreator;
        public bool gearIsOn;
        public bool characterAppereanceSavingIsReady;
        public int characterCreationCounter;
        public GameObject parentHeadGameObject;
        public GameObject parentHairGameObject;
        public GameObject parentEyebrownsGameObject;
        public GameObject parentFacialHairGameObject;
        public int appereanceIndex;

        public Color appereanceColor;
        public Color hairColor;
        public Color eyebrownsColor;
        public Color eyeColor;
        public Color facialHairColor;
        public Color skinColor;
        public Color marksColor;

        public List<SkinnedMeshRenderer> skinRendererList = new List<SkinnedMeshRenderer>();

        protected override void Awake()
        {
            base.Awake();
            interactableUI = FindObjectOfType<InteractableUI>();
            uIManager = FindObjectOfType<UIManager>();
            gameManager = FindObjectOfType<GameManager>();
            saveGameManager = FindObjectOfType<WorldSaveGameManager>();
            //dialogueCanvas = FindObjectOfType<StandardDialogueUI>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerMovement = GetComponent<PlayerMovement>();

            //if (playerStartingPosition != null)

            if (WorldSaveGameManager.instance != null)
            {
                WorldSaveGameManager.instance.player = this;
            }

            //Delete later, now it is just for testing purpose
            //transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
            playerStatsManager.currentSoulCount = PlayerPrefs.GetInt("currentSoulCount");

            if (bloodstain != null)
            {
                bloodstain.transform.position = new Vector3(PlayerPrefs.GetFloat("bloodStainX"), PlayerPrefs.GetFloat("bloodStainY"), PlayerPrefs.GetFloat("bloodStainZ"));
                bloodstain.SetActive(true);
            }

        }

        protected override void Start()
        {
            base.Start();
            cameraHandler = CameraHandler.singleton;
            isGrounded = true;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            //playerMovement.HandleFalling(playerMovement.moveDirection);
            playerMovement.HandleFallingAndLanding();
            playerMovement.HandleMovement();
            playerMovement.HandleRotation();
            //playerEffectsManager.HandleAllBuildUpEffects();
            //playerEffectsManager.HandleAllWeaponBuffs();

            // if (isTalking)
            // {
            //     RotatePlayerHead();
            // }

            // if (!isTalking)
            // {
            //     if (headRig.weight != 0)
            //     {
            //         characterHead.transform.localPosition = Vector3.Lerp(characterHead.transform.localPosition, characterHeadOriginalPosition, Time.deltaTime * headReturnSpeed);
            //         headRig.weight = Mathf.Lerp(headRig.weight, rigWeight, Time.deltaTime * 2);
            //     }
            // }

            if (!isDead && !isDodging /*&& !isInteracting*/ && characterHead != null)
            {
                HandlePointOfInterest();
            }

            if (isOnSandGlider)
            {
                sandGliderMovement.HandleSandGliderMovement();
                sandGliderMovement.HandleFalling();
            }

            if (isOnShip)
            {
                shipMovement.HandleShipMovement();
            }

        }

        protected override void Update()
        {
            base.Update();
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isJumping = animator.GetBool("isJumping");
            //isClimbing = animator.GetBool("isClimbing");
            isMoving = animator.GetBool("isMoving");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isInVulnerable = animator.GetBool("isInVulnerable");
            isDodging = animator.GetBool("isDodging");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isSwimming", isSwimming);
            animator.SetBool("isUnderWater", isUnderWater);
            animator.SetBool("isRiding", isRiding);
            animator.SetBool("isTwoHanding", isTwoHanding);
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", isDead);
            animator.SetBool("isSitting", isSitting);
            animator.SetBool("isClimbing", isClimbing);
            animator.SetBool("isCrouching", isCrouching);
            animator.SetBool("leftHandIsShield", leftHandIsShield);
            //Debug.Log("Is Holding Arrow is " + isHoldingArrow);

            //Debug.Log($"Player's Combat Status is {isInCombat}");

            inputHandler.TickInput();

            playerMovement.HandleRollingAndSprinting();
            playerMovement.HandleJumping();
            playerStatsManager.RegenerateStamina();

            if (cameraHandler != null)
            {
                CheckInteractableObject();
            }
        }

        void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.y_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            if (isInCombat && !uIManager.hud.statsBars.activeInHierarchy)
            {
                uIManager.ShowHUD();
            }
            else if (!isInCombat && uIManager.hideHUDTimer > 0 && uIManager.hud.statsBars.activeInHierarchy)
            {
                //Debug.Log("Now uitimer should go down");
                uIManager.hideHUDTimer -= 0.025f;
            }
            else if (!isInCombat && uIManager.hideHUDTimer <= 0 && uIManager.hud.statsBars.activeInHierarchy && playerStatsManager.poisonBuildUp < 0 && !playerStatsManager.isPoisoned)
            {
                uIManager.hideHUDTimer = 10.0f;
                uIManager.HideHUD();
            }

            PlayerPrefs.SetInt("currentSoulCount", playerStatsManager.currentSoulCount);

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }

            if (isInAir)
            {
                playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
            }
        }

        public void EnableIsRiderAttack()
        {
            isRiderAttack = true;
        }

        public void DisableIsRiderAttack()
        {
            isRiderAttack = false;
        }

        public void Teleport(Transform teleportTransformPosition)
        {
            inputHandler.mapFlag = false;
            uIManager.loadingWindow.SetActive(true);
            transform.position = teleportTransformPosition.position;
            StartCoroutine(LoadingScreenTest(2f));
        }

        public void LoadLoadingScreen(float loadingProgress)
        {
            if (uIManager.hudWindow.gameObject.activeInHierarchy)
            {
                uIManager.hudWindow.SetActive(false);
            }

            uIManager.loadingWindow.SetActive(true);

            uIManager.loadingProgress.value = loadingProgress;
            //StartCoroutine(LoadingScreenTest(2f));
        }

        public void CloseLoadingScreen()
        {
            uIManager.loadingWindow.SetActive(false);
            uIManager.hudWindow.SetActive(true);
        }

        IEnumerator LoadingScreenTest(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            uIManager.loadingWindow.SetActive(false);
            uIManager.hudWindow.SetActive(true);
        }

        public void HideSoulCountHUD()
        {
            StartCoroutine(HideSoulCountHUDCoroutine());
        }

        IEnumerator HideSoulCountHUDCoroutine()
        {
            Debug.Log("Start of the coroutine");
            yield return new WaitForSeconds(6.0f);
            if (!uIManager.hud.statsBars.activeInHierarchy)
            {
                Debug.Log("inside of the coroutine");
                uIManager.hud.soulCount.SetActive(false);
            }
        }

        public void HandlePlayerDeath()
        {
            uIManager.hudWindow.SetActive(false);
            uIManager.ActivateDeathPopUp();
            hasBloodStain = true;
            bloodstain.SetActive(true);
            bloodstain.transform.position = lastSafePosition;
            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[1000] = false;
            lostSoulsCount = playerStatsManager.currentSoulCount;
            playerStatsManager.currentSoulCount = 0;

            PlayerPrefs.SetFloat("bloodStainX", lastSafePosition.x);
            PlayerPrefs.SetFloat("bloodStainY", lastSafePosition.y);
            PlayerPrefs.SetFloat("bloodStainZ", lastSafePosition.z);
            PlayerPrefs.SetInt("lostSoulsCount", lostSoulsCount);

            gameManager.Invoke("LoadGameFromPlayerManager", gameManager.levelLoadDelay);
            uIManager.Invoke("DeactiveDeathPopUp", gameManager.levelLoadDelay - 0.01f);
            //gameManager.Invoke("ReloadLevel", gameManager.levelLoadDelay);
        }

        void RotatePlayerHead()
        {
            if (npcTalking == null)
            {
                npcTalking = FindObjectOfType<CharacterManager>();
            }

            characterHead.transform.position = npcTalking.lockOnTransform.position + new Vector3(0, 0, 0.1f);
            //rigWeight = 0.9f;
        }

        public override void HandlePointOfInterest()
        {
            //Vector3 tempHeadPos = characterHead.transform.position;

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
                            if (!poi.isPlayer)
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
            if (tracking != null && !isDodging && !isInCombat)
            {
                // characterHead.transform.position = tracking.position + new Vector3(0, 0, 0.1f);
                // Debug.Log("tracking  position is " + tracking.position);
                targetPos = tracking.position + new Vector3(0, 0.2f, 0f);
                rigWeight = 1f;
            }
            else
            {
                float angle = Vector3.Angle(transform.forward, cameraHandler.transform.transform.forward);
                if (angle < maxAngle)
                {
                    //characterHead.transform.position = transform.position + cameraHandler.transform.forward; // targetPos = characterHead.transform.position
                    targetPos = transform.position + (cameraHandler.transform.transform.forward * 2f) + (cameraHandler.transform.transform.up * 1.5f);
                    rigWeight = 1f;
                }
                else
                {
                    targetPos = transform.position + (transform.forward * 2f) + (transform.up * 1.5f);
                    rigWeight = 1f;
                }
            }
            characterHead.transform.position = Vector3.Lerp(characterHead.transform.position, targetPos, Time.deltaTime * headReturnSpeed);
            headRig.weight = Mathf.Lerp(headRig.weight, rigWeight, Time.deltaTime * 2);

            // HAVE TO TEST MORE
            // if (characterHead.transform.position.y <= (tempHeadPos.y - 0.02f))
            // {
            //     Debug.Log("Shoul be Inside When head is spinning traget");
            //     characterHead.transform.localPosition = new Vector3(0.0f, 0.0f, 1.5f);
            // }
        }

        public void TogglePlayerTalkingBools() // THIS IS CALLED WHEN DIALOGUE ENDS
        {
            uIManager.hudWindow.SetActive(true);
            isTalking = false;
            // dialogueCanvas.enabled = false;
            // dialogueCanvas.enabled = true;
            // characterHead.transform.localPosition = characterHeadOriginalPosition;
            // //characterHead.transform.localPosition = Vector3.Lerp(characterHead.transform.localPosition, characterHeadOriginalPosition, Time.deltaTime * headReturnSpeed);
            // //headRig.weight = Mathf.Lerp(headRig.weight, rigWeight, Time.deltaTime * 2);
        }

        #region  Player Interactions

        public void CheckInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.interactableLayer) && !isOnSandGlider)
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        if (interactableObject.GetComponent<LeverInteractable>() != null)
                        {
                            LeverInteractable interactableObjectLever = interactableObject as LeverInteractable;
                            if (!interactableObjectLever.isDisabled && !interactableObjectLever.isAtRightPosition) //!interactableCannotBeUsedUIGameObject.activeInHierarchy)
                            {
                                string interactableLeverText = interactableObject.interactableText;
                                interactableUI.interactableText.text = interactableLeverText;
                                interactableUIGameObject.SetActive(true);
                            }
                            // else if (interactableObjectLever.isAtRightPosition && interactableCannotBeUsedUIGameObject.activeInHierarchy)
                            // {
                            //     interactableUIGameObject.SetActive(false);
                            // }
                        }
                        else if (interactableObject.GetComponent<LeverMoveWalls>() != null)
                        {
                            LeverMoveWalls interactableObjectLever = interactableObject as LeverMoveWalls;
                            if (!interactableObjectLever.isDisabled) //!interactableCannotBeUsedUIGameObject.activeInHierarchy)
                            {
                                string interactableLeverText = interactableObject.interactableText;
                                interactableUI.interactableText.text = interactableLeverText;
                                interactableUIGameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            if (isClimbing) { return; }
                            string interactableText = interactableObject.interactableText;
                            interactableUI.interactableText.text = interactableText;
                            interactableUIGameObject.SetActive(true);
                        }

                        if (inputHandler.y_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else if (isOnSandGlider)
            {
                Interactable interactableObject = sandGliderMovement.GetComponent<Interactable>();

                if (isOnMiningCart)
                {
                    interactableObject = miningCart.GetComponent<Interactable>();
                }

                string interactableText = "Get Off";
                interactableUI.interactableText.text = interactableText;
                interactableUIGameObject.SetActive(true);

                if (inputHandler.y_Input)
                {
                    interactableObject.Interact(this);
                }
            }
            else if (isOnShip)
            {
                Interactable interactableObject = shipMovement.GetComponentInChildren<Interactable>();

                string interactableText = "Get Off";
                interactableUI.interactableText.text = interactableText;
                interactableUIGameObject.SetActive(true);

                if (inputHandler.y_Input)
                {
                    interactableObject.Interact(this);
                }
            }
            else if (isSwimming)
            {
                Interactable interactableObject = waterInteractable;
                Debug.Log(interactableObject.name);

                string interactableText = interactableObject.interactableText;
                interactableUI.interactableText.text = interactableText;
                interactableUIGameObject.SetActive(true);

                if (inputHandler.y_Input)
                {
                    interactableObject.Interact(this);
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.y_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }

                if (interactableCannotBeUsedUIGameObject != null && inputHandler.y_Input)
                {
                    interactableUIGameObject.SetActive(false);
                    interactableCannotBeUsedUIGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform PlayerStandHereWhenOpeningChest)
        {
            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            transform.position = PlayerStandHereWhenOpeningChest.transform.position;
            transform.rotation = PlayerStandHereWhenOpeningChest.rotation;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        public void OpenDoorInteraction(Transform PlayerStandHereWhenOpeningChest, bool isBig)
        {
            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            transform.position = PlayerStandHereWhenOpeningChest.transform.position;
            playerAnimatorManager.EraseHandIKForWeapon();
            if (isBig)
            {
                playerAnimatorManager.PlayTargetAnimation("Open Door Big Start", true);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Open Door", true);
            }
        }

        public void PullLeverInteraction(Transform playerStandHereWhenPullingLever)
        {
            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            transform.position = playerStandHereWhenPullingLever.transform.position;
            transform.rotation = playerStandHereWhenPullingLever.transform.rotation;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Pull Lever Start", true);
        }

        public void ShowCantCurrentlyUsePopUp()
        {
            //string interactableText = "This contraption cannot not be used rgiht now";
            //interactableCannotBeUsedUIGameObject.GetComponentInChildren<>() = interactableText;
            interactableUIGameObject.SetActive(false);
            interactableCannotBeUsedUIGameObject.SetActive(true);
        }

        public void MountSandGliderInteraction(Transform playerStandHereWhenMountingSandGlider, Transform sittingPosition)
        {
            playerMovement.rb.velocity = Vector3.zero;
            transform.position = playerStandHereWhenMountingSandGlider.transform.position;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Mount Sand Glider", true);
            StartCoroutine(SetSittingPositonOnSandGlider(sittingPosition));
        }

        public void RideMiningCartInteraction(Transform playerStandHereWhenMountingSandGlider, Transform sittingPosition)
        {
            playerMovement.rb.velocity = Vector3.zero;
            transform.position = playerStandHereWhenMountingSandGlider.transform.position;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Mount Sand Glider", true);
            StartCoroutine(SetSittingPositonOnMiningCart(sittingPosition));
        }

        public void MountShipWheelInteraction(Transform playerStandHereWhenMountingShip)
        {
            playerMovement.rb.velocity = Vector3.zero;
            transform.position = playerStandHereWhenMountingShip.transform.position;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerWeaponSlotManager.rightHandSlot.UnloadWeaponAndDestroy();
            playerWeaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
            playerAnimatorManager.PlayTargetAnimation("Mount Ship Wheel", true);
            shipMovement.meshCollider.convex = true;
            isOnShip = true;
            animator.SetBool("isOnShip", isOnShip);
            transform.SetParent(shipMovement.transform);
            playerCollider.enabled = false;
            playerRigidbody.isKinematic = true;
            combatColliders.SetActive(false);
        }

        public void MountToAnimalInteraction()
        {
            //Do SOME MORE LOGIC LATER
            isRiding = !isRiding;
            playerMovement.rb.useGravity = false;
        }

        public void PetAnimal()
        {
            playerMovement.rb.velocity = Vector3.zero;
            // transform.position = playerStandHereWhenStartClimbing.transform.position;
            // transform.rotation = playerStandHereWhenStartClimbing.transform.rotation;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Pet Animal", true);
        }

        public void LadderInteraction(Transform playerStandHereWhenStartClimbing)
        {
            playerMovement.rb.velocity = Vector3.zero;
            transform.position = playerStandHereWhenStartClimbing.transform.position;
            transform.rotation = playerStandHereWhenStartClimbing.transform.rotation;
            playerAnimatorManager.EraseHandIKForWeapon();
            isClimbing = true;
            playerMovement.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            playerWeaponSlotManager.rightHandSlot.UnloadWeaponAndDestroy();
            playerWeaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
        }

        public void DiveUnderWater(Vector3 swimmingLevel)
        {
            isUnderWater = true;
            Vector3 originalPos = transform.position;
            Vector3 swimmingPos = transform.position + swimmingLevel;
            //     transform.position = swimmingPos;
            // }
            Destroy(playerEffectsManager.currentWaterParticleFX);
            isSwimming = false;
            StartCoroutine(DiveToUnderWater(swimmingPos));
            Debug.Log("Out of courotine");
            playerCollider.enabled = true;
            combatColliders.SetActive(true);
        }

        IEnumerator DiveToUnderWater(Vector3 swimmingPos)
        {
            float tempTime = Time.deltaTime;
            float timer = 0;

            while (timer <= tempTime + 1f)
            {
                //isUnderWater = true;
                timer += Time.deltaTime;
                playerCollider.enabled = false;
                combatColliders.SetActive(false);
                transform.position = Vector3.Lerp(transform.position, swimmingPos, Time.deltaTime * 2f);
                yield return new WaitForEndOfFrame();
            }
            playerCollider.enabled = true;
            combatColliders.SetActive(true);
        }

        IEnumerator SetSittingPositonOnSandGlider(Transform sittingPosition)
        {
            yield return new WaitForSeconds(2f);
            transform.position = sittingPosition.transform.position;
            transform.rotation = sittingPosition.transform.rotation;
            playerAnimatorManager.PlayTargetAnimation("Sit_Down_01", true);
            isSitting = true;
            isOnSandGlider = true;
            transform.SetParent(sandGliderMovement.transform);
            playerCollider.enabled = false;
            playerRigidbody.isKinematic = true;
            combatColliders.SetActive(false);
        }

        IEnumerator SetSittingPositonOnMiningCart(Transform sittingPosition)
        {
            yield return new WaitForSeconds(2f);
            transform.position = sittingPosition.transform.position;
            transform.rotation = sittingPosition.transform.rotation;
            //playerAnimatorManager.PlayTargetAnimation("Sit_Down_01", true);
            //isSitting = true;
            isOnSandGlider = true;
            isOnMiningCart = true;
            transform.SetParent(miningCart.transform);
            playerCollider.enabled = false;
            playerRigidbody.isKinematic = true;
            combatColliders.SetActive(false);
            miningCart.GetComponent<PlayableDirector>().enabled = true;
        }

        public void GetOffOnSandGliderInteraction(Transform playerStandHereWhenGettingOffOnSandGlider)
        {
            transform.position = playerStandHereWhenGettingOffOnSandGlider.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Get Off Sand Glider", true);
            sandGliderMovement.sandGliderRb.isKinematic = true;
            isSitting = false;
            isOnSandGlider = false;
            transform.SetParent(null);
            playerCollider.enabled = true;
            playerRigidbody.isKinematic = false;
            combatColliders.SetActive(true);
        }

        public void GetOffOnMiningCartInteraction(Transform playerOffPosition)
        {
            transform.position = playerOffPosition.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Get Off Sand Glider", true);
            //sandGliderMovement.sandGliderRb.isKinematic = true;
            //isSitting = false;
            isOnSandGlider = false;
            transform.SetParent(null);
            playerCollider.enabled = true;
            playerRigidbody.isKinematic = false;
            combatColliders.SetActive(true);
        }

        public void GetOffOnShipWheelInteraction(Transform playerStandHereWhenGettingOffOnShipWheel)
        {
            transform.position = playerStandHereWhenGettingOffOnShipWheel.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Get Off Ship Wheel", true);
            shipMovement.shipRb.isKinematic = true;
            //isSitting = false;
            isOnShip = false;
            animator.SetBool("isOnShip", isOnShip);
            shipMovement.meshCollider.convex = false;
            transform.SetParent(null);
            playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            if (playerWeaponSlotManager.backSlot.currentWeaponModel == null && playerWeaponSlotManager.shieldBackSlot.currentWeaponModel == null)
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
            }
            playerCollider.enabled = true;
            playerRigidbody.isKinematic = false;
            combatColliders.SetActive(true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            //Make sure we are facing Fog Wall firs

            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            Vector3 rotationDirection = fogWallEntrance.transform.right * -1; //transform.forward should be direction
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            //Rotate over time (TODO later)

            playerAnimatorManager.EraseHandIKForWeapon();
            isInCombat = true;
            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);

        }

        public void SummonPhantomInteraction(Transform playerStandingPosition)
        {
            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            transform.position = playerStandingPosition.transform.position;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true);
        }

        public void TalkInteraction(Transform playerStandingPosition, Interactable npc)
        {
            //uIManager.hudWindow.SetActive(false);
            isTalking = true;
        }

        public void StartChessInteraction(Transform playerStandingPosition)
        {
            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            transform.position = playerStandingPosition.transform.position;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Sit_Down_01", true);
            isSitting = true;
            isPlayingChess = true;
            playerWeaponSlotManager.rightHandSlot.UnloadWeaponAndDestroy();
            playerWeaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
            uIManager.hudWindow.SetActive(false);
        }

        public void SitAtCampFireInteraction(Transform bonfireTeleportTransform)
        {
            if (saveGameManager == null)
            {
                saveGameManager = FindObjectOfType<WorldSaveGameManager>();
            }
            playerWeaponSlotManager.rightHandSlot.UnloadWeaponAndDestroy();
            playerWeaponSlotManager.leftHandSlot.UnloadWeaponAndDestroy();
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Sit_Down_01", true);
            isSitting = true;

            saveGameManager.SaveGame();

            // playerStartingPosition = bonfireTeleportTransform;
            // PlayerPrefs.SetFloat("PlayerX", bonfireTeleportTransform.position.x);
            // PlayerPrefs.SetFloat("PlayerY", bonfireTeleportTransform.position.y);
            // PlayerPrefs.SetFloat("PlayerZ", bonfireTeleportTransform.position.z);

            foreach (ConsumableItem item in playerInventoryManager.consumablesInQuickSlots)
            {
                if (item.itemName == "Estus Flask")
                {
                    item.currentItemAmount = item.maxItemAmount;
                    uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(item);
                    playerInventoryManager.currentConsumableIndex = 0;
                }
                else if (item.itemName == "Mana Flask")
                {
                    item.currentItemAmount = item.maxItemAmount;
                }
            }

            foreach (ConsumableItem item in playerInventoryManager.consumablesInventory)
            {
                if (item.itemName == "Estus Flask")
                {
                    item.currentItemAmount = item.maxItemAmount;
                    // uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(item);
                    // playerInventoryManager.currentConsumableIndex = 0;
                }
                else if (item.itemName == "Mana Flask")
                {
                    item.currentItemAmount = item.maxItemAmount;
                }
            }

            if (animal != null)
            {
                animal.gameObject.SetActive(true);
            }
        }

        public void WriteDownMessage(string message)
        {
            playerMovement.rb.velocity = Vector3.zero; //Stop player from "ice skating"
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.PlayTargetAnimation("Write Message", true);
            GameObject messageInstance =  Instantiate(messageGameObject, transform.position + transform.forward, transform.rotation);
            messageInstance.GetComponent<MessageInteractable>().SetMessageText(message);
        }

        public void ReadMessageInteraction(string message)
        {
            uIManager.OpenMessage(message);
        }

        public void ToggleGodModeInteraction()
        {
            isInGodMode = !isInGodMode;
            playerCollider.enabled = !isInGodMode;
            combatColliders.SetActive(!isInGodMode);
            Debug.Log("GOD MODE IS ACTIVATE = " + isInGodMode);
        }

        public void ToggleLanternInteraction()
        {
            lanterIsOn = !lanterIsOn;
            playerEquipmentManager.lantern.SetActive(lanterIsOn);
        }



        #endregion

        public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            if (!isInCharacterCreator)
            {
                // CHARACTER INFO
                currentCharacterSaveData.characterName = playerStatsManager.characterName;
                currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;

                // WORLD POSITION
                currentCharacterSaveData.xPosition = lastSafePosition.x; //transform.position.x;
                currentCharacterSaveData.yPosition = lastSafePosition.y;
                currentCharacterSaveData.zPosition = lastSafePosition.z;
            }
            else if (isInCharacterCreator && !characterAppereanceSavingIsReady)
            {
                SaveCharacterCreationDataToCurrentSaveData(ref currentCharacterSaveData);
            }

            // if (characterAppereanceSavingIsReady)
            // {
            //     SaveCharacterCreationDataToCurrentSaveData(ref currentCharacterSaveData);
            // }

            // EQUIPMENT
            // __________

            currentCharacterSaveData.currentRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
            currentCharacterSaveData.currentLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;

            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                currentCharacterSaveData.currentHeadEquipmentID = playerInventoryManager.currentHelmetEquipment.itemID;
            }
            else if (gearIsOn)
            {
                currentCharacterSaveData.currentHeadEquipmentID = 1;
            }
            else
            {
                currentCharacterSaveData.currentHeadEquipmentID = -1;
            }

            if (playerInventoryManager.currentTorsoEquipment != null)
            {
                currentCharacterSaveData.currentBodyEquipmentID = playerInventoryManager.currentTorsoEquipment.itemID;
            }
            else if (gearIsOn)
            {
                currentCharacterSaveData.currentBodyEquipmentID = 101;
            }
            else
            {
                currentCharacterSaveData.currentBodyEquipmentID = -1;
            }

            if (playerInventoryManager.currentHandEquipment != null)
            {
                currentCharacterSaveData.currentHandEquipmentID = playerInventoryManager.currentHandEquipment.itemID;
            }
            else if (gearIsOn)
            {
                currentCharacterSaveData.currentHandEquipmentID = 201;
            }
            else
            {
                currentCharacterSaveData.currentHandEquipmentID = -1;
            }

            if (playerInventoryManager.currentLegEquipment != null)
            {
                currentCharacterSaveData.currentLegEquipmentID = playerInventoryManager.currentLegEquipment.itemID;
            }
            else if (gearIsOn)
            {
                currentCharacterSaveData.currentLegEquipmentID = 301;
            }
            else
            {
                currentCharacterSaveData.currentLegEquipmentID = -1;
            }

            // WEAPON SLOTS
            currentCharacterSaveData.currentRightHandSlot01WeaponID = playerInventoryManager.weaponsInRightHandSlots[0].itemID;
            currentCharacterSaveData.currentRightHandSlot02WeaponID = playerInventoryManager.weaponsInRightHandSlots[1].itemID;
            currentCharacterSaveData.currentRightHandSlot03WeaponID = playerInventoryManager.weaponsInRightHandSlots[2].itemID;
            currentCharacterSaveData.currentLeftHandSlot01WeaponID = playerInventoryManager.weaponsInLeftHandSlots[0].itemID;
            currentCharacterSaveData.currentLeftHandSlot02WeaponID = playerInventoryManager.weaponsInLeftHandSlots[1].itemID;
            currentCharacterSaveData.currentLeftHandSlot03WeaponID = playerInventoryManager.weaponsInLeftHandSlots[2].itemID;

            // RANGED AMMO
            currentCharacterSaveData.currentAmmo01ItemID = playerInventoryManager.currentAmmo01.itemID;
            currentCharacterSaveData.currentAmmo02ItemID = playerInventoryManager.currentAmmo02.itemID;

            currentCharacterSaveData.currentBowArrowSlot01ItemID = playerInventoryManager.rangedAmmoItemsInAmmoSlots[0].itemID;
            currentCharacterSaveData.currentBowArrowSlot02ItemID = playerInventoryManager.rangedAmmoItemsInAmmoSlots[1].itemID;
            currentCharacterSaveData.currentCrossBowArrowSlot01ItemID = playerInventoryManager.rangedAmmoItemsInAmmoSlots[2].itemID;
            currentCharacterSaveData.currentCrossBowArrowSlot02ItemID = playerInventoryManager.rangedAmmoItemsInAmmoSlots[3].itemID;

            // CONSUMABLES
            currentCharacterSaveData.currentConsumableID = playerInventoryManager.currentConsumable.itemID;

            currentCharacterSaveData.currentQuickSlot01ItemID = playerInventoryManager.consumablesInQuickSlots[0].itemID;
            currentCharacterSaveData.currentQuickSlot02ItemID = playerInventoryManager.consumablesInQuickSlots[1].itemID;
            currentCharacterSaveData.currentQuickSlot03ItemID = playerInventoryManager.consumablesInQuickSlots[2].itemID;
            currentCharacterSaveData.currentQuickSlot04ItemID = playerInventoryManager.consumablesInQuickSlots[3].itemID;
            currentCharacterSaveData.currentQuickSlot05ItemID = playerInventoryManager.consumablesInQuickSlots[4].itemID;
            currentCharacterSaveData.currentQuickSlot06ItemID = playerInventoryManager.consumablesInQuickSlots[5].itemID;
            currentCharacterSaveData.currentQuickSlot07ItemID = playerInventoryManager.consumablesInQuickSlots[6].itemID;
            currentCharacterSaveData.currentQuickSlot08ItemID = playerInventoryManager.consumablesInQuickSlots[7].itemID;
            currentCharacterSaveData.currentQuickSlot09ItemID = playerInventoryManager.consumablesInQuickSlots[8].itemID;
            currentCharacterSaveData.currentQuickSlot10ItemID = playerInventoryManager.consumablesInQuickSlots[9].itemID;

            // AMULETS
            currentCharacterSaveData.currentAmuletSlot01ID = playerInventoryManager.currentAmuletSlot01.itemID;
            currentCharacterSaveData.currentAmuletSlot02ID = playerInventoryManager.currentAmuletSlot02.itemID;
            currentCharacterSaveData.currentAmuletSlot03ID = playerInventoryManager.currentAmuletSlot03.itemID;
            currentCharacterSaveData.currentAmuletSlot04ID = playerInventoryManager.currentAmuletSlot04.itemID;


            // INVETORY
            // ________

            // CONSUMANLES
            currentCharacterSaveData.consumableInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.consumablesInventory.Count; i++)
            {
                if (playerInventoryManager.consumablesInventory[i] != null)
                {
                    currentCharacterSaveData.consumableInventoryItemIDs.Add(playerInventoryManager.consumablesInventory[i].itemID);
                    //currentCharacterSaveData.consumableInventoryItemIDs[i] = playerInventoryManager.consumablesInventory[i].itemID;
                }
                // else
                // {
                //     currentCharacterSaveData.consumableItemIDs[i] = -1;
                // }
            }

            // WEAPONS
            currentCharacterSaveData.weaponInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.weaponsInventory.Count; i++)
            {
                if (playerInventoryManager.weaponsInventory[i] != null)
                {
                    currentCharacterSaveData.weaponInventoryItemIDs.Add(playerInventoryManager.weaponsInventory[i].itemID);
                }
            }

            // HELMET EQUIPMENT
            currentCharacterSaveData.headInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.headEquipmentInventory.Count; i++)
            {
                if (playerInventoryManager.headEquipmentInventory[i] != null)
                {
                    currentCharacterSaveData.headInventoryItemIDs.Add(playerInventoryManager.headEquipmentInventory[i].itemID);
                }
            }

            // BODY EQUIPMENT
            currentCharacterSaveData.bodyInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.bodyEquipmentInventory.Count; i++)
            {
                if (playerInventoryManager.bodyEquipmentInventory[i] != null)
                {
                    currentCharacterSaveData.bodyInventoryItemIDs.Add(playerInventoryManager.bodyEquipmentInventory[i].itemID);
                }
            }

            // HAND EQUIPMENT
            currentCharacterSaveData.handInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.handEquipmentInventory.Count; i++)
            {
                if (playerInventoryManager.handEquipmentInventory[i] != null)
                {
                    currentCharacterSaveData.handInventoryItemIDs.Add(playerInventoryManager.handEquipmentInventory[i].itemID);
                }
            }

            // LEG EQUIPMENT
            currentCharacterSaveData.legInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.legEquipmentInventory.Count; i++)
            {
                if (playerInventoryManager.legEquipmentInventory[i] != null)
                {
                    currentCharacterSaveData.legInventoryItemIDs.Add(playerInventoryManager.legEquipmentInventory[i].itemID);
                }
            }

            // RANGED AMMO
            currentCharacterSaveData.rangedAmmoInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.rangedAmmoItemsInventory.Count; i++)
            {
                if (playerInventoryManager.rangedAmmoItemsInventory[i] != null)
                {
                    currentCharacterSaveData.rangedAmmoInventoryItemIDs.Add(playerInventoryManager.rangedAmmoItemsInventory[i].itemID);
                }
            }

            // AMULETS
            currentCharacterSaveData.amuletInventoryItemIDs.Clear();
            for (int i = 0; i < playerInventoryManager.amuletsInventory.Count; i++)
            {
                if (playerInventoryManager.amuletsInventory[i] != null)
                {
                    currentCharacterSaveData.amuletInventoryItemIDs.Add(playerInventoryManager.amuletsInventory[i].itemID);
                }
            }

            if (characterCreationCounter <= 0)
            {
                Debug.Log("Round number is " + characterCreationCounter);
                characterAppereanceSavingIsReady = true;
            }

            // Delete later, just for testing
            // PlayerPrefs.SetFloat("PlayerX", currentCharacterSaveData.xPosition);
            // PlayerPrefs.SetFloat("PlayerY", currentCharacterSaveData.yPosition);
            // PlayerPrefs.SetFloat("PlayerZ", currentCharacterSaveData.zPosition);
        }

        public void SaveCharacterCreationDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            // CHARACTER INFO
            currentCharacterSaveData.characterName = playerStatsManager.characterName;
            currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;
            currentCharacterSaveData.characterClass = playerStatsManager.className;

            // CHARACTER STAT LEVELS
            currentCharacterSaveData.characterHealth = playerStatsManager.healthLevel;
            currentCharacterSaveData.characterStamina = playerStatsManager.staminaLevel;
            currentCharacterSaveData.characterMana = playerStatsManager.focusLevel;
            currentCharacterSaveData.characterStrenght = playerStatsManager.strenghtLevel;
            currentCharacterSaveData.characterDexterity = playerStatsManager.dexterityLevel;
            currentCharacterSaveData.characterIntelligence = playerStatsManager.intelligenceLevel;
            currentCharacterSaveData.characterFaith = playerStatsManager.faithLevel;
            currentCharacterSaveData.characterArcane = playerStatsManager.arcaneLevel;

            // CHARACTER APPEREANCE
            // Naked Class
            GetCharacterAppereanceIndex(parentHeadGameObject, "_Color_BodyArt");
            currentCharacterSaveData.characterNakedHeadIndex = appereanceIndex;
            marksColor = appereanceColor;
            GetCharacterAppereanceIndex(parentHairGameObject, "_Color_Hair");
            currentCharacterSaveData.characterHairIndex = appereanceIndex;
            hairColor = appereanceColor;
            GetCharacterAppereanceIndex(parentEyebrownsGameObject, "_Color_Hair");
            currentCharacterSaveData.characterEyebrownsIndex = appereanceIndex;
            eyebrownsColor = appereanceColor;
            GetCharacterAppereanceIndex(parentFacialHairGameObject, "_Color_Hair");
            currentCharacterSaveData.characterFacialHairIndex = appereanceIndex;
            facialHairColor = appereanceColor;

            // Knight Class and Other with gear on


            currentCharacterSaveData.characterHairColor = hairColor;
            currentCharacterSaveData.hairRedAmount = hairColor.r;
            currentCharacterSaveData.hairGreenAmount = hairColor.g;
            currentCharacterSaveData.hairBlueAmount = hairColor.b;

            currentCharacterSaveData.characterEyebrownsColor = eyebrownsColor;
            currentCharacterSaveData.eyebrownsRedAmount = eyebrownsColor.r;
            currentCharacterSaveData.eyebrownsGreenAmount = eyebrownsColor.g;
            currentCharacterSaveData.eyebrownsBlueAmount = eyebrownsColor.b;

            currentCharacterSaveData.characterEyeColor = eyeColor;
            currentCharacterSaveData.eyeRedAmount = eyeColor.r;
            currentCharacterSaveData.eyeGreenAmount = eyeColor.g;
            currentCharacterSaveData.eyeBlueAmount = eyeColor.b;

            currentCharacterSaveData.characterFacialHairColor = facialHairColor;
            currentCharacterSaveData.facialHairRedAmount = facialHairColor.r;
            currentCharacterSaveData.facialHairGreenAmount = facialHairColor.g;
            currentCharacterSaveData.facialHairBlueAmount = facialHairColor.b;

            currentCharacterSaveData.characterSkinColor = skinColor;
            currentCharacterSaveData.skinRedAmount = skinColor.r;
            currentCharacterSaveData.skinGreenAmount = skinColor.g;
            currentCharacterSaveData.skinBlueAmount = skinColor.b;

            currentCharacterSaveData.characterMarksColor = marksColor;
            currentCharacterSaveData.marksRedAmount = marksColor.r;
            currentCharacterSaveData.marksGreenAmount = marksColor.g;
            currentCharacterSaveData.marksBlueAmount = marksColor.b;

            // WORLD POSITION
            Vector3 characterStartingPosition = new Vector3(-0.356999993f, 100.107002f, -4.92500019f);
            currentCharacterSaveData.xPosition = characterStartingPosition.x;
            currentCharacterSaveData.yPosition = characterStartingPosition.y;
            currentCharacterSaveData.zPosition = characterStartingPosition.z;
        }

        void GetCharacterAppereanceIndex(GameObject parentGameObject, string color)
        {
            for (int i = 0; i < parentGameObject.transform.childCount; i++)
            {
                var child = parentGameObject.transform.GetChild(i).gameObject;

                if (child != null)
                {
                    if (child.activeInHierarchy)
                    {
                        appereanceIndex = i;
                        appereanceColor = child.GetComponent<SkinnedMeshRenderer>().material.GetColor(color);
                        if (parentGameObject == parentHeadGameObject)
                        {
                            eyeColor = child.GetComponent<SkinnedMeshRenderer>().material.GetColor("_Color_Eyes");
                            skinColor = child.GetComponent<SkinnedMeshRenderer>().material.GetColor("_Color_Skin");
                        }
                        break;
                    }
                }
            }

            if (parentGameObject == parentHairGameObject)
            {
                if (appereanceIndex == 0)
                {
                    appereanceIndex = -1;
                }
            }
        }

        public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            if (!isInCharacterCreator)
            {
                // CHARACTER INFO
                playerStatsManager.characterName = currentCharacterSaveData.characterName;
                playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;
            }
            else
            {
                //Debug.Log("This this is it " + currentCharacterSaveData.currentHeadEquipmentID);
                isInCharacterCreator = false;
                LoadCharacterCreationDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
            }

            // WORLD POSITION
            Vector3 characterPosition = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);
            transform.position = characterPosition;

            // EQUIPMENT
            // _________
            playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentRightHandWeaponID);
            playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentLeftHandWeaponID);
            playerWeaponSlotManager.LoadBothWeaponsOnSlot();
            //uIManager.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, )

            // TODO
            // There are still some probles with equipment gear loading if I have equipment items placed via Editor
            // Have to double check

            EquipmentItem headEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentHeadEquipmentID);

            // If this item exist in the data base, we apply it
            if (headEquipment != null)
            {
                playerInventoryManager.currentHelmetEquipment = headEquipment as HelmetEquipment;
            }

            EquipmentItem bodyEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentBodyEquipmentID);

            if (bodyEquipment != null)
            {
                playerInventoryManager.currentTorsoEquipment = bodyEquipment as TorsoEquipment;
            }

            EquipmentItem handEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentHandEquipmentID);

            if (handEquipment != null)
            {
                playerInventoryManager.currentHandEquipment = handEquipment as HandEquipment;
            }

            EquipmentItem legEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.currentLegEquipmentID);

            if (legEquipment != null)
            {
                playerInventoryManager.currentLegEquipment = legEquipment as LegEquipment;
            }

            playerEquipmentManager.EquipAllEquipmentModels();
            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(playerInventoryManager);

            // Diffrent idea:
            //playerInventoryManager.currentHelmetEquipment = WorldItemDataBase.Instance.GetEquipmnetItemByID(currentCharacterSaveData.currentHeadEquipmentID) as HelmetEquipment;

            // WEAPON SLOTS
            playerInventoryManager.weaponsInRightHandSlots[0] = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentRightHandSlot01WeaponID);
            playerInventoryManager.weaponsInRightHandSlots[1] = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentRightHandSlot02WeaponID);
            playerInventoryManager.weaponsInRightHandSlots[2] = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentRightHandSlot03WeaponID);
            playerInventoryManager.weaponsInLeftHandSlots[0] = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentLeftHandSlot01WeaponID);
            playerInventoryManager.weaponsInLeftHandSlots[1] = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentLeftHandSlot02WeaponID);
            playerInventoryManager.weaponsInLeftHandSlots[2] = WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.currentLeftHandSlot03WeaponID);
            uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);

            //playerWeaponSlotManager.LoadBothWeaponsOnSlot();

            // RANGED AMMO
            playerInventoryManager.currentAmmo01 = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.currentAmmo01ItemID);
            playerInventoryManager.currentAmmo02 = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.currentAmmo02ItemID);

            playerInventoryManager.rangedAmmoItemsInAmmoSlots[0] = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.currentBowArrowSlot01ItemID);
            playerInventoryManager.rangedAmmoItemsInAmmoSlots[1] = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.currentBowArrowSlot02ItemID);
            playerInventoryManager.rangedAmmoItemsInAmmoSlots[2] = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.currentCrossBowArrowSlot01ItemID);
            playerInventoryManager.rangedAmmoItemsInAmmoSlots[3] = WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.currentCrossBowArrowSlot02ItemID);

            uIManager.equipmentWindowUI.LoadRangedAmmoOnEquipmentScreen(playerInventoryManager);
            uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(playerInventoryManager.currentAmmo01, true);
            uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(playerInventoryManager.currentAmmo02, false);

            // AMULETS
            playerInventoryManager.currentAmuletSlot01 = WorldItemDataBase.Instance.GetAmuletItemByID(currentCharacterSaveData.currentAmuletSlot01ID);
            playerInventoryManager.currentAmuletSlot02 = WorldItemDataBase.Instance.GetAmuletItemByID(currentCharacterSaveData.currentAmuletSlot02ID);
            playerInventoryManager.currentAmuletSlot03 = WorldItemDataBase.Instance.GetAmuletItemByID(currentCharacterSaveData.currentAmuletSlot03ID);
            playerInventoryManager.currentAmuletSlot04 = WorldItemDataBase.Instance.GetAmuletItemByID(currentCharacterSaveData.currentAmuletSlot04ID);

            uIManager.equipmentWindowUI.LoadAmuletItemsOnEquipmentScreen(playerInventoryManager);

            if (playerInventoryManager.currentAmuletSlot01 != null)
            {
                if (!playerInventoryManager.currentAmuletSlot01.isEmpty)
                {
                    playerInventoryManager.currentAmuletSlot01.EquipAmulet(this);
                }
            }
            if (playerInventoryManager.currentAmuletSlot02 != null)
            {
                if (!playerInventoryManager.currentAmuletSlot02.isEmpty)
                {
                    playerInventoryManager.currentAmuletSlot02.EquipAmulet(this);
                }
            }
            if (playerInventoryManager.currentAmuletSlot03 != null)
            {
                if (!playerInventoryManager.currentAmuletSlot03.isEmpty)
                {
                    playerInventoryManager.currentAmuletSlot03.EquipAmulet(this);
                }
            }
            if (playerInventoryManager.currentAmuletSlot04 != null)
            {
                if (!playerInventoryManager.currentAmuletSlot04.isEmpty)
                {
                    playerInventoryManager.currentAmuletSlot04.EquipAmulet(this);
                }
            }

            // CONSUMABLE
            playerInventoryManager.currentConsumable = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentConsumableID);

            playerInventoryManager.consumablesInQuickSlots[0] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot01ItemID);
            playerInventoryManager.consumablesInQuickSlots[1] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot02ItemID);
            playerInventoryManager.consumablesInQuickSlots[2] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot03ItemID);
            playerInventoryManager.consumablesInQuickSlots[3] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot04ItemID);
            playerInventoryManager.consumablesInQuickSlots[4] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot05ItemID);
            playerInventoryManager.consumablesInQuickSlots[5] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot06ItemID);
            playerInventoryManager.consumablesInQuickSlots[6] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot07ItemID);
            playerInventoryManager.consumablesInQuickSlots[7] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot08ItemID);
            playerInventoryManager.consumablesInQuickSlots[8] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot09ItemID);
            playerInventoryManager.consumablesInQuickSlots[9] = WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.currentQuickSlot10ItemID);

            uIManager.equipmentWindowUI.LoadQuickSlotItemsOnEquipmentScreen(playerInventoryManager);
            uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(playerInventoryManager.currentConsumable);

            // INVETORY
            // ________

            // CONSUMABLES
            //playerInventoryManager.consumablesInventory = new List<ConsumableItem>(currentCharacterSaveData.consumableItemIDs.Count);

            for (int i = 0; i < currentCharacterSaveData.consumableInventoryItemIDs.Count; i++)
            {
                playerInventoryManager.consumablesInventory.Add(WorldItemDataBase.Instance.GetConsumableItemByID(currentCharacterSaveData.consumableInventoryItemIDs[i]));
            }

            // WEAPONS
            for (int i = 0; i < currentCharacterSaveData.weaponInventoryItemIDs.Count; i++)
            {
                playerInventoryManager.weaponsInventory.Add(WorldItemDataBase.Instance.GetWeaponItemByID(currentCharacterSaveData.weaponInventoryItemIDs[i]));
            }

            // HELMET EQUIPMENT
            for (int i = 0; i < currentCharacterSaveData.headInventoryItemIDs.Count; i++)
            {
                EquipmentItem headEquipmentInInventory = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.headInventoryItemIDs[i]);

                if (headEquipmentInInventory != null)
                {
                    playerInventoryManager.headEquipmentInventory.Add(headEquipmentInInventory as HelmetEquipment);
                }
            }

            // BODY EQUIPMENT
            for (int i = 0; i < currentCharacterSaveData.bodyInventoryItemIDs.Count; i++)
            {
                EquipmentItem bodyEquipmentInInventory = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.bodyInventoryItemIDs[i]);

                if (bodyEquipmentInInventory != null)
                {
                    playerInventoryManager.bodyEquipmentInventory.Add(bodyEquipmentInInventory as TorsoEquipment);
                }
            }

            // HAND EQUIPMENT
            for (int i = 0; i < currentCharacterSaveData.handInventoryItemIDs.Count; i++)
            {
                EquipmentItem handEquipmentInInventory = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.handInventoryItemIDs[i]);

                if (handEquipmentInInventory != null)
                {
                    playerInventoryManager.handEquipmentInventory.Add(handEquipmentInInventory as HandEquipment);
                }
            }

            // LEG EQUIPMENT
            for (int i = 0; i < currentCharacterSaveData.legInventoryItemIDs.Count; i++)
            {
                EquipmentItem legEquipmentInInventory = WorldItemDataBase.Instance.GetEquipmentItemByID(currentCharacterSaveData.legInventoryItemIDs[i]);

                if (legEquipmentInInventory != null)
                {
                    playerInventoryManager.legEquipmentInventory.Add(legEquipmentInInventory as LegEquipment);
                }
            }

            // RANGED AMMO
            for (int i = 0; i < currentCharacterSaveData.rangedAmmoInventoryItemIDs.Count; i++)
            {
                playerInventoryManager.rangedAmmoItemsInventory.Add(WorldItemDataBase.Instance.GetRangedAmmoItemByID(currentCharacterSaveData.rangedAmmoInventoryItemIDs[i]));
            }

            // AMULETS
            for (int i = 0; i < currentCharacterSaveData.amuletInventoryItemIDs.Count; i++)
            {
                playerInventoryManager.amuletsInventory.Add(WorldItemDataBase.Instance.GetAmuletItemByID(currentCharacterSaveData.amuletInventoryItemIDs[i]));
            }

            uIManager.UptadeUI();
        }

        public void LoadCharacterCreationDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            // CHARACTER INFO
            playerStatsManager.characterName = currentCharacterSaveData.characterName;
            playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;
            playerStatsManager.className = currentCharacterSaveData.characterClass;

            // CHARACTER STAT LEVELS
            playerStatsManager.healthLevel = currentCharacterSaveData.characterHealth;
            playerStatsManager.staminaLevel = currentCharacterSaveData.characterStamina;
            playerStatsManager.focusLevel = currentCharacterSaveData.characterMana;
            playerStatsManager.strenghtLevel = currentCharacterSaveData.characterStrenght;
            playerStatsManager.dexterityLevel = currentCharacterSaveData.characterDexterity;
            playerStatsManager.intelligenceLevel = currentCharacterSaveData.characterIntelligence;
            playerStatsManager.faithLevel = currentCharacterSaveData.characterFaith;
            playerStatsManager.arcaneLevel = currentCharacterSaveData.characterArcane;

            // CHARACTER APPEREANCE
            marksColor = new Color(currentCharacterSaveData.marksRedAmount, currentCharacterSaveData.marksGreenAmount, currentCharacterSaveData.marksBlueAmount);
            hairColor = new Color(currentCharacterSaveData.hairRedAmount, currentCharacterSaveData.hairGreenAmount, currentCharacterSaveData.hairBlueAmount);
            eyebrownsColor = new Color(currentCharacterSaveData.eyebrownsRedAmount, currentCharacterSaveData.eyebrownsGreenAmount, currentCharacterSaveData.eyebrownsBlueAmount);
            eyeColor = new Color(currentCharacterSaveData.eyeRedAmount, currentCharacterSaveData.eyeGreenAmount, currentCharacterSaveData.eyeBlueAmount);
            facialHairColor = new Color(currentCharacterSaveData.facialHairRedAmount, currentCharacterSaveData.facialHairGreenAmount, currentCharacterSaveData.facialHairBlueAmount);
            skinColor = new Color(currentCharacterSaveData.skinRedAmount, currentCharacterSaveData.skinGreenAmount, currentCharacterSaveData.skinBlueAmount);

            SetCharacterAppereanceGameObjectActiveFromIndex(parentHeadGameObject, currentCharacterSaveData.characterNakedHeadIndex, "_Color_BodyArt", marksColor);
            SetCharacterAppereanceGameObjectActiveFromIndex(parentHairGameObject, currentCharacterSaveData.characterHairIndex, "_Color_Hair", hairColor);
            SetCharacterAppereanceGameObjectActiveFromIndex(parentEyebrownsGameObject, currentCharacterSaveData.characterEyebrownsIndex, "_Color_Hair", eyebrownsColor);
            SetCharacterAppereanceGameObjectActiveFromIndex(parentFacialHairGameObject, currentCharacterSaveData.characterFacialHairIndex, "_Color_Hair", facialHairColor);
            SetSkinColor();
        }

        void SetCharacterAppereanceGameObjectActiveFromIndex(GameObject parentGameObject, int savedAppereanceIndex, string colorName, Color neededColor)
        {
            for (int i = 0; i < parentGameObject.transform.childCount; i++)
            {
                var child = parentGameObject.transform.GetChild(i).gameObject;

                if (child != null)
                {
                    child.SetActive(false);
                    if (i == savedAppereanceIndex)
                    {
                        child.SetActive(true);
                        child.GetComponent<SkinnedMeshRenderer>().material.SetColor(colorName, neededColor);

                        if (parentGameObject == parentHeadGameObject)
                        {
                            playerEquipmentManager.nakedHeadModel = child;
                            child.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color_Skin", skinColor);
                            child.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color_Eyes", eyeColor);
                        }
                    }
                }
            }
        }

        void SetSkinColor()
        {
            for (int i = 0; i < skinRendererList.Count; i++)
            {
                //If using SYNTY models
                skinRendererList[i].material.SetColor("_Color_Skin", skinColor);

                //If using regular models
                //rendererList[i].material.SetColor("_Color", currentHairColor);
            }
        }
    }
}
