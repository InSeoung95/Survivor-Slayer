using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_test_range : MonoBehaviour
{
    private SphereCollider _sphereCollider;       // 좀비 공격범위 인식
    private Enemy_test tester;
    private Base _base;                           // 좀비가 인식한 거점의 상태확인

    [SerializeField]
    private float Enemy_Range = 40;

    void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        tester = GetComponentInParent<Enemy_test>();
        _base = GetComponent<Base>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tester.Target = other.gameObject;
            tester.testMove = true;
            tester.chasePlayer = true;
            _sphereCollider.radius = Enemy_Range;  // 플레이어 인식 -> 플레이어 추적범위 : 플레이어가 추적범위(20f) 벗어나면 다시 인식(10f)으로
        }

        if (other.gameObject.tag == "ObstacleWall")
        {
            tester.Target = other.gameObject;
            tester.testMove = true;
        }

        if (other.gameObject.tag == "Base")
        {
            _base = other.gameObject.GetComponentInParent<Base>();
            if (!tester.chasePlayer)
            {
                if (_base.state != Base.State.Enemy_Occupation)
                {
                    tester.Target = other.gameObject.transform.parent.gameObject;
                    tester.testMove = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tester.testMove = false;
            tester.chasePlayer = false;
            tester.Target = null;
            _sphereCollider.radius = Enemy_Range/2;
        }
    }
}
