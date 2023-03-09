using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AG
{
    public class OpenDoor : Interactable
    {
        [Header("Door information")]
        [SerializeField] int doorWorldID; // This is unique  ID for this door in the game world, each door you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenOpened;

        Animator[] animators;
        OpenDoor openDoor;
        NavMeshObstacle navMeshObstacle;
        [SerializeField] Transform leftDoor;
        [SerializeField] Transform rightDoor;
        [SerializeField] Transform singleDoor;
        [SerializeField] Vector3 endRotationLeft = new Vector3(0, 90, 0);
        [SerializeField] Vector3 endRotationRight = new Vector3(0, 270, 0);

        public bool isBig;
        bool hasNavMeshObstacle;
        public Transform playerStandingPosition_01;
        public Transform playerStandingPosition_02;
        bool isOpening;
        bool regularAnim;
        SphereCollider interactionCollider;

        protected override void Awake()
        {
            base.Awake();
            animators = GetComponentsInChildren<Animator>();
            openDoor = GetComponent<OpenDoor>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            interactionCollider = GetComponent<SphereCollider>();
        }

        protected override void Start()
        {
            base.Start();

            if (navMeshObstacle != null)
            {
                hasNavMeshObstacle = true;
            }

            // if (isLootItem)
            // {
            //     itemPickUpID = WorldSaveGameManager.instance.currentCharacterSaveData.lastInstantiateLootItemID + 1;
            //     WorldSaveGameManager.instance.currentCharacterSaveData.lastInstantiateLootItemID = itemPickUpID;
            // }

            // If the saves data doesn't contais this door, we haven't opened it yet, so we add it to the list it as NOT OPENED
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.doorsOpened.ContainsKey(doorWorldID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.doorsOpened.Add(doorWorldID, false);
            }

            hasBeenOpened = WorldSaveGameManager.instance.currentCharacterSaveData.doorsOpened[doorWorldID];

            if (hasBeenOpened)
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].enabled = false;
                }

                if (isBig)
                {
                    leftDoor.localRotation = Quaternion.Euler(endRotationLeft.x, endRotationLeft.y, endRotationLeft.z);
                    rightDoor.localRotation = Quaternion.Euler(endRotationRight.x, endRotationRight.y, endRotationRight.z);
                }
                else
                {
                    singleDoor.localRotation = Quaternion.Euler(endRotationLeft.x, endRotationLeft.y, endRotationLeft.z);
                }

                if (hasNavMeshObstacle)
                {
                    navMeshObstacle.enabled = false;
                }
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (playerManager.isInteracting || isOpening == true) { return; }

            isOpening = true;

            base.Interact(playerManager);

            // Notify the character data that this door has been opened from the world, so it doesn't close again.
            if (WorldSaveGameManager.instance.currentCharacterSaveData.doorsOpened.ContainsKey(doorWorldID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.doorsOpened.Remove(doorWorldID);
            }

            // Saves the door state to our save data so it doesn't close again when we re-load the area
            WorldSaveGameManager.instance.currentCharacterSaveData.doorsOpened.Add(doorWorldID, true);

            hasBeenOpened = true;

            //Rotate player towards door handle
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            //Lock his transform in front of chest
            float tempDistance_01 = Mathf.Abs(Vector3.Distance(playerManager.transform.position, playerStandingPosition_01.position));
            float tempDistance_02 = Mathf.Abs(Vector3.Distance(playerManager.transform.position, playerStandingPosition_02.position));

            // Animate player and moves they at right position
            if (tempDistance_01 < tempDistance_02)
            {
                playerManager.OpenDoorInteraction(playerStandingPosition_01, isBig);
            }
            else
            {
                playerManager.OpenDoorInteraction(playerStandingPosition_02, isBig);
                regularAnim = true;
            }

            // Open the door/doors
            for (int i = 0; i < animators.Length; i++)
            {
                if (isBig && regularAnim)
                {
                    animators[i].Play("Door Open Big");
                }
                if (isBig && !regularAnim)
                {
                    animators[i].Play("Door Open Big Mirrored");
                }
                else
                {
                    animators[i].Play("Door Open");
                }
            }

            if (hasNavMeshObstacle)
            {
                navMeshObstacle.enabled = false;
            }

            //Destroy(openDoor, 2f);
            // StartCoroutine(SetEndRotation());
            interactionCollider.enabled = false;
            this.enabled = false;
            //gameObject.SetLayer(0);
        }

        // IEnumerator SetEndRotation()
        // {
        //     yield return new WaitForSeconds(4f);
        //     endRotationLeft = leftDoor.rotation.eulerAngles;
        //     endRotationRight = rightDoor.rotation.eulerAngles;
        // }
    }
}
