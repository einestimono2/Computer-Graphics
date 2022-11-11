using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public BlockingCollider blockingCollider;
    PlayerInventory playerInventory;
    
    private void Awake() {
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    public void EnableBlocking(){
        blockingCollider.SetDamageAbsorption(playerInventory.leftWeapon);
        blockingCollider.EnableCollider();
    }

    public void DisableBlocking(){
        blockingCollider.DisableCollider();
    }
}
