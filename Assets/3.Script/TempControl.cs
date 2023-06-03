using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempControl : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed;
    float animspeed;
    Rigidbody rb;
    public Vector3 pos;
    [SerializeField] Vector3 inputVec; 

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //inputVec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //anim.SetFloat("Speed", inputVec.normalized.magnitude);

        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }


    }
}
