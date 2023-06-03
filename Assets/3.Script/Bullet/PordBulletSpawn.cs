using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PordBulletSpawn : MonoBehaviour
{
    [SerializeField] public GameObject[] Bullet;
    [SerializeField] GameObject bulletPrefabs;
    private void Awake()
    {
        BulletSpawner(60);
    }

    public void BulletSpawner(int count)
    {
        Bullet = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            Bullet[i] = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
            Bullet[i].SetActive(false);
        }
    }
}
