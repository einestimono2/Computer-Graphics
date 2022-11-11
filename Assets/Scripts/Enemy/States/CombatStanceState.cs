using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator){
        // Kiểm tra tầm đánh
        float distanceFromTarget = Vector3.Distance(enemyManager.target.transform.position, enemyManager.transform.position);

        // Nhìn vào mục tiêu
        HandleRotateTowardsTarget(enemyManager);

        // Nếu đang hành động => return
        if(enemyManager.isPerformingAction){
            enemyAnimator.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        }
        
        // Đi xung quanh mục tiêu
        
        // Nếu trong tầm đánh => Chuyển sang trạng thái "Attack"
        // Nếu mục tiêu ngoài tầm => Chuyển sang trạng thái "Purse Target"
        // Nếu trong cooldown time => return và đi xung quanh mục tiêu
        if(enemyManager.currentCooldownTime <= 0 && distanceFromTarget <= enemyManager.maxAttackRange) return attackState;
        else if(distanceFromTarget > enemyManager.maxAttackRange) return pursueTargetState;
        else return this;
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
