using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    StateManager sm;
    Coroutine co_dodge;
    Coroutine co_atk;

    [SerializeField] private InventoryUI Inventory_UI;
    public delegate void Item(int num);
    public static event Item UseItem;

    [Header("대쉬 입력 대기 시간")]
    public float dashCount;

    private void Start()
    {
        sm = FindObjectOfType<StateManager>();
        co_dodge = StartCoroutine(DodgeListener());
        co_atk = StartCoroutine(AttackListener());
    }
    Vector3 inputVec;
    private void Update()
    {
        if (isCanMove())
        {
            if (sm.currentState != sm.idle)
            {
                sm.ChangeState(sm.idle);
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) // 왼쪽아래 작은 인벤툴팁 열기
        {
            Inventory_UI.InvenActive();
        }
        if (Inventory_UI.isActiveInven)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                UseItem?.Invoke(Inventory_UI.ListNum);
                Inventory_UI.UpdateUI();
            }            
            if (Input.GetKeyDown(KeyCode.K))
            {
                Inventory_UI.UpSelected();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Inventory_UI.DownSelected();
            }

        }
    }


    public bool isCanMove()
    {
        if (!Main_Player.Instance.isAtk && !Main_Player.Instance.isDash && !Main_Player.Instance.isHitted)
        {
            return true;
        }
        else return false;
    }


    #region 공격 리스너
    private IEnumerator AttackListener()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (sm.currentState != sm.atk && Main_Player.Instance.isCanAttack())
                    sm.ChangeState(sm.atk);
            }
            yield return null;
        }
    }
    private IEnumerator BigAttackListener()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (sm.currentState != sm.atk_big && Main_Player.Instance.isCanAttack())
                    sm.ChangeState(sm.atk_big);
            }
            yield return null;
        }
    }
    #endregion
    #region Dash_Dodge 관련
    private IEnumerator DodgeListener()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                yield return DoubleWClick();
            }
            else if (Input.GetKeyDown(KeyCode.A)) { yield return DoubleAClick(); }
            else if (Input.GetKeyDown(KeyCode.S)) { yield return DoubleSClick(); }
            else if (Input.GetKeyDown(KeyCode.D)) { yield return DoubleDClick(); }

            yield return null;
        }
    }
    private IEnumerator DoubleWClick()
    {
        yield return new WaitForEndOfFrame();
        float count = 0f;
        while (count < dashCount)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!Main_Player.Instance.isDash)
                    DashWW();
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator DoubleAClick()
    {
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < dashCount)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!Main_Player.Instance.isDash)
                    DashAA();
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator DoubleSClick()
    {
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < dashCount)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!Main_Player.Instance.isDash)
                    DashSS();
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator DoubleDClick()
    {
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < dashCount)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!Main_Player.Instance.isDash)
                    DashDD();
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region 대쉬인풋관련
    private void DashWW()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 playerForward = transform.forward;

        float angle = Vector3.SignedAngle(cameraForward, playerForward, Vector3.up);
        if (Mathf.Abs(angle) <= 45f)
        {
            sm.ChangeState(sm.dashstates[0]);
        }
        else if (angle > 45f && angle <= 135f)
        {
            sm.ChangeState(sm.dashstates[1]);
        }
        else if (angle > -135f && angle <= -45f)
        {
            sm.ChangeState(sm.dashstates[3]);
        }
        else
        {
            sm.ChangeState(sm.dashstates[2]);
        }
    }

    private void DashAA()
    {
        Vector3 cameraLeft = -Camera.main.transform.right;
        Vector3 playerForward = transform.forward;
        float angle = Vector3.SignedAngle(cameraLeft, playerForward, Vector3.up);
        if (Mathf.Abs(angle) <= 45f)
        {
            sm.ChangeState(sm.dashstates[0]);
        }
        else if (angle > 45f && angle <= 135f)
        {
            sm.ChangeState(sm.dashstates[1]);
        }
        else if (angle > -135f && angle <= -45f)
        {
            sm.ChangeState(sm.dashstates[3]);
        }
        else
        {
            sm.ChangeState(sm.dashstates[2]);
        }
    }

    private void DashSS()
    {
        Vector3 cameraBack = -Camera.main.transform.forward;
        Vector3 playerForward = transform.forward;
        float angle = Vector3.SignedAngle(cameraBack, playerForward, Vector3.up);
        if (Mathf.Abs(angle) <= 45f)
        {
            sm.ChangeState(sm.dashstates[0]);
        }
        else if (angle > 45f && angle <= 135f)
        {
            sm.ChangeState(sm.dashstates[1]);
        }
        else if (angle > -135f && angle <= -45f)
        {
            sm.ChangeState(sm.dashstates[3]);
        }
        else
        {
            sm.ChangeState(sm.dashstates[2]);
        }
    }

    private void DashDD()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 playerForward = transform.forward;
        float angle = Vector3.SignedAngle(cameraRight, playerForward, Vector3.up);
        if (Mathf.Abs(angle) <= 45f)
        {
            sm.ChangeState(sm.dashstates[0]);
        }
        else if (angle > 45f && angle <= 135f)
        {
            sm.ChangeState(sm.dashstates[1]);
        }
        else if (angle > -135f && angle <= -45f)
        {
            sm.ChangeState(sm.dashstates[3]);
        }
        else
        {
            sm.ChangeState(sm.dashstates[2]);
        }
    }

    #endregion
}
