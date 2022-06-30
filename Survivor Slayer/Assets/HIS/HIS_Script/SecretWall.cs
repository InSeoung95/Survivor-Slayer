using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    public GameObject[] SecretWalls; //숨길 벽들.

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            foreach(var wall in SecretWalls)
            {
                wall.layer = 11; // SeeThrough 레이어로 변경.
            }
        }
    }
}
