using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    protected override void OnStart()
    {
        Debug.Log($"OnStart{message}");
    }
    protected override void OnStop()
    {
        Debug.Log($"OnStop{message}");
    }
    protected override BehaviorState OnUpdate()
    {
        Debug.Log($"OnUpdate{message}");
        return BehaviorState.Success;
    }
}
