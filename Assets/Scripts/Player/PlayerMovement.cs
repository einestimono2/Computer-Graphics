using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    Transform cameraTransform; // cameraObject
    InputHandler inputHandler;
    public Vector3 direction; // moveDirection

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    [HideInInspector] public Transform myTransform;
    [HideInInspector] public AnimatorHandler animatorHandler;

    public Rigidbody rig;
    // public GameObject cameraObject; // normalCamera

    [Header("Ground & Fall Stats")]
    [SerializeField] private float groundStartPoint = -0.5f; // Thay đổi phải chỉnh lại HandleFalling
    [SerializeField] private float minimumDistanceToFall = 1f; // Thay đổi phải chỉnh lại HandleFalling
    [SerializeField] private float groundCheckDistance = 0.2f; // Thay đổi phải chỉnh lại HandleFalling
    public LayerMask groundCheck;
    public float inAirTimer;

    [Header("Movement Stats")]
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float sprintSpeed = 15f;
    [SerializeField] float rotationSpeed = 8f;
    [SerializeField] float fallingSpeed = 80f;
    [SerializeField] float jumpForce = 500f;
    public bool jumpForceApplied;

    // Start is called before the first frame update
    void Start(){
        playerManager = GetComponent<PlayerManager>();
        rig = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();

        cameraTransform = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();

        playerManager.isGrounded = true;

        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    void FixedUpdate(){
        if (jumpForceApplied)
        {
            StartCoroutine(DisableForce());
            rig.AddForce(transform.up * jumpForce);
        }
    }

    IEnumerator DisableForce(){
        yield return new WaitForSeconds(0.3f);
        jumpForceApplied = false;
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleMovement(){
        if(inputHandler.rollFlag || playerManager.isInteracting || !playerManager.isGrounded) return;

        direction = cameraTransform.forward * inputHandler.vertical + cameraTransform.right * inputHandler.horizontal;
        direction.Normalize();
        direction.y = 0;

        float speed = movementSpeed;

        if(inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f){
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            direction *= speed;
        }else{
            if(inputHandler.moveAmount < 0.5f){
                direction *= walkingSpeed;
                playerManager.isSprinting = false;
            } 
            else{
                direction *= speed;
                playerManager.isSprinting = false;
            } 
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(direction, normalVector);
        rig.velocity = projectedVelocity;

        animatorHandler.UpdateAnimator(inputHandler.moveAmount, 0, playerManager.isSprinting);
    }

    public void HandleRolling(){
        if(animatorHandler.anim.GetBool("isInteracting")) return;

        if(inputHandler.rollFlag){
            direction = cameraTransform.forward * inputHandler.vertical + cameraTransform.right * inputHandler.horizontal;

            if(inputHandler.moveAmount > 0){
                animatorHandler.PlayAnimation("Rolling", true);
                direction.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(direction);
                myTransform.rotation = rollRotation;
            }else{
                animatorHandler.PlayAnimation("Backstep", true);
            }
        }
    }

    public void HandleFalling(Vector3 direction){
        playerManager.isGrounded = false;
        RaycastHit hit;

        Vector3 origin = myTransform.position;
        origin.y += groundStartPoint;

        if(Physics.Raycast(origin, myTransform.forward, out hit, 0.4f)){
            direction = Vector3.zero;
        }

        if(playerManager.isInAir){
            rig.AddForce(-Vector3.up * fallingSpeed);
            rig.AddForce(direction * fallingSpeed / 10f);
        }

        Vector3 dir = direction;
        dir.Normalize();
        origin += dir * groundCheckDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceToFall, Color.red, 0.1f, false);
        if(Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceToFall, groundCheck)){
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y + 1; // +1 ==> Fix lỗi bị kẹt ở giữa nền

            if(playerManager.isInAir){ 
                if(inAirTimer > 0.3f){
                    animatorHandler.PlayAnimation("Land", true);
                    inAirTimer = 0;
                }else{
                    animatorHandler.PlayAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }else{
            if(playerManager.isGrounded){
                playerManager.isGrounded = false;
            }

            if(playerManager.isInAir == false){
                if(playerManager.isInteracting == false){
                    animatorHandler.PlayAnimation("Falling", true);
                }

                Vector3 vel = rig.velocity;
                vel.Normalize();
                rig.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if(playerManager.isInteracting || inputHandler.moveAmount > 0){
            myTransform.position  = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }else{
            myTransform.position  = targetPosition;
        }
    }

    public void HandleJumping(){
        if(playerManager.isInteracting) return;

        if(inputHandler.jump_Input){
            if(inputHandler.moveAmount > 0){
                direction = cameraTransform.forward * inputHandler.vertical + cameraTransform.right * inputHandler.horizontal;

                animatorHandler.PlayAnimation("Jump", true);

                jumpForceApplied = true;

                direction.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(direction);
                myTransform.rotation = jumpRotation;
            }
        }
    }

    public void HandleRotation(){
        if(animatorHandler.canRotate){
            Vector3 targetDir = Vector3.zero;
            targetDir = cameraTransform.forward * inputHandler.vertical + cameraTransform.right * inputHandler.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero) targetDir = myTransform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed * Time.deltaTime);

            myTransform.rotation = targetRotation;
        }
    }

    #endregion

    // [Header("Movement")]
    // public float moveSpeed;
    // public float runSpeed;
    // public float gravity = -9.8f;
    // public float jumpHeight;

    // [Header("Ground Check")]
    // public bool isGrounded;
    // public LayerMask groundMask;
    // public Transform groundCheck;
    // public float groundCheckDistance;

    // private float speed;

    // private Vector3 moveDirection;
    // private CharacterController controller;
    // private Animator anim;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     controller = GetComponent<CharacterController>();
    //     anim = GetComponentInChildren<Animator>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     // Ground Check
    //     isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

    //     if(isGrounded && moveDirection.y < 0){
    //         moveDirection.y = -2f;
    //     }

    //     float x = Input.GetAxis("Horizontal");
    //     float z = Input.GetAxis("Vertical");

    //     Vector3 move = transform.right * x + transform.forward * z;
    //     controller.Move(move * speed * Time.deltaTime);

    //     // Animation
    //     anim.SetFloat("vertical", z);
    //     anim.SetFloat("horizontal", x);

    //     // Run
    //     if(Input.GetKey(KeyCode.LeftShift)){
    //         speed = runSpeed;
    //         anim.SetBool("run", true);
    //     }else{
    //         speed = moveSpeed;
    //         anim.SetBool("run", false);
    //     }

    //     // Jump
    //     if(Input.GetKey(KeyCode.Space) && isGrounded){
    //         moveDirection.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    //     }

    //     if(isGrounded){
    //         anim.SetBool("jump", false);
    //     }else{
    //         anim.SetBool("jump", true);
    //     }

    //     moveDirection.y += gravity * Time.deltaTime;
    //     controller.Move(moveDirection * Time.deltaTime);

    // }

// =========================
    
    // [SerializeField] private float moveSpeed;
    // [SerializeField] private float walkSpeed;
    // [SerializeField] private float runSpeed;
    // [SerializeField] private float jumpHeight;

    // private Vector3 moveDirection;
    // private Vector3 velocity;

    // [SerializeField] private bool isGrounded;
    // [SerializeField] private float groundCheckDistance;
    // [SerializeField] private LayerMask groundMask;
    // [SerializeField] Transform groundCheck;

    // [SerializeField] private float gravity;

    // private CharacterController controller;
    // private Animator anim;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     controller = GetComponent<CharacterController>();
    //     anim = GetComponentInChildren<Animator>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     Move();
    // }

    // private void Move()
    // {
    //     isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

    //     if(isGrounded && velocity.y < 0){
    //         velocity.y = -2f;
    //     }

    //     float moveZ = Input.GetAxis("Vertical");

    //     moveDirection = new Vector3(0, 0, moveZ);
    //     moveDirection = transform.TransformDirection(moveDirection);
        
    //     if(isGrounded){
    //         if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)){
    //             Walk();
    //         }
    //         else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)){
    //             Run();
    //         }
    //         else if(moveDirection != Vector3.zero){
    //             Idle();
    //         }

    //         moveDirection *= moveSpeed; 

    //         if(Input.GetKeyDown(KeyCode.Space)){
    //             Jump();
    //         }
    //     }

    //     controller.Move(moveDirection * Time.deltaTime);

    //     velocity.y += gravity * Time.deltaTime;
    //     controller.Move(velocity * Time.deltaTime);
    // }

    // private void Walk()
    // {
    //     moveSpeed = walkSpeed;
    //     anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    // }

    // private void Run()
    // {
    //     moveSpeed = runSpeed;
    //     anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    // }

    // private void Jump(){
    //     velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    // }
    
    // private void Idle()
    // {
    //     anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    // }
    
}
