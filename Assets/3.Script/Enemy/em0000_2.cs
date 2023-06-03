using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class em0000_2 : Enemy
{
    [Header("Enemy 총알 관련")]
    [Space(10f)]
    [SerializeField] GameObject Bullet_Prefab;
    [Tooltip("미리 만들어둘 총알 갯수")]
    [SerializeField] int bullet_count;
    GameObject[] Bullet_Soft;
    [SerializeField] float bullet_speed = 1.0f;
    [SerializeField] float bullet_reload = 0.5f;

    [Header("발사할 위치")]
    [Space(10f)]
    [SerializeField] Transform Cannon_pos;

    int count = 0;
    float timer = 0f;

    void Start()
    {
        Bullet_Soft = new GameObject[bullet_count];

        if (Cannon_pos == null)
        {
            Debug.Log("Cannon_pos 할당 안되어있다. ");
            Cannon_pos = transform.Find("bone-1/bone4094/bone000/bone001/em0000_wp/bone-1/bone000/bone3841");
        }

        Initialized_Bullet();

    }

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

            BulletAttack();

            yield return null;
        }

    }

    void BulletAttack()
    {
        timer += Time.deltaTime;

        if (timer > bullet_reload && !anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Hit"))
        {
            if (count >= Bullet_Soft.Length)
            {
                count = 0;
            }

            //Vector3 Can_pos = Cannon_pos.position;
            //Can_pos.y = 1f;

            Bullet_Soft[count].SetActive(true);
            Bullet_Soft[count].transform.position = Cannon_pos.position;

            Vector3 direction = (target.transform.position - Bullet_Soft[count].transform.position).normalized;

            //Bullet_Soft[count].transform.LookAt(target.transform);

            Rigidbody bulletRigidbody = Bullet_Soft[count].GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = direction * bullet_speed;
            }
            count++;
            timer = 0f;
        }
    }


    void Initialized_Bullet()
    {
        for (int i = 0; i < Bullet_Soft.Length; i++)
        {
            GameObject bullet = Instantiate(Bullet_Prefab);
            Bullet_Soft[i] = bullet;
            bullet.SetActive(false);
        }
    }
}
