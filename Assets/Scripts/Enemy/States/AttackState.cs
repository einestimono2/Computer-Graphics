using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;
    
    [Header("Attack")]
    public EnemyAttackData[] enemyAttacks;
    public EnemyAttackData currentAttack;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator){
        // Đang thực hiện hành động => return
        if(enemyManager.isPerformingAction) return combatStanceState; 

        // Cập nhật góc và khoảng cách tới mục tiêu
        Vector3 direction = enemyManager.target.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.target.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(direction, transform.forward);

        // Nhìn vào mục tiêu
        HandleRotateTowardsTarget(enemyManager);

        // Lựa chọn loại tấn công (nếu có nhiều loại)
        // Nếu loại tấn công hiện tại không thực hiện (do góc, khoảng cách) => Chọn loại mới
        if(currentAttack != null){
            
            // Trường hợp quá gần mục tiêu => return
            if(distanceFromTarget < currentAttack.minDistanceToAttack) return this;
            else if(distanceFromTarget < currentAttack.maxDistanceToAttack){
                // Góc thỏa mãn
                if(viewableAngle >= currentAttack.minAttackAngle && viewableAngle <= currentAttack.maxAttackAngle){
                    // Không trong thời gian cooldown tấn công
                    //  - Đứng lại và thực hiện tấn công
                    //  - Cập nhật cooldown tấn công
                    //  - Chuyển sang trạng thái "Combat Stance"
                    if(enemyManager.currentCooldownTime <= 0 && !enemyManager.isPerformingAction){
                        enemyManager.isPerformingAction = true;
                        enemyAnimator.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimator.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimator.PlayAnimation(currentAttack.actionAnimation, true);
                        enemyManager.currentCooldownTime = currentAttack.cooldown;
                        currentAttack = null;

                        return combatStanceState;
                    }
                }
            }
        }else{
            GetAttack(enemyManager);
        }

        return combatStanceState;
    }

    void GetAttack(EnemyManager enemyManager){
        Vector3 target = enemyManager.target.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(target, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.target.transform.position, transform.position);
        
        // Random loại tấn công nếu có nhiều hơn 2 loại
        int maxScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++){
            EnemyAttackData data = enemyAttacks[i];

            if(distanceFromTarget <= data.maxDistanceToAttack && distanceFromTarget >= data.minDistanceToAttack){
                if(viewableAngle <= data.maxAttackAngle && viewableAngle >= data.minAttackAngle){
                    maxScore += data.attackScore;
                }
            }
        }

        int rand = Random.Range(0, maxScore);
        int temporaryScore = 0;
        for (int i = 0; i < enemyAttacks.Length; i++){
            EnemyAttackData data = enemyAttacks[i];

            if(distanceFromTarget <= data.maxDistanceToAttack && distanceFromTarget >= data.minDistanceToAttack){
                if(viewableAngle <= data.maxAttackAngle && viewableAngle >= data.minAttackAngle){
                    if(currentAttack != null) return;

                    temporaryScore += data.attackScore;
                    if(temporaryScore > rand){
                        currentAttack = data;
                    }
                }
            }
        }
    }

    void HandleRotateTowardsTarget(EnemyManager enemyManager){
        // Trường hợp bình thường
        if(enemyManager.isPerformingAction){
            Vector3 direction = enemyManager.target.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero){
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        // Trường hợp tìm đường di chuyển tới mục tiêu với Nav Mesh Agent
        else{
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.nav.desiredVelocity);
            Vector3 targetVelocity = enemyManager.rig.velocity;

            enemyManager.nav.enabled = true;
            enemyManager.nav.SetDestination(enemyManager.target.transform.position);
            enemyManager.rig.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.nav.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);

            // Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.nav.desiredVelocity);
            
            // Vector3 direction = enemyManager.target.transform.position - transform.position;

            // direction.Normalize();
            // direction.y = 0;

            // float speed = 2;
            // direction *= speed;

            // Vector3 projectedVelocity = Vector3.ProjectOnPlane(direction, Vector3.up);
            // Vector3 targetVelocity = projectedVelocity; // Everything in the IF statement from this line and above this line is new

            // enemyManager.nav.enabled = true;
            // enemyManager.nav.SetDestination(enemyManager.target.transform.position);
            // enemyManager.rig.velocity = targetVelocity;
            // enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.nav.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            
        }

        // Cập nhật trasform so với parent
        enemyManager.nav.transform.localPosition = Vector3.zero;
        enemyManager.nav.transform.localRotation = Quaternion.identity;
    }
}
