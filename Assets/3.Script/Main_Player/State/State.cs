using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void Enter(State before);
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
    public abstract void Exit(State next);
}
