using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorNodeView : UnityEditor.Experimental.GraphView.Node
{
    public BehaviorNode node;
    
    public BehaviorNodeView(BehaviorNode node)
    {
        this.node = node;
        this.title = node.name;
    }
}
