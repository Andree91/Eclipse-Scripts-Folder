using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Dropecho;

namespace AG
{
    public class PlayerMovement : MonoBehaviour
    {
        PlayerManager player;
        public Vector3 moveDirection;

        public Rigidbody rb;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1.0f;
        [SerializeField] float groundDirectionRayDistance = 0.2f;
        [SerializeField] float maxDistance = 1f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] LayerMask ignoreLayer;
        [SerializeField] int fallDamage = 25;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 5.0f;
        [SerializeField] float walkingSpeed = 2.0f;
        [SerializeField] float crouchingSpeed = 2.0f;
        [SerializeField] float sprintSpeed = 7.0f;
        [SerializeField] float rotationSpeed = 10.0f;
        [SerializeField] float rotationSpeedWhileAiming = 5.0f;
        [SerializeField] float fallingSpeed = 45f;
        [SerializeField] float leapingVelocity = 0f;

        [Header("Jump Stats")]
        [SerializeField] float jumpHeight = 3.0f;
        [SerializeField] float gravityIntensity = -15.0f;

        [Header("Stamina Costs")]
        [SerializeField] int rollStaminaCost = 15;
        [SerializeField] int backStepStaminaCost = 12;
        [SerializeField] int jumpCost = 15;
        [SerializeField] int sprintStaminaCost = 1;
        [SerializeField] float staminaFallDamage = 35;

        Vector3 normalVector;
        Vector3 targetPosition;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        void Awake() 
        {
            player = GetComponent<PlayerManager>();
            rb = GetComponent<Rigidbody>();
            //Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        void Start()
        {
            player.isGrounded = true;
            //groundLayer = ~(1 << 8 | 1 << 11);
        }

        #region Movement

        public void HandleRotation()
        {
            if (player.isJumping)  { return; }

            if (player.isClimbing) { return; }

            if (player.isRiding) { return; }

            if (player.canRotate)
            {
                if (player.isAiming)
                {
                    Quaternion targerRotation = Quaternion.Euler(0, player.cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targerRotation, rotationSpeedWhileAiming * Time.deltaTime);
                    transform.rotation = playerRotation;
                }
                else
                {
                    if (player.inputHandler.lockOnFlag)
                    {
                        if (player.inputHandler.sprintFlag || player.inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = player.cameraHandler.cameraTransform.forward * player.inputHandler.vertical;
                            targetDirection += player.cameraHandler.cameraTransform.right * player.inputHandler.horizontal;
                            targetDirection.Normalize();
                            targetDirection.y = 0;

                            if (targetDirection == Vector3.zero)
                            {
                                targetDirection = player.transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = player.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.Normalize();
                            rotationDirection.y = 0;

                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                    }
                    else
                    {
                        Vector3 targetDirection = Vector3.zero;
                        float moveOverride = player.inputHandler.moveAmount;

                        targetDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                        targetDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = player.transform.forward;
                        }

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDirection); // target rotation
                        Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);

                        player.transform.rotation = targetRotation;
                    }
                }
            }

        }

        public void HandleMovement()
        {
            if (player.isJumping) { return; }

            if (player.inputHandler.rollFlag) { return; }

            if (player.isInteracting) { return; }

            if (player.isRiding) { return; }

            if (player.isOnSandGlider) { return; }

            if (player.isOnShip) { return; }

            // if (player.animator.GetBool("isMoving") == false) { 
            //     Debug.Log("isMoving is  " + player.animator.GetBool("isMoving"));
            //     return ;}

            if (!player.isMoving) 
            { 
                moveDirection.x = 0;
                moveDirection.y = 0;
                return; 
            }

            if (player.isClimbing)
            {
                moveDirection = player.cameraHandler.cameraObject.transform.up * player.inputHandler.vertical;
                moveDirection.Normalize();
                moveDirection = moveDirection / 4;
            }
            else if (player.isUnderWater || player.isInGodMode)
            {
                moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;

                if (player.inputHandler.hold_x_Input)
                {
                    moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                    moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                    moveDirection.Normalize();

                    float diveVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                    Vector3 playerVelocity = moveDirection / 2;
                    playerVelocity.y = diveVelocity;
                    //Debug.Log($"x input is {player.inputHandler.x_Input}");
                    //player.inputHandler.x_Input = false;
                    rb.AddForce(Vector3.down * playerVelocity.y * 20);
                }
                else if (player.inputHandler.hold_jump_Input)
                {
                    //player.inputHandler.jump_Input = false;

                    moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                    moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                    moveDirection.y = 0;

                    float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                    Vector3 playerVelocity = moveDirection / 2;
                    playerVelocity.y = jumpingVelocity;
                    //rb.velocity = playerVelocity;
                    rb.AddForce(Vector3.up * playerVelocity.y * 40);
                    //Debug.Log($"Player total velocity is = {rb.velocity} and Y velocity is = {playerVelocity.y}");
                }
            }
            else
            {
                moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                moveDirection.Normalize();
                moveDirection.y = 0;
            }

            float speed = movementSpeed;

            if (player.inputHandler.sprintFlag && player.inputHandler.moveAmount > 0.5f && !player.isJumping && inAirTimer <= 0)
            {
                //player.playerAnimatorManager.EraseHalfOfHandIKForWeapon();
                if (player.isCrouching)
                {
                    speed = crouchingSpeed * 2f;
                    player.footStepAudio.crouchingVolume = true;
                }
                else
                {
                    speed = sprintSpeed;
                    player.footStepAudio.crouchingVolume = false;
                }
                player.isSprinting = true;
                moveDirection *= speed;
                player.playerStatsManager.DetuctStamina(sprintStaminaCost);
            }
            else if (player.isCrouching && !player.inputHandler.sprintFlag)
            {
                moveDirection *= crouchingSpeed;
                player.footStepAudio.crouchingVolume = true;
                player.isSprinting = false;
            }
            else
            {
                player.footStepAudio.crouchingVolume = false;
                if (player.inputHandler.moveAmount <= 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    player.isInteracting = false;
                    player.isSprinting = false;
                }
                else
                {
                    //player.playerAnimatorManager.EraseHalfOfHandIKForWeapon();
                    moveDirection *= speed;
                    player.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rb.velocity = projectedVelocity;

            if (player.inputHandler.lockOnFlag && player.inputHandler.sprintFlag == false)
            {
                player.playerAnimatorManager.UptadeAnimatorValues(player.inputHandler.vertical, player.inputHandler.horizontal, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.UptadeAnimatorValues(player.inputHandler.moveAmount, 0, player.isSprinting);
            }

        }

        public void HandleRollingAndSprinting()
        {
            if (player.animator.GetBool("isInteracting")) { return; }

            if (player.playerStatsManager.currentStamina <= 0) 
            {
                player.inputHandler.rollFlag = false;
                return; 
            }

            if (player.isRiding) { return; }

            if (player.inputHandler.rollFlag)
            {
                player.inputHandler.rollFlag = false;

                if (player.isJumping) { return; }

                if (inAirTimer > 0) { return; }

                if (player.isClimbing) { return; }

                if (player.isSwimming) { return; }

                if (player.isUnderWater) { return; }

                moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

                if (player.inputHandler.moveAmount > 0)
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                    //player.playerAnimatorManager.EraseHalfOfHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    player.transform.rotation = rollRotation;
                    player.playerStatsManager.DetuctStamina(rollStaminaCost);
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Backstep", true);
                    //player.playerAnimatorManager.EraseHalfOfHandIKForWeapon();
                    player.playerStatsManager.DetuctStamina(backStepStaminaCost);
                }
            }
        }

        public void HandleFalling(Vector3 moveDirection)
        {
            player.isGrounded = false;
            RaycastHit hit;
            Vector3 originalPos = player.transform.position;
            originalPos.y +=  groundDetectionRayStartPoint;

            if (Physics.Raycast(originalPos, player.transform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (player.isInAir)
            {
                rb.AddForce(-(Vector3.up) * fallingSpeed);
                rb.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 direction = moveDirection;
            direction.Normalize();
            originalPos = originalPos + direction * groundDirectionRayDistance;

            targetPosition = player.transform.position;

            Debug.DrawRay(originalPos, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(originalPos, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundLayer))
            {
                normalVector = hit.normal;
                Vector3 temp = hit.point;
                player.isGrounded = true;
                targetPosition.y = temp.y;

                
                if (player.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for " + inAirTimer);
                        player.playerAnimatorManager.PlayTargetAnimation("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0;

                    }

                    player.isInAir = false;
                }
                else
                {
                    if (player.isGrounded)
                    {
                        player.isGrounded = false;
                    }

                    if (player.isInAir == false)
                    {
                        if (player.isInteracting == false)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation("Falling", true);
                        }

                        Vector3 velocity = rb.velocity;
                        velocity.Normalize();
                        rb.velocity = velocity * (movementSpeed / 2);
                        player.isInAir = true;
                    }
                }

                if (player.isGrounded)
                {
                    if (player.isInAir || player.inputHandler.moveAmount > 0)
                    {
                        player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime);
                    }
                    else
                    {
                        player.transform.position = targetPosition;
                    }
                }
            }

            if (player.isInteracting || player.inputHandler.moveAmount > 0)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                player.transform.position = targetPosition;
            }
        }

        public void HandleFallingAndLanding()
        {
            if (player.isOnSandGlider) { return; }

            if (player.isOnShip) { return; }

            if (player.isSwimming) { return; }

            if (player.isUnderWater) { return; }

            if (player.isClimbing) { return; }

            if (player.isRiding) { return; }

            if (player.isInGodMode) { return; }

            RaycastHit hit;
            Vector3 rayCastOrigin = transform.position;
            Vector3 targetPosition;
            rayCastOrigin.y = rayCastOrigin.y + groundDetectionRayStartPoint;
            targetPosition = transform.position;

            if (!player.isGrounded && !player.isJumping)
            {
                inAirTimer = inAirTimer + Time.deltaTime;
                rb.AddForce(transform.forward * leapingVelocity);
                rb.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
                //player.isInAir = true;

                if (!player.isInteracting && inAirTimer > 0.7f)
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Falling_Low", true);
                }
                else if (!player.isInteracting && inAirTimer >= 1f)
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Falling", true);
                }
                else if (inAirTimer >= 5f && !player.isDiedByFalling)
                {
                    player.isDiedByFalling = true;
                    player.HandlePlayerDeath();
                }
            }
            if (!player.isTakingScreenShots) Debug.DrawRay(rayCastOrigin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer))
            {
                if (!player.isGrounded && player.isInteracting)
                {
                    if (inAirTimer >= 1f)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation("Land", true);

                        // Change this to height based stamina and damage calculation
                        if (inAirTimer >= 1.2f && inAirTimer < 1.5f)
                        {
                            // this.enabled = false;
                            // StartCoroutine(Enablemovement());
                            player.playerStatsManager.DetuctStamina(staminaFallDamage * inAirTimer);
                            //player.playerStatsManager.TakeDamageNoAnimation(Mathf.RoundToInt(fallDamage * inAirTimer), 0, null);
                            Debug.Log("Air Timer Is " + inAirTimer);
                        }
                        else if (inAirTimer >= 1.5f && inAirTimer < 1.8f)
                        {
                            player.playerStatsManager.TakeDamageNoAnimation(Mathf.RoundToInt(fallDamage * inAirTimer), 0, 0, null);
                            Debug.Log("Air Timer Is " + inAirTimer);
                        }
                        else if (inAirTimer >= 1.8f && inAirTimer < 2f)
                        {
                            player.playerStatsManager.TakeDamageNoAnimation(Mathf.RoundToInt(fallDamage * 2 * inAirTimer), 0, 0, null);
                            Debug.Log("Air Timer Is " + inAirTimer);
                            Debug.Log("Fall damge is " + Mathf.RoundToInt(fallDamage * 2 * inAirTimer));
                        }
                        else if (inAirTimer >= 2f)
                        {
                            player.playerStatsManager.TakeDamageNoAnimation(Mathf.RoundToInt(fallDamage * 3 * inAirTimer), 0, 0, null);
                            Debug.Log("Air Timer Is " + inAirTimer);
                            Debug.Log("Fall damge is " + Mathf.RoundToInt(fallDamage * 3 * inAirTimer));
                        }
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation("Empty", false);
                    }
                }

                Vector3 rayCastHitPoint = hit.point;
                targetPosition.y = rayCastHitPoint.y;
                inAirTimer = 0;
                player.isGrounded = true;
                //player.isInteracting = false;
            }
            else
            {
                player.isGrounded = false;
            }

            if (player.isGrounded && !player.isJumping)
            {
                if (hit.collider.gameObject.layer != 15)
                {
                    player.lastSafePosition = transform.position;
                }

                if (player.isInteracting || player.inputHandler.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        // IEnumerator Enablemovement()
        // {
        //     yield return new WaitForSeconds(1.0f);
        //     this.enabled = true;
        // }

        // public void HandleJumping()
        // {
        //     if (player.isInteracting) { return; }

        //     if (player.playerStatsManager.currentStamina <= 0) { return; }

        //     if (player.inputHandler.jump_Input)
        //     {
        //         if (!player.isOnSandGlider)
        //         {
        //             player.inputHandler.jump_Input = false;
        //             player.isJumping = true;
        //             moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
        //             moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
        //             player.playerAnimatorManager.PlayTargetAnimation("Jump", true);
        //             player.playerAnimatorManager.EraseHalfOfHandIKForWeapon();
        //             player.animator.SetBool("isInteracting", false);
        //             moveDirection.y = 0;
        //             if (moveDirection != null)
        //             {
        //                 Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
        //                 player.transform.rotation = jumpRotation;
        //             }
        //             else
        //             {
        //                 Quaternion jumpRotation = Quaternion.LookRotation(player.transform.position);
        //                 player.transform.rotation = jumpRotation;
        //             }
                    
        //             StartCoroutine(ResetJumping());
        //         }  
        //     }
        // }

        public void HandleJumping()
        {
            if (player.isInteracting) { return; }

            if (player.playerStatsManager.currentStamina <= 0) { return; }

            if (player.isSwimming) { return; }

            if (player.isRiding) { return; }

            if (player.isJumping) { return; }

            if (player.inputHandler.rollFlag) { return; }

            //if (player.isInGodMode) { return; }

            if (player.inputHandler.jump_Input)
            {
                if (player.playerStatsManager.currentStamina <= 0)
                {
                    player.inputHandler.jump_Input = false;
                    return;
                }

                if (!player.isOnSandGlider && !player.isUnderWater && player.isGrounded && !player.isInGodMode && !player.isOnShip && !player.isClimbing)
                {
                    player.inputHandler.jump_Input = false;

                    moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                    moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                    moveDirection.y = 0;

                    player.canJumpAttack = true; //Propably going to change this on animation event

                    player.animator.SetBool("isJumping", true);
                    player.playerAnimatorManager.PlayTargetAnimation("Jump", false);

                    float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                    Vector3 playerVelocity = moveDirection / 3;
                    playerVelocity.y = jumpingVelocity;
                    //rb.velocity = playerVelocity;
                    rb.AddForce(Vector3.up * playerVelocity.y * 40);
                    //Debug.Log($"Player total velocity is = {rb.velocity} and Y velocity is = {playerVelocity.y}");

                    player.playerStatsManager.DetuctStamina(jumpCost);

                    StartCoroutine(ResetJumpAttack());
                }
                else if (player.isUnderWater || player.isInGodMode)
                {
                    player.inputHandler.jump_Input = false;

                    moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                    moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
                    moveDirection.y = 0;

                    float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
                    Vector3 playerVelocity = moveDirection / 2;
                    playerVelocity.y = jumpingVelocity;
                    //rb.velocity = playerVelocity;
                    rb.AddForce(Vector3.up * playerVelocity.y * 20);
                    //Debug.Log($"Player total velocity is = {rb.velocity} and Y velocity is = {playerVelocity.y}");
                }
            }
        }

        IEnumerator ResetJumpAttack()
        {
            yield return new WaitForSeconds(1.3f);
            player.canJumpAttack = false;
        }

        #endregion

    }
}
