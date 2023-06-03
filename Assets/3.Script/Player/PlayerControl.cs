using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("플레이어 세팅")]
    public float moveSpeed = 2.0f;
    public float sprintSpeed = 2.7f;
    [Range(0.0f, 0.3f)]
    [Tooltip("캐릭터 회전 속도")]
    public float rotationSmoothTime = 0.12f;
    [Tooltip("가감속도")]
    public float speedChangeRate = 10f;

    [Space(10)]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    // 낙하 애니메이션 돌입 시간
    // => 계단 내려갈때 점프모션 방지
    [Range(0.0f, 0.2f)]
    [Tooltip("낙하 판정 돌입하는 시간")]
    public float fallTime = 0.05f;

    [Header("땅 체크")]
    private bool isGrounded = true;
    // ??
    public float groundedOffset = -0.14f;
    // 땅체크 반경
    private float groundedRadius = 0.2f;
    [Tooltip("땅으로 판단할 레이어들")]
    public LayerMask GroundLayers;

    [Header("시네머신 카메라")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("위에서 아래 바라보는 각도")]
    public float TopClamp = 70.0f;
    [Tooltip("아래에서 위 바라보는 각도")]
    public float BottomClamp = -30.0f;
    [Range(0.5f, 2.0f)]
    [Tooltip("카메라 수직 감도")]
    public float CameraVerticalSensitivity = 1.0f;
    [Range(1.0f, 4.0f)]
    [Tooltip("카메라 수평 감도")]
    public float CameraHorizontalSensitivity = 2.0f;
    [Tooltip("카메라 고정")]
    public bool LockCameraPosition = false;


    // 시네머신
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // 플레이어
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // 애니매이션 해시
    private int hashSpeed;
    private int hashGrounded;
    private int hashJump;
    private int hashFreeFall;
    private int hashMotionSpeed;


    //private PlayerInput _playerInput;
    private Animator anim;
    private CharacterController controller;
    //private StarterAssetsInputs input;
    private GameObject mainCamera;

    private const float _threshold = 0.01f;


    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        
        TryGetComponent(out anim);
        TryGetComponent(out controller);
        //TryGetComponent(input);

        GetAnimHash();

        _fallTimeoutDelta = fallTime;
    }

    private void Update()
    {
        Move();
    }
    private void LateUpdate()
    {
        CameraRotation();
    }

    private void GetAnimHash()
    {
        hashSpeed = Animator.StringToHash("Speed");
        hashGrounded = Animator.StringToHash("Grounded");
        hashJump = Animator.StringToHash("Jump");
        hashFreeFall = Animator.StringToHash("FreeFall");
        hashMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
    private void Move()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintSpeed : moveSpeed;
        if(move.Equals(Vector2.zero))
        {
            targetSpeed = 0f;
        }

        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // 커브
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                Time.deltaTime * speedChangeRate);
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (_animationBlend < 0.01f)
        {
            _animationBlend = 0f;
        }
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;
        if (move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                rotationSmoothTime);

            // 카메라 보는 방향으로 회전
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        anim.SetFloat(hashSpeed, _animationBlend);
        anim.SetFloat(hashMotionSpeed, 1);
    }
    private void CameraRotation()
    {
        // 마우스 입력에 의한 회전값
        Vector2 look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            _cinemachineTargetYaw += look.x * CameraHorizontalSensitivity;
            _cinemachineTargetPitch -= look.y * CameraVerticalSensitivity;
        }
        
        // 360도 제한
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // 카메라 세팅
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
