using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletTimer = 5;
    public int UpgradeRate;

    public float[] Damage = { 10, 15, 20 };
    private float[] Size = { 0.5f, 1.5f, 2.5f };

    public GameObject BulletMark;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ShowEffect(collision);
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            ShowEffect(collision);
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.one * Size[UpgradeRate];
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

    private void ShowEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        var mark = Instantiate(BulletMark, contact.point, rot);
        Destroy(mark, 5f);
    }
}
