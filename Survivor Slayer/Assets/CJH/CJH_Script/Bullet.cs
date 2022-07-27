using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletTimer = 5;
    public int UpgradeRate;

    public float[] Damage = { 1.0f, 1.5f, 2.0f };           // 총알 데미지 배율
    private float[] Size = { 0.5f, 1.5f, 2.5f };

    public GameObject BulletMark;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ShowEffect(collision,true);
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ShowEffect(collision,false);
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

    private void ShowEffect(Collision collision, bool ground)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        Vector3 vec = Vector3.zero;
        if(ground)
        vec = new Vector3(0, 0.005f, 0);

        var mark = Instantiate(BulletMark, contact.point + vec, rot);
        Destroy(mark, 5f);
    }
}
