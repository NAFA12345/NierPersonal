using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected enum State
    {
        IDLE,
        WALK,
        ATTACK,
        DASH,
        JUMP
    }

    [Header("Enemy 상태")]
    [SerializeField] protected State state;

    [Header("공격 패턴의 수")]
    [Space(10f)]
    [SerializeField] protected int pattonNum = 4;
    [SerializeField] protected int damage = 4;

    [Header("사정거리 (없으면 0으로)")]
    [Space(10f)]
    [Tooltip("범위 안에 오면 공격해요~")]
    [SerializeField] float attackDistance = 2.8f;
    [Tooltip("범위 밖이면 대쉬해요~")]
    [SerializeField] float dashDistance = 6f;

    [Header("회전속도")]
    [Space(10f)]
    [SerializeField] float rotationSpeed = 10f;


    //체력 정보 받아오기
    protected EnemyHp enemyHp;

    //타겟과의 거리
    protected float distance;

    //공격용 콜라이더
    BoxCollider[] boxCollider;


    //애니메이터
    [HideInInspector] public Animator anim;
    //AnimatorClipInfo[] animatorinfo;

    //타겟 위치
    public Transform target;

    //타겟 위치를 저장하는 변수
    [SerializeField] protected Vector3 targetPosition;


    private void Awake()
    {

        //enemyHp = GetComponentInChildren<EnemyHp>();
        boxCollider = GetComponentsInChildren<BoxCollider>();

        TryGetComponent(out enemyHp);
        TryGetComponent(out anim);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void OnEnable()
    {

        state = State.IDLE;
    }

    //타겟 바라보기
    public void TargetLookat()
    {
        targetPosition = target.position;
        targetPosition.y = 0f;

        //타겟을 쳐다봄
        //대쉬 할 경우에는 바로 쳐다보고, 아닌경우 자연스러운 회전
        if (state == State.DASH)
        {
            transform.LookAt(targetPosition);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //타겟과의 거리를 측정
        distance = Vector3.Distance(transform.position, target.transform.position);

    }

    //정지
    protected virtual IEnumerator OnIdle()
    {
        if (state != State.IDLE)
        {
            Debug.Log("가만히있기");
            state = State.IDLE;

            anim.SetBool("Run", false);
        }

        yield return null;

        if (distance <= attackDistance)
        {
            state = State.ATTACK;
        }
        else if (distance >= dashDistance)
        {
            //대쉬가 없는 경우는 0
            if (dashDistance == 0)
            {
                state = State.WALK;
            }
            else
            {
                state = State.DASH;
            }

        }
        else
        {
            state = State.WALK;
        }
    }

    //걷기
    protected virtual void Onwalk()
    {
        anim.SetBool("Run", true);

        state = State.IDLE;
    }

    //대쉬
    protected virtual IEnumerator OnDash()
    {
        if (state != State.DASH)
        {
            state = State.DASH;
        }

        //대쉬 출력
        anim.SetTrigger("Dash");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Idle") ||
        anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Walk"));

        //애니메이션이 Idle이 되면, 상태도 Idle이 됌
        state = State.IDLE;

    }

    //공격
    protected virtual IEnumerator OnAttack(int PattonNum)
    {
        if (state != State.ATTACK)
        {
            state = State.ATTACK;
        }

        for (int i = 0; i < boxCollider.Length; i++)
        {
            boxCollider[i].enabled = true;
        }

        int value = Random.Range(1, PattonNum + 1);
        anim.SetFloat("Patton", value);
        if (anim.GetFloat("Patton") == 2 || anim.GetFloat("Patton") == 4 || anim.GetFloat("Patton") == 5)
        {
            anim.SetBool("IsAttack", true);
        }


        yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Idle"));
        //yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("em0000_Idle"));

        //패턴 초기화 되고
        anim.SetFloat("Patton", 0);
        anim.SetBool("IsAttack", false);

        //무기 콜라이더 끄고
        for (int i = 0; i < boxCollider.Length; i++)
        {
            boxCollider[i].enabled = false;
        }

        //애니메이션이 Idle이 되면, 상태도 Idle이 됌
        state = State.IDLE;
    }

    //사망
    public virtual IEnumerator Die()
    {
        anim.SetTrigger("DieTrigger");
        anim.SetBool("Die", true);

        if (enemyHp.capsuleCollider != null)
        {
            enemyHp.capsuleCollider.enabled = false;
        }

        yield return new WaitUntil(() => anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f);
        enemyHp.isdead_effect.SetActive(true);

        yield return new WaitForSeconds(3f);
        enemyHp.isdead = false;
        enemyHp.isdead_effect.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Main_Player.Instance.OnDamage(damage);
        }
    }
}




