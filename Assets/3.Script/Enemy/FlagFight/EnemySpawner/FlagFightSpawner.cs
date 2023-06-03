using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;

public class FlagFightSpawner : MonoBehaviour
{
    [Header("Enemy를 넣어주세요.")]
    public GameObject[] em0030s;
    public GameObject[] em0031s;
    public GameObject[] em0032s;
    public GameObject em0070;

    [Header("Managers")]
    [SerializeField] FlagFightSubTitleManager flagFightSubTitleManager;

    public FlagEmInformation FlagPrefab;
    private Vector3 emPosition;
    private Vector3 emRotation;
    private Vector3 emFirstDesPosition;
    private Vector3 emLastDesPosition;
    private Vector3 emRotatePoint;
    private Vector3 emRotateAxis;
    private float emfirstMoveSpeed;
    private float emlastMoveSpeed;
    private bool isCanLook;
    

    // 적이 없으면 이벤트를 발생시킴
    public int RemainEnemies;
    private bool isAlone = true;

    #region 발생시킬 이벤트들
    public UnityEvent phase1_15_EMDie; // phase2 시작
    public UnityEvent phase2_01_EMDie; // phase3 시작
    public UnityEvent phase3_04_EMDie; // phase4 시작 
    public UnityEvent phase4_01_EMDie; // Phase5 시작
    public UnityEvent Phase5_02_EMDie; // Phase6 시작
    public UnityEvent Phase6_01_EMDie; // Phase7 시작
    public UnityEvent Phase7_06_EMDie; // Phase8 시작
    public UnityEvent Phase8_01_EMDie; // Phase9 시작
    public UnityEvent Phase9_01_EMDie; // Phase10 시작
    public UnityEvent Phase10_01_EMDie; // Phase11 시작
    public UnityEvent Phase11_01_EMDie; // Phase12 시작
    public UnityEvent Phase12_01_EMDie; // Phase13 시작
    public UnityEvent Phase13_01_EMDie; // Phase14 시작
    public UnityEvent Phase14_01_EMDie; // Phase15 시작
    public UnityEvent Phase15_01_EMDie; // Phase16 시작
    public UnityEvent Phase16_01_EMDie; // Phase17 시작
    public UnityEvent Phase17_01_EMDie; // Phase18 시작
    public UnityEvent Phase18_01_EMDie; // Phase19 시작

    public UnityEvent Phase18_01_Alone; // 중간보스 팔이 다 떨어짐

    #endregion

    #region WaitForSeconds 모음
    private WaitForSeconds wait_half_Second = new WaitForSeconds(0.5f);
    private WaitForSeconds wait_1_Second = new WaitForSeconds(1.0f);

    #endregion

    private void Awake()
    {
        #region 이벤트 add
        flagFightSubTitleManager.phase1_15.AddListener(Phase1_15);
        flagFightSubTitleManager.phase2_01.AddListener(Phase2_01);
        flagFightSubTitleManager.phase3_04.AddListener(Phase3_04);
        flagFightSubTitleManager.phase4_01.AddListener(Phase4_01);
        flagFightSubTitleManager.phase5_02.AddListener(Phase5_02);
        flagFightSubTitleManager.phase6_01.AddListener(Phase6_01);
        flagFightSubTitleManager.phase7_06.AddListener(Phase7_06);

        Phase7_06_EMDie.AddListener(Phase8_01);
        Phase8_01_EMDie.AddListener(Phase9_01);
        Phase9_01_EMDie.AddListener(Phase10_01);
        Phase10_01_EMDie.AddListener(Phase11_01);
        Phase11_01_EMDie.AddListener(Phase12_01);
        Phase12_01_EMDie.AddListener(Phase13_01);
        Phase13_01_EMDie.AddListener(Phase14_01);
        Phase14_01_EMDie.AddListener(Phase15_01);
        Phase15_01_EMDie.AddListener(Phase16_01);
        Phase16_01_EMDie.AddListener(Phase17_01);
        Phase17_01_EMDie.AddListener(Phase18_01);
        #endregion
    }

    // Em0030 생성 메소드
    private void SpawnEm0030(Vector3 emRotation, Vector3 emPosition, Vector3 emFirstDesPosition, Vector3 emLastDesPosition, float emfirstMoveSpeed, float emlastMoveSpeed, bool isCanLook, int num)
    {
        em0030s[num].SetActive(true);
        em0030s[num].transform.rotation = Quaternion.Euler(emRotation);
        em0030s[num].transform.position = emPosition;
        Em0030Movement em0030Movement = em0030s[num].GetComponent<Em0030Movement>();
        em0030Movement.firstMoveSpeed = emfirstMoveSpeed;
        em0030Movement.lastMoveSpeed = emlastMoveSpeed;
        em0030Movement.desPos = emFirstDesPosition;
        em0030Movement.lastDesPos = emLastDesPosition;
        em0030Movement.isCanLook = isCanLook;
        em0030Movement.isReady = false;
        StopCoroutine(em0030Movement.Move_co());
        StartCoroutine(em0030Movement.Move_co());
    }

    // Em0031 생성 메소드
    private void SpawnEm0031(Vector3 emRotation, Vector3 emPosition, Vector3 emFirstDesPosition, int num)
    {
        em0031s[num].SetActive(true);
        em0031s[num].transform.rotation = Quaternion.Euler(emRotation);
        em0031s[num].transform.position = emPosition;
        Em0031Movement em0031Movement = em0031s[num].GetComponent<Em0031Movement>();
        em0031Movement.desPos = emFirstDesPosition;
        StopCoroutine(em0031Movement.Move_co());
        StartCoroutine(em0031Movement.Move_co());
    }

    // Em0032 생성 메소드
    private void SpawnEm0032(Vector3 emRotation, Vector3 emPosition, Vector3 emFirstDesPosition, Vector3 emRotatePoint, Vector3 emRotateAxis, float emfirstMoveSpeed, float emlastMoveSpeed, bool isCanLook, int num)
    {
        em0032s[num].SetActive(true);
        em0032s[num].transform.rotation = Quaternion.Euler(emRotation);
        em0032s[num].transform.position = emPosition;
        Em0032Movement em0032Movement = em0032s[num].GetComponent<Em0032Movement>();
        em0032Movement.desPos = emFirstDesPosition;
        em0032Movement.RotatePoint = emRotatePoint;
        em0032Movement.RotateAxis = emRotateAxis;
        em0032Movement.firstMoveSpeed = emfirstMoveSpeed;
        em0032Movement.lastMoveSpeed = emlastMoveSpeed;
        em0032Movement.isCanLook = isCanLook;
        em0032Movement.isReady = false;
        StopCoroutine(em0032Movement.Move_co());
        StartCoroutine(em0032Movement.Move_co());

    }

    // em0030 4기를 생성 // 0, 1, 2, 3
    public void Phase1_15()
    {
        RemainEnemies = 4;
        emRotation = new Vector3(0, -180.0f, 0);
        emfirstMoveSpeed = 0.05f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        emPosition = new Vector3(-0.15f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(-0.15f, 0.00f, 0.07f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 0);

        emPosition = new Vector3(-0.05f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(-0.05f, 0.00f, 0.10f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 1);

        emPosition = new Vector3(0.05f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(0.05f, 0.00f, 0.10f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 2);

        emPosition = new Vector3(0.15f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(0.15f, 0.00f, 0.07f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 3);

        StartCoroutine(Co_CheckNumEm01_15());

    }

    IEnumerator Co_CheckNumEm01_15()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                phase1_15_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0032 10기를 생성 // 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
    public void Phase2_01()
    {
        RemainEnemies = 10;
        emRotation = new Vector3(0, -180.0f, 0);
        emRotateAxis = Vector3.up;
        emfirstMoveSpeed = 0.05f;
        emlastMoveSpeed = 8.00f;
        isCanLook = true;

        StartCoroutine(Co_Phase2_01());
    }

    IEnumerator Co_Phase2_01()
    {
        for (int i = 0; i < 10; i += 2)
        {
            emPosition = new Vector3(-0.25f, 0.00f, 0.30f);
            emFirstDesPosition = new Vector3(-0.20f, 0.00f, 0.20f);
            emRotatePoint = new Vector3(0.30f, 0.00f, 0.725f);
            emRotateAxis = Vector3.down;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i);
            yield return wait_half_Second;

            emPosition = new Vector3(0.25f, 0.00f, 0.30f);
            emFirstDesPosition = new Vector3(0.20f, 0.00f, 0.20f);
            emRotatePoint = new Vector3(-0.30f, 0.00f, 0.725f);
            emRotateAxis = Vector3.up;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i + 1);
            yield return wait_half_Second;
        }
        StartCoroutine(Co_CheckNumEm02_01());
    }

    IEnumerator Co_CheckNumEm02_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                phase2_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0030 6기를 생성 // 4, 5, 6, 7, 8, 9
    public void Phase3_04()
    {
        RemainEnemies = 6;
        emRotation = new Vector3(0, -180.0f, 0);
        emfirstMoveSpeed = 0.05f;
        isCanLook = true;

        StartCoroutine(Co_Phase3_04());
    }

    IEnumerator Co_Phase3_04()
    {
        for (int i = 4; i < 10; i += 2)
        {
            emPosition = new Vector3(-0.30f, 0.00f, 0.10f);
            
            emFirstDesPosition = new Vector3(-0.28f, 0.00f, 0.10f);
            emLastDesPosition = new Vector3(0.50f, 0.00f, 0.16f);
            SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, i);
            yield return wait_half_Second;

            emPosition = new Vector3(0.30f, 0.00f, 0.10f);
            emFirstDesPosition = new Vector3(0.28f, 0.00f, 0.10f);
            emLastDesPosition = new Vector3(-0.50f, 0.00f, 0.04f);
            SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, i+1);
            yield return wait_half_Second;
        }

        StartCoroutine(Co_CheckNumEm03_04());

    }

    IEnumerator Co_CheckNumEm03_04()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                phase3_04_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0030 4기를 생성 // 0, 1, 2, 3
    public void Phase4_01()
    {
        RemainEnemies = 4;
        emRotation = new Vector3(0.00f, 180.0f, 0.00f);
        emfirstMoveSpeed = 0.3f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        emPosition = new Vector3(-0.10f, 0.00f, -0.30f);
        emFirstDesPosition = new Vector3(-0.20f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 0);

        emPosition = new Vector3(-0.10f, 0.00f, -0.30f);
        emFirstDesPosition = new Vector3(-0.10f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 1);

        emPosition = new Vector3(0.10f, 0.00f, -0.30f);
        emFirstDesPosition = new Vector3(0.10f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 2);

        emPosition = new Vector3(0.10f, 0.00f, -0.30f);
        emFirstDesPosition = new Vector3(0.20f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 3);

        StartCoroutine(Co_CheckNumEm04_01());
    }

    IEnumerator Co_CheckNumEm04_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                phase4_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0030 8기를 생성 // 10, 11, 12, 13, 14, 15, 16, 17
    public void Phase5_02()
    {
        RemainEnemies = 8;
        emfirstMoveSpeed = 0.0053f;
        emlastMoveSpeed = 0.0f;
        isCanLook = true;

        emRotation = new Vector3(0.0f, 210.0f, 0.0f);
        emPosition = new Vector3(0.15f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(0.06f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 10);

        emRotation = new Vector3(0.0f, 240.0f, 0.0f);
        emPosition = new Vector3(0.30f, 0.00f, 0.15f);
        emFirstDesPosition = new Vector3(0.12f, 0.00f, 0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 11);

        emRotation = new Vector3(0.0f, 300.0f, 0.0f);
        emPosition = new Vector3(0.30f, 0.00f, -0.15f);
        emFirstDesPosition = new Vector3(0.12f, 0.00f, -0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 12);

        emRotation = new Vector3(0.0f, 330.0f, 0.0f);
        emPosition = new Vector3(0.15f, 0.00f, -0.30f);
        emFirstDesPosition = new Vector3(0.06f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 13);

        emRotation = new Vector3(0.0f, 30.0f, 0.0f);
        emPosition = new Vector3(-0.15f, 0.00f, -0.30f);
        emFirstDesPosition = new Vector3(-0.06f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 14);

        emRotation = new Vector3(0.0f, 60.0f, 0.0f);
        emPosition = new Vector3(-0.30f, 0.00f, -0.15f);
        emFirstDesPosition = new Vector3(-0.12f, 0.00f, -0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 15);

        emRotation = new Vector3(0.0f, 120.0f, 0.0f);
        emPosition = new Vector3(-0.30f, 0.00f, 0.15f);
        emFirstDesPosition = new Vector3(-0.12f, 0.00f, 0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 16);

        emRotation = new Vector3(0.0f, 150.0f, 0.0f);
        emPosition = new Vector3(-0.15f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(-0.06f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 17);

        StartCoroutine(Co_CheckNumEm05_02());
    }

    IEnumerator Co_CheckNumEm05_02()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase5_02_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0032 18기와 em0031 3기 생성 // 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 / 0, 1, 2
    public void Phase6_01()
    {
        RemainEnemies = 21;
        emRotation = Vector3.zero;
        emRotatePoint = Vector3.zero;
        emlastMoveSpeed = 40.00f;
        isCanLook = true;

        StartCoroutine(Co_Phase6_01());
    }

    IEnumerator Co_Phase6_01()
    {
        emfirstMoveSpeed = 0.05f;
        emPosition = new Vector3(-0.23f, 0.00f, -0.27f);
        emFirstDesPosition = new Vector3(0.07f, 0.00f, -0.07f);
        emRotateAxis = Vector3.down;

        for (int i = 10; i < 19; i++)
        {
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i);
            yield return wait_half_Second;
        }

        emfirstMoveSpeed = 0.06f;
        emPosition = new Vector3(0.21f, 0.00f, -0.29f);
        emFirstDesPosition = new Vector3(-0.09f, 0.00f, -0.09f);
        emRotateAxis = Vector3.up;

        for (int i = 19; i < 28; i++)
        {
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i);
            yield return wait_half_Second;
        }

        emfirstMoveSpeed = 0.10f;
        emPosition = new Vector3(-0.30f, 0.00f, 0.15f);
        emFirstDesPosition = new Vector3(0.40f, 0.00f, 0.15f);

        for (int i = 0; i < 3; i++)
        {
            SpawnEm0031(emRotation, emPosition, emFirstDesPosition, i);
            yield return wait_1_Second;
        }

        StartCoroutine(Co_CheckNumEm06_01());
    }

    IEnumerator Co_CheckNumEm06_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase6_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // Side View
    // em0030 4기를 생성 // 0, 1, 2, 3
    public void Phase7_06()
    {
        RemainEnemies = 4;
        emRotation = new Vector3(0, 180f, 0);
        emfirstMoveSpeed = 0.05f;
        emlastMoveSpeed = 0.05f;
        isCanLook = false;

        emPosition = new Vector3(0.00f, 0.20f, 0.25f);
        emFirstDesPosition = new Vector3(0.00f, 0.18f, 0.23f);
        emLastDesPosition = new Vector3(0.00f, 0.12f, 0.20f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 0);

        emPosition = new Vector3(0.00f, 0.15f, 0.25f);
        emFirstDesPosition = new Vector3(0.00f, 0.13f, 0.23f);
        emLastDesPosition = new Vector3(0.00f, 0.08f, 0.20f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 1);

        emPosition = new Vector3(0.00f, -0.15f, 0.25f);
        emFirstDesPosition = new Vector3(0.00f, -0.13f, 0.23f);
        emLastDesPosition = new Vector3(0.00f, -0.08f, 0.20f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 2);

        emPosition = new Vector3(0.00f, -0.20f, 0.25f);
        emFirstDesPosition = new Vector3(0.00f, -0.18f, 0.23f);
        emLastDesPosition = new Vector3(0.00f, -0.12f, 0.20f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 3);

        StartCoroutine(Co_CheckNumEm07_06());
    }

    IEnumerator Co_CheckNumEm07_06()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase7_06_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0032 6기를 생성 // 28, 29, 30, 31, 32, 33
    public void Phase8_01()
    {
        RemainEnemies = 6;
        emRotation = new Vector3(0, 180f, 0);
        emfirstMoveSpeed = 0.1f;
        emlastMoveSpeed = 15.00f;
        isCanLook = false;

        StartCoroutine(Co_Phase8_01());
    }

    IEnumerator Co_Phase8_01()
    {
        for (int i = 28; i < 33; i += 2)
        {
            emPosition = new Vector3(0.00f, 0.15f, -0.30f);
            emFirstDesPosition = new Vector3(0.00f, 0.15f, -0.15f);
            emRotatePoint = new Vector3(0.00f, -0.15f, -0.15f);
            emRotateAxis = -Vector3.left;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i);
            yield return wait_1_Second;

            emPosition = new Vector3(0.00f, -0.15f, -0.30f);
            emFirstDesPosition = new Vector3(0.00f, -0.15f, -0.15f);
            emRotatePoint = new Vector3(0.00f, 0.15f, -0.15f);
            emRotateAxis = Vector3.left;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i+1);
            yield return wait_1_Second;
        }

        StartCoroutine(Co_CheckNumEm08_01());
    }

    IEnumerator Co_CheckNumEm08_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase8_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em 0032 3기를 생성 // 34, 35, 36
    public void Phase9_01()
    {
        RemainEnemies = 3;
        emRotation = new Vector3(0, 180f, 0);
        emfirstMoveSpeed = 0.10f;
        emlastMoveSpeed = 15.00f;
        isCanLook = false;

        StartCoroutine(Co_Phase9_01());
    }

    IEnumerator Co_Phase9_01()
    {
        for (int i = 34; i < 37; i++)
        {
            emPosition = new Vector3(0.00f, 0.10f, -0.30f);
            emFirstDesPosition = new Vector3(0.00f, 0.10f, 0.10f);
            emRotatePoint = new Vector3(0.00f, -0.20f, -0.20f);
            emRotateAxis = Vector3.right;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i);
            yield return wait_1_Second;
        }

        StartCoroutine(Co_CheckNumEm09_01());
    }

    IEnumerator Co_CheckNumEm09_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase9_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em 0030 6기를 생성 // 18, 19, 20, 21, 22, 23
    public void Phase10_01()
    {
        RemainEnemies = 6;
        emRotation = new Vector3(0, 180f, 0);
        emfirstMoveSpeed = 0.1f;
        isCanLook = false;

        StartCoroutine(Co_Phase10_01());
    }

    IEnumerator Co_Phase10_01()
    {
        emPosition = new Vector3(0.00f, 0.30f, 0.20f);
        emFirstDesPosition = new Vector3(0.00f, 0.24f, 0.19f);
        emLastDesPosition = new Vector3(0.00f, -0.60f, 0.05f);
        for (int i = 18; i < 21; i++)
        {
            SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, i);
            yield return wait_1_Second;
        }

        emPosition = new Vector3(0.00f, -0.30f, 0.07f);
        emFirstDesPosition = new Vector3(0.00f, -0.24f, 0.08f);
        emLastDesPosition = new Vector3(0.00f, 0.60f, 0.22f);
        for (int i = 21; i < 24; i++)
        {
            SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emLastDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, i);
            yield return wait_1_Second;
        }

        StartCoroutine(Co_CheckNumEm10_01());
    }

    IEnumerator Co_CheckNumEm10_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase10_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em 0030 4기를 생성 // 24, 25, 26, 27
    public void Phase11_01()
    {
        RemainEnemies = 4;
        emRotation = new Vector3(0, 180f, 0);
        emfirstMoveSpeed = 0.1f;
        isCanLook = false;

        emPosition = new Vector3(0.00f, 0.17f, -0.30f);
        emFirstDesPosition = new Vector3(0.00f, 0.03f, 0.20f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, 0);

        emPosition = new Vector3(0.00f, 0.14f, -0.27f);
        emFirstDesPosition = new Vector3(0.00f, 0.00f, 0.23f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, 1);

        emPosition = new Vector3(0.00f, 0.11f, -0.30f);
        emFirstDesPosition = new Vector3(0.00f, -0.03f, 0.20f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, 2);

        emPosition = new Vector3(0.00f, 0.14f, -0.33f);
        emFirstDesPosition = new Vector3(0.00f, 0.00f, 0.17f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emfirstMoveSpeed, isCanLook, 3);

        StartCoroutine(Co_CheckNumEm11_01());
    }

    IEnumerator Co_CheckNumEm11_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase11_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0032 6기를 생성 // 37, 38, 39, 40, 41, 42
    public void Phase12_01()
    {
        RemainEnemies = 6;
        emRotation = new Vector3(0, 180f, 0);
        emfirstMoveSpeed = 0.1f;
        emlastMoveSpeed = 15.00f;
        isCanLook = false;

        StartCoroutine(Co_Phase12_01());
    }

    IEnumerator Co_Phase12_01()
    {
        for (int i = 37; i < 43; i += 2)
        {
            emPosition = new Vector3(0.00f, 0.15f, -0.30f);
            emFirstDesPosition = new Vector3(0.00f, 0.15f, -0.15f);
            emRotatePoint = new Vector3(0.00f, -0.15f, -0.15f);
            emRotateAxis = -Vector3.left;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i);
            yield return wait_1_Second;

            emPosition = new Vector3(0.00f, -0.15f, -0.30f);
            emFirstDesPosition = new Vector3(0.00f, -0.15f, -0.15f);
            emRotatePoint = new Vector3(0.00f, 0.15f, -0.15f);
            emRotateAxis = Vector3.left;
            SpawnEm0032(emRotation, emPosition, emFirstDesPosition, emRotatePoint, emRotateAxis, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, i + 1);
            yield return wait_1_Second;
        }

        StartCoroutine(Co_CheckNumEm012_01());
    }

    IEnumerator Co_CheckNumEm012_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase12_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // Top View (거꾸로 가요)
    // em0030 2기를 생성 // 28, 29
    public void Phase13_01()
    {
        RemainEnemies = 2;
        emRotation = Vector3.zero;
        emfirstMoveSpeed = 0.1f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        StartCoroutine(Co_Phase13_01());
    }

    IEnumerator Co_Phase13_01()
    {
        yield return new WaitForSeconds(4.0f);

        emPosition = new Vector3(-0.15f, 0.00f, -0.20f);
        emFirstDesPosition = new Vector3(-0.08f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 28);

        emPosition = new Vector3(0.15f, 0.00f, -0.20f);
        emFirstDesPosition = new Vector3(0.08f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 29);

        StartCoroutine(Co_CheckNumEm013_01());
    }

    IEnumerator Co_CheckNumEm013_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase13_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0030 4기를 생성 // 30, 31, 32, 33
    public void Phase14_01()
    {
        RemainEnemies = 4;
        emRotation = new Vector3(0, -180.0f, 0);
        emfirstMoveSpeed = 0.05f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        emPosition = new Vector3(-0.15f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(-0.15f, 0.00f, 0.07f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 30);

        emPosition = new Vector3(-0.05f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(-0.05f, 0.00f, 0.10f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 31);

        emPosition = new Vector3(0.05f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(0.05f, 0.00f, 0.10f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 32);

        emPosition = new Vector3(0.15f, 0.00f, 0.30f);
        emFirstDesPosition = new Vector3(0.15f, 0.00f, 0.07f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 33);

        StartCoroutine(Co_CheckNumEm14_01());

    }

    IEnumerator Co_CheckNumEm14_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase14_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0030 2기를 생성 // 34, 35
    public void Phase15_01()
    {
        RemainEnemies = 2;
        emRotation = Vector3.zero;
        emfirstMoveSpeed = 0.1f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        emPosition = new Vector3(-0.15f, 0.00f, -0.20f);
        emFirstDesPosition = new Vector3(-0.08f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 34);

        emPosition = new Vector3(0.15f, 0.00f, -0.20f);
        emFirstDesPosition = new Vector3(0.08f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 35);

        StartCoroutine(Co_CheckNumEm015_01());
    }

    IEnumerator Co_CheckNumEm015_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase15_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // Top View (앞으로 가요)
    // em0030 3기 생성 // 36, 37, 38
    public void Phase16_01()
    {
        RemainEnemies = 3;
        emRotation = Vector3.zero;
        emfirstMoveSpeed = 0.1f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        emPosition = new Vector3(-0.15f, 0.30f, -0.20f);
        emFirstDesPosition = new Vector3(-0.08f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 36);

        emPosition = new Vector3(0.00f, 0.30f, -0.20f);
        emFirstDesPosition = new Vector3(0.00f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 37);

        emPosition = new Vector3(0.15f, 0.30f, -0.20f);
        emFirstDesPosition = new Vector3(0.08f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 38);

        StartCoroutine(Co_CheckNumEm016_01());
    }

    IEnumerator Co_CheckNumEm016_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase16_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // em0030 8기 생성 //39, 40, 41, 42, 43, 44, 45, 46
    public void Phase17_01()
    {
        RemainEnemies = 8;
        emfirstMoveSpeed = 0.1f;
        emlastMoveSpeed = 0.001f;
        isCanLook = true;

        emPosition = new Vector3(0.00f, 0.00f, -0.30f);

        emRotation = new Vector3(0.0f, 210.0f, 0.0f);
        emFirstDesPosition = new Vector3(0.06f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 39);

        emRotation = new Vector3(0.0f, 240.0f, 0.0f);
        emFirstDesPosition = new Vector3(0.12f, 0.00f, 0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 40);

        emRotation = new Vector3(0.0f, 300.0f, 0.0f);
        emFirstDesPosition = new Vector3(0.12f, 0.00f, -0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 41);

        emRotation = new Vector3(0.0f, 330.0f, 0.0f);
        emFirstDesPosition = new Vector3(0.06f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 42);

        emPosition = new Vector3(0.00f, 0.00f, 0.30f);

        emRotation = new Vector3(0.0f, 30.0f, 0.0f);
        emFirstDesPosition = new Vector3(-0.06f, 0.00f, -0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 43);

        emRotation = new Vector3(0.0f, 60.0f, 0.0f);
        emFirstDesPosition = new Vector3(-0.12f, 0.00f, -0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 44);

        emRotation = new Vector3(0.0f, 120.0f, 0.0f);
        emFirstDesPosition = new Vector3(-0.12f, 0.00f, 0.06f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 45);

        emRotation = new Vector3(0.0f, 150.0f, 0.0f);
        emFirstDesPosition = new Vector3(-0.06f, 0.00f, 0.12f);
        SpawnEm0030(emRotation, emPosition, emFirstDesPosition, emFirstDesPosition, emfirstMoveSpeed, emlastMoveSpeed, isCanLook, 46);

        StartCoroutine(Co_CheckNumEm017_01());
    }

    IEnumerator Co_CheckNumEm017_01()
    {
        while (true)
        {
            if (RemainEnemies <= 0)
            {
                Phase17_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

    // 중간 보스 생성
    public void Phase18_01()
    {
        RemainEnemies = 5;
        isAlone = false;

        em0070.SetActive(true);
        em0070.transform.position = new Vector3(0.00f, 0.00f, 0.32f);
        FlagEmInformation flagEmInformation = em0070.GetComponent<FlagEmInformation>();
        flagEmInformation.isDie = false;
        Em0070Movement em0070Movement = em0070.GetComponent<Em0070Movement>();
        StartCoroutine(em0070Movement.Co_Move());

        StartCoroutine(Co_CheckNumEm018_01());
    }

    IEnumerator Co_CheckNumEm018_01()
    {
        while (true)
        {
            if (RemainEnemies == 1 && !isAlone)
            {
                Phase18_01_Alone.Invoke();
                isAlone = true;
            }
            if (RemainEnemies <= 0)
            {
                Phase18_01_EMDie.Invoke();
                break;
            }
            yield return null;
        }
    }

}
