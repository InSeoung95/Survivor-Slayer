using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public BehaviorNode rootNode;
    public BehaviorNode.BehaviorState treeState = BehaviorNode.BehaviorState.Running;

    public BehaviorNode.BehaviorState Update()
    {
        if (rootNode.state == BehaviorNode.BehaviorState.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }
}
