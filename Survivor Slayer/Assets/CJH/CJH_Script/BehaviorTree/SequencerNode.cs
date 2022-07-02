using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    private int current;

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override BehaviorState OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case BehaviorState.Running:
                return BehaviorState.Running;
            case BehaviorState.Failure:
                return BehaviorState.Failure;
            case BehaviorState.Success:
                current++;
                break;
        }

        return current == children.Count ? BehaviorState.Success : BehaviorState.Running;
    }
}
