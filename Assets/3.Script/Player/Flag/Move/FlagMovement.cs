using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSideViewMove : IFlagViewStrategy
{
    public Vector3 Move(FlagControl player, out Vector3 move)
    {
        move = new Vector3(0, player.v, player.h);

        return move;
    }
}
public class FlagBackViewMove : IFlagViewStrategy
{
    public Vector3 Move(FlagControl player, out Vector3 move)
    {
        move = new Vector3(player.h, player.v, 0);

        return move;
    }
}
public class FlagTopViewMove : IFlagViewStrategy
{
    public Vector3 Move(FlagControl player, out Vector3 move)
    {
        move = new Vector3(player.h, 0, player.v);

        return move;
    }
}
public class GundamTopViewMove : IFlagViewStrategy
{
    public Vector3 Move(FlagControl player, out Vector3 move)
    {
        move = new Vector3(player.h, 0, player.v);
        Rotate(player, move);
        return move;
    }

    private void Rotate(FlagControl player, Vector3 move)
    {
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.z).normalized;
        if (player.h != 0 || player.v != 0)
        {
            float targetRotation = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            if (targetRotation == 0 && move.z < 0)
            {
                targetRotation = -180f;
            }
            player.transform.rotation = Quaternion.Euler(player.transform.eulerAngles.x, 0.0f, targetRotation);
        }
    }
}

