using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PordControl : MonoBehaviour
{
    [Header("설정을 위해 넣어주세용")]
    [SerializeField] Camera Cam;
    [SerializeField] PordBulletSpawn PordBullet;
    [SerializeField] GameObject PordLaser;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Lockon;
    [SerializeField] GameObject MagicCircle;
    [SerializeField] LaserCoolTime LaserCoolTime;
    [SerializeField] ParticleSystem Smoke;


    // 록온을 위한 변수
    private bool isLockOn = false;
    private bool isMonster = false;
    private Collider target;

    // 록온시 타겟 위치
    private Vector3 targetpos;

    //오브젝트 풀링용 갯수
    private int bulletCount = 0;

    //총알 딜레이용 변수
    private float BulletDealyTime = 0.1f;
    private float CurrentTime = 0f;

    //레이저 딜레이용 변수
    private bool isLaser = false;

    //포드의 위치를 위한 변수
    private Vector3 PlayerAround = new Vector3(1, 1, 0);
    private Vector3 TopPosition = new Vector3(0, 1.8f, 0);
    private Vector3 MagicCirclePositon = new Vector3(0, 0, 0.4f);

    //포드의 움직임 제어용 변수
    private bool isActive = false;

    //포드의 총위치를 위한 변수
    Vector3 ScreenCenter;

    private void Update()
    {

        if (transform.position.y < (Player.transform.position + TopPosition).y && !isActive)
        {
            transform.position += Vector3.up * 0.2f * Time.deltaTime;
        }

        ScreenCenter = Camera.main.ScreenToWorldPoint(new Vector3(960, 0, 840));
        LaserCoolTime.transform.rotation = Cam.transform.rotation;
        MagicCircle.transform.rotation = Cam.transform.rotation;

        // ------------------------인풋 ----------------------------
        if (Input.GetKey(KeyCode.RightShift) && !isLaser ||
            Input.GetKey(KeyCode.LeftShift) && !isLaser) //레이저와 동시에 나가는것 방지
        {
            CurrentTime += Time.deltaTime;
            if (BulletDealyTime > CurrentTime)
            {
                return; //딜레이 시간동안은 총알을 못쏘도록
            }

            isActive = true;
            transform.position = new Vector3(transform.position.x, Player.transform.position.y + PlayerAround.y, transform.position.z);

            MagicCircle.transform.position = transform.position + MagicCirclePositon;
            MagicCircle.SetActive(true);

            PordBullet.Bullet[bulletCount].transform.position = transform.position + MagicCirclePositon;
            PordBullet.Bullet[bulletCount].SetActive(true);

            Smoke.Play();
            Smoke.transform.position = transform.position + MagicCirclePositon;



            if (isLockOn) //록온시 타겟방향으로
            {
                PordBullet.Bullet[bulletCount].GetComponent<PordBulletMovement>().Move((targetpos - PordBullet.Bullet[bulletCount].transform.position).normalized);
            }
            else //록온이 아닐시 앞으로
            {

                PordBullet.Bullet[bulletCount].GetComponent<PordBulletMovement>().Move(new Vector3(ScreenCenter.x, transform.position.y, ScreenCenter.z).normalized);
                //방향 조정 필요 임시로 넣어뒀음
            }

            //소리를 여기에 넣어주세용
            Invoke("ActiveFalse", 0.3f);
            bulletCount++;
            if (bulletCount >= 60) // BulletSpawn 에서 생성한 갯수만큼
            {
                bulletCount = 0;
            }
            CurrentTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) ||
            Input.GetKeyUp(KeyCode.RightShift))  // 키를 뗐을 때 마법진 사라지게
        {
            MagicCircle.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Q)) // 록온
        {

            if (isLockOn) //록온일시 록온해제
            {
                isLockOn = false;
                Lockon.SetActive(false);
            }
            else if (isMonster) //록온이 아닐시 , 주변에 몬스터가있는(록온가능상태)라면 록온
            {
                isLockOn = true;
                Lockon.SetActive(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && LaserCoolTime.CanLaser) // 1입력시 레이저 쿨타임이 아니라면
        {

            StartCoroutine(Laser_co());
            LaserCoolTime.gameObject.SetActive(true);


        }
        if (!isLaser)
        {

            transform.position = new Vector3(Player.transform.position.x + PlayerAround.x,
                                             transform.position.y,
                                             Player.transform.position.z + PlayerAround.z);
            //transform.position = new Vector3(Player.transform.position.x + Cam.transform.rotation.x,
            //                                 transform.position.y,
            //                                 Player.transform.position.z + Cam.transform.rotation.z);
            transform.rotation = Cam.transform.rotation;
        }


    }

    private IEnumerator Laser_co() // 레이저 사용시 코루틴
    {
        //카메라를 살짝 흔들어주세용
        //소리도 여기에 넣어주세용
        transform.position = new Vector3(transform.position.x, Player.transform.position.y + PlayerAround.y, transform.position.z);
        isActive = true;
        isLaser = true;
        PordLaser.SetActive(true);
        PordLaser.transform.position = transform.position + MagicCirclePositon;


        MagicCircle.SetActive(true);
        MagicCircle.transform.position = transform.position + MagicCirclePositon;
        MagicCircle.transform.rotation = Cam.transform.rotation;

        if (isLockOn) //록온시 타겟 에게 방향 설정
        {
            PordLaser.transform.LookAt(targetpos);
        }
        else //록온이 아닐시 앞으로 나가게
        {
            Vector3 LaserLotation = Cam.transform.rotation.eulerAngles;
            LaserLotation = new Vector3(0, LaserLotation.y, 0);
            PordLaser.transform.rotation = Quaternion.Euler(LaserLotation);

        }

        yield return new WaitForSeconds(1f);
        // 레이저가 끝난 이후 
        PordLaser.SetActive(false);
        MagicCircle.SetActive(false);
        isLaser = false;
        ActiveFalse();
    }
    private void OnTriggerStay(Collider other) // 범위안에 들어왔을시 록온 가능설정, 록온시 타겟위치 설정
    {
        if (other.CompareTag("Enemy"))
        {
            isMonster = true;
            target = other;
            if (isLockOn && other == target)
            {
                targetpos = other.transform.position;
            }
        }

    }
    private void OnTriggerExit(Collider other) // 범위 밖으로 나갈시 록온 해제
    {
        if (target != null)
        {

            if ((other == target && other.CompareTag("Enemy")) || !target.gameObject.activeSelf)
            {
                Debug.Log("몬스터 빠져나갔당");
                isMonster = false;
                isLockOn = false;
                Lockon.SetActive(false);
                target = null;
            }

        }
    }


    public void ActiveFalse()
    {
        isActive = false;
    }
}
