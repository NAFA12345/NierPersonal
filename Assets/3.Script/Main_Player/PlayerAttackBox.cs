using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ãæµ¹");
        if (other.CompareTag("Enemy"))
        {
            other.transform.root.GetComponent<EnemyHp>().TakeDamage(Random.Range(12,18));
        }
        else if (other.CompareTag("BulletSoft"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
