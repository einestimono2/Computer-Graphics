using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    public GameObject leftWeapon;
    public GameObject rightWeapon;

    DamageCollider leftHandDamage;
    DamageCollider rightHandDamage;

    void Awake(){
        leftHandDamage = leftWeapon.GetComponentInChildren<DamageCollider>();
        rightHandDamage = rightWeapon.GetComponentInChildren<DamageCollider>();
    }

    public void EnableLeftWeaponDamage(){
        leftHandDamage.EnableCollider();
    }

    public void EnableRightWeaponDamage(){
        rightHandDamage.EnableCollider();
    }

    public void DisableWeaponDamage(){
        rightHandDamage.DisableCollider();
        leftHandDamage.DisableCollider();
    }

}
