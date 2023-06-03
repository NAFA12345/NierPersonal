using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBarrier : MonoBehaviour
{
    private float meleeDamage = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out FlagEmInformation enemy))
            {
                enemy.OnDamage(PlayerData.Instance.atk * meleeDamage);
            }
        }
        if (other.CompareTag("BulletSoft") || other.CompareTag("BulletHard"))
        {
            Destroy(other.gameObject);
        }
    }
}
