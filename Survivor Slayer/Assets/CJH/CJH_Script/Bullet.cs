using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletTimer = 5;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        
        StartCoroutine(TimeOverDestroyBullet());
    }

    private void OnDisable()
    {
        
        gameObject.transform.position = Vector3.zero;
        StopCoroutine(TimeOverDestroyBullet());
    }

    private IEnumerator TimeOverDestroyBullet()
    {
        yield return new WaitForSeconds(bulletTimer);
        gameObject.SetActive(false);
    }
}
