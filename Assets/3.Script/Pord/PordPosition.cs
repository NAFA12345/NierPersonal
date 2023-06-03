using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PordPosition : MonoBehaviour
{
    private GameObject player;
    //private Camera cam;
    private Vector3 defaultPos;
    private void Awake()
    {
        defaultPos = new Vector3(-0.6f, 1.8f, 0);
    }
    private void Update()
    {
        //todo 0518 민수야 카메라 위치에따라 포드위치도 조금 옮겨주렴.. 파이팅.. 
        transform.position = player.transform.position + defaultPos;
    }


}
