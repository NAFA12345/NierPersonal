using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class em0000 : Enemy
{
    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (!enemyHp.isdead)
        {
            TargetLookat();

            switch (state)
            {
                case State.IDLE:
                    yield return StartCoroutine(OnIdle());
                    break;

                case State.WALK:
                    Onwalk();
                    break;

                case State.ATTACK:
                    yield return StartCoroutine(OnAttack(pattonNum));
                    break;

                case State.DASH:
                    yield return StartCoroutine(OnDash());
                    break;
            }
            TargetLookat();
            yield return null;
        }

    }

    protected override IEnumerator OnAttack(int PattonNum)
    {
        state = State.ATTACK;

        int random = Random.Range(1, PattonNum + 1);

        switch (random)
        {
            case 1:
                yield return StartCoroutine(WindmillAttack2());
                break;

            case 2:
                yield return StartCoroutine(PunchAttact2());
                break;
        }
        state = State.IDLE;
    }

    IEnumerator WindmillAttack2()
    {
        anim.SetBool("Attack2", true);
        yield return new WaitForSeconds(4f);
        anim.SetBool("Attack2", false);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("em0000_Idle"));
    }

    IEnumerator PunchAttact2()
    {
        anim.SetTrigger("Attack1");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("em0000_Idle"));
    }

}
