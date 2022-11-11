using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public override void Interact(PlayerManager playerManager){
        // Nhìn vào rương
        Vector3 rotation = transform.position - playerManager.transform.position;
        rotation.y = 0;
        rotation.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotation);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);

        playerManager.transform.rotation = targetRotation;
        // Aniamtion
    }
}
