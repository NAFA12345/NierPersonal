using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaControl : MonoBehaviour
{
    private FlagFightSubTitleManager flagFightSubTitleManager;
    private ObjectScrolling objectScrolling;

    private void Awake()
    {
        objectScrolling = GetComponent<ObjectScrolling>();

        flagFightSubTitleManager = FindObjectOfType<FlagFightSubTitleManager>();
        flagFightSubTitleManager.phase7_02.AddListener(phase7_02);
    }

    private void phase7_02()
    {
        objectScrolling.scrollingSpeed = 5.0f;
    }
}