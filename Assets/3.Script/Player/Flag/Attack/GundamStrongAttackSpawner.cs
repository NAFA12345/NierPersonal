using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamStrongAttackSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject sword;

    private void OnEnable()
    {
        sword.SetActive(true);
        sword.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
    private void OnDisable()
    {
        sword.SetActive(false);
    }
}
