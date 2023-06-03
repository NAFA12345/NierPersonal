using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Em0070Movement : MonoBehaviour
{

    [Header("적 정보")]
    [SerializeField] Animator animator;
    [SerializeField] Vector3 firstDestPos = Vector3.zero;
    [SerializeField] float fireDelay = 1.0f;
    [SerializeField] float moveSpeed = 0.03f;
    [SerializeField] float dashSpeed = 2.0f;
    [SerializeField] float LookSpeed = 60.0f;

    [Space(0.5f)]
    [Header("총알")]
    [SerializeField] GameObject bulletHard;
    [SerializeField] GameObject bulletSoft;
    [SerializeField] float bulletSpeed = 0.20f;

    [Space(0.5f)]
    [Header("확인용")]
    [SerializeField] GameObject playerObject;
    [SerializeField] Transform playerTransform;
    [SerializeField] FlagEmInformation flagEmInformation;
    [SerializeField] FlagFightSpawner flagFightSpawner;

    public UnityEvent Ready;
    public UnityEvent StopSlowSpin;
    public UnityEvent SlowSpin;

    // Start is called before the first frame update
    void Start()
    {
        flagFightSpawner = FindObjectOfType<FlagFightSpawner>();
        flagEmInformation = GetComponent<FlagEmInformation>();
        if (flagEmInformation == null)
        {
            Debug.LogError("FlagEmInformation 컴포넌트를 찾을 수 없습니다.");
        }
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("플레이어 오브젝트를 찾을 수 없습니다.");
        }

        flagFightSpawner.Phase18_01_Alone.AddListener(Alone);
        StartCoroutine(Co_Move());
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0.00f, transform.position.z);
    }

    public IEnumerator Co_Move()
    {
        while (Vector3.SqrMagnitude(transform.position - firstDestPos) >= 0.00005f)
        {

            if (!flagEmInformation.isDie)
            {
                transform.position = Vector3.MoveTowards(transform.position, firstDestPos, moveSpeed * Time.deltaTime);
            }
            yield return null;
        }
        transform.position = firstDestPos;

        StartCoroutine(Co_0070Movement());
        StartCoroutine(Co_Look());
        StartCoroutine(Co_Look());
    }

    public IEnumerator Co_Look()
    {
        while (!flagEmInformation.isDie)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, LookSpeed * Time.deltaTime);
            yield return null;

        }

    }

    IEnumerator Co_0070Movement()
    {
        animator.SetTrigger("Ready");
        Ready.Invoke();

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(Co_FastSpinMovement());
    }

    IEnumerator Co_FastSpinMovement()
    {
        animator.SetTrigger("SpinFast");
        yield return new WaitForSeconds(3.0f);

        animator.SetTrigger("Stay");
        yield return new WaitForSeconds(3.7f);

        StartCoroutine(Co_SlowSpinMovement());

    }

    IEnumerator Co_SlowSpinMovement()
    {
        animator.SetTrigger("SpinSlow");
        SlowSpin.Invoke();
        yield return new WaitForSeconds(5.0f);

        animator.SetTrigger("Stay");
        yield return new WaitForSeconds(3.7f);

        StartCoroutine(Co_DashMovement());
    }

    IEnumerator Co_DashMovement()
    {
        animator.SetTrigger("SpinFast");
        Vector3 playerDirection = (playerTransform.position - transform.position).normalized;
        Vector3 dashDestPos = transform.position + playerDirection * dashSpeed;
        dashDestPos.x = Mathf.Clamp(dashDestPos.x, -0.15f, 0.15f);
        dashDestPos.y = Mathf.Clamp(dashDestPos.y, -0.15f, 0.15f);
        dashDestPos.z = Mathf.Clamp(dashDestPos.z, -0.15f, 0.15f);

        while (Vector3.SqrMagnitude(transform.position - dashDestPos) >= 0.00005f)
        {
            if (!flagEmInformation.isDie)
            {
                transform.position = Vector3.MoveTowards(transform.position, dashDestPos, dashSpeed * Time.deltaTime);
            }
            yield return null;
        }

        transform.position = dashDestPos;

        animator.SetTrigger("Stay");
        StopSlowSpin.Invoke();
        yield return new WaitForSeconds(3.7f);

        StartCoroutine(Co_FastSpinMovement());
    }

    // 혼자 남았을 때
    void Alone()
    {
        StopCoroutine(Co_Move());
        StopCoroutine(Co_0070Movement());
        StopCoroutine(Co_FastSpinMovement());
        StopCoroutine(Co_SlowSpinMovement());
        StopCoroutine(Co_DashMovement());
        StartCoroutine(Fire_co());
    }

    IEnumerator Fire_co()
    {
        while (true)
        {
            GameObject Bullet = bulletSoft;
            int bulletType = Random.Range(0, 4);
            if (bulletType == 0)
            {
                Bullet = bulletHard;
            }
            Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject bullet = Instantiate(Bullet, bulletPosition, transform.rotation);
            Vector3 direction = (playerObject.transform.position - bullet.transform.position).normalized;
            bullet.transform.LookAt(playerObject.transform);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = direction * bulletSpeed;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
