using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator){
        // Nếu đang hành động => return
        if(enemyManager.isPerformingAction){
            enemyAnimator.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        // Đuổi theo mục tiêu
        Vector3 direction = enemyManager.target.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.target.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(direction, enemyManager.transform.forward);

        if(distanceFromTarget > enemyManager.maxAttackRange){
            enemyAnimator.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

            // Thực hiện di chuyển tới mục tiêu
            direction.Normalize();
            direction.y = 0;

            float speed = 2;
            direction *= speed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(direction, Vector3.up);
            enemyManager.rig.velocity = projectedVelocity;
        } 

        // Hướng tới mục tiêu (Nhìn vào mục tiêu)
        HandleRotateTowardsTarget(enemyManager);

        // Nếu nằm đủ tầm đánh => Chuyển sang trạng thái "Combat Stance"
        // Nếu ngoài tầm đánh, return và đuổi theo mục tiêu
        if(distanceFromTarget <= enemyManager.maxAttackRange) return combatStanceState;
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
