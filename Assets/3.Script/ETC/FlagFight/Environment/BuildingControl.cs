using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    [SerializeField] GameObject Buildings;
    [SerializeField] FlagFightSubTitleManager flagFightSubTitleManager;


    // Start is called before the first frame update
    void Start()
    {
        flagFightSubTitleManager = FindObjectOfType<FlagFightSubTitleManager>();
        flagFightSubTitleManager.phase7_02.AddListener(phase7_02);
        flagFightSubTitleManager.phase7_07.AddListener(phase7_07);


    }

    void phase7_02()
    {
        Buildings.SetActive(true);
    }

    void phase7_07()
    {

        Buildings.SetActive(false);

    }
}
