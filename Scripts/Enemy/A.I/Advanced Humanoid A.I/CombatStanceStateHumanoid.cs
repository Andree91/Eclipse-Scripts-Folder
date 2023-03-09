using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CombatStanceStateHumanoid : State
    {
        public AttackStateHumanoid attackState;
        public PursueTargetStateHumanoid pursueTargetState;
        public IdleStateHumanoid idleState;
        public ItemBasedAttackAction[] enemyAttacks;

        public bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        [Header("State Flags")]
        bool willPerformBlock = false;
        bool willPerformDodge = false;
        bool willPerformParry = false;
        bool willPerformRiposte = false;

        bool hasPerformedDodge = false;
        bool hasRandomDodgeDirection = false;

        bool hasPerformedParry = false;

        bool hasAmmoLoaded = false;

        Quaternion targetDodgeDirection;

        public override State Tick(EnemyManager enemy)
        {
            if (enemy.combatStyle == AICombatStyle.SwordAndShield)
            {
                if (enemy.currentTarget != null)
                {
                    return ProcessSwordAndShieldCombatStyle(enemy);
                }
                else
                {
                    ResetStateFlags();
                    return idleState;
                }
            }
            else if (enemy.combatStyle == AICombatStyle.Archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }

        protected State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {
            if (enemy.currentTarget != null)
            {
                if (enemy.currentTarget.isDead)
                {
                    ResetStateFlags();
                    enemy.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                    enemy.currentTarget = null;
                    enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    enemy.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    return idleState;
                }
            }

            //float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            // enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            //attackState.hasPerformedAttack = false;

            //If the A.I is falling or is performing some sort of action STOP all movement
            if (!enemy.isGrounded || enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (enemy.isBlocking)
            {
                Debug.Log("Inside is blocking");
                enemy.animator.runtimeAnimatorController = enemy.characterInventoryManager.leftWeapon.weaponController;
            }
            // else if (!enemy.isBlocking)
            // {
            //     enemy.leftHandIsShield = false;
            //     enemy.animator.runtimeAnimatorController = enemy.characterInventoryManager.rightWeapon.weaponController;
            // }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                enemy.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                return pursueTargetState;
            }

            // if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget > enemyManager.maximumAggroRadius)

            // {

            //     return pursueTargetState;

            // }

            //Randomizes the walking pattern of our A.I so they circle the player
            //Circle player or walk around
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            if (willPerformRiposte)
            {
                if (enemy.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(enemy);
                    return this;
                }
            }

            if (enemy.allowAIToPerformBlock)
            {
                //Roll for block chance
                RollForBlockChance(enemy);
            }

            if (enemy.allowAIToPerformDodge)
            {
                //Roll for dodge chance
                RollForDodgeChance(enemy);
            }

            if (enemy.allowAIToPerformParry)
            {
                //Roll for parry chance
                RollForParryChance(enemy);
            }

            if (enemy.allowAIToPerformRiposte)
            {
                //Roll for Riposte chance
                RollForRiposteChance(enemy);
            }

            if (willPerformParry && enemy.currentTarget.isAttacking && !hasPerformedParry)
            {
                // PARRY INCOMING ATTACK
                ParryCurrentTarget(enemy);
                return this;
            }

            if (willPerformBlock)
            {
                // BLOCK USING OFF HAND
                BlockUsingOffHand(enemy);
            }
            else if (!willPerformBlock)
            {
                enemy.isBlocking = false;
                enemy.leftHandIsShield = false;
            }

            if (willPerformDodge && enemy.currentTarget.isAttacking)
            {
                // DODGE INCOMING ATTACK
                Dodge(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            //Different idea
            // if (enemyManager.isPerformingAction)
            // {
            //     enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            // }

            if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null && enemy.distanceFromTarget <= enemy.maximunAttackRange)
            {
                //randomDestinationSet = false;
                ResetStateFlags();
                //enemy.animator.runtimeAnimatorController = enemy.characterInventoryManager.rightWeapon.weaponController;
                //Debug.Log("Going for the Attack State");
                return attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }

            HandleMovement(enemy);

            return this;
        }

        protected State ProcessArcherCombatStyle(EnemyManager enemy)
        {
            if (enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return idleState;
            }

            //float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            // enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            //attackState.hasPerformedAttack = false;

            //If the A.I is falling or is performing some sort of action STOP all movement
            if (!enemy.isGrounded || enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            // if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget > enemyManager.maximumAggroRadius)

            // {

            //     return pursueTargetState;

            // }

            //Randomizes the walking pattern of our A.I so they circle the player
            //Circle player or walk around
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            if (enemy.allowAIToPerformDodge)
            {
                //Roll for dodge chance
                RollForDodgeChance(enemy);
            }

            if (willPerformDodge && enemy.currentTarget.isAttacking)
            {
                // DODGE INCOMING ATTACK
                Dodge(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            //Different idea
            // if (enemyManager.isPerformingAction)
            // {
            //     enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            // }

            RotateWhileAiming(enemy);

            if (!hasAmmoLoaded)
            {
                //Draw an Arrow
                DrawArrow(enemy);

                //Aim at target before firing
                AimAtTargetBeforeFiring(enemy);
            }

            if (enemy.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                //randomDestinationSet = false;
                ResetStateFlags();
                Debug.Log("Going for the Attack State");
                return attackState;
            }

            if (enemy.isStationaryArcher)
            {
                //Don't MOVE
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);

            }
            else
            {
                HandleMovement(enemy);
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemy)
        {
            //Rotate manually
            if (enemy.isPerformingAction)
            {
                Vector3 direction = enemy.currentTarget.transform.position - transform.position; //Maybe delete enemymanager.?
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed * Time.deltaTime); //Should be * not / ?
            }
            //Rotate with pathfinding (navmesh)
            else
            {
                // enemyManager.navMeshAgent.enabled = true;

                // enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);



                // float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
                // float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                // if (distanceFromTarget > 5) enemyManager.navMeshAgent.angularSpeed = 500f;

                // else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30) enemyManager.navMeshAgent.angularSpeed = 50f;

                // else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30) enemyManager.navMeshAgent.angularSpeed = 500f;



                // Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;

                // Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);





                // if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)

                // {

                //     enemyManager.navMeshAgent.updateRotation = false;

                //     enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,

                //     Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);

                // }

                // else

                // {

                //     enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);

                // }



                // Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                // Vector3 targerVelocity = enemyManager.enemyRigidbody.velocity;

                // enemyManager.navMeshAgent.enabled = true;
                // enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                // enemyManager.enemyRigidbody.velocity = targerVelocity;
                // enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed * Time.deltaTime);
            }
        }

        protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorHandler)
        {
            //Cirle with only forward vertical movement
            //Cirle with running

            //Cirle with walking only
            WalkAroundTarget(enemyAnimatorHandler);
        }

        protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorHandler)
        {
            //verticalMovementValue could just be 0.5f, but I wanted to make more modular version of this script
            //verticalMovementValue = 0.5f;
            verticalMovementValue = Random.Range(0, 2);
            // Debug for vertical movement
            //Debug.Log($"Value of Vertical Movement = {verticalMovementValue}"); //Enemy will only walk facing Player (not negative values) if negative then enemy can walk backwards
            if (verticalMovementValue <= 1 && verticalMovementValue >= 0)
            {
                verticalMovementValue = 0.5f;
            }
            else if (verticalMovementValue >= -1 && verticalMovementValue < 0)
            {
                verticalMovementValue = 0.5f;
            }

            horizontalMovementValue = Random.Range(-1, 1);
            // Debug for horizontal movement
            //Debug.Log($"Value of Horizontal Movement = {horizontalMovementValue}");
            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemy)
        {
            //Vector3 targetDirection = enemy.currentTarget.transform.position - transform.position;
            //float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            //float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];
                //Debug.Log(enemyAttackAction.name);

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }

            }

            int randomValue = Random.Range(0, maxScore + 1);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null) { return; }

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

        //AI ROLLS

        protected void RollForBlockChance(EnemyManager enemy)
        {
            int blockChance = Random.Range(0, 101);

            if (blockChance <= enemy.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        protected void RollForDodgeChance(EnemyManager enemy)
        {
            int dodgeChance = Random.Range(0, 101);

            if (dodgeChance <= enemy.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        protected void RollForParryChance(EnemyManager enemy)
        {
            int parryChance = Random.Range(0, 101);

            if (parryChance <= enemy.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        protected void RollForRiposteChance(EnemyManager enemy)
        {
            int riposteChance = Random.Range(0, 101);

            if (riposteChance <= enemy.riposteLikelyHood)
            {
                willPerformRiposte = true;
            }
            else
            {
                willPerformRiposte = false;
            }
        }

        //AI ACTIONS
        protected void BlockUsingOffHand(EnemyManager enemy)
        {
            if (enemy.isBlocking == false)
            {
                if (enemy.allowAIToPerformBlock)
                {
                    enemy.isBlocking = true;
                    //enemy.leftHandIsShield = true;
                    enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                    enemy.isUsingLeftHand = true;
                    //enemy.animator.runtimeAnimatorController = enemy.characterInventoryManager.leftWeapon.weaponController;
                    enemy.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapons();
                }
            }
        }

        protected void Dodge(EnemyManager enemy)
        {
            if (!hasPerformedDodge)
            {
                if (!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
                }

                if (enemy.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, targetDodgeDirection, 1f);
                    enemy.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = enemy.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if (rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        enemy.transform.rotation = targetDodgeDirection;
                        enemy.enemyAnimatorManager.PlayTargetAnimation("Rolling", true);
                    }
                }
            }
        }

        protected void ParryCurrentTarget(EnemyManager enemy)
        {
            if (enemy.currentTarget.canBeParried)
            {
                if (enemy.distanceFromTarget <= 2)
                {
                    willPerformParry = false;
                    hasPerformedParry = true;
                    enemy.isParrying = true;
                    enemy.enemyAnimatorManager.PlayTargetAnimation("Parry_01", true);
                    //Debug.Log($"Enemy is parrying while inside parry function is {enemy.isParrying}");
                }
            }
        }

        protected void CheckForRiposte(EnemyManager enemy)
        {
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }

            if (enemy.distanceFromTarget >= 1.0f)
            {
                HandleRotateTowardsTarget(enemy);
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            }
            else
            {
                willPerformRiposte = false;
                enemy.isBlocking = false;

                if (!enemy.isInteracting && !enemy.currentTarget.isBeingBackStabbed && !enemy.currentTarget.isBeingRiposted)
                {
                    enemy.enemyRigidbody.velocity = Vector3.zero;
                    enemy.animator.SetFloat("Vertical", 0);
                    enemy.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        protected void DrawArrow(EnemyManager enemy)
        {
            //We must two hand the bow to fire and load it
            if (!enemy.isTwoHanding)
            {
                enemy.isTwoHanding = true;
                //enemy.characterWeaponSlotManager.LoadBothWeaponsOnSlot();
            }
            else
            {
                hasAmmoLoaded = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_hold_RB_action.PerformAction(enemy);
            }

            // hasAmmoLoaded = true;
            // enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
            // enemy.characterInventoryManager.rightWeapon.th_hold_RB_action.PerformAction(enemy);
        }

        protected void AimAtTargetBeforeFiring(EnemyManager enemy)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range(enemy.minimunTimeToAimAtTarget, enemy.maximunTimeToAimAtTarget);
            enemy.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        protected void RotateWhileAiming(EnemyManager enemy)
        {
            if (enemy.currentRecoveryTime > 0 && hasAmmoLoaded)
            {
                Vector3 rotationDirection = enemy.currentTarget.transform.position - enemy.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, tr, Time.deltaTime * 2f);
                //transform.rotation = targetRotation;
                //Debug.Log("Before Courotine");
                StartCoroutine(RotateAIWhileAiming(targetRotation, enemy));
            }
        }

        IEnumerator RotateAIWhileAiming(Quaternion targetRotation, EnemyManager enemy)
        {
            float tempTime = Time.deltaTime;
            float timer = 0;

            while (timer <= tempTime + 2f)
            {
                timer += Time.deltaTime;
                enemy.transform.rotation = targetRotation;
                yield return new WaitForEndOfFrame();
            }
        }

        protected void HandleMovement(EnemyManager enemy)
        {
            if (enemy.distanceFromTarget <= enemy.stoppingDistance)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }

        //CALLED WHEN EVER WE EXIT THIS STATE, SO WHEN WE RETURN ALL FLAGS ARE RESET AND CAN BE RE-ROLLED
        protected void ResetStateFlags()
        {
            hasRandomDodgeDirection = false;
            hasPerformedDodge = false;
            hasPerformedParry = false;

            hasAmmoLoaded = false;

            randomDestinationSet = false;

            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
            willPerformRiposte = false;
        }
    }
}
