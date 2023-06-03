using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideControl : MonoBehaviour
{
    [SerializeField] GameObject[] Insides;
    [SerializeField] FlagFightSubTitleManager flagFightSubTitleManager;


    // Start is called before the first frame update
    void Start()
    {
        flagFightSubTitleManager = FindObjectOfType<FlagFightSubTitleManager>();
        flagFightSubTitleManager.phase7_02.AddListener(phase7_02);
    }

    void phase7_02()
    {
        for(int i = 0; i < Insides.Length; i++)
        {
            Insides[i].SetActive(true);
        }
    }

}
