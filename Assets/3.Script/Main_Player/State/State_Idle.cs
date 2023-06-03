using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : State
{
    public override void Enter(State before)
    {
        Main_Player.Instance.anim_player.SetBool("Idle", true);
        Main_Player.Instance.anim_player.SetTrigger("Idle 0");
        Main_Player.Instance.anim_sword.SetTrigger("Idle");
        Main_Player.Instance.anim_bigsword.SetTrigger("Idle");
    }

    public override void Exit(State next)
    {
        Main_Player.Instance.anim_player.SetBool("Idle", false);
        Main_Player.Instance.anim_player.SetFloat("Speed", 0f);
    }

    public override void StateFixedUpdate()
    {
        
    }

    float speed = 5f;
    public override void StateUpdate()
    {
        Main_Player.Instance.Rotate();
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0f, 
            Input.GetAxis("Vertical"));
        if (inputVec.z > 0)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
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
                Main_Player.Instance.anim_player.SetFloat("Speed", inputVec.z);
            }
        }
        else Main_Player.Instance.anim_player.SetFloat("Speed", 0f);
    }
}
