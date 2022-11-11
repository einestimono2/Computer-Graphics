using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    AnimatorHandler animatorHandler;

    public WeaponSlotHolder leftHand;
    public WeaponSlotHolder rightHand;
    public WeaponData unarmedWeapon;
    WeaponSlotHolder backSlot;

    DamageCollider leftHandDamage;
    DamageCollider rightHandDamage;

    RightHandIKTarget rightHandIKTarget;
    LeftHandIKTarget leftHandIKTarget;

    void Awake(){
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        animatorHandler = GetComponent<AnimatorHandler>();

        WeaponSlotHolder[] weaponHolderSlots = GetComponentsInChildren<WeaponSlotHolder>();
        foreach (WeaponSlotHolder item in weaponHolderSlots)
        {
            if(item.isLeftHand) leftHand = item;
            else if(item.isRightHand) rightHand = item;
            else if(item.isBackSlot) backSlot = item;
        }
    }

    public void LoadTwoHandIKTargets(bool isTwoHand){
        leftHandIKTarget = rightHand.weaponObject.GetComponentInChildren<LeftHandIKTarget>();
        rightHandIKTarget = rightHand.weaponObject.GetComponentInChildren<RightHandIKTarget>();

        animatorHandler.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHand);
    }

    public void LoadAllWeapons(){
        LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        LoadWeaponOnSlot(playerInventory.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponData weapon, bool isLeftHand){
        if(weapon != null){
            if(isLeftHand){
            leftHand.currentWeapon = weapon;
            leftHand.LoadWeapon(weapon);
            LoadWeaponDamage(true);

            // animatorHandler.anim.runtimeAnimatorController = weapon.weaponController;
            } 
            else{
                animatorHandler.PlayAnimation("Two Hand Empty", false, false);

                if(inputHandler.twoHandFlag){
                    // Cất/Ẩn vũ khí bên tay trái (nếu có)
                    backSlot.LoadWeapon(leftHand.currentWeapon);
                    leftHand.UnloadWeaponAndDestroy();
                    animatorHandler.PlayAnimation("TH Idle", false, false);
                }else{
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHand.currentWeapon = weapon;
                rightHand.LoadWeapon(weapon);
                LoadWeaponDamage(false);
                LoadTwoHandIKTargets(playerManager.isTwoHand);
                animatorHandler.anim.runtimeAnimatorController = weapon.weaponController;
            } 
        }else{
            weapon = unarmedWeapon;

            if(isLeftHand){
                playerInventory.leftWeapon = unarmedWeapon;
                leftHand.currentWeapon = weapon;
                leftHand.LoadWeapon(weapon);
                LoadWeaponDamage(true);
            }else{
                playerInventory.rightWeapon = unarmedWeapon;
                rightHand.currentWeapon = weapon;
                rightHand.LoadWeapon(weapon);
                LoadWeaponDamage(false);
                animatorHandler.anim.runtimeAnimatorController = weapon.weaponController;
            }
        }
    }

    void LoadWeaponDamage(bool isLeft){
        if(isLeft) leftHandDamage = leftHand.weaponObject.GetComponentInChildren<DamageCollider>();
        else rightHandDamage = rightHand.weaponObject.GetComponentInChildren<DamageCollider>();
    }

    public void EnableWeaponDamage(){
        if(playerManager.isUsingRightHand) rightHandDamage.EnableCollider();
        else leftHandDamage.EnableCollider();
    }

    public void DisableWeaponDamage(){
        if(playerManager.isUsingRightHand) rightHandDamage.DisableCollider();
        else leftHandDamage.DisableCollider();
    }

}
