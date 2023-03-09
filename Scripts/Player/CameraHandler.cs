using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CameraHandler : MonoBehaviour
    {
        PlayerManager player;

        public Transform targetTransfrom; //Transform the camera follows (The Player)
        public Transform targetTransformWhileAiming; //Transform the camera follows while aiming on bows etc
        public Transform targetTransformWhileOnSandGlider;
        public Transform targetTransformWhileOnShip;
        public Transform targetTransformWhilePlayingChess;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        public Camera cameraObject;

        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask enviromentLayer;
        public LayerMask interactableLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float leftAndRightLookSpeed = 250f;
        public float leftAndRightAimingLookSpeed = 25f;
        public float followSpeed = 50f;
        public float upAndDownLookSpeed = 250f;
        public float upAndDownAimingLookSpeed = 25f;

        private float targetPosition;
        private float defaultPosition;

        private float leftAndRightAngle;
        private float upAndDownAngle;

        public float minimunLookDownAngle = -35;
        public float maximunLookUpAngle = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffSet = 0.2f;
        public float minimunCollisionOffSet = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximunLockOnDistance = 30;

        void Awake()
        {
            singleton = this;
            defaultPosition = cameraTransform.localPosition.z;
            //ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12);
            targetTransfrom = FindObjectOfType<PlayerManager>().transform;
            player = FindObjectOfType<PlayerManager>();
            cameraObject = GetComponentInChildren<Camera>();
        }

        void Start()
        {
            enviromentLayer = LayerMask.NameToLayer("Environment");
        }

        public void FollowTarget()
        {
            if (player.isAiming)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }
            else if (player.isOnSandGlider)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileOnSandGlider.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }
            else if (player.isOnShip)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileOnShip.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }
            else if (player.isPlayingChess)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhilePlayingChess.position, ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }
            else
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransfrom.position,
                                                        ref cameraFollowVelocity, Time.deltaTime * followSpeed);
                transform.position = targetPosition;
            }

            HandleCameraCollision();

        }

        public void HandleCameraRotation()
        {
            if (player.isPlayingChess) 
            {
                cameraPivotTransform.localRotation = Quaternion.Euler(61, 0, 0);
                return; 
            }
            if (player.inputHandler.lockOnFlag && currentLockOnTarget != null)
            {
                if (currentLockOnTarget.isDead)
                {
                    ResetLockedCamera();
                }
                else
                {
                    HandleLockedCameraRotation();
                }
            }
            else if (player.isAiming)
            {
                HandleAimedCameraRotation();
            }
            else
            {
                HandleStandardCameraRotation();
            }
        }

        public void HandleStandardCameraRotation()
        {
            leftAndRightAngle += (player.inputHandler.mouseX * leftAndRightLookSpeed) * Time.deltaTime;
            upAndDownAngle -= (player.inputHandler.mouseY * upAndDownLookSpeed) * Time.deltaTime;
            upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimunLookDownAngle, maximunLookUpAngle);

            Vector3 rotation = Vector3.zero;
            rotation.y = leftAndRightAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = upAndDownAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        void HandleLockedCameraRotation()
        {
            Vector3 direction = currentLockOnTarget.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            direction = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }

        void ResetLockedCamera()
        {
            player.inputHandler.lockOn_Input = true;
            player.inputHandler.lockOn_Input = false;
            player.inputHandler.lockOnFlag = false;
            player.cameraHandler.ClearLockOnTargets();
            player.cameraHandler.SetCameraHeight();
        }

        void HandleAimedCameraRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);

            Quaternion targetRotationX;
            Quaternion targetRotationY;

            Vector3 cameraRotationX = Vector3.zero;
            Vector3 cameraRotationY = Vector3.zero;

            leftAndRightAngle += (player.inputHandler.mouseX * leftAndRightAimingLookSpeed) * Time.deltaTime;
            upAndDownAngle -= (player.inputHandler.mouseY * upAndDownAimingLookSpeed) * Time.deltaTime;

            cameraRotationY.y = leftAndRightAngle;
            targetRotationY = Quaternion.Euler(cameraRotationY);
            targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
            transform.localRotation = targetRotationY;

            cameraRotationX.x = upAndDownAngle;
            targetRotationX = Quaternion.Euler(cameraRotationX);
            targetRotationX = Quaternion.Slerp(cameraTransform.localRotation, targetRotationX, 1);
            cameraTransform.localRotation = targetRotationX;
        }

        public void ResetAimCameraRotations()
        {
            cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        void HandleCameraCollision()
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast
                                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit,
                                Mathf.Abs(targetPosition), ignoreLayers))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(distance - cameraCollisionOffSet);
            }

            if (Mathf.Abs(targetPosition) < minimunCollisionOffSet)
            {
                targetPosition = -minimunCollisionOffSet;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, Time.deltaTime / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            availableTargets.Clear();

            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransfrom.position, 26);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransfrom.position;
                    float distanceFromTarget = Vector3.Distance(targetTransfrom.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (character.transform.root != targetTransfrom.transform.root
                        && viewableAngle > -50 && viewableAngle < 50
                        && distanceFromTarget <= maximunLockOnDistance)
                    {
                        if (Physics.Linecast(player.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(player.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer == enviromentLayer)
                            {
                                //cannot lock on
                            }
                            else
                            {
                                if (character.characterStatsManager.teamIDNumeber == 1)
                                {
                                    availableTargets.Add(character);
                                }
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransfrom.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (player.inputHandler.lockOnFlag)
                {
                    // Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    // var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    // var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;
                    Vector3 relativeEnemyPosition = player.inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k];
                        //leftLockTarget.isLockedOn = true;
                    }

                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k];
                        //rightLockTarget.isLockedOn = true;
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnLockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                                                                newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition,
                                                                newUnLockedPosition, ref velocity, Time.deltaTime);
            }
        }

    }
}
