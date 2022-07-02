using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : BehaviorNode
{
    public List<BehaviorNode> children = new List<BehaviorNode>();
}
