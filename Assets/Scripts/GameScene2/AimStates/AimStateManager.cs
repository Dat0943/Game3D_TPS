using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class AimStateManager : MonoBehaviour
{
    [Header ("Camera Follow")]
    [SerializeField] private Transform cameraFollowPosition;
    [SerializeField] private float minLimitY;
    [SerializeField] private float maxLimitY;
    [SerializeField] private float mouseSense = 1;
    [HideInInspector] public Animator anim;
    float xAxis, yAxis;

    [Header ("Aim State Animation")]
    public AimBaseState currentState;
    public AimState Aim = new AimState();
    public HipFireState Hip = new HipFireState();

    [Header ("Camera Zoom")]
    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float FOVSmoothSpeed = 10f;
    public float adsFOV = 40f;
    [HideInInspector] public float hipFOV; // FieldOfView ban đầu
    [HideInInspector] public float currentFOV;

    [Header ("Aim State Raycast")]
    public Transform aimPosition;
    [SerializeField] private float aimSmoothSpeed = 20f;
    [SerializeField] private LayerMask aimMask;

    [Header("Crouch Camera")]
    [SerializeField] private float crounchCameraHeight = 0.6f;
    [SerializeField] private float shoulderSwapSpeed = 10f;
    MovementStateManager movement;
    float xFollowPosition;
    float yFollowPosition, ogYPosition;

    void Awake()
    {
        anim = GetComponent<Animator>();
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        movement = GetComponent<MovementStateManager>(); 
    }

    void Start()
    {
        xFollowPosition = cameraFollowPosition.localPosition.x;
        ogYPosition = cameraFollowPosition.localPosition.y;
        yFollowPosition = ogYPosition;

        hipFOV = vCam.m_Lens.FieldOfView;

        SwitchState(Hip);
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, minLimitY, maxLimitY);

        // Di chuyển Camera khi ngắm
        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFOV, FOVSmoothSpeed * Time.deltaTime);

        // Điểm ngắm
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPosition.position = Vector3.Lerp(aimPosition.position, hit.point, aimSmoothSpeed * Time.deltaTime);

        // Camera di chuyển khi ngồi xuống và chuyển hướng
        MoveCamera();

        currentState.UpdateState(this);
    }

    void LateUpdate()
    {
        cameraFollowPosition.localEulerAngles = new Vector3(yAxis, cameraFollowPosition.localEulerAngles.y, cameraFollowPosition.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            xFollowPosition = -xFollowPosition; // Tức là khi nhấn đi nhấn lại thì nó sẽ chỉ có 2 vị trí đó

        if (movement.currentState == movement.Crouch)
            yFollowPosition = crounchCameraHeight;
        else
            yFollowPosition = ogYPosition;

        Vector2 newFollowPosition = new Vector3(xFollowPosition, yFollowPosition, cameraFollowPosition.localPosition.z);
        cameraFollowPosition.localPosition = Vector3.Lerp(cameraFollowPosition.localPosition, newFollowPosition, shoulderSwapSpeed * Time.deltaTime);
    }
}
