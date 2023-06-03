using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlagModeStrategy
{
    public void WeakAttack(FlagControl player, bool isHorizontal = true);
    public void StrongAttack(FlagControl player);
    public void Dash(FlagControl player);
    public void Rotate(FlagControl player);
}
