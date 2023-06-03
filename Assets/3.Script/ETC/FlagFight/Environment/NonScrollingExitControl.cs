using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonScrollingExitControl : MonoBehaviour
{
    [SerializeField] GameObject exit;
    [SerializeField] FlagFightSubTitleManager flagFightSubTitleManager;
    [SerializeField] FlagFightSpawner flagFightSpawner;

    private void Start()
    {
        flagFightSubTitleManager = FindObjectOfType<FlagFightSubTitleManager>();
        flagFightSpawner = FindObjectOfType<FlagFightSpawner>();
        flagFightSubTitleManager.phase7_07.AddListener(phase7_07);
        flagFightSpawner.Phase18_01_EMDie.AddListener(Phase18_01_EMDie);

    }

    void phase7_07()
    {
        exit.SetActive(true);
    }
    void Phase18_01_EMDie()
    {
        exit.SetActive(false);
    }


}
