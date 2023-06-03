using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAttack : IFlagState
{
    public void Action(FlagControl player)
    {
        if (!player.gameObject.layer.Equals(player.defaultLayer))
        {
            player.gameObject.layer = player.defaultLayer;
        }
    }
}
public class FlagTransformation : IFlagState
{
    public void Action(FlagControl player)
    {
        if (!player.gameObject.layer.Equals(player.invincibleLayer))
        {
            player.gameObject.layer = player.invincibleLayer;
        }
    }
}
public class FlagNomal : IFlagState
{
    public void Action(FlagControl player)
    {
        if (!player.gameObject.layer.Equals(player.defaultLayer))
        {
            player.gameObject.layer = player.defaultLayer;
        }
    }
}
public class FlagDash : IFlagState
{
    public void Action(FlagControl player)
    {
        if (!player.gameObject.layer.Equals(player.invincibleLayer))
        {
            player.gameObject.layer = player.invincibleLayer;
        }
    }
}
