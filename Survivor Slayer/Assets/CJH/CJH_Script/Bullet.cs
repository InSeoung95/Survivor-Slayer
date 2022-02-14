using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 2);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
