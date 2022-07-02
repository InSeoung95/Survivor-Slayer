using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1;
    private float startTime;
    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {
        
    }

    protected override BehaviorState OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return BehaviorState.Success;
        }

        return BehaviorState.Running;
    }
}
