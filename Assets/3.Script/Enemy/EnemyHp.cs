using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    [Header("Enemy 체력 관련")]
    [SerializeField] private float MaxHP;
    [SerializeField] private float CurrentHP;
    [Tooltip("이정도 이상의 데미지면 아파함")]
    [SerializeField] private float MinDamage;
    [SerializeField] private Slider enemyHPBar;
    [SerializeField] private Canvas enemyUI;


    [Header("Enemy 사망 관련")]
    [Space(10f)]
    public bool isdead = false;
    public GameObject isdead_effect;

    //피격용 콜라이더
    public CapsuleCollider capsuleCollider;

    //스크립트
    Enemy enemy;

    Camera camera;

    //체력 프로퍼티
    public float maxHp => MaxHP;
    public float currentHp
    {
        get { return CurrentHP; }
        set
        {
            CurrentHP = value;

            if (CurrentHP <= 0f && !isdead)
            {
                isdead = true;
                StartCoroutine(enemy.Die());
            }
        }
    }

    private int HitNum;
    public int hitNum
    {
        get { return HitNum; }
        set
        {
            HitNum = value;

            if (value >= 4)
            {
                HitNum = 1;
            }
        }
    }


    void Awake()
    {
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();

        TryGetComponent(out enemy);

        camera = Camera.main;
    }
    void OnEnable()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (enemyHPBar != null)
        {
            //Vector3 screenPosition = camera.WorldToScreenPoint(transform.position);
            //enemyHPBar.transform.position = screenPosition + new Vector3(0, 150f, 0);
            enemyUI.transform.LookAt(camera.transform);
            enemyHPBar.value = currentHp / maxHp;
        }


    }

    //데미지를 받을 때
    public void TakeDamage(int Damage)
    {

        //앞 뒤에 있는지 확인
        Vector3 targetDirection = enemy.target.transform.position - transform.position;
        Vector3 forwardDirection = transform.forward;
        bool isTargetInFront = Vector3.Dot(targetDirection, forwardDirection) > 0;
        enemy.anim.SetBool("FrontPlayer", isTargetInFront);

        currentHp -= Damage;

        //이 데미지 이상이면 충격받아요~
        if (Damage >= MinDamage )
        {
            hitNum++;
            enemy.anim.SetFloat("HitNum", hitNum);
            enemy.anim.SetTrigger("Hit");

            if (enemy.anim.GetFloat("Patton") == 2 || enemy.anim.GetFloat("Patton") == 4 || enemy.anim.GetFloat("Patton") == 5 || enemy.anim.GetBool("Attack2"))
            {
                enemy.anim.ResetTrigger("Hit");
            }
        }

    }

    ////사망
    //IEnumerator Die()
    //{
    //    enemy.anim.SetTrigger("DieTrigger");
    //    enemy.anim.SetBool("Die", true);

    //    if (capsuleCollider != null)
    //    {
    //        capsuleCollider.enabled = false;
    //    }

    //    yield return new WaitUntil(() => enemy.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Die") && enemy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f);
    //    isdead_effect.SetActive(true);

    //    yield return new WaitForSeconds(3f);
    //    isdead = false;
    //    isdead_effect.SetActive(false);
    //    gameObject.SetActive(false);
    //}
}
