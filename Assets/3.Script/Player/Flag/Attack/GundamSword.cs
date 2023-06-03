using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamSword : MonoBehaviour
{
    private float moveSpeed = 0.6f;
    private float damage = 5f;

    private void FixedUpdate()
    {
        transform.position += moveSpeed * Time.deltaTime * -transform.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.EnemyTagType.Enemy.ToString()))
        {
            if (other.TryGetComponent(out FlagEmInformation enemy))
            {
                enemy.OnDamage(PlayerData.Instance.atk * damage);
            }
        }
        else if (other.CompareTag(Define.EnemyTagType.BulletSoft.ToString()) || other.CompareTag(Define.EnemyTagType.BulletHard.ToString()))
        {
            Destroy(other.gameObject);
        }
    }
}
