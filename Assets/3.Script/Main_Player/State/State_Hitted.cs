using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hitted : State
{
    public override void Enter(State before)
    {
        Main_Player.Instance.isHitted = true;
        StartCoroutine(Hitted_co());
    }

    public override void Exit(State next)
    {
        Main_Player.Instance.isHitted = false;
    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {

    }

    float HitNum = 0;
    private IEnumerator Hitted_co()
    {
        Main_Player.Instance.anim_player.SetFloat("HitNum", (HitNum % 2));
        Main_Player.Instance.anim_player.SetBool("Hitted", true);
        Main_Player.Instance.anim_player.SetTrigger("HittedTrigger");
        HitNum++;
        Debug.Log("맞음");
        yield return new WaitForSeconds(0.5f);
        Main_Player.Instance.isHitted = false;
        //todo 히티드 상태일 때, 포스트프로세싱
    }
}
