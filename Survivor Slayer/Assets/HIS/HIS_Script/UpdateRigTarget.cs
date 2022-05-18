using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRigTarget : MonoBehaviour
{
    public Transform target;

   
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = target.position;
    }
}
