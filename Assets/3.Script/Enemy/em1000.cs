using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class em1000 : Enemy
{
    enum Dir
    {
        Left,
        Center,
        Right
    }

    [Header("톱 관련")]
    [Space(10f)]
    [SerializeField] GameObject saw;
    [Range(1f, 700f)]
    [SerializeField] float maxsawrotateSpeed = 700f;
    [Range(1f, 700f)]
    [SerializeField] float minsawrotateSpeed = 150f;
    [SerializeField] float speed_down_time = 5f;
    [SerializeField] bool isattack = false;

    private float target_x;
    private float currentSawRotateSpeed;

    bool allattack;
    bool allattack2;

    Dir dir;

    void Start()
    {
        if (saw == null)
        {
            Debug.Log("saw 할당 안되어있다. ");
            saw = GameObject.Find("bone-1/bone000/bone001/bone002/bone003/bone004/bone005/bone007/bone008/bone009");
        }


        currentSawRotateSpeed = maxsawrotateSpeed;

        anim.SetTrigger("Intro");
        StartCoroutine(Attack());

    }

    private void FixedUpdate()
    {
        //톱날 회전
        Saw();

        TargetCheck();

    }

    IEnumerator Attack()
    {

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(All)go"));
        yield return null;

        while (true)
        {
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f);
            anim.SetFloat("Random", Random.Range(0, 2));
            isattack = false;


            switch (dir)
            {
                case Dir.Left:
                    anim.SetBool("Left", true);
                    anim.SetBool("Center", false);
                    anim.SetBool("Right", false);
                    break;

                case Dir.Center:
                    anim.SetBool("Left", false);
                    anim.SetBool("Center", true);
                    anim.SetBool("Right", false);
                    break;

                case Dir.Right:
                    anim.SetBool("Left", false);
                    anim.SetBool("Center", false);
                    anim.SetBool("Right", true);
                    break;
            }

            //yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Attack(All)go"));
            yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack"));
            isattack = true;
            yield return null;
        }
    }

    


    //좌우 판단
    void TargetCheck()
    {
        target_x = target.root.position.x;

        if (target_x > 5f)
        {
            dir = Dir.Right;
        }
        else if (target_x <= 5f && target_x > -15f)
        {
            dir = Dir.Center;
        }
        else if (target_x <= -15f)
        {
            dir = Dir.Left;
        }

    }

    //톱날 공격
    void Saw()
    {
        if (!enemyHp.isdead)
        {
            //톱날 회전
            saw.transform.Rotate(-currentSawRotateSpeed * Time.deltaTime, 0, 0);

            //최고속도이고, 공격이 실행되면, 톱 회전속도가 천천히 느려지게
            if (currentSawRotateSpeed == maxsawrotateSpeed && isattack)
            {
                StartCoroutine(DecreaseRotationSpeed());
            }
            else if (!isattack)
            {
                currentSawRotateSpeed = maxsawrotateSpeed;
            }
        }
    }

    //톱 회전속도 느리게하기
    IEnumerator DecreaseRotationSpeed()
    {
        float Timer = 0f;
        float startSpeed = currentSawRotateSpeed;

        //2초 정도 속도 줄어듬
        while (Timer < speed_down_time)
        {
            Timer += Time.deltaTime;

            currentSawRotateSpeed = Mathf.Lerp(startSpeed, minsawrotateSpeed, Timer / speed_down_time);
            yield return null;
        }

        currentSawRotateSpeed = minsawrotateSpeed;
    }

    public override IEnumerator Die()
    {
        anim.SetTrigger("DieTrigger");
        anim.SetBool("Die", true);

        if (enemyHp.capsuleCollider != null)
        {
            enemyHp.capsuleCollider.enabled = false;
        }

        //yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("SawUp"));
        enemyHp.isdead_effect.SetActive(true);

        yield return new WaitForSeconds(3f);
        enabled = false;
        //StopCoroutine(Attack());
        //enemyHp.isdead = false;
    }
}
