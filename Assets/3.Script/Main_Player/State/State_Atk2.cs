using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Atk2 : State
{
    private Camera mainCamera;
    private GameObject sword;
    private GameObject bigSword;
    private GameObject idleSword;
    private GameObject idleBigSword;
    public bool isCanStr;
    [SerializeField]bool stronged;
    int index = 0;
    float holdTime = 0;
    private void Start()
    {
        mainCamera = Camera.main;
        sword = Main_Player.Instance.sword;
        bigSword = Main_Player.Instance.bigSword;
        idleSword = Main_Player.Instance.idleSword;
        idleBigSword = Main_Player.Instance.idleBigSword;
    }
    public override void Enter(State before)
    {
        Main_Player.Instance.isAtk = true;
        Main_Player.Instance.anim_player.applyRootMotion = true;
        Atk();
        Main_Player.Instance.anim_sword.SetTrigger("Atk");
        Main_Player.Instance.anim_player.SetTrigger("Atk");
        ResetStrong();
    }

    public override void Exit(State next)
    {
        Main_Player.Instance.anim_player.applyRootMotion = false;
        Main_Player.Instance.collider_sword.enabled = false;
        EndAtk();
        ResetBool();
        isCanStr = false;
        stronged = false;
        index = 0;
    }

    public override void StateFixedUpdate()
    {
 
    }

    public override void StateUpdate()
    {
        if (!stronged) { 
            if (Input.GetMouseButtonDown(0))
            {
                holdTime = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                holdTime += Time.deltaTime;
                if (holdTime > 0.45f)
                {
                    isCanStr = true;
                }
            }
        }
    }


    void ResetBool()
    {
        for (int i = 1; i <= 5; i++)
        {
            Main_Player.Instance.anim_sword.SetBool("Atk" + i, false);
            Main_Player.Instance.anim_player.SetBool("Atk" + i, false);
            if (i <= 3)
            {
                Main_Player.Instance.anim_bigsword.SetBool("Atk" + i, false);
            }
        }
    }
    public void AtkStrong()
    {
        stronged = true;
        isCanStr = false;//강공격 불변수 체크
        //강공격 애니메이션 재생
        LoadSword();
        Main_Player.Instance.anim_sword.SetTrigger("AtkStrong");
        Main_Player.Instance.anim_player.SetTrigger("AtkStrong");
        //holdtime 초기화
        holdTime = 0f;
    }
    void ResetStrong()
    {
        Main_Player.Instance.anim_player.SetBool("CanStrong", true);
        Main_Player.Instance.anim_sword.SetBool("CanStrong", true);
        stronged = false;
        isCanStr = false;
        holdTime = 0f;
    }
    void Rotate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        Vector3 movement = (moveHorizontal * mainCamera.transform.right + moveVertical * cameraForward).normalized;

        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = toRotation;
    }


    //============================ 공격 메소드 =========================
    public void Atk()
    {
        index++;
        int i = index;
        //index값을 추가하고 추가 된 인덱스 값을 i로 받음.
        if (i <= 5)
        {
            Atk_co(i);
        }
        //공격 5콤보가 끝난 상태이므로 아무것도 안 함.
    }
    void Atk_co(int i)
    {
        //공격 애니메이션 실행
        LoadSword();
        Atk_anim(i);
        //공격 애니메이션 중 콜라이더가 온 될 시점 + 지속 될 시간
        // == Sword 스크립트에서 처리 ==
        //다음 공격을 입력받고 입력받으면 다음 공격을 실행해야 돼!
        // == Sword 스크립트에서 처리 ==
    }

    void Atk_anim(int i)
    {
        Main_Player.Instance.anim_sword.SetBool("Atk" + i, true);
        Main_Player.Instance.anim_player.SetBool("Atk" + i, true);
    }

    IEnumerator listener;
    public void EndAtk()
    {
        //지금 애니메이션이 끝까지 재생 될 거임.
        //근데 WASD나 공격키, 점프키 등 입력값이 있으면 언제든지 애니메이션 재생이 종료되어야 함.
        //애니메이션 재생 종료는 Main_Player의 불값이 초기화되기만 해도 ForceIdle이 됨.
        listener = CancleListener();
        StopCoroutine(listener);
        StartCoroutine(listener);
        //칼을 집어넣음
    }

    IEnumerator CancleListener()
    {
        yield return new WaitForEndOfFrame();
        float count = 0f;
        while (count < 2f)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetMouseButton(0))
            {
                break;
            }
            count += Time.deltaTime;
            yield return null;
        }
        Main_Player.Instance.isAtk = false;
        ResetSword();
    }





    #region 메소드 Load,Reset + Sword, Big

    Vector3 trashposition = new Vector3(0, 100, 0);
    void LoadSword()
    {
        //sword.SetActive(true);
        sword.transform.position = transform.position;
        sword.transform.rotation = transform.rotation;
        idleSword.SetActive(false);
    }
    void ResetSword()
    {
        //sword.SetActive(false);
        sword.transform.position = trashposition;
        idleSword.SetActive(true);
    }
    void LoadBig()
    {
        //bigSword.SetActive(true);
        bigSword.transform.position = transform.position;
        bigSword.transform.rotation = transform.rotation;
        idleBigSword.SetActive(false);
    }
    void ResetBig()
    {
        //bigSword.SetActive(false);
        bigSword.transform.position = trashposition;

        idleBigSword.SetActive(true);
    }
    #endregion

}
