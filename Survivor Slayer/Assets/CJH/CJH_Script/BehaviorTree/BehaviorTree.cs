using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public BehaviorNode rootNode;
    public BehaviorNode.BehaviorState treeState = BehaviorNode.BehaviorState.Running;
    public List<BehaviorNode> nodes = new List<BehaviorNode>();

    public BehaviorNode.BehaviorState Update()
    {
        if (rootNode.state == BehaviorNode.BehaviorState.Running)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }

    public BehaviorNode CreateNode(System.Type type)
    {
        BehaviorNode node = ScriptableObject.CreateInstance(type) as BehaviorNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(BehaviorNode node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }
}
