using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode : ScriptableObject
{
    public enum BehaviorState
    {
        Running,
        Failure,
        Success
    }

    public BehaviorState state = BehaviorState.Running;
    public bool started = false;
    public string guid;

    public BehaviorState Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == BehaviorState.Failure || state == BehaviorState.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract BehaviorState OnUpdate();
}
