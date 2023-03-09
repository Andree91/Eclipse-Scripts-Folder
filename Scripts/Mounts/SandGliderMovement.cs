using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class SandGliderMovement : MonoBehaviour
    {
        PlayerManager player;
        public Vector3 moveDirection;

        public Rigidbody sandGliderRb;
        public GameObject sandGliderCamera;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 10.0f;
        [SerializeField] float rotationSpeed = 10.0f;
        [SerializeField] float fallingSpeed = 45f;
        [SerializeField] float leapingVelocity = 5f;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        //[SerializeField] float minimumDistanceNeededToBeginFall = 1.0f;
        //[SerializeField] float groundDirectionRayDistance = 0.2f;
        [SerializeField] float maxDistance = 1f;
        [SerializeField] LayerMask groundLayer;
        public float inAirTimer;

        Vector3 normalVector;
        Vector3 targetPosition;

        void Awake() 
        {
            sandGliderRb = GetComponent<Rigidbody>();
            player = FindObjectOfType<PlayerManager>();
        }

        // public void HandleSandGliderRotation()
        // {
        //     Vector3 targetDirection = Vector3.zero;
        //     float moveOverride = player.inputHandler.moveAmount;

        //     targetDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
        //     targetDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

        //     targetDirection.Normalize();
        //     targetDirection.y = 0;

        //     if (targetDirection == Vector3.zero)
        //     {
        //         targetDirection = player.transform.forward;
        //     }

        //     float rs = rotationSpeed;

        //     Quaternion tr = Quaternion.LookRotation(targetDirection); // target rotation
        //     Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);

        //     player.transform.rotation = targetRotation;
        // }

        public void HandleSandGliderMovement()
        {
            if (!player.isOnSandGlider)
            {
                sandGliderRb.velocity = Vector3.zero;
                sandGliderRb.isKinematic = true;
            }
            else
            {
                sandGliderRb.isKinematic = false;
                ProcessThrust();
                ProcessRotation();
            }
        }

        public void HandleFalling()
        {
            RaycastHit hit;
            Vector3 rayCastOrigin = transform.position;
            Vector3 targetPosition;
            rayCastOrigin.y = rayCastOrigin.y + groundDetectionRayStartPoint;
            targetPosition = transform.position;

            if (!player.isGroundedSandGlider)
            {
                inAirTimer = inAirTimer + Time.deltaTime;
                sandGliderRb.AddForce(transform.forward * leapingVelocity);
                sandGliderRb.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
            }

          
            if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer))
            {
                Vector3 rayCastHitPoint = hit.point;
                targetPosition.y = rayCastHitPoint.y;
                inAirTimer = 0;
                player.isGroundedSandGlider = true;
                //player.isInteracting = false;
            }
            else
            {
                player.isGroundedSandGlider = false;
            }

            if (player.isGroundedSandGlider)
            {
                if (player.isInteracting)//|| player.inputHandler.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
        }

        void ProcessThrust()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartingThrust();
                Debug.Log("Here Should Thrust");
            }
            else if (Input.GetKey(KeyCode.S))
            {
                sandGliderRb.AddRelativeForce(-Vector3.forward * movementSpeed);
            }
            else
            {
                StopThrusting();
            }
        }

        void ProcessRotation()
        {
            if (Input.GetKey(KeyCode.A))
            {
                StartingLeftThrust();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                StartingRightThrust();
            }
            else
            {
                StopSideThrusting();
            }
        }

        void StartingThrust()
        {
            //sandGliderRb.AddRelativeForce(Vector3.forward * movementSpeed);
            sandGliderRb.AddRelativeForce(Vector3.forward * movementSpeed);
            //transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            // if (!audioSource.isPlaying)
            // {
            //     audioSource.PlayOneShot(mainEngine, 1f);
            // }
            // if (!mainThrustParticle.isPlaying)
            // {
            //     mainThrustParticle.Play();
            // }
        }

        void StopThrusting()
        {
            // audioSource.Stop();
            // mainThrustParticle.Stop();
        }

        void StartingLeftThrust()
        {
            ApplyRotation(-rotationSpeed);
            // if (!leftThrustParticle.isPlaying)
            // {
            //     leftThrustParticle.Play();
            // }
        }

        void StartingRightThrust()
        {
            ApplyRotation(rotationSpeed);
            // if (!rightThrustParticle.isPlaying)
            // {
            //     rightThrustParticle.Play();
            // }

        }

        void StopSideThrusting()
        {
            // leftThrustParticle.Stop();
            // rightThrustParticle.Stop();
        }

        void ApplyRotation(float rotationThisFrame)
        {
            //sandGliderRb.freezeRotation = true;
            transform.Rotate(Vector3.up * rotationThisFrame * Time.deltaTime);
            //sandGliderRb.freezeRotation = false;
        }
    }
}
