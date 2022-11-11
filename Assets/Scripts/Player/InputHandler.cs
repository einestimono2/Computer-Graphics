using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool roll_Input; // Ctrl
    public bool sprint_Input; // Left Shift
    public bool jump_Input; // Space
    public bool lm_Input; // Left Mouse
    public bool rm_Input; // Right Mouse
    public bool pickup_Input; // F
    public bool inventory_Input; // E
    public bool block_Input; // Q
    public bool mm_Input; // Middle Mouse - Two Hand
    public bool drink_Input; // X

    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public bool twoHandFlag;

    PlayerController playerController;
    PlayerCombat playerCombat;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    PlayerEffects playerEffects;
    AnimatorHandler animatorHandler;
    WeaponSlotManager weaponSlotManager;
    UIManager uiManager;

    BlockingCollider blockingCollider;

    Vector2 movement;
    Vector2 cameraInput;

    void Awake(){
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerEffects = GetComponentInChildren<PlayerEffects>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void OnEnable(){
        if(playerController == null){
            playerController = new PlayerController();
            playerController.PlayerMovement.Movement.performed += playerController => movement = playerController.ReadValue<Vector2>();
            playerController.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerController.PlayerActions.LM.performed += i => lm_Input = true;
            playerController.PlayerActions.RM.performed += i => rm_Input = true;

            playerController.PlayerActions.Jump.performed += i => jump_Input = true;

            playerController.PlayerActions.Block.performed += i => block_Input = true;
            playerController.PlayerActions.Block.canceled += i => block_Input = false;

            playerController.PlayerActions.PickUp.performed += i => pickup_Input = true;

            playerController.PlayerActions.Inventory.performed += i => inventory_Input = true;

            playerController.PlayerActions.TwoHand.performed += i => mm_Input = true;

            playerController.PlayerActions.Drink.performed += i => drink_Input = true;
        } 

        playerController.Enable();
    }

    public void OnDisable(){
        playerController.Disable();
    }

    public void TickInput(){
        MoveInput();
        RollInput();
        SprintInput();
        CombatInput();
        InventoryInput();
        TwoHandInput();
        UseConsumableInput();
    }

    private void MoveInput(){
        horizontal = movement.x;
        vertical = movement.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void RollInput(){
        roll_Input = playerController.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        if(roll_Input){
            rollFlag = true;
        }else{
            rollFlag = false;
        }
    }

    private void SprintInput(){
        sprint_Input = playerController.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        if(sprint_Input && !rollFlag){
            sprintFlag = true;
        }else{
            sprintFlag = false;
        }
    }

    private void CombatInput(){
        if(lm_Input){
            playerCombat.HandleLMAction();
        }

        if(rm_Input){
            if(playerManager.isInteracting) return;
            
            playerCombat.HandleHeavyAttack(playerInventory.rightWeapon);
        }

        if(block_Input){
            playerCombat.HandleBlockAction();
        }else{
            playerManager.isBlocking = false;
            if(blockingCollider.blockingCollider.enabled) blockingCollider.DisableCollider();
        }
    }

    private void InventoryInput(){
        if(inventory_Input){
            inventoryFlag = !inventoryFlag;

            if(inventoryFlag){
                uiManager.OpenInventory();
                uiManager.LoadSlots();
            }else{
                uiManager.CloseInventory();
            }
        } 
    }

    private void TwoHandInput(){
        if(mm_Input){
            mm_Input = false;
            twoHandFlag = !twoHandFlag;

            if(twoHandFlag){
                playerManager.isTwoHand = true;
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadTwoHandIKTargets(true);
            }else{
                playerManager.isTwoHand = false;
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                weaponSlotManager.LoadTwoHandIKTargets(false);
            }
        }
    }

    private void UseConsumableInput(){
        if(drink_Input){
            drink_Input = false;

            playerInventory.currentConsumable.AttemptToConsumableItem(animatorHandler, weaponSlotManager, playerEffects);
        }
    }
}
