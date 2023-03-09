using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace AG
{
    public class AnimatorManager : MonoBehaviour
    {
        protected CharacterManager character;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstrait;
        public TwoBoneIKConstraint rightHandConstrait;

        [Header("DAMAGE ANIMATIONS")]
        [HideInInspector] public string OH_Damage_Forward_Medium_01 = "OH_Damage_Forward_Medium_01";
        [HideInInspector] public string OH_Damage_Forward_Medium_02 = "OH_Damage_Forward_Medium_02";

        [HideInInspector] public string OH_Damage_Back_Medium_01 = "OH_Damage_Back_Medium_01";
        [HideInInspector] public string OH_Damage_Back_Medium_02 = "OH_Damage_Back_Medium_02";

        [HideInInspector] public string OH_Damage_Left_Medium_01 = "OH_Damage_Left_Medium_01";
        [HideInInspector] public string OH_Damage_Left_Medium_02 = "OH_Damage_Left_Medium_02";

        [HideInInspector] public string OH_Damage_Right_Medium_01 = "OH_Damage_Right_Medium_01";
        [HideInInspector] public string OH_Damage_Right_Medium_02 = "OH_Damage_Right_Medium_02";

        [HideInInspector] public string TH_Damage_Forward_Medium_01 = "TH_Damage_Forward_Medium_01";
        [HideInInspector] public string TH_Damage_Forward_Medium_02 = "TH_Damage_Forward_Medium_02";

        [HideInInspector] public string TH_Damage_Back_Medium_01 = "TH_Damage_Back_Medium_01";
        [HideInInspector] public string TH_Damage_Back_Medium_02 = "TH_Damage_Back_Medium_02";

        [HideInInspector] public string TH_Damage_Left_Medium_01 = "TH_Damage_Left_Medium_01";
        [HideInInspector] public string TH_Damage_Left_Medium_02 = "TH_Damage_Left_Medium_02";

        [HideInInspector] public string TH_Damage_Right_Medium_01 = "TH_Damage_Right_Medium_01";
        [HideInInspector] public string TH_Damage_Right_Medium_02 = "TH_Damage_Right_Medium_02";

        [HideInInspector] public string OH_Damage_Forward_Large_01 = "OH_Damage_Forward_Large_01";
        [HideInInspector] public string OH_Damage_Forward_Large_02 = "OH_Damage_Forward_Large_02";

        [HideInInspector] public string OH_Damage_Back_Large_01 = "OH_Damage_Back_Large_01";
        [HideInInspector] public string OH_Damage_Back_Large_02 = "OH_Damage_Back_Large_02";

        [HideInInspector] public string OH_Damage_Left_Large_01 = "OH_Damage_Left_Large_01";
        [HideInInspector] public string OH_Damage_Left_Large_02 = "OH_Damage_Left_Large_02";

        [HideInInspector] public string OH_Damage_Right_Large_01 = "OH_Damage_Right_Large_01";
        [HideInInspector] public string OH_Damage_Right_Large_02 = "OH_Damage_Right_Large_02";

        [HideInInspector] public string TH_Damage_Forward_Large_01 = "TH_Damage_Forward_Large_01";
        [HideInInspector] public string TH_Damage_Forward_Large_02 = "TH_Damage_Forward_Large_02";

        [HideInInspector] public string TH_Damage_Back_Large_01 = "TH_Damage_Back_Large_01";
        [HideInInspector] public string TH_Damage_Back_Large_02 = "TH_Damage_Back_Large_02";

        [HideInInspector] public string TH_Damage_Left_Large_01 = "TH_Damage_Left_Large_01";
        [HideInInspector] public string TH_Damage_Left_Large_02 = "TH_Damage_Left_Large_02";

        [HideInInspector] public string TH_Damage_Right_Large_01 = "TH_Damage_Right_Large_01";
        [HideInInspector] public string TH_Damage_Right_Large_02 = "TH_Damage_Right_Large_02";

        [HideInInspector] public string OH_Damage_Forward_Colossal_01 = "OH_Damage_Forward_Colossal_01";
        [HideInInspector] public string OH_Damage_Forward_Colossal_02 = "OH_Damage_Forward_Colossal_02";

        [HideInInspector] public string OH_Damage_Back_Colossal_01 = "OH_Damage_Back_Colossal_01";
        [HideInInspector] public string OH_Damage_Back_Colossal_02 = "OH_Damage_Back_Colossal_02";

        [HideInInspector] public string OH_Damage_Left_Colossal_01 = "OH_Damage_Left_Colossal_01";
        [HideInInspector] public string OH_Damage_Left_Colossal_02 = "OH_Damage_Left_Colossal_02";

        [HideInInspector] public string OH_Damage_Right_Colossal_01 = "OH_Damage_Right_Colossal_01";
        [HideInInspector] public string OH_Damage_Right_Colossal_02 = "OH_Damage_Right_Colossal_02";

        [HideInInspector] public string TH_Damage_Forward_Colossal_01 = "TH_Damage_Forward_Colossal_01";
        [HideInInspector] public string TH_Damage_Forward_Colossal_02 = "TH_Damage_Forward_Colossal_02";

        [HideInInspector] public string TH_Damage_Back_Colossal_01 = "TH_Damage_Back_Colossal_01";
        [HideInInspector] public string TH_Damage_Back_Colossal_02 = "TH_Damage_Back_Colossal_02";

        [HideInInspector] public string TH_Damage_Left_Colossal_01 = "TH_Damage_Left_Colossal_01";
        [HideInInspector] public string TH_Damage_Left_Colossal_02 = "TH_Damage_Left_Colossal_02";

        [HideInInspector] public string TH_Damage_Right_Colossal_01 = "TH_Damage_Right_Colossal_01";
        [HideInInspector] public string TH_Damage_Right_Colossal_02 = "TH_Damage_Right_Colossal_02";

        [HideInInspector] public string Last_Damage_Animation_Played = "";

        [HideInInspector] public List<string> OH_Damage_Animations_Forward_Medium = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Back_Medium = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Left_Medium = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Right_Medium = new List<string>();

        [HideInInspector] public List<string> TH_Damage_Animations_Forward_Medium = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Back_Medium = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Left_Medium = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Right_Medium = new List<string>();

        [HideInInspector] public List<string> OH_Damage_Animations_Forward_Large = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Back_Large = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Left_Large = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Right_Large = new List<string>();

        [HideInInspector] public List<string> TH_Damage_Animations_Forward_Large = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Back_Large = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Left_Large = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Right_Large = new List<string>();

        [HideInInspector] public List<string> OH_Damage_Animations_Forward_Colossal = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Back_Colossal = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Left_Colossal = new List<string>();
        [HideInInspector] public List<string> OH_Damage_Animations_Right_Colossal = new List<string>();

        [HideInInspector] public List<string> TH_Damage_Animations_Forward_Colossal = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Back_Colossal = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Left_Colossal = new List<string>();
        [HideInInspector] public List<string> TH_Damage_Animations_Right_Colossal = new List<string>();

        public bool canUseConsumeItem = true;

        bool handIKWeightReset = false;
        
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        protected virtual void Start()
        {
            OH_Damage_Animations_Forward_Medium.Add(OH_Damage_Forward_Medium_01);
            OH_Damage_Animations_Forward_Medium.Add(OH_Damage_Forward_Medium_02);

            OH_Damage_Animations_Back_Medium.Add(OH_Damage_Back_Medium_01);
            OH_Damage_Animations_Back_Medium.Add(OH_Damage_Back_Medium_02);

            OH_Damage_Animations_Left_Medium.Add(OH_Damage_Left_Medium_01);
            OH_Damage_Animations_Left_Medium.Add(OH_Damage_Left_Medium_02);

            OH_Damage_Animations_Right_Medium.Add(OH_Damage_Right_Medium_01);
            OH_Damage_Animations_Right_Medium.Add(OH_Damage_Right_Medium_02);

            TH_Damage_Animations_Forward_Medium.Add(TH_Damage_Forward_Medium_01);
            TH_Damage_Animations_Forward_Medium.Add(TH_Damage_Forward_Medium_02);

            TH_Damage_Animations_Back_Medium.Add(TH_Damage_Back_Medium_01);
            TH_Damage_Animations_Back_Medium.Add(TH_Damage_Back_Medium_02);

            TH_Damage_Animations_Left_Medium.Add(TH_Damage_Left_Medium_01);
            TH_Damage_Animations_Left_Medium.Add(TH_Damage_Left_Medium_02);

            TH_Damage_Animations_Right_Medium.Add(TH_Damage_Right_Medium_01);
            TH_Damage_Animations_Right_Medium.Add(TH_Damage_Right_Medium_02);

            OH_Damage_Animations_Forward_Large.Add(OH_Damage_Forward_Large_01);
            OH_Damage_Animations_Forward_Large.Add(OH_Damage_Forward_Large_02);

            OH_Damage_Animations_Back_Large.Add(OH_Damage_Back_Large_01);
            OH_Damage_Animations_Back_Large.Add(OH_Damage_Back_Large_02);

            OH_Damage_Animations_Left_Large.Add(OH_Damage_Left_Large_01);
            OH_Damage_Animations_Left_Large.Add(OH_Damage_Left_Large_02);

            OH_Damage_Animations_Right_Large.Add(OH_Damage_Right_Large_01);
            OH_Damage_Animations_Right_Large.Add(OH_Damage_Right_Large_02);

            TH_Damage_Animations_Forward_Large.Add(TH_Damage_Forward_Large_01);
            TH_Damage_Animations_Forward_Large.Add(TH_Damage_Forward_Large_02);

            TH_Damage_Animations_Back_Large.Add(TH_Damage_Back_Large_01);
            TH_Damage_Animations_Back_Large.Add(TH_Damage_Back_Large_02);

            TH_Damage_Animations_Left_Large.Add(TH_Damage_Left_Large_01);
            TH_Damage_Animations_Left_Large.Add(TH_Damage_Left_Large_02);

            TH_Damage_Animations_Right_Large.Add(TH_Damage_Right_Large_01);
            TH_Damage_Animations_Right_Large.Add(TH_Damage_Right_Large_02);

            OH_Damage_Animations_Forward_Colossal.Add(OH_Damage_Forward_Colossal_01);
            OH_Damage_Animations_Forward_Colossal.Add(OH_Damage_Forward_Colossal_02);

            OH_Damage_Animations_Back_Colossal.Add(OH_Damage_Back_Colossal_01);
            OH_Damage_Animations_Back_Colossal.Add(OH_Damage_Back_Colossal_02);

            OH_Damage_Animations_Left_Colossal.Add(OH_Damage_Left_Colossal_01);
            OH_Damage_Animations_Left_Colossal.Add(OH_Damage_Left_Colossal_02);

            OH_Damage_Animations_Right_Colossal.Add(OH_Damage_Right_Colossal_01);
            OH_Damage_Animations_Right_Colossal.Add(OH_Damage_Right_Colossal_02);

            TH_Damage_Animations_Forward_Colossal.Add(TH_Damage_Forward_Colossal_01);
            TH_Damage_Animations_Forward_Colossal.Add(TH_Damage_Forward_Colossal_02);

            TH_Damage_Animations_Back_Colossal.Add(TH_Damage_Back_Colossal_01);
            TH_Damage_Animations_Back_Colossal.Add(TH_Damage_Back_Colossal_02);

            TH_Damage_Animations_Left_Colossal.Add(TH_Damage_Left_Colossal_01);
            TH_Damage_Animations_Left_Colossal.Add(TH_Damage_Left_Colossal_02);

            TH_Damage_Animations_Right_Colossal.Add(TH_Damage_Right_Colossal_01);
            TH_Damage_Animations_Right_Colossal.Add(TH_Damage_Right_Colossal_02);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false, bool mirrorAnimation = false)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("canRotate", canRotate);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.SetBool("isMirrored", mirrorAnimation);
            character.animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("isRotatingWithRootMotion", true);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.CrossFade(targetAnim, 0.2f);
        }

        public string GetRandomDamageAnimationFromList(List<string> animationList)
        {
            int randomValue = Random.Range(0, animationList.Count);

            if (animationList[randomValue] == Last_Damage_Animation_Played)
            {
                randomValue++;
                if (randomValue > animationList.Count)
                {
                    randomValue = 0;
                }
            }
            return animationList[randomValue];
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            character.isBeingBackStabbed = true;
            character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage, 0, 0, character);
            character.pendingCriticalDamage = 0;
        }

        public virtual void CanRotate()
        {
            character.animator.SetBool("canRotate", true);
        }

        public virtual void StopRotation()
        {
            character.animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            character.animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            character.animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInVulnerable()
        {
            character.animator.SetBool("isInVulnerable", true);
        }

        public virtual void DisableIsInVulnerable()
        {
            character.animator.SetBool("isInVulnerable", false);
        }

        public virtual void EnableIsDodging()
        {
            character.animator.SetBool("isDodging", true);
        }

        public virtual void DisableIsDodging()
        {
            character.animator.SetBool("isDodging", false);
        }

        public virtual void EnableRollingCollider()
        {
            //character.GetComponentInChildren<SphereCollider>().enabled = true;
        }

        public virtual void DisableRollingCollider()
        {
            //character.GetComponentInChildren<SphereCollider>().enabled = false;
        }

        public virtual void EnableIsParrying()
        {
            character.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            character.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            if (character.isUsingRightHand)
            {
                character.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
            }
            character.canBeRiposted = true;
        }

        public virtual void DisableCanBeRiposted()
        {
            character.canBeRiposted = false;
        }

        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHandingWeapon)
        {
            if (isTwoHandingWeapon)
            {
                if (rightHandIKTarget != null)
                {
                    rightHandConstrait.data.target = rightHandIKTarget.transform;
                    rightHandConstrait.data.targetPositionWeight = 1; //Assingn this from a wepon variable if you'd like
                    rightHandConstrait.data.targetRotationWeight = 1;
                }
                if (leftHandIKTarget != null)
                {
                    leftHandConstrait.data.target = leftHandIKTarget.transform;
                    leftHandConstrait.data.targetPositionWeight = 1;
                    leftHandConstrait.data.targetRotationWeight = 1;
                }
            }
            else
            {
                rightHandConstrait.data.target = null;
                leftHandConstrait.data.target = null;
            }

            rigBuilder.Build();
        }

        public  virtual void CheckHandIKWeight(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHanding)
        {
            if (character.isInteracting) { return; }

            if (handIKWeightReset)
            {
                handIKWeightReset = false;

                if (rightHandConstrait.data.target != null)
                {
                    rightHandConstrait.data.target = rightHandIKTarget.transform;
                    rightHandConstrait.data.targetPositionWeight = 1;
                    rightHandConstrait.data.targetRotationWeight = 1;
                }

                if (leftHandConstrait.data.target != null)
                {
                    leftHandConstrait.data.target = leftHandIKTarget.transform;
                    leftHandConstrait.data.targetPositionWeight = 1;
                    leftHandConstrait.data.targetRotationWeight = 1;
                }
            }
        }

        public virtual void EraseHandIKForWeapon()
        {
            handIKWeightReset = true;

            if (rightHandConstrait.data.target != null)
            {
                rightHandConstrait.data.targetPositionWeight = 0;
                rightHandConstrait.data.targetRotationWeight = 0;
            }
            
            if (leftHandConstrait.data.target != null)
            {
                leftHandConstrait.data.targetPositionWeight = 0;
                leftHandConstrait.data.targetRotationWeight = 0;
            }
        }

        public virtual void EraseHalfOfHandIKForWeapon()
        {
            handIKWeightReset = true;

            if (rightHandConstrait.data.target != null)
            {
                rightHandConstrait.data.targetPositionWeight = 0.3f;
                rightHandConstrait.data.targetRotationWeight = 0.3f;
            }

            if (leftHandConstrait.data.target != null)
            {
                leftHandConstrait.data.targetPositionWeight = 0.3f;
                leftHandConstrait.data.targetRotationWeight = 0.3f;
            }
        }

    }
}
