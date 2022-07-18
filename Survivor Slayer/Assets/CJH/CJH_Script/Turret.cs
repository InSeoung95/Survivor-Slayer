using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool _onPlayerTrigger;
    private float FireTime;
    private float missileTime = 5f;                      // 미사일 발사시간

    private float TurretHealth = 50;
    [SerializeField] private GameObject _tower;         // 포탑 발사관 y축 회전용
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
        if (_onPlayerTrigger)
        {
            FireTime += Time.deltaTime;
            var targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            var towerPos = new Vector3(transform.position.x, transform.position.y, target.position.z);
            gameObject.transform.LookAt(targetPos);
            _tower.gameObject.transform.LookAt(towerPos);
            missileSpawn.transform.LookAt(target);
           

            if (FireTime > missileTime)
            {
                FireTime = 0;
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
