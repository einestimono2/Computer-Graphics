using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    EnemyMovement enemyMovement;
    EnemyAnimator enemyAnimator;
    EnemyStats enemyStats;
    
    public Rigidbody rig;
    public NavMeshAgent nav;

    [Header("Detection Settings")]
    public float radius = 25;
    // Tương tự như tầm nhìn của mắt
    public float maxAngle = 45;
    public float minAngle = -45;
    
    [Header("Enemy Info")]
    public float rotationSpeed = 15f;
    public float currentCooldownTime = 0;
    public float maxAttackRange = 2f;

    [Header("Enemy State")]
    public bool isPerformingAction;
    public bool isInteracting;
    public State currentState;
    public CharacterStats target;

    void Awake(){
        enemyMovement = GetComponent<EnemyMovement>();
        enemyStats = GetComponent<EnemyStats>();
        rig = GetComponent<Rigidbody>();
        nav = GetComponentInChildren<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    // Start is called before the first frame update
    void Start(){
        nav.enabled = false;
        rig.isKinematic = false;
    }

    // Update is called once per frame
    void Update(){
        isInteracting = enemyAnimator.anim.GetBool("isInteracting");
        HandleCooldownTime();
    }

    void FixedUpdate(){
        HandleState();
    }

    public void HandleState(){
        if(currentState != null){
            State nextState = currentState.Tick(this, enemyStats, enemyAnimator);

            if(nextState != null){
                currentState = nextState;
            }
        }
    }

    // void SwitchToState(State nextState){
    //     currentState = nextState;
    // }

    void HandleCooldownTime(){
        if(currentCooldownTime > 0){
            currentCooldownTime -= Time.deltaTime;
        }

        if(isPerformingAction && currentCooldownTime <= 0){
            isPerformingAction = false;
        }
    }

}
