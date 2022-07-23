using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlasmaBomb : MonoBehaviour
{
    public int bulletTimer = 5;
    [SerializeField] private float BOMB_RANGE = 10;     // 폭탄 폭발범위

    //인성 추가
    public ParticleSystem Center;
    public ParticleSystem Glow;
    public ParticleSystem Energy;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, BOMB_RANGE);
    }

    private void OnEnable()
    {
        Debug.Log("플라즈마 발사");
        
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
        //인성 추가
        Glow.Play();
        Energy.Play();
        //
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
