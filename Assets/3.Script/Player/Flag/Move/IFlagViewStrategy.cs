using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlagViewStrategy
{    
    public Vector3 Move(FlagControl player, out Vector3 move);
}
