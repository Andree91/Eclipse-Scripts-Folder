using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CompanionStateCombatStance : State
    {
        public CompanionStateAttackTarget attackState;
        public CompanionStatePursueTarget pursueTargetState;
        public CompanionStateIdle idleState;
        public CompanionStateFollowHost followHostState;
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

        public override State Tick(EnemyManager aiCharacter)
        {
            // If we are too far away from our companion, we want to return to them
            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion * 2f)
            {
                return followHostState;
            }

            if (aiCharacter.combatStyle == AICombatStyle.SwordAndShield)
            {
                if (aiCharacter.currentTarget != null)
                {
                    return ProcessSwordAndShieldCombatStyle(aiCharacter);
                }
                else
                {
                    ResetStateFlags();
                    return idleState;
                }
            }
            else if (aiCharacter.combatStyle == AICombatStyle.Archer)
            {
                return ProcessArcherCombatStyle(aiCharacter);
            }
            else
            {
                return this;
            }
        }

        protected State ProcessSwordAndShieldCombatStyle(EnemyManager aiCharacter)
        {
            if (aiCharacter.currentTarget != null)
            {
                if (aiCharacter.currentTarget.isDead)
                {
                    ResetStateFlags();
                    aiCharacter.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
                    aiCharacter.currentTarget = null;
                    aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                    aiCharacter.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                    return idleState;
                }
            }

            //float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            // enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            //attackState.hasPerformedAttack = false;

            //If the A.I is falling or is performing some sort of action STOP all movement
            if (!aiCharacter.isGrounded || aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                ResetStateFlags();
                aiCharacter.characterWeaponSlotManager.rightHandDamageCollider.DisableDamageCollider();
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
                DecideCirclingAction(aiCharacter.enemyAnimatorManager);
            }

            if (willPerformRiposte)
            {
                if (aiCharacter.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(aiCharacter);
                    return this;
                }
            }

            if (aiCharacter.allowAIToPerformBlock)
            {
                //Roll for block chance
                RollForBlockChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                //Roll for dodge chance
                RollForDodgeChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                //Roll for parry chance
                RollForParryChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformRiposte)
            {
                //Roll for Riposte chance
                RollForRiposteChance(aiCharacter);
            }

            if (willPerformParry && aiCharacter.currentTarget.isAttacking && !hasPerformedParry)
            {
                // PARRY INCOMING ATTACK
                ParryCurrentTarget(aiCharacter);
                return this;
            }

            if (willPerformBlock)
            {
                // BLOCK USING OFF HAND
                BlockUsingOffHand(aiCharacter);
            }

            if (willPerformDodge && aiCharacter.currentTarget.isAttacking)
            {
                // DODGE INCOMING ATTACK
                Dodge(aiCharacter);
            }

            HandleRotateTowardsTarget(aiCharacter);

            //Different idea
            // if (enemyManager.isPerformingAction)
            // {
            //     enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            // }

            if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                //randomDestinationSet = false;
                ResetStateFlags();
                Debug.Log("Going for the Attack State");
                return attackState;
            }
            else
            {
                GetNewAttack(aiCharacter);
            }

            HandleMovement(aiCharacter);

            return this;
        }

        protected State ProcessArcherCombatStyle(EnemyManager aiCharacter)
        {
            if (aiCharacter.currentTarget.isDead)
            {
                ResetStateFlags();
                aiCharacter.currentTarget = null;
                return idleState;
            }

            //float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
            // enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            // enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            //attackState.hasPerformedAttack = false;

            //If the A.I is falling or is performing some sort of action STOP all movement
            if (!aiCharacter.isGrounded || aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If the A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
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
                DecideCirclingAction(aiCharacter.enemyAnimatorManager);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                //Roll for dodge chance
                RollForDodgeChance(aiCharacter);
            }

            if (willPerformDodge && aiCharacter.currentTarget.isAttacking)
            {
                // DODGE INCOMING ATTACK
                Dodge(aiCharacter);
            }

            HandleRotateTowardsTarget(aiCharacter);

            //Different idea
            // if (enemyManager.isPerformingAction)
            // {
            //     enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            // }

            if (!hasAmmoLoaded)
            {
                //Draw an Arrow
                DrawArrow(aiCharacter);

                //Aim at target before firing
                AimAtTargetBeforeFiring(aiCharacter);
            }

            if (aiCharacter.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                //randomDestinationSet = false;
                ResetStateFlags();
                Debug.Log("Going for the Attack State");
                return attackState;
            }

            if (aiCharacter.isStationaryArcher)
            {
                //Don't MOVE
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);

            }
            else
            {
                HandleMovement(aiCharacter);
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager aiCharacter)
        {
            //Rotate manually
            if (aiCharacter.isPerformingAction)
            {
                Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position; //Maybe delete enemymanager.?
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aiCharacter.rotationSpeed * Time.deltaTime); //Should be * not / ?
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
            Debug.Log($"Value of Vertical Movement = {verticalMovementValue}"); //Enemy will only walk facing Player (not negative values) if negative then enemy can walk backwards
            if (verticalMovementValue <= 1 && verticalMovementValue >= 0)
            {
                verticalMovementValue = 0.5f;
            }
            else if (verticalMovementValue >= -1 && verticalMovementValue < 0)
            {
                verticalMovementValue = 0.5f;
            }

            horizontalMovementValue = Random.Range(-1, 1);
            Debug.Log($"Value of Horizontal Movement = {horizontalMovementValue}");
            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager aiCharacter)
        {
            //Vector3 targetDirection = enemy.currentTarget.transform.position - transform.position;
            //float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            //float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];
                Debug.Log(enemyAttackAction.name);

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

        protected void RollForBlockChance(EnemyManager aiCharacter)
        {
            int blockChance = Random.Range(0, 101);

            if (blockChance <= aiCharacter.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        protected void RollForDodgeChance(EnemyManager aiCharacter)
        {
            int dodgeChance = Random.Range(0, 101);

            if (dodgeChance <= aiCharacter.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        protected void RollForParryChance(EnemyManager aiCharacter)
        {
            int parryChance = Random.Range(0, 101);

            if (parryChance <= aiCharacter.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        protected void RollForRiposteChance(EnemyManager aiCharacter)
        {
            int riposteChance = Random.Range(0, 101);

            if (riposteChance <= aiCharacter.riposteLikelyHood)
            {
                willPerformRiposte = true;
            }
            else
            {
                willPerformRiposte = false;
            }
        }

        //AI ACTIONS
        protected void BlockUsingOffHand(EnemyManager aiCharacter)
        {
            if (aiCharacter.isBlocking == false)
            {
                if (aiCharacter.allowAIToPerformBlock)
                {
                    aiCharacter.isBlocking = true;
                    aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.leftWeapon;
                    aiCharacter.isUsingLeftHand = true;
                    aiCharacter.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapons();
                }
            }
        }

        protected void Dodge(EnemyManager aiCharacter)
        {
            if (!hasPerformedDodge)
            {
                if (!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(aiCharacter.transform.eulerAngles.x, randomDodgeDirection, aiCharacter.transform.eulerAngles.z);
                }

                if (aiCharacter.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetDodgeDirection, 1f);
                    aiCharacter.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = aiCharacter.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if (rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        aiCharacter.transform.rotation = targetDodgeDirection;
                        aiCharacter.enemyAnimatorManager.PlayTargetAnimation("Rolling", true);
                    }
                }
            }
        }

        protected void ParryCurrentTarget(EnemyManager aiCharacter)
        {
            if (aiCharacter.currentTarget.canBeParried)
            {
                if (aiCharacter.distanceFromTarget <= 2)
                {
                    willPerformParry = false;
                    hasPerformedParry = true;
                    aiCharacter.isParrying = true;
                    aiCharacter.enemyAnimatorManager.PlayTargetAnimation("Parry_01", true);
                    Debug.Log($"Enemy is parrying while inside parry function is {aiCharacter.isParrying}");
                }
            }
        }

        protected void CheckForRiposte(EnemyManager aiCharacter)
        {
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }

            if (aiCharacter.distanceFromTarget >= 1.0f)
            {
                HandleRotateTowardsTarget(aiCharacter);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            }
            else
            {
                willPerformRiposte = false;
                aiCharacter.isBlocking = false;

                if (!aiCharacter.isInteracting && !aiCharacter.currentTarget.isBeingBackStabbed && !aiCharacter.currentTarget.isBeingRiposted)
                {
                    aiCharacter.enemyRigidbody.velocity = Vector3.zero;
                    aiCharacter.animator.SetFloat("Vertical", 0);
                    aiCharacter.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        protected void DrawArrow(EnemyManager aiCharacter)
        {
            //We must two hand the bow to fire and load it
            if (!aiCharacter.isTwoHanding)
            {
                aiCharacter.isTwoHanding = true;
                //enemy.characterWeaponSlotManager.LoadBothWeaponsOnSlot();
            }
            else
            {
                hasAmmoLoaded = true;
                aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
                aiCharacter.characterInventoryManager.rightWeapon.th_hold_RB_action.PerformAction(aiCharacter);
            }

            // hasAmmoLoaded = true;
            // enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
            // enemy.characterInventoryManager.rightWeapon.th_hold_RB_action.PerformAction(enemy);
        }

        protected void AimAtTargetBeforeFiring(EnemyManager aiCharacter)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range(aiCharacter.minimunTimeToAimAtTarget, aiCharacter.maximunTimeToAimAtTarget);
            aiCharacter.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        protected void HandleMovement(EnemyManager aiCharacter)
        {
            if (aiCharacter.distanceFromTarget <= aiCharacter.stoppingDistance)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
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
