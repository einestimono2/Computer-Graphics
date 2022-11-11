using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingCollider : MonoBehaviour
{
    public BoxCollider blockingCollider;

    public float blockingDamageAbsorption;

    private void Awake() {
        blockingCollider = GetComponent<BoxCollider>();
    }

    public void SetDamageAbsorption(WeaponData weapon){
        if(weapon != null){
            blockingDamageAbsorption = weapon.damageAbsorption;
        } 
    }

    public void EnableCollider(){
        blockingCollider.enabled = true;
    }

    public void DisableCollider(){
        blockingCollider.enabled = false;
    }
}
