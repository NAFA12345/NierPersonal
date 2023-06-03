using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float Damage = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerData playerData))
            {
                playerData.OnDamage(Damage);
            }
            Destroy(gameObject);
        }
    }
}
