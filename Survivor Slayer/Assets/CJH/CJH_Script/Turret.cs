using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool _onPlayerTrigger;
    private float FireTime;
    public float missileTime = 5f;                      // 미사일 발사시간
    public bool BossType;                               // 보스전만 따로 뺴서 프리팹 만들고 보스전-false, 일반-true로 사용

    private float TurretHealth = 50;
    [SerializeField] private GameObject _missile;
    [SerializeField] private Transform missileSpawn;
    private Transform target;
    private void OnDrawGizmos()
       {
           Gizmos.color = Color.red;
           Gizmos.DrawWireSphere (transform.position, 25f);
       }
    
    void Update()
    {
        if (_onPlayerTrigger && BossType)
        {
            FireTime += Time.deltaTime;
            var targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            gameObject.transform.LookAt(targetPos);
            missileSpawn.transform.LookAt(target);
           

            if (FireTime > missileTime)
            {
                FireTime = 0f;
                FireMissile();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _onPlayerTrigger = true;
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _onPlayerTrigger = false;
            FireTime = 0;
        }
    }

    private void FireMissile()
    {
        Instantiate(_missile, missileSpawn.transform.position, missileSpawn.transform.rotation);
    }
}
