using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator){
        // Tìm kiếm mục tiêu xung quanh
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.radius, detectionLayer);

        for(int i = 0; i < colliders.Length; i++){
            CharacterStats character = colliders[i].transform.GetComponent<CharacterStats>();

            if(character != null){
                Vector3 direction = character.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(direction, transform.forward);

                if(viewableAngle > enemyManager.minAngle && viewableAngle < enemyManager.maxAngle){
                    enemyManager.target = character;
                }
            }
        }

        // Chuyển sang trạng thái "Pursue Target" nếu tìm thấy
        if(enemyManager.target != null) return pursueTargetState;
        else return this;
    }
}
