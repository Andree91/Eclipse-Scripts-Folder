using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class ShipMovement : MonoBehaviour
    {
        PlayerManager player;
        public Vector3 moveDirection;

        public Rigidbody shipRb;
        public GameObject shipCamera;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 10.0f;
        [SerializeField] float rotationSpeed = 10.0f;
        //[SerializeField] float fallingSpeed = 45f;
        //[SerializeField] float leapingVelocity = 5f;

        [Header("Ground & Air Detection Stats")]
        // [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        // [SerializeField] float minimumDistanceNeededToBeginFall = 1.0f;
        // [SerializeField] float groundDirectionRayDistance = 0.2f;
        // [SerializeField] float maxDistance = 1f;
        // [SerializeField] LayerMask groundLayer;
        // public float inAirTimer;

        Vector3 normalVector;
        Vector3 targetPosition;

        public MeshCollider meshCollider;

        void Awake()
        {
            meshCollider = GetComponent<MeshCollider>();
            shipRb = GetComponent<Rigidbody>();
            player = FindObjectOfType<PlayerManager>();
        }

        public void HandleShipMovement()
        {
            if (!player.isOnShip)
            {
                shipRb.velocity = Vector3.zero;
                shipRb.isKinematic = true;
            }
            else
            {
                shipRb.isKinematic = false;
                ProcessThrust();
                ProcessRotation();
            }
        }

        // public void HandleFalling()
        // {
        //     RaycastHit hit;
        //     Vector3 rayCastOrigin = transform.position;
        //     Vector3 targetPosition;
        //     rayCastOrigin.y = rayCastOrigin.y + groundDetectionRayStartPoint;
        //     targetPosition = transform.position;

        //     //if (!player.isGroundedSandGlider)
        //     {
        //         inAirTimer = inAirTimer + Time.deltaTime;
        //         shipRb.AddForce(transform.forward * leapingVelocity);
        //         shipRb.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
        //     }


        //     if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance, groundLayer))
        //     {
        //         Vector3 rayCastHitPoint = hit.point;
        //         targetPosition.y = rayCastHitPoint.y;
        //         inAirTimer = 0;
        //         player.isGroundedSandGlider = true;
        //         //player.isInteracting = false;
        //     }
        //     else
        //     {
        //         player.isGroundedSandGlider = false;
        //     }

        //     if (player.isGroundedSandGlider)
        //     {
        //         if (player.isInteracting)//|| player.inputHandler.moveAmount > 0)
        //         {
        //             transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
        //         }
        //         else
        //         {
        //             transform.position = targetPosition;
        //         }
        //     }
        // }

        void ProcessThrust()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartingThrust();
                Debug.Log("Here Should Thrust");
            }
            else if (Input.GetKey(KeyCode.S))
            {
                shipRb.AddRelativeForce(-Vector3.forward * movementSpeed);
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
            shipRb.AddRelativeForce(Vector3.forward * movementSpeed);
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
