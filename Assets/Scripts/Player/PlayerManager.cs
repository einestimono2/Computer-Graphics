using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    Animator anim;

    PlayerMovement playerMovement;
    AnimatorHandler animatorHandler;

    public bool isInteracting;

    [Header("Pick Up Item")]
    public GameObject pickUpUI;
    public GameObject pickUpInstruction;

    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canCombo;
    public bool isInvulnerable;
    public bool isBlocking;
    public bool isParrying;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isTwoHand;

    void Awake() {
        inputHandler = GetComponent<InputHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update(){
        isInteracting = anim.GetBool("isInteracting");
        isInvulnerable = anim.GetBool("isInvulnerable");
        isParrying = anim.GetBool("isParrying");
        isUsingRightHand = anim.GetBool("isUsingRightHand");
        isUsingLeftHand = anim.GetBool("isUsingLeftHand");
        canCombo = anim.GetBool("canCombo");

        anim.SetBool("isInAir", isInAir);
        anim.SetBool("isBlocking", isBlocking);
        anim.SetBool("isTwoHand", isTwoHand);

        inputHandler.TickInput();

        animatorHandler.canRotate = anim.GetBool("canRotate");

        #region PlayerMovement
        playerMovement.HandleRotation();
        playerMovement.HandleRolling();
        playerMovement.HandleJumping();
        #endregion

        CheckInteractableObject();
    }

    // Nên thực hiện FixedUpdate khi có AddForce or MovePosition
    void FixedUpdate(){
        #region PlayerMovement
        playerMovement.HandleMovement();
        playerMovement.HandleFalling(playerMovement.direction);
        #endregion
    }

    void LateUpdate(){
        isSprinting = inputHandler.sprint_Input;

        inputHandler.roll_Input = false;
        inputHandler.sprint_Input = false;
        inputHandler.lm_Input = false;
        inputHandler.rm_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.pickup_Input = false;
        inputHandler.inventory_Input = false;
        
        if(isInAir){
            playerMovement.inAirTimer = playerMovement.inAirTimer + Time.deltaTime;
        }

    }

    public void CheckInteractableObject(){
        RaycastHit hit;

        if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f)){
            if(hit.collider.tag == "Interactable"){
                Interactable objectItem = hit.collider.GetComponent<Interactable>();

                if(objectItem != null){
                    pickUpUI.SetActive(true);
                    pickUpInstruction.SetActive(true);

                    TextMeshProUGUI tmp = pickUpUI.GetComponentInChildren<TextMeshProUGUI>();
                    if(tmp != null) tmp.text = objectItem.interactableText;

                    Image ri = pickUpUI.GetComponentInChildren<Image>();

                    WeaponPickUp wpu = (WeaponPickUp) objectItem;
                    ri.sprite = wpu.weaponData.itemIcon;

                    if(inputHandler.pickup_Input){
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }else{
            if(pickUpUI != null) pickUpUI.SetActive(false);
            if(pickUpInstruction != null) pickUpInstruction.SetActive(false);
        }
    }
}
