using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool y_Input;
        public bool yy_Input;
        public bool x_Input;
        public bool hold_x_Input;
        public bool jump_Input;
        public bool hold_jump_Input;
        public bool hold_p_Input;

        public bool tap_rb_Input;
        public bool hold_rb_Input;
        public bool tap_rt_Input;
        public bool hold_rt_Input;
        public bool tap_lb_Input;
        public bool hold_lb_Input;
        public bool tap_lt_Input;


        public bool inventory_Input;
        public bool map_Input;
        public bool lockOn_Input;
        public bool crouch_Input;
        public bool right_Stick_Left_Input;
        public bool right_Stick_Right_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool hold_d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool sprintFlag;
        public bool twoHandFlag;
        public bool comboFlag;
        public bool fireFlagRB;
        public bool fireFlagRT;
        public bool lockOnFlag;
        public bool crouchFlag;
        public bool inventoryFlag;
        public bool mapFlag;
        public float rollInputTimer;

        [Header("Queue Input")]
        public bool input_Has_Been_Queued;
        public float current_Queued_Input_Timer;
        public float default_Queued_Input_Time = 0.3f;
        public bool queued_rb_Input;
        public bool queued_rt_Input;
        public bool queued_lb_Input;
        public bool queued_lt_Input;

        PlayerControls inputActions;
        public PlayerManager player;


        Vector2 movementInput;
        Vector2 cameraInput;

        void Awake()
        {
            player = GetComponent<PlayerManager>();

        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.Player.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.Player.TapRB.performed += i => tap_rb_Input = true;
                inputActions.Player.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.Player.HoldRB.canceled += i => hold_rb_Input = false;
                inputActions.Player.HoldRB.canceled += i => fireFlagRB = true;
                inputActions.Player.TapRT.performed += i => tap_rt_Input = true;
                inputActions.Player.HoldRT.performed += i => hold_rt_Input = true;
                inputActions.Player.HoldRT.canceled += i => hold_rt_Input = false;
                inputActions.Player.HoldRT.canceled += i => fireFlagRT = true;
                inputActions.Player.TapLB.performed += i => tap_lb_Input = true;
                inputActions.Player.HoldLB.performed += i => hold_lb_Input = true;
                inputActions.Player.HoldLB.canceled += i => hold_lb_Input = false;
                inputActions.Player.TapLT.performed += i => tap_lt_Input = true;
                inputActions.Player.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.Player.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.Player.DPadUp.performed += i => d_Pad_Up = true;
                inputActions.Player.DPadDown.performed += i => d_Pad_Down = true;
                inputActions.Player.HoldDPadDown.performed += i => hold_d_Pad_Down = true;
                //inputActions.Player.HoldDPadDown.performed += i => d_Pad_Down = false;
                inputActions.Player.HoldDPadDown.canceled += i => hold_d_Pad_Down = false;
                inputActions.Player.Interact.performed += i => y_Input = true;
                inputActions.Player.Roll.performed += i => b_Input = true;
                inputActions.Player.Roll.canceled += i => b_Input = false;
                inputActions.Player.Jump.performed += i => jump_Input = true;
                inputActions.Player.HoldJump.performed += i => hold_jump_Input = true;
                inputActions.Player.HoldJump.canceled += i => hold_jump_Input = false;
                inputActions.Player.Inventory.performed += i => inventory_Input = true;
                inputActions.Player.LockOn.performed += i => lockOn_Input = true;
                inputActions.Player.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.Player.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.Player.Crouch.performed += i => crouch_Input = true;
                inputActions.Player.Y.performed += i => yy_Input = true;
                inputActions.Player.X.performed += i => x_Input = true;
                inputActions.Player.Map.performed += i => map_Input = true;
                inputActions.Player.HoldX.performed += i => hold_x_Input = true;
                inputActions.Player.HoldX.canceled += i => hold_x_Input = false;
                inputActions.Player.HoldP.performed += i => hold_p_Input = true;
                inputActions.Player.HoldP.canceled += i => hold_p_Input = false;

                inputActions.Player.QueuedRB.performed += i => QueueInput(ref queued_rb_Input);
                inputActions.Player.QueuedRT.performed += i => QueueInput(ref queued_rt_Input);
                inputActions.Player.QueuedLB.performed += i => QueueInput(ref queued_lb_Input);
                inputActions.Player.QueuedLT.performed += i => QueueInput(ref queued_lt_Input);
            }

            inputActions.Enable();
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if (player.isDead) { return; }

            HandleMoveInput();
            HandleRollInput();

            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLBInput();
            HandleTapLTInput();

            HandleHoldLBInput();
            HandleHoldRBInput();
            HandleHoldRTInput();

            HandleQueuedInput();

            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleMapInput();
            HandleShowHUDInput();
            HandleLockOnInput();
            HandleCrouchInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            fireFlagRB = false;
            fireFlagRT = false;

            HandlLanternInput();
        }

        void HandleMoveInput()
        {
            if (player.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)) / 2f);
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else if (player.isCrouching)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                if (sprintFlag)
                {
                    Debug.Log("Move amount sprint crouch");
                    moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                }
                else
                {
                    float moveTemp = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                    moveAmount = Mathf.Clamp(moveTemp, 0f, 0.5f);
                }
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else if (player.isRiding)
            {
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        void HandleRollInput()
        {
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;

                if (player.playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
                else if (crouchFlag && player.playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;

                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                //sprintFlag = false;
                rollInputTimer = 0;
            }
        }

        void HandleTapRBInput()
        {
            //RB Input handles the RIGHT hand weapon's light attack
            if (tap_rb_Input)
            {
                tap_rb_Input = false;

                if (inventoryFlag)
                {
                    if (player.uIManager.isInInventoryWindow)
                    {
                        player.uIManager.ChangeInventoryItemWindowToRight();
                    }
                    return;
                }

                if (player.playerInventoryManager.rightWeapon.tap_RB_action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RB_action.PerformAction(player);
                }
            }
        }

        void HandleHoldRBInput()
        {
            if (player.isInAir || player.isSprinting || player.isFiringSpell)
            {
                //hold_lb_Input = false;
                hold_rb_Input = false;
                return;
            }

            if (hold_rb_Input)
            {
                if (player.playerInventoryManager.rightWeapon.hold_RB_action != null)
                {
                    if (player.isRiding) { return; }
                    //Debug.Log("Player is riding is" + player.isRiding);
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_RB_action.PerformAction(player);
                }
            }
            else if (hold_rb_Input == false && fireFlagRB)
            {
                if (player.playerInventoryManager.rightWeapon.release_RB_action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.release_RB_action.PerformAction(player);
                }
            }
        }

        void HandleTapRTInput()
        {
            //RT Input handles the RIGHT hand weapon's heavy attack
            if (tap_rt_Input)
            {
                tap_rt_Input = false;

                if (player.playerInventoryManager.rightWeapon.tap_RT_action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RT_action.PerformAction(player);
                }
            }
        }

        void HandleHoldRTInput()
        {
            player.animator.SetBool("isChargingAttack", hold_rt_Input);

            if (hold_rt_Input)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHanding)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_RT_action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_RT_action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.hold_RT_action != null)
                    {
                        player.playerInventoryManager.rightWeapon.hold_RT_action.PerformAction(player);
                    }
                }
            }
            else if (hold_rt_Input == false && fireFlagRT)
            {
                if (player.playerInventoryManager.rightWeapon.release_RT_action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    Debug.Log("Before performing release RT action");
                    player.playerInventoryManager.rightWeapon.release_RT_action.PerformAction(player);
                }
            }
        }

        void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;

                if (player.playerInventoryManager.rightWeapon.tap_LT_action != null)
                {
                    if (player.isTwoHanding)
                    {
                        if (player.playerInventoryManager.rightWeapon.tap_LT_action != null)
                        {
                            //It will be right hand weapon
                            player.UpdateWhichHandCharacterIsUsing(true);
                            player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                            player.playerInventoryManager.rightWeapon.tap_LT_action.PerformAction(player);
                        }
                    }
                    else
                    {
                        if (player.playerInventoryManager.leftWeapon.tap_LT_action != null)
                        {
                            player.UpdateWhichHandCharacterIsUsing(false);
                            player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                            player.playerInventoryManager.leftWeapon.tap_LT_action.PerformAction(player);
                        }
                    }
                }
            }
        }

        void HandleTapLBInput()
        {
            if (tap_lb_Input)
            {
                tap_lb_Input = false;

                if (inventoryFlag)
                {
                    if (player.uIManager.isInInventoryWindow)
                    {
                        player.uIManager.ChangeInventoryItemWindowToLeft();
                    }
                    return;
                }

                if (player.isTwoHanding)
                {
                    if (player.playerInventoryManager.rightWeapon.tap_LB_action != null)
                    {
                        //It will be right hand weapon
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.tap_LB_action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.tap_LB_action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LB_action.PerformAction(player);
                    }
                }
            }
        }

        void HandleHoldLBInput()
        {
            if (player.isInAir || player.isSprinting || player.isFiringSpell)
            {
                hold_lb_Input = false;
                return;
            }

            if (hold_lb_Input)
            {
                if (player.isTwoHanding)
                {
                    if (player.playerInventoryManager.rightWeapon.hold_LB_action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.hold_LB_action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.hold_LB_action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.hold_LB_action.PerformAction(player);
                    }
                }
            }
            else if (hold_lb_Input == false)
            {
                if (player.isAiming)
                {
                    player.isAiming = false;
                    player.uIManager.crossHair.SetActive(false);
                    player.cameraHandler.ResetAimCameraRotations();
                }

                if (player.isBlocking)
                {
                    player.isBlocking = false;
                    player.leftHandIsShield = false;
                }
            }
        }

        void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                player.playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                player.playerInventoryManager.ChangeLeftWeapon();
            }
            else if (d_Pad_Up)
            {
                player.playerInventoryManager.ChangeSpellItem();
            }
            // else if (hold_d_Pad_Up)
            // {
            //     player.playerInventoryManager.ChangeSpellItemBackToFirst();
            // }
            else if (d_Pad_Down)
            {
                player.playerInventoryManager.ChangeConsumableItem();
            }
            else if (hold_d_Pad_Down)
            {
                player.playerInventoryManager.ChangeConsummableItemBackToFirst();
            }
        }

        void HandleInventoryInput()
        {
            if (inventoryFlag)
            {
                player.uIManager.UptadeUI();
            }

            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    player.uIManager.OpenSelectWindow();
                    player.uIManager.hudWindow.SetActive(false);
                }
                else
                {
                    player.uIManager.CloseSelectWindow();
                    player.uIManager.CloseAllInventoryWindows();
                    player.uIManager.hudWindow.SetActive(true);
                    player.uIManager.ShowHUD();
                }
            }
        }

        void HandleMapInput()
        {
            if (player.uIManager.mapWindow != null)
            {
                if (map_Input)
                {
                    map_Input = false;
                    mapFlag = !mapFlag;

                    if (mapFlag)
                    {
                        player.uIManager.mapWindow.SetActive(true);
                        player.uIManager.hudWindow.SetActive(false);
                    }
                    else if (!mapFlag && !player.isTalking)
                    {
                        player.uIManager.mapWindow.SetActive(false);
                        player.uIManager.hudWindow.SetActive(true);
                        player.uIManager.ShowHUD();
                    }
                }

            }
        }

        void HandleLockOnInput()
        {
            if (lockOn_Input && !lockOnFlag)
            {
                lockOn_Input = false;
                player.cameraHandler.HandleLockOn();

                if (player.cameraHandler.nearestLockOnTarget != null && !player.cameraHandler.nearestLockOnTarget.isDead)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                    //player.isInCombat = true; // GOING TO FIND BETTER PLACE FOR THIS
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTargets();
                //player.isInCombat = false; // GOING TO FIND BETTER PLACE FOR THIS
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                player.cameraHandler.HandleLockOn();

                if (player.cameraHandler.leftLockTarget != null)
                {
                    EnemyManager enemy = player.cameraHandler.currentLockOnTarget as EnemyManager;
                    enemy.enemyStatsManager.enemyHealthBar.slider.gameObject.SetActive(false);
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                }

            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                player.cameraHandler.HandleLockOn();

                if (player.cameraHandler.rightLockTarget != null)
                {
                    EnemyManager enemy = player.cameraHandler.currentLockOnTarget as EnemyManager;
                    enemy.enemyStatsManager.enemyHealthBar.slider.gameObject.SetActive(false);
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
                }
            }

            if (player.cameraHandler != null)
            {
                player.cameraHandler.SetCameraHeight();
            }
        }

        void HandleCrouchInput()
        {
            if (!player.isSwimming && !player.isUnderWater && !player.isJumping && player.playerMovement.inAirTimer <= 0)
            {
                if (crouch_Input)
                {
                    crouch_Input = false;

                    crouchFlag = !crouchFlag;

                    if (crouchFlag)
                    {
                        player.isCrouching = true;
                    }
                    else
                    {
                        player.isCrouching = false;
                    }
                }
            }
        }

        void HandleTwoHandInput()
        {
            if (yy_Input)
            {
                yy_Input = false;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    player.isTwoHanding = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    //player.playerWeaponSlotManager.LoadTwoHandIKTargets(true);
                    //Play animation on changing to TWO handing
                }
                else
                {
                    player.isTwoHanding = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    //player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
                    //Play animation on changing to ONE handing
                }
            }
        }



        //This was an old Critical attack input handler
        // void HandleCriticalAttackInput()
        // {
        //     if (critical_Attack_Input && (playerWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword || 
        //                                     playerWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.GreatSword ||
        //                                     playerWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.ColossalSword))
        //     {
        //         critical_Attack_Input = false;
        //         playerCombatManager.AttemptBackStabOrRiposte();
        //     }
        // }

        void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                if (player.isUnderWater || player.isSwimming || player.isInGodMode)
                {
                    x_Input = false;
                    return;
                }
                else if (inventoryFlag)
                {
                    x_Input = false;
                    // Check which equipment item we have selected and unequip it back to inventory
                    player.uIManager.CheckWhichEquipmentSlotIsSelected();

                }
                else
                {
                    x_Input = false;

                    player.playerInventoryManager.currentConsumable.AttempToConsumeItem(player);
                }
            }
        }

        void QueueInput(ref bool queuedInput)
        {
            //  Disable all other queued inputs
            queued_rb_Input = false;
            queued_rt_Input = false;
            queued_lb_Input = false;
            queued_lt_Input = false;

            // Enable refecense bool queued input
            // If we are intertacting, we can queue input, otherwise queue is not needed
            if (player.isInteracting)
            {
                queuedInput = true;
                current_Queued_Input_Timer = default_Queued_Input_Time;
                input_Has_Been_Queued = true;
            }
        }

        void HandleQueuedInput()
        {
            if (input_Has_Been_Queued)
            {
                if (current_Queued_Input_Timer > 0)
                {
                    current_Queued_Input_Timer -= Time.deltaTime;
                    // Try and process input
                    ProcessQueuedInput();
                }
            }
            else
            {
                input_Has_Been_Queued = false;
                current_Queued_Input_Timer = 0;
            }
        }

        void ProcessQueuedInput()
        {
            if (queued_rb_Input)
            {
                tap_rb_Input = true;
            }
            else if (queued_rt_Input)
            {
                tap_rt_Input = true;
            }
            else if (queued_lb_Input)
            {
                tap_lb_Input = true;
            }
            else if (queued_lt_Input)
            {
                tap_lt_Input = true;
            }
        }

        void HandlLanternInput()
        {
            if (hold_p_Input)
            {
                hold_p_Input = false;
                player.ToggleLanternInteraction();
            }
        }

        void HandleShowHUDInput()
        {
            if (!yy_Input)
            {
                if (y_Input)
                {
                    if (!player.uIManager.hud.statsBars.activeInHierarchy && !player.interactableUIGameObject.activeInHierarchy && !player.itemInteractableGameObject.activeInHierarchy)
                    {
                        y_Input = false;
                        player.uIManager.ShowHUD();
                    }
                }
            }
        }

    }
}
