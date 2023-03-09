using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class Elevator : MonoBehaviour
    {
        public BoxCollider trigger;
        public LayerMask activationLayers;
        public Vector3 moveTo;
        public LeverInteractable leverInteractableBottom;
        public LeverInteractable leverInteractableTop;
        public Animator triggerPlatformAnimator;
        public float moveSpeed = 0.5f;
        public float waitTime = 1;

        Rigidbody rbElevator;
        List<GameObject> attached = new List<GameObject>();

        Vector3 originalPosition;
        Vector3 translationPosition;
        bool isMoving = false;
        float elapsedWaitTime = 0;

        ParticleSystem elevatorPlatformParticle;

        void OnEnable()
        {
            rbElevator = GetComponent<Rigidbody>();
            originalPosition = transform.position;
            // if (trigger == null) 
            // {
            //     SetupTrigger();
            // }

            elevatorPlatformParticle = GetComponentInChildren<ParticleSystem>();
        }

        void FixedUpdate()
        {
            rbElevator.MovePosition(rbElevator.position + translationPosition * Time.fixedDeltaTime);
        }

        // void Update() 
        // {
        //     foreach (var child in attached) 
        //     {
        //         child.transform.position += rbElevator.velocity * Time.fixedDeltaTime;
        //     }
        // }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                attached.Add(other.gameObject);

                if (!isMoving)
                {
                    Debug.Log("Trigger Enter");
                    StartCoroutine(MoveToCR());
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            //if (other.gameObject.tag == "Character")
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //StartCoroutine(MoveTriggerPlatform());
                attached.Remove(other.gameObject);
            }
        }

        private void SetupTrigger()
        {
            trigger = gameObject.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.center = new Vector3(0, 1, 0);
            var size = trigger.size;
            size.y = 4;
            trigger.size = size;
        }

        public void MoveElevator()
        {
            StartCoroutine(MoveToCR());
        }

        // IEnumerator MoveTriggerPlatform()
        // {
        //     // if (isTriggerEnter)
        //     // {
        //     //     for (float timer = 0.05f; timer < 0.7f; timer += 0.05f)
        //     //     {
        //     //         transform.position = Vector3.Lerp(transform.position, triggerTranslationPosition, 1);
        //     //         yield return new WaitForSeconds(0.05f);
        //     //     }
        //     // }
        //     // else if (!isTriggerEnter)
        //     // {
        //     //     for (float timer = 0.05f; timer < 0.7f; timer += 0.05f)
        //     //     {
        //     //         transform.position = Vector3.Lerp(transform.position, originalPosition, 1);
        //     //         yield return new WaitForSeconds(0.05f);
        //     //     }
        //     // }

        //     isTriggerMoving = true;
        //     var dir = (triggerMoveTo - triggerOriginalPosition).normalized;
        //     while (Vector3.Distance(rbTrigger.position, triggerMoveTo) > 0.1f && Vector3.Dot(dir, triggerMoveTo - rbTrigger.position) > 0)
        //     {
        //         triggerTranslationPosition = dir * moveSpeed / 2;
        //         yield return null;
        //     }

        //     Debug.Log("out of while loop trigger");
        //     Vector3 swap = triggerMoveTo;
        //     moveTo = triggerOriginalPosition;
        //     triggerOriginalPosition = swap;
        //     triggerTranslationPosition = Vector3.zero;
        // }

        IEnumerator MoveToCR()
        {
            triggerPlatformAnimator.Play("Elevator Trigger Down");
            //triggerPlatformAnimator.GetComponentInChildren<ParticleSystem>().Play();
            if (elevatorPlatformParticle != null)
            {
                elevatorPlatformParticle.Play();
            }
            leverInteractableBottom.isAtRightPosition = !leverInteractableBottom.isAtRightPosition;
            leverInteractableTop.isAtRightPosition = !leverInteractableTop.isAtRightPosition;

            leverInteractableBottom.isDisabled = true;
            leverInteractableTop.isDisabled = true;

            while (elapsedWaitTime < waitTime)
            {
                elapsedWaitTime += Time.deltaTime;
                yield return null;
            }

            isMoving = true;
            var dir = (moveTo - originalPosition).normalized;

            while (Vector3.Distance(rbElevator.position, moveTo) > 0.1f && Vector3.Dot(dir, moveTo - rbElevator.position) > 0)
            {
                translationPosition = dir * moveSpeed;
                yield return null;
            }

            Debug.Log("out of while loop elevator");
            triggerPlatformAnimator.Play("Elevator Trigger Up");
            //triggerPlatformAnimator.GetComponentInChildren<ParticleSystem>().Play();
            if (elevatorPlatformParticle != null)
            {
                elevatorPlatformParticle.Play();
            }
            Vector3 swap = moveTo;
            moveTo = originalPosition;
            originalPosition = swap;
            isMoving = false;
            elapsedWaitTime = 0;
            translationPosition = Vector3.zero;
            leverInteractableBottom.isDisabled = false;
            leverInteractableTop.isDisabled = false;
            Debug.Log(moveTo);
        }
    }
}
