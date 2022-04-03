using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_test_range : MonoBehaviour
{
    private SphereCollider _sphereCollider;       // 좀비 공격범위 인식
    private Enemy_test tester;
    private Base _base;                           // 좀비가 인식한 거점의 상태확인

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
            _sphereCollider.radius = 20f;  // 플레이어 인식 -> 플레이어 추적범위 : 플레이어가 추적범위(20f) 벗어나면 다시 인식(10f)으로
        }

        if (other.gameObject.tag == "ObstacleWall")
        {
            tester.Target = other.gameObject;
            tester.testMove = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //플레이어가 탈출했을때 근처에 거점 있으면 거점을 확인하기 위해서 stay로 계속 체크해놓기
        if (other.gameObject.tag == "Base")
        {
            if(_base == null)
                _base = other.gameObject.GetComponent<Base>();
            if (_base != null)
            {
                if (_base.state != Base.State.Enemy_Occupation)
                {
                    tester.Target = other.gameObject;
                    tester.testMove = true;
                }
                else
                {
                    tester.Target = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tester.testMove = false;
            _sphereCollider.radius = 10f;
        }

        // 거점에서 나갈때 거점확인변수를 제거
        if (other.gameObject.tag == "Base")
        {
            if (_base != null)
            {
                _base = null;
            }
        }
    }
}
