using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorHandler : AnimatorManager
{
    InputHandler inputHandler;
    PlayerMovement playerMovement;
    PlayerManager playerManager;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    int vertical;
    int horizontal;

    public void Initialize(){
        anim = GetComponent<Animator>();

        inputHandler = GetComponentInParent<InputHandler>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    private void Awake() {
        rigBuilder = GetComponent<RigBuilder>();
    }

    public void UpdateAnimator(float verticalMovement, float horizontalMovement, bool isSprinting){
        #region Vertical
        float v = 0;

        if(verticalMovement > 0 && verticalMovement < 0.55f) v = 0.5f;
        else if(verticalMovement > 0.55f) v = 1;
        else if(verticalMovement < 0 && verticalMovement > -0.55f) v = -0.5f;
        else if(verticalMovement < -0.55f) v = -1;
        else v = 0;
        #endregion

        #region Horizontal
        float h = 0;

        if(horizontalMovement > 0 && horizontalMovement < 0.55f) h = 0.5f;
        else if(horizontalMovement > 0.55f) h = 1;
        else if(horizontalMovement < 0 && horizontalMovement > -0.55f) h = -0.5f;
        else if(horizontalMovement < -0.55f) h = -1;
        else h = 0;
        #endregion

        if(isSprinting){
            v = 2;
            h = horizontalMovement;
        }

        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate(){
        anim.SetBool("canRotate", true);
    }

    public void StopRotate(){
        anim.SetBool("canRotate", false);
    }

    public void EnableInvulnerable(){
        anim.SetBool("isInvulnerable", true);
    }

    public void DisableInvulnerable(){
        anim.SetBool("isInvulnerable", false);
    }

    public void EnableParry(){
        anim.SetBool("isParrying", true);
    }

    public void DisableParry(){
        anim.SetBool("isParrying", false);
    }

    public void CanCombo(){
        anim.SetBool("canCombo", true);
    }

    public void StopCombo(){
        anim.SetBool("canCombo", false);
    }

    private void OnAnimatorMove(){
        if(playerManager.isInteracting == false) return;

        playerMovement.rig.drag = 0;
        
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;

        Vector3 velocity = deltaPosition / Time.deltaTime;
        playerMovement.rig.velocity = velocity; 
    }

    public void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHand){
        if(isTwoHand){
            rightHandConstraint.data.target = rightHandIKTarget.transform;
            rightHandConstraint.data.targetPositionWeight = 1;
            rightHandConstraint.data.targetRotationWeight = 1;

            leftHandConstraint.data.target = leftHandIKTarget.transform;
            leftHandConstraint.data.targetPositionWeight = 1;
            leftHandConstraint.data.targetRotationWeight = 1;
        }else{
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }

        rigBuilder.Build();
    }

    // public void EraseHandIKWeapon(){
    //     rightHandConstraint.data.targetPositionWeight = 0;
    //     rightHandConstraint.data.targetRotationWeight = 0;

    //     leftHandConstraint.data.targetPositionWeight = 0;
    //     leftHandConstraint.data.targetRotationWeight = 0;
    // }
}
