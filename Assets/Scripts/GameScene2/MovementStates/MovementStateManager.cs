using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [Header("Movement")]
    public float currentMoveSpeed;
    public float walkSpeed = 3f, walkBackSpeed = 2f;
    public float runSpeed = 7f, runBackSpeed = 5f;
    public float crouchSpeed = 2f, crouchBackSpeed = 1f;
    CharacterController controller;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float vInput, hInput;

    [Header("Ground Check")]
    [SerializeField] private float groundOffset = 0.08f;
    [SerializeField] private LayerMask groundMask;
    Vector3 spherePos;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    Vector3 velocity;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 2f;
    [HideInInspector] public bool jumped;

    public MovementBaseState previousState;
    public MovementBaseState currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();
    public JumpState Jump = new JumpState();

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        SwitchState(Idle);
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        Falling();

        anim.SetFloat("hInput", hInput);
        anim.SetFloat("vInput", vInput);

        currentState.UpdateState(this); // Khi gọi lên thì tất cả các trạng thái được duyệt sẽ hoạt động
    }

    void GetDirectionAndMove()
    {
        vInput = Input.GetAxis("Vertical");
        hInput = Input.GetAxis("Horizontal");

        direction = transform.forward * vInput + transform.right * hInput;

        controller.Move(direction.normalized * currentMoveSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        if(Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded())
            velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0)
            velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    void Falling()
    {
        anim.SetBool("Falling", jumped);
    }

    #region Event Animation Jump
    public void JumpForce()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    public void Jumped() => jumped = true;
    #endregion

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
