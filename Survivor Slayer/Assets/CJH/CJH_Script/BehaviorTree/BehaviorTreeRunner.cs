using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    private BehaviorTree tree;
    
    // Start is called before the first frame update
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviorTree>();
        
        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "First Start 1";
        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "First Start 2";
        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "First Start 3";
        
        var pause1 = ScriptableObject.CreateInstance<WaitNode>();
        var pause2 = ScriptableObject.CreateInstance<WaitNode>();
        var pause3 = ScriptableObject.CreateInstance<WaitNode>();
        
        var sequence = ScriptableObject.CreateInstance<SequencerNode>();
        sequence.children.Add(log1);
        sequence.children.Add(pause1);
        sequence.children.Add(log2);
        sequence.children.Add(pause2);
        sequence.children.Add(log3);
        sequence.children.Add(pause3);
        
        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = sequence;

        tree.rootNode = loop;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
