using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;
        public WeaponHolderSlot shieldBackSlot;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Hand IK Targets")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            LoadWeaponHolderSlots();
        }

        protected virtual void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
                else if (weaponSlot.isShieldBackSlot)
                {
                    shieldBackSlot = weaponSlot;
                }
            }
        }

        public virtual void LoadBothWeaponsOnSlot()
        {
            LoadWeaponOnSlot(character.characterInventoryManager.rightWeapon, false);
            if (!character.isTwoHanding)
            {
                LoadWeaponOnSlot(character.characterInventoryManager.leftWeapon, true);
                // if (character.characterInventoryManager.leftWeapon.weaponType == WeaponType.Shield)
                // {
                //     character.leftHandIsShield = true;
                // }
                // else
                // {
                //     character.leftHandIsShield = false;
                // }
            }
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftHandWeaponDamageCollider();
                    //character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (character.isTwoHanding)
                    {
                        if (leftHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                        {
                            shieldBackSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            character.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                            //animatorManager.animator.CrossFade("TH_Idle_01", 0.2f);
                        }
                        else
                        {
                            backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                            leftHandSlot.UnloadWeaponAndDestroy();
                            character.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                            //animatorManager.animator.CrossFade("TH_Idle_01", 0.2f);
                        }
                    }
                    else
                    {
                        //animatorManager.animator.CrossFade("Both Arms Empty", 0.2f);
                        backSlot.UnloadWeaponAndDestroy();
                        shieldBackSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightHandWeaponDamageCollider();
                    LoadTwoHandIKTargets(character.isTwoHanding);
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    character.characterInventoryManager.leftWeapon = weaponItem;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftHandWeaponDamageCollider();
                    //character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    character.characterInventoryManager.rightWeapon = weaponItem;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightHandWeaponDamageCollider();
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }

        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
            character.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        #region Handle Weapon's Damage Collider

        public virtual void LoadLeftHandWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            if (leftHandDamageCollider == null) { return; }

            leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
            leftHandDamageCollider.lightningDamage = character.characterInventoryManager.leftWeapon.lightingDamage;

            leftHandDamageCollider.character = character;
            leftHandDamageCollider.teamIDNumeber = character.characterStatsManager.teamIDNumeber;

            leftHandDamageCollider.poiseBreak = character.characterInventoryManager.leftWeapon.poiseBreak;
            character.characterEffectsManager.leftWeaponManager = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
            //leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        }

        public virtual void LoadRightHandWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            if (rightHandDamageCollider == null) { return; }

            rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
            rightHandDamageCollider.lightningDamage = character.characterInventoryManager.rightWeapon.lightingDamage;

            rightHandDamageCollider.character = character;
            rightHandDamageCollider.teamIDNumeber = character.characterStatsManager.teamIDNumeber;

            rightHandDamageCollider.poiseBreak = character.characterInventoryManager.rightWeapon.poiseBreak;
            character.characterEffectsManager.rightWeaponManager = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
            //rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        }

        public virtual void OpenDamageCollider()
        {
            character.characterSoundFXManager.PlayRandomWeaponWhoosh();

            if (character.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (character.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (character.isUsingRightHand && rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
            else if (character.isUsingLeftHand && leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }

        #endregion

        #region  Handle Weapon's Poise Bonus

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.armorPoiseBonus;
        }

        #endregion
    }
}
