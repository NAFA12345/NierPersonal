using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBullet : MonoBehaviour
{
    private float moveSpeed = 0.6f;


    private void FixedUpdate()
    {
        transform.position += moveSpeed * Time.deltaTime * -transform.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.EnemyTagType.Enemy.ToString()))
        {
            if(other.TryGetComponent(out FlagEmInformation enemy))
            {
                enemy.OnDamage(PlayerData.Instance.atk);
            }
            gameObject.SetActive(false);
        }
        else if(other.CompareTag(Define.EnemyTagType.BulletSoft.ToString()))
        {
            Destroy(other.gameObject);
            gameObject.SetActive(false);
        }
    }
}