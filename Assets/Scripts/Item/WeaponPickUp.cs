using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    public WeaponData weaponData;

    public override void Interact(PlayerManager playerManager){
        base.Interact(playerManager);

        PickUp(playerManager);

        
    }

    void PickUp(PlayerManager playerManager){
        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
        PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
        AnimatorHandler animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

        playerMovement.rig.velocity = Vector3.zero; // Dừng di chuyển khi nhặt item

        // Animation
        animatorHandler.PlayAnimation("PickUp", true);

        // Add to inventory
        playerInventory.weaponsInventory.Add(weaponData);

        // Destroy
        Destroy(gameObject);
    }
}
