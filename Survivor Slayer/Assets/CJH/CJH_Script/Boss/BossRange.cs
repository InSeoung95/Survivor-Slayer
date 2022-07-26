using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRange : MonoBehaviour
{
    private SphereCollider _sphereCollider;       // 좀비 공격범위 인식
    private BossZombie _boss;

    [SerializeField] private float AttackRange = 40;

    void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _boss = GetComponentInParent<BossZombie>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _boss.Target = other.gameObject;
            _boss.testMove = true;
            _boss.chasePlayer = true;
            _sphereCollider.radius = AttackRange;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _sphereCollider.radius = AttackRange/2;
        }
    }
}
