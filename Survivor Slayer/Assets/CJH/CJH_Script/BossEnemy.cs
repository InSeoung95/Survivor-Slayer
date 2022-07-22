using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public const float SPINSPEED = 1500f;       // 최대 회전속도
    private bool vec;       // up : true, down : false
    private Vector3 UpMaxPos;
    private Vector3 DownMaxPos;
    private Vector3 Pos;
    Vector3 velo = Vector3.zero;
    
    // 보스의 회전과 이동을 담당할 오브젝트
    [SerializeField] private GameObject _BossBody;
    [SerializeField] private BossChild _BossChild1;
    [SerializeField] private BossChild _BossChild2;
    [SerializeField] private BossChild _BossChild3;

    [SerializeField] private Transform[] ChildTransform;        // 자식들이 공격패턴후에 원래 위치로 이동할 좌표
    [SerializeField] private Transform[] ChildAttackTransform;  // 공격패턴할 위치할당
    [SerializeField] private Turret[] _turrets;
    
    private float BodySpinSpeed;
    private float child1SpinSpeed;
    private float child2SpinSpeed;
    private float child3SpinSpeed;

    private float BodySpinSpeedMax = SPINSPEED;
    private float child1SpinSpeedMax = SPINSPEED;
    private float child2SpinSpeedMax = SPINSPEED;
    private float child3SpinSpeedMax = SPINSPEED;

    [SerializeField]private bool SpinBody;
    [SerializeField]private bool SpinChild1;
    [SerializeField]private bool SpinChild2;
    [SerializeField]private bool SpinChild3;

    public bool test1;
    public bool test2;

    private void Start()
    {
        Pos = transform.position;
        UpMaxPos = Pos + new Vector3(0, 2, 0);
        DownMaxPos = Pos + new Vector3(0, -2, 0);
        vec = true;
    }

    private void Update()
    {
        UpDown();
        Spin();

        if (test1)
        {
            CallAttack(0);
            CallAttack(1);
            CallAttack(2);
            test1 = false;
        }

        if (test2)
        {
            CallBackChild(0);
            CallBackChild(1);
            CallBackChild(2);
            test2 = false;
        }
    }

    private void UpDown()
    {
        if (vec)
        {
            transform.position = Vector3.SmoothDamp(transform.position, UpMaxPos,ref velo, 1.5f);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, DownMaxPos,ref velo, 1.5f);
        }

        if (transform.position.y >= UpMaxPos.y -1)
            vec = false;
        else if (transform.position.y <= DownMaxPos.y +1)
            vec = true;
    }

    private void Spin()
    {
        if (SpinBody)
        {
            if (BodySpinSpeed < BodySpinSpeedMax)
            {
                BodySpinSpeed += 1f;
            }
            _BossBody.transform.Rotate(new Vector3(0,BodySpinSpeed * Time.deltaTime, 0));
        }
        if (SpinChild1)
        {
            if (child1SpinSpeed < child1SpinSpeedMax)
            {
                child1SpinSpeed += 1f;
            }
            _BossChild1.transform.Rotate(new Vector3(0,child1SpinSpeed * Time.deltaTime, 0));
        }
        if (SpinChild2)
        {
            if (child2SpinSpeed < child2SpinSpeedMax)
            {
                child2SpinSpeed += 1f;
            }
            _BossChild2.transform.Rotate(new Vector3(0,child2SpinSpeed * Time.deltaTime, 0));
        }
        if (SpinChild3)
        {
            if (child3SpinSpeed < child3SpinSpeedMax)
            {
                child3SpinSpeed += 1f;
            }
            _BossChild3.transform.Rotate(new Vector3(0,child3SpinSpeed * Time.deltaTime, 0));
        }
    }

    public void CallAttack(int childNum)
    {
        switch (childNum)
        {
            case 0 :
                _BossChild1.transform.position = ChildAttackTransform[childNum].transform.position;
                SpinChild1 = true;
                child1SpinSpeed = 0;
                break;
            case 1 :
                _BossChild2.transform.position = ChildAttackTransform[childNum].transform.position;
                SpinChild2 = true;
                child2SpinSpeed = 0;
                break;
            case 2 :
                _BossChild3.transform.position = ChildAttackTransform[childNum].transform.position;
                SpinChild3 = true;
                child3SpinSpeed = 0;
                break;
        }
    }

    public void CallBackChild(int childNum)
    {
        switch (childNum)
        {
            case 0 :
                _BossChild1.transform.position = ChildTransform[childNum].transform.position;
                SpinChild1 = false;
                child1SpinSpeed = 0;
                break;
            case 1 :
                _BossChild2.transform.position = ChildTransform[childNum].transform.position;
                SpinChild2 =  false;
                child2SpinSpeed = 0;
                break;
            case 2 :
                _BossChild3.transform.position = ChildTransform[childNum].transform.position;
                SpinChild3 =  false;
                child3SpinSpeed = 0;
                break;
        }
    }

    private void OnTurret()
    {
        foreach (var turret in _turrets)
        {
            turret.BossType = true;
        }
    }
    
    private void OffTurret()
    {
        foreach (var turret in _turrets)
        {
            turret.BossType = false;
        }
    }
}
