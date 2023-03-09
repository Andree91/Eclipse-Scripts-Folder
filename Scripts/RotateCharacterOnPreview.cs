using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class RotateCharacterOnPreview : MonoBehaviour
    {
        PlayerControls playerControls;

        public float rotationAmount = 1f;
        public float rotationSpeed = 5f;

        Vector2 cameraInput;

        Vector3 currentRotation;
        Vector3 targetRotation;

        void OnEnable() 
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        void Start() 
        {
            currentRotation = transform.eulerAngles;
            targetRotation = transform.eulerAngles;
        }

        void Update()
        {
            if (cameraInput.x > 0)
            {
                targetRotation.y = targetRotation.y + rotationAmount;
            }
            else if (cameraInput.x < 0)
            {
                targetRotation.y = targetRotation.y - rotationAmount;
            }
            // else if (cameraInput.y > 0)
            // {
            //     targetRotation.x = Mathf.Clamp((targetRotation.x + rotationAmount), 0, 5f);
            // }
            // else if (cameraInput.y < 0)
            // {
            //     targetRotation.x = Mathf.Clamp((targetRotation.x - rotationAmount), -5f, 0);
            // }

            currentRotation = Vector3.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = currentRotation;
        }
    }
}