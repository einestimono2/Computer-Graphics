using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorManager
{
    EnemyManager enemyManager;

    void Awake(){
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    void OnAnimatorMove(){
        enemyManager.rig.drag = 0;

        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;

        Vector3 velocity = deltaPosition / Time.deltaTime;
        enemyManager.rig.velocity = velocity;
    }
}
