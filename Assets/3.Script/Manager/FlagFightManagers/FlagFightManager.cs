using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagFightManager : MonoBehaviour
{
    // Unity 이벤트 정의
    public UnityEvent phase1;

    [SerializeField] FlagFightSpawner flagFightSpawner;

    private void Awake()
    {
        flagFightSpawner = FindObjectOfType<FlagFightSpawner>();

        flagFightSpawner.Phase18_01_EMDie.AddListener(NextPhase);
    }

    private void Start()
    {
        phase1.Invoke();
    }

    private void NextPhase()
    {
        Debug.Log("넘어가욧!");
    }













}
