using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Atk : State
{
    float lastAtkTime;
    float strongCount;
    StateManager sm;
    bool isStrongAtk;

    public override void Enter(State before)
    {
        isStrongAtk = false;
        Main_Player.Instance.isAtk = true;
        Main_Player.Instance.anim_player.applyRootMotion = true;
        Attack1();
    }

    public override void Exit(State next)
    {
        Main_Player.Instance.anim_player.applyRootMotion = false;
        Main_Player.Instance.collider_sword.enabled = false;
        Main_Player.Instance.anim_sword.SetTrigger("Idle");
        Main_Player.Instance.anim_bigsword.SetTrigger("Idle");
    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {

    }

    //private IEnumerator StrongAttack_co()
    //{
    //    isStrongAtk = true;
    //    Main_Player.Instance.anim_player.SetTrigger("AtkStrong");
    //    yield return new WaitForSeconds(2f);
    //    Main_Player.Instance.isAtk = false;
    //}
    //private void StrongAttack()
    //{
    //    StartCoroutine(StrongAttack_co());
    //}
    private IEnumerator Attack_co()
    {
        lastAtkTime = Time.time;
        Main_Player.Instance.anim_player.SetTrigger("Atk1");
        Main_Player.Instance.anim_sword.SetTrigger("Atk1");
        Main_Player.Instance.collider_sword.enabled = true;
        yield return AtkListener(2);
        Main_Player.Instance.collider_sword.enabled = false;
        yield return CancleListener();
        Main_Player.Instance.isAtk = false;
    }

    private IEnumerator CancleListener()
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
    }
    private IEnumerator AtkListener(int i)
    {
        yield return new WaitForEndOfFrame();
        if (i > 5)
        {
            yield return new WaitForSeconds(2f);
            yield break;
        }
        float count = 0f;
        while (count < 2f)
        {
            if (count > 0.5f && Main_Player.Instance.isCanAttack())
            {
                if (!isStrongAtk && Input.GetMouseButtonDown(0)) //АјАн
                {
                    lastAtkTime = Time.time;
                    Main_Player.Instance.collider_sword.enabled = true;
                    Main_Player.Instance.anim_player.SetTrigger("Atk" + i);
                    Main_Player.Instance.anim_sword.SetTrigger("Atk" + i);
                    yield return AtkListener(i + 1);
                    Main_Player.Instance.collider_sword.enabled = false;
                    break;
                }
            }
            count += Time.deltaTime;
            yield return null;  
        }
        yield break;
    }
    public void Attack1()
    {
        StartCoroutine(Attack_co());
    }
        
}
