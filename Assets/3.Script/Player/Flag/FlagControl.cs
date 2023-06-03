using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagControl : MonoBehaviour
{
    [Header("플레이어 세팅")]
    [Tooltip("플레이어 이동 속도")]
    [SerializeField]
    private float moveSpeed = 2.0f;
    [Tooltip("플레이어 대쉬 속도")]
    [SerializeField]
    private float dashSpeed = 2.7f;
    [Tooltip("플레이어 가감속도")]
    [SerializeField]
    private float speedChangeRate = 10f;
    [Tooltip("원거리 공격 연사 딜레이")]
    [SerializeField]
    private float fireDelay = 0.1f;


    //private PlayerData player;

    // 플레이어
    public float h { get; private set; }
    public float v { get; private set; }
    private float animationBlend;
    public int invincibleLayer { get; private set; }
    public int defaultLayer { get; private set; }
    private bool canMove = true;
    public float threshold = 1f;
    private readonly Vector3 defaultPos = new Vector3(0, 0.02f, 0);

    // 대쉬
    public KeyCode lastKeyPressed { get; private set; }  // 대쉬를 위해 이전 키입력 저장
    private readonly KeyCode[] keysToCheck = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W };    // 대쉬를 위해 비교할 이동키 모음
    private float lastKeyPressTime = 0f;
    private readonly float timeAllowedBetweenKeyPresses = 0.5f; // 연속 입력으로 판단하기 위한 최대 시간차

    // 공격
    public bool isCombo { get; private set; }   // 건담 약공격 콤보
    private float preInputDelay = 0.9f;         // 선입력 유지시간

    // 전략, 상태
    private IFlagViewStrategy currentViewStrategy = new FlagTopViewMove();
    private IFlagModeStrategy currentModeStrategy = new ModeFlag();
    public IFlagState currentState = new FlagNomal();
    private FlagNomal nomalState;
    private FlagAttack attackState;
    private FlagDash dashState;
    private FlagTransformation transfomationState;


    //[Header("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ")]
    //[Space(50)]
    // 애니매이션 해시
    public int hashHSpeed { get; private set; }
    public int hashAniSpeed { get; private set; }
    public int hashDash { get; private set; }
    public int hashToGundam { get; private set; }
    public int hashToFlag { get; private set; }
    public int hashVerticalWeakAttack { get; private set; }
    public int hashHorizontalWeakAttack { get; private set; }
    public int hashFlagStrongAttack { get; private set; }
    public int hashGundamWeakAttack1 { get; private set; }
    public int hashGundamWeakAttack2 { get; private set; }
    public int hashGundamStrongAttack { get; private set; }

    // 컴포넌트
    private Animator anim;
    private Rigidbody rigid;
    private FlagBulletSpawner[] bulletSpawners = new FlagBulletSpawner[2];

    // 캐싱
    #region 코루틴 Wait
    public WaitUntil InputFireButton_wait { get; private set; }
    public WaitUntil ResetCombo_wait { get; private set; }
    public WaitUntil EnterDashAni_wait { get; private set; }
    public WaitUntil ExitDashAni_wait { get; private set; }
    public WaitUntil EnterAttackAni_wait { get; private set; }
    public WaitUntil ExitAttackAni_wait { get; private set; }
    public WaitUntil EnterTransformation_wait { get; private set; }
    public WaitUntil ExitTransformation_wait { get; private set; }
    public WaitUntil EnterGundamAttack1_wait { get; private set; }

    public WaitForSeconds FireDelay_wait { get; private set; }
    public WaitForSeconds AnimaReset_wait { get; private set; }
    #endregion 코루틴 Wait

    #region 초기화
    private void Awake()
    {
        GetComponentAndCashing();
        GetAnimHash();

        invincibleLayer = LayerMask.NameToLayer("Invincible");
        defaultLayer = LayerMask.NameToLayer("Default");

        nomalState = new FlagNomal();
        attackState = new FlagAttack();
        dashState = new FlagDash();
        transfomationState = new FlagTransformation();
    }
    private void OnEnable()
    {
        // 이벤트 구독 (시점 변환)
        Init();
        SetViewStrategy(new FlagTopViewMove());
    }
    private void OnDisable()
    {
        // 이벤트 해제 (시점 변환)
        StopCoroutine(nameof(Fire_co));
        StopCoroutine(nameof(ResetCombo_co));
        StopCoroutine(nameof(SetTransformationState_co));
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        //Cursor.lockState = CursorLockMode.Locked;
        // todo 버튼 눌러야돼서 나중에 바꿀것
        Cursor.lockState = CursorLockMode.None;
    }
    private void GetComponentAndCashing()
    {
        TryGetComponent(out anim);
        TryGetComponent(out rigid);
        bulletSpawners = GetComponentsInChildren<FlagBulletSpawner>();

        InputFireButton_wait = new WaitUntil(() => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        EnterDashAni_wait = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("FlagDash") || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamDash"));
        ExitDashAni_wait = new WaitUntil(() => !(anim.GetCurrentAnimatorStateInfo(0).IsName("FlagDash") || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamDash")));
        EnterAttackAni_wait = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("HorizontalWeakAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("VerticalWeakAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("FlagStrongAttack") ||
                                                  anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack2") || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamStrongAttack"));
        ExitAttackAni_wait = new WaitUntil(() => !(anim.GetCurrentAnimatorStateInfo(0).IsName("HorizontalWeakAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("VerticalWeakAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("FlagStrongAttack") ||
                                                  anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack1") || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack2") || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamStrongAttack")));
        EnterTransformation_wait = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("ToFlag") || anim.GetCurrentAnimatorStateInfo(0).IsName("ToGundam"));
        ExitTransformation_wait = new WaitUntil(() => !(anim.GetCurrentAnimatorStateInfo(0).IsName("ToFlag") || anim.GetCurrentAnimatorStateInfo(0).IsName("ToGundam")));
        EnterGundamAttack1_wait = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack1"));
        FireDelay_wait = new WaitForSeconds(fireDelay);
        AnimaReset_wait = new WaitForSeconds(preInputDelay);
        ResetCombo_wait = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f);
    }
    private void GetAnimHash()
    {
        hashHSpeed = Animator.StringToHash("hSpeed");
        hashAniSpeed = Animator.StringToHash("aniSpeed");
        hashDash = Animator.StringToHash("dash");
        hashToGundam = Animator.StringToHash("toGundam");
        hashToFlag = Animator.StringToHash("toFlag");
        hashVerticalWeakAttack = Animator.StringToHash("verticalWeakAttack");
        hashHorizontalWeakAttack = Animator.StringToHash("horizontalWeakAttack");
        hashFlagStrongAttack = Animator.StringToHash("flagStrongAttack");
        hashGundamWeakAttack1 = Animator.StringToHash("gundamWeakAttack1");
        hashGundamWeakAttack2 = Animator.StringToHash("gundamWeakAttack2");
        hashGundamStrongAttack = Animator.StringToHash("gundamStrongAttack");
    }
    private void Init()
    {
        currentState = nomalState;
        isCombo = false;
        SetModeStrategy(new ModeFlag());

        StartCoroutine(nameof(Fire_co));
        StartCoroutine(nameof(ResetCombo_co));
        StartCoroutine(nameof(SetTransformationState_co));
    }
    #endregion 초기화

    #region 전략, 상태
    // 시점
    public void SetTopViewStrategy()
    {
        SetViewStrategy(new FlagTopViewMove());
    }
    public void SetBackViewStrategy()
    {
        SetViewStrategy(new FlagBackViewMove());
    }
    public void SetSideViewStrategy()
    {
        SetViewStrategy(new FlagSideViewMove());
    }
    private void SetViewStrategy(IFlagViewStrategy strategy)
    {
        // 시점 변환하면 플레이어를 중앙으로 다시 이동시킴
        StartCoroutine(MoveToCenter(0.5f));
        currentViewStrategy = strategy;
    }

    // 모드
    public void TransGundam()
    {
        SetModeStrategy(new ModeGundam());
    }
    public void TransFlag()
    {
        SetModeStrategy(new ModeFlag());
    }
    private void SetModeStrategy(IFlagModeStrategy strategy)
    {
        // 모드 변경에 따른 애니메이션 출력
        if (!(currentModeStrategy is ModeGundam) && strategy is ModeGundam)
        {
            anim.SetTrigger(hashToGundam);
        }
        else if (!(currentModeStrategy is ModeFlag) && strategy is ModeFlag)
        {
            anim.SetTrigger(hashToFlag);
        }

        currentModeStrategy = strategy;
    }

    // 상태
    private void SetState(IFlagState state)
    {
        StopCoroutine(nameof(ReturnToNomalState_co));
        currentState = state;
    }
    public IEnumerator ReturnToNomalState_co(WaitUntil wait1 = null, WaitUntil wait2 = null)
    {
        yield return wait1; // 애니메이션 시작, 키입력 등 
        yield return wait2; // 애니메이션 완료, null 등

        SetState(nomalState);
    }
    private IEnumerator SetTransformationState_co()
    {
        while (true)
        {
            yield return EnterTransformation_wait;
            // 모드 변환 애니메이션 실행되면 플레이어 회전값 초기화 (건담 모드에서 돌아가기 때문에)
            transform.rotation = Quaternion.Euler(-90f, 0, 0);
            SetState(transfomationState);
            yield return ExitTransformation_wait;
            // 모드 변환 완료 시 노말 상태로 복귀
            SetState(nomalState);
        }
    }
    #endregion 전략, 상태

    private void Update()
    {
        currentState.Action(this);  // 상태에 따라 레이어 변경하여 무적여부 결정

        if (!currentState.Equals(transfomationState) && canMove)
        {
            InputMoveKey();
            if (currentState.Equals(nomalState))
            {
                CheckDash();
            }
            Attack();
        }
    }
    private void FixedUpdate()
    {
        if (!currentState.Equals(transfomationState) && canMove)
        {
            currentModeStrategy.Rotate(this);
            Move();
        }
    }

    #region 이동
    private void InputMoveKey()
    {
        // 키입력에 따라 이동 방향 결정
        // 방향만 결정하는 이유는 시점에 따라 이동할 방향이 달라 이동 자체는 다른 곳에서 처리하기 때문
        if (Input.GetKey(KeyCode.A))
        {
            h = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            h = 1;
        }
        else
        {
            h = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            v = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
    }
    private void Move()
    {
        float targetSpeed = moveSpeed;  // 대쉬 속도 등 모두 계산된 최종 이동속도
        Vector3 moveDir;
        // 시점에 따른 이동 방향 결정
        currentViewStrategy.Move(this, out moveDir);    

        // 이동 방향에 따른 애니메이션 설정
        if (currentModeStrategy is ModeFlag)
        {
            animationBlend = Mathf.Lerp(animationBlend, moveDir.x, Time.deltaTime * speedChangeRate);
            if (Mathf.Abs(animationBlend) < 0.01f)
            {
                animationBlend = 0f;
            }
            anim.SetFloat(hashHSpeed, animationBlend);
        }
        else
        {
            if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.z))
            {
                animationBlend = Mathf.Lerp(animationBlend, Mathf.Abs(moveDir.x), Time.deltaTime * speedChangeRate);
            }
            else
            {
                animationBlend = Mathf.Lerp(animationBlend, Mathf.Abs(moveDir.z), Time.deltaTime * speedChangeRate);
            }
            if (animationBlend < 0.01f)
            {
                animationBlend = 0f;
            }
            anim.SetFloat(hashAniSpeed, animationBlend);
        }

        // 플레이어 이동
        if (currentState is FlagDash)
        {
            targetSpeed *= dashSpeed;
        }
        Vector3 newPosition = new Vector3(
            Mathf.Clamp((rigid.position.x + targetSpeed * Time.deltaTime * moveDir.x), -0.3f, 0.3f),
            Mathf.Clamp((rigid.position.y + targetSpeed * Time.deltaTime * moveDir.y), -0.18f, 0.18f),
            Mathf.Clamp((rigid.position.z + targetSpeed * Time.deltaTime * moveDir.z), -0.15f, 0.15f));
        rigid.MovePosition(newPosition);
    }
    private void CheckDash()
    {
        // 방향키만 두번 입력 체크함
        foreach (KeyCode key in keysToCheck)
        {
            if (Input.GetKeyDown(key))
            {
                // 일정 시간 안에 같은 방향키를 두번 눌렀으면 대쉬
                if (lastKeyPressed == key && Time.time - lastKeyPressTime <= timeAllowedBetweenKeyPresses)
                {
                    SetState(dashState);
                    currentModeStrategy.Dash(this);
                    lastKeyPressed = KeyCode.None;
                }
                else
                {
                    lastKeyPressed = key;
                }
                lastKeyPressTime = Time.time;
            }
        }
    }
    public IEnumerator ResetScaleX_co()
    {
        yield return EnterDashAni_wait;
        yield return ExitDashAni_wait;
        transform.localScale = Vector3.one;
    }

    // 시점변경 등 플레이어 중앙으로 강제 이동시키는 메소드
    public IEnumerator MoveToCenter(float speed)
    {
        canMove = false;

        while (Vector3.SqrMagnitude(transform.position - defaultPos) >= 0.0001f)
        {
            Vector3 direction = (defaultPos - transform.position).normalized;   // 이동해야 할 방향 계산
            Vector3 moveVector = direction * speed * Time.deltaTime;            // 속도를 고려해 이동 해야할 위치 계산

            rigid.MovePosition(transform.position + moveVector);
            yield return null;
        }
        transform.position = defaultPos;

        canMove = true;
    }
    public void StopMove()
    {
        canMove = false;
    }
    public void CanMove()
    {
        canMove = true;
    }
    #endregion 이동

    #region 공격
    private void Attack()
    {
        if (!currentState.Equals(attackState))
        {
            if (Input.GetKeyDown(KeyCode.Period) || Input.GetMouseButtonDown(0))
            {
                // 비행기 모드의 약공격이 시점에 따라 수직오르 돌지 수평으로 돌지 결정하기 위한 변수
                bool isHorizontal;
                if (currentViewStrategy is FlagSideViewMove)
                {
                    isHorizontal = false;
                }
                else
                {
                    isHorizontal = true;
                }

                currentModeStrategy.WeakAttack(this, isHorizontal);
                SetState(attackState);
                StopCoroutine(nameof(ReturnToNomalState_co));
                StartCoroutine(ReturnToNomalState_co(EnterAttackAni_wait, ExitAttackAni_wait));
            }
            if (Input.GetKeyDown(KeyCode.Slash) || Input.GetMouseButtonDown(1))
            {
                SetState(attackState);
                currentModeStrategy.StrongAttack(this);
                StopCoroutine(nameof(ReturnToNomalState_co));
                StartCoroutine(ReturnToNomalState_co(EnterAttackAni_wait, ExitAttackAni_wait));
            }
        }
        // 건담모드는 공격중에도 약공격 콤보로 발동 가능
        else if (currentModeStrategy is ModeGundam)
        {
            if (Input.GetKeyDown(KeyCode.Period) || Input.GetMouseButtonDown(0))
            {
                SetState(attackState);
                currentModeStrategy.WeakAttack(this);
                StopCoroutine(nameof(ReturnToNomalState_co));
                StartCoroutine(ReturnToNomalState_co(EnterAttackAni_wait, ExitAttackAni_wait));
            }
        }
    }
    private IEnumerator Fire_co()
    {
        while (true)
        {
            yield return FireDelay_wait;
            yield return InputFireButton_wait;

            foreach (FlagBulletSpawner b in bulletSpawners)
            {
                b.Fire();
                AudioManager.Instance.PlaySfx(Define.SFX.Shot);
            }
        }
    }
    public IEnumerator ResetCombo_co()
    {
        float waitTime = 5f;
        float startTime;
        while (true)
        {
            // 건담 1단 공격
            yield return EnterGundamAttack1_wait;
            isCombo = true;
            startTime = Time.time;
            // 2단 공격이 실행되거나 1단 공격이 실행 된 지 오래 됐으면 콤보 리셋
            yield return new WaitUntil(() => (Time.time >= startTime + waitTime) || anim.GetCurrentAnimatorStateInfo(0).IsName("GundamWeakAttack2"));
            isCombo = false;
        }
    }
    public void PlayGundamAttackSfx()
    {
        AudioManager.Instance.PlaySfx(Define.SFX.GundamAttack);
    }
    #endregion 공격

    public void SetAnimaTrigger(int hashAni)
    {
        // 트리거가 발동되기 전까지 On되있지 않고 몇초 뒤 꺼지도록 하기 위한 메소드
        anim.SetTrigger(hashAni);
        StopCoroutine(nameof(ResetAnimaTrigger_co));
        StartCoroutine(nameof(ResetAnimaTrigger_co), hashAni);
    }
    private IEnumerator ResetAnimaTrigger_co(int hashAni)
    {
        yield return AnimaReset_wait;
        anim.ResetTrigger(hashAni);
    }
}
