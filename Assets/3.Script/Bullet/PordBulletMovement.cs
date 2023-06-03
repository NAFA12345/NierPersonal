using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PordBulletMovement : MonoBehaviour
{
    [SerializeField] private Vector3 Direction = Vector3.zero;
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        transform.position += Direction * speed * Time.deltaTime;
    }

    public void Move(Vector3 direction)
    {
        Direction = direction;
    }
}
