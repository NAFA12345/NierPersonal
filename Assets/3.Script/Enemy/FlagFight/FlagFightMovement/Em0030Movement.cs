using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Em0030Movement : MonoBehaviour
{
    [Header("적 0030 정보")]
    [SerializeField] float rotateSpeed = 60.0f; 
    [SerializeField] float fireDelay = 3.0f;

    [Space(0.5f)]
    [Header("총알")]
    [SerializeField] GameObject bulletHard;
    [SerializeField] GameObject bulletSoft;
    [SerializeField] float bulletSpeed = 0.3f;

    [Space(0.5f)]
    [Header("Enemy Spawner에서 정해주어야 할 것")]
    [SerializeField] public float firstMoveSpeed = 2.0f;
    [SerializeField] public float lastMoveSpeed = 1.0f;
    [SerializeField] public Vector3 desPos;
    [SerializeField] public Vector3 lastDesPos;
    [SerializeField] public bool isCanLook;
    [SerializeField] public bool isReady = false;

    [Space(0.5f)]
    [Header("확인용")]
    [SerializeField] float fireTimer;
    [SerializeField] GameObject playerObject;
    [SerializeField] Transform playerTransform;
    [SerializeField] FlagEmInformation flagEmInformation;
    [SerializeField] float speed;

    /*start는 확인용이니, 에너미 스폰 생성시 삭제할것.*/
    private void Start()
    {
        fireTimer = 0.0f;
        StartCoroutine(Move_co());
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("플레이어 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void OnEnable()
    {
        fireTimer = 0.0f;
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        // 죽거나 준비되지 않았으면 return
        if (!isReady || flagEmInformation.isDie)
        {
            return;
        }
        // 플레이어를 처다볼 수 있다면 회전
        if (playerTransform != null && isCanLook)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        if (fireDelay >= fireTimer)
        {
            return;
        }
        fireTimer = 0.0f;
        StartCoroutine(Fire_co());
    }

    public IEnumerator Move_co()
    {
        speed = firstMoveSpeed;
        while (Vector3.SqrMagnitude(transform.position - desPos) >= 0.00005f)    
        {
            if (!flagEmInformation.isDie)
            {
                if (0.35f < transform.position.x || -0.35f > transform.position.x
                    || 0.35f < transform.position.y || -0.35f > transform.position.y
                    || 0.35f < transform.position.z || -0.35f > transform.position.z)
                {
                    transform.position = Vector3.zero;
                    flagEmInformation.Disappear();
                }
                transform.position = Vector3.MoveTowards(transform.position, desPos, speed * Time.deltaTime);
            }
            yield return null;
        }
        transform.position = desPos;
        desPos = lastDesPos;
        speed = lastMoveSpeed;
        isReady = true;
        while (Vector3.SqrMagnitude(transform.position - desPos) >= 0.00005f)
        {
            if (!flagEmInformation.isDie)
            {
                if (0.35f < transform.position.x || -0.35f > transform.position.x
                    || 0.35f < transform.position.y || -0.35f > transform.position.y
                    || 0.35f < transform.position.z || -0.35f > transform.position.z)
                {
                    transform.position = Vector3.zero;
                    flagEmInformation.Disappear();
                }
                transform.position = Vector3.MoveTowards(transform.position, desPos, speed * Time.deltaTime);
            }
            yield return null;
        }
    }

    private IEnumerator Fire_co()
    {
        for (int i = 0; i < 5; i++)
        {
        GameObject Bullet = bulletSoft;
        int bulletType = Random.Range(0, 4);
        if (bulletType == 0)
        {
            Bullet = bulletHard;
        }
        Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y + 0.009f, transform.position.z);
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
