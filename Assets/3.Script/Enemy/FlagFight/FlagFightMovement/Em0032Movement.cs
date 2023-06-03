using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Em0032Movement : MonoBehaviour
{
    [Header("�� 0032 ����")]
    [SerializeField] float rotateSpeed = 60.0f;
    [SerializeField] float fireDelay = 1.0f;

    [Space(0.5f)]
    [Header("�Ѿ�")]
    [SerializeField] GameObject bulletHard;
    [SerializeField] GameObject bulletSoft;
    [SerializeField] float bulletSpeed = 0.3f;

    [Space(0.5f)]
    [Header("Enemy Spawner���� �����־�� �� ��")]
    [SerializeField] public Vector3 RotatePoint;
    [SerializeField] public Vector3 RotateAxis = Vector3.up;
    [SerializeField] public Vector3 desPos;
    [SerializeField] public float firstMoveSpeed = 2.0f;
    [SerializeField] public float lastMoveSpeed = 30.0f;
    [SerializeField] public bool isCanLook;
    [SerializeField] public bool isReady = false;

    [Space(0.5f)]
    [Header("Ȯ�ο�")]
    [SerializeField] float fireTimer;
    [SerializeField] GameObject playerObject;
    [SerializeField] Transform playerTransform;
    [SerializeField] FlagEmInformation flagEmInformation;
    [SerializeField] float speed; 

    /*start�� Ȯ�ο��̴�, ���ʹ� ���� ������ �����Ұ�.*/
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
            Debug.LogError("�÷��̾� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void OnEnable()
    {
        fireTimer = 0.0f;
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        // �װų� �غ���� �ʾ����� return
        if (!isReady || flagEmInformation.isDie)
        {
            return;
        }
        // �÷��̾ ó�ٺ� �� �ִٸ� ȸ��
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
        Fire();
    }

    public IEnumerator Move_co()
    {
        speed = firstMoveSpeed;
        Quaternion startRotation = transform.rotation;
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
                if(!isCanLook)
                {
                    transform.rotation = startRotation;
                }
            }
            yield return null;
        }
        transform.position = desPos;
        speed = lastMoveSpeed;
        isReady = true;
        while (!flagEmInformation.isDie)
        {
            if (0.35f < transform.position.x || -0.35f > transform.position.x
                || 0.35f < transform.position.y || -0.35f > transform.position.y
                || 0.35f < transform.position.z || -0.35f > transform.position.z)
            {
                transform.position = Vector3.zero;
                flagEmInformation.Disappear();
            }
            transform.RotateAround(RotatePoint, RotateAxis, speed * Time.deltaTime);
            if (!isCanLook)
            {
                transform.rotation = startRotation;
            }
            yield return null;
        }
    }

    private void Fire()
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
    }
}