using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    
    public int brokenCount=0;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(brokenCount==2)
        {
            anim.SetTrigger("Move");
        }
    }
}
