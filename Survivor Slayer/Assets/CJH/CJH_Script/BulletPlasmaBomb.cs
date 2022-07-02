using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlasmaBomb : MonoBehaviour
{
    public int bulletTimer = 5;
    [SerializeField] private float BOMB_RANGE = 10;     // 폭탄 폭발범위
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, BOMB_RANGE);
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

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, BOMB_RANGE, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy_test>().HitBomb();
        }
        
        Destroy(gameObject, 2);
        
        yield return null;
    }
}
