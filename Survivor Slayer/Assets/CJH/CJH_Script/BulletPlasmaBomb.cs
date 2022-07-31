using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlasmaBomb : MonoBehaviour
{
    private bool once;
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
    private IEnumerator TimeOverDestroyBullet()
    {
        yield return new WaitForSeconds(bulletTimer);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!once)
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        once = true;
        //인성 추가
        Glow.Play();
        Energy.Play();
        //
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, BOMB_RANGE, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            if(hitObj.transform.CompareTag("Enemy"))
                hitObj.transform.GetComponent<Enemy_test>().HitBomb();
            else if (hitObj.transform.CompareTag("EnemyFog"))
                hitObj.transform.GetComponent<Enemy_Fog>().HitBomb();
            else if (hitObj.transform.CompareTag("EnemyBoss"))
                hitObj.transform.GetComponent<BossZombie>().HitBomb();
        }
        Destroy(gameObject, 2);
        
        yield return null;
    }
}
