using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Move : State
{
    public override void Enter(State before)
    {
        Main_Player.Instance.anim_player.SetFloat("Speed", 1f);
        Main_Player.Instance.anim_sword.SetTrigger("Idle");
        Main_Player.Instance.anim_player.SetTrigger("Move");

    }

    public override void Exit(State next)
    {
        Main_Player.Instance.anim_player.SetFloat("Speed", 0f);
    }

    public override void StateFixedUpdate()
    {

    }
    public float speed = 5f;
    public override void StateUpdate()
    {
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Main_Player.Instance.Rotate();
        if (inputVec.z > 0)
        { 
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 8f;
            Main_Player.Instance.anim_player.SetFloat("Speed", 1.5f);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = 2f;
            Main_Player.Instance.anim_player.SetFloat("Speed", 0.5f);
        }
        else
        {
            speed = 5f;
            Main_Player.Instance.anim_player.SetFloat("Speed", 1f);
        }
    }
}
