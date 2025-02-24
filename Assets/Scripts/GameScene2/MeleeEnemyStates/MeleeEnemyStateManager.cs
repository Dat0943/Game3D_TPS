using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyStateManager : MonoBehaviour
{
    [Header ("Idle State")]
    public float detectionArea = 18f;
    public float idleTime = 4f;

    [Header ("Patrol State")]
    public float patrolSpeed = 2f;
    public float patrolingTime = 10f;

    [Header ("Chase State")]
    public float stopChasingDistance = 21f;
    public float attackingDistance = 1f;
    public float chaseSpeed = 4f;

    [Header ("Attack State")]
    public float stopAttackingDistance = 2.5f;

    MeleeEnemyBaseState currentState;
    public MeleeIdleState MeleeIdle = new MeleeIdleState();
    public MeleePatrolState MeleePatrol = new MeleePatrolState();
    public MeleeChaseState MeleeChase = new MeleeChaseState();
    public MeleeAttackState MeleeAttack = new MeleeAttackState();

    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent navMesh;
    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public Transform player;

    Rigidbody[] rbs;
    public float knockBackSpeed = 2.5f;
    public float currentSpeed;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        rbs = GetComponentsInChildren<Rigidbody>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        foreach (Rigidbody rb in rbs)
            rb.isKinematic = true;

        SwitchState(MeleeIdle);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TriggerRagdoll()
    {
        foreach (Rigidbody rb in rbs)
            rb.isKinematic = false;
    }

    public void SwitchState(MeleeEnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
