using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public string lastAttack;

    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerEquipment playerEquipment;
    AnimatorHandler animatorHandler;
    InputHandler inputHandler;

    [Header("Animation One Hand")]
    string oh_heavyAttack = "1H Attack 0";
    string oh_lightAttack1 = "1H Attack 1";
    string oh_lightAttack2 = "1H Attack 2";

    string oh_shieldAttack = "1H Shield Attack";

    [Header("Animation Two Hand")]
    string th_heavyAttack = "2H Attack 0";
    string th_lightAttack1 = "2H Attack 1";
    string th_lightAttack2 = "2H Attack 2";

    void Awake(){
        playerEquipment = GetComponent<PlayerEquipment>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
    }

    void HandleLightAttackCombo(WeaponData weapon){
        if(!inputHandler.comboFlag || oh_lightAttack2 == "") return;

        animatorHandler.anim.SetBool("canCombo", false);

        if(lastAttack == oh_lightAttack1){
            animatorHandler.PlayAnimation(oh_lightAttack2, true);
            lastAttack = oh_lightAttack2;
        }else if(lastAttack == th_lightAttack1){
            animatorHandler.PlayAnimation(th_lightAttack2, true);
            lastAttack = th_lightAttack2;
        }
    }

    void HandleLightAttack(WeaponData weapon){
        if(inputHandler.twoHandFlag){
            animatorHandler.PlayAnimation(th_lightAttack1, true);
            lastAttack = th_lightAttack1;
        }else{
            animatorHandler.PlayAnimation(oh_lightAttack1, true);
            lastAttack = oh_lightAttack1;
        }
    }

    public void HandleHeavyAttack(WeaponData weapon){
        if(inputHandler.twoHandFlag){
            animatorHandler.PlayAnimation(th_heavyAttack, true);
        }else if(playerInventory.leftWeapon.weaponType == WeaponType.Unarmed){
            animatorHandler.anim.SetBool("isUsingRightHand", true);
            animatorHandler.PlayAnimation(oh_heavyAttack, true);
        }else if(playerInventory.leftWeapon.weaponType == WeaponType.Shield){
            animatorHandler.anim.SetBool("isUsingLeftHand", true);
            animatorHandler.PlayAnimation(oh_shieldAttack, true);
        }else if(playerInventory.leftWeapon.weaponType == WeaponType.Sword){
            animatorHandler.anim.SetBool("isUsingRightHand", true);
            animatorHandler.PlayAnimation(oh_heavyAttack, true);
        }
    }

    #region Input
    public void HandleLMAction(){
        if(playerInventory.rightWeapon.weaponType == WeaponType.Sword || playerInventory.rightWeapon.weaponType == WeaponType.Unarmed){
            PerformMeleeAction();
        }
    }

    public void HandleBlockAction(){
        PerformBlockAction();
    }
    #endregion

    #region Attack
    void PerformMeleeAction(){
        if(playerManager.canCombo){
            inputHandler.comboFlag = true;
            HandleLightAttackCombo(playerInventory.rightWeapon);
            inputHandler.comboFlag = false;
        }else{
            if(playerManager.isInteracting || playerManager.canCombo) return;

            animatorHandler.anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }
    #endregion

    #region Defense
    void PerformBlockAction(){
        if(playerManager.isInteracting) return;

        if(playerManager.isBlocking) return;

        animatorHandler.PlayAnimation("Block Start", false, true);
        playerEquipment.EnableBlocking();
        playerManager.isBlocking = true;
    }
    #endregion

    
}
