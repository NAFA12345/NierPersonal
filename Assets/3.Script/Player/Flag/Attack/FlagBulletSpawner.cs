using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBulletSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet_pref;
    private GameObject[] bullets;
    private int bulletNum = 20;
    private int cnt = 0;

    private void Awake()
    {
        bullets = new GameObject[bulletNum];

        for(int i = 0; i< bulletNum; i++)
        {
            bullets[i] = Instantiate(bullet_pref);
            bullets[i].SetActive(false);
        }
    }
    public void Fire()
    {
        if (!bullets[cnt].activeSelf)
        {
            bullets[cnt].SetActive(true);
            bullets[cnt].transform.SetPositionAndRotation(transform.position, transform.rotation);
        }

        cnt++;
        if(cnt >= bulletNum)
        {
            cnt = 0;
        }
    }
}
