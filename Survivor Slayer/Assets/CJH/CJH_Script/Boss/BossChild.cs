using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class BossChild : MonoBehaviour
{
    public enum ChildAttackType
    {
        MakeBullet,
        MakeCube,
        MakeGround
    }
    
    private bool vec;       // up : true, down : false
    private Vector3 UpMaxPos;
    private Vector3 DownMaxPos;
    private Vector3 Pos;
    Vector3 velo = Vector3.zero;
    
    public const float SPINSPEED = 1500f;       // 최대 회전속도
    public bool SpinBody;
    private float BodySpinSpeed;
    private float BodySpinSpeedMax = SPINSPEED;

    public ChildAttackType _attackType;
    public bool AttackRun;
    public bool Attack;
    private GameObject _target;
    public GameObject _player;
    public GameObject _BossZombie;                          // 어택타입 변경시 플레이어->보스로 목적지 변경
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private GameObject BulletPrefabs;      // 보스전 플레이어 공격 총알 프리팹
    [SerializeField] private GameObject PlayerBullet;       // 점령시  좀비 공격할 총알 프리팹
    public Transform[] BulletPoint;                         // 보스용 총알 생성될 위치
    public BossFloor Ground;                             // 장판효과

    private float Timer;
    [SerializeField]private float AttackDelay = 10f;

    private void Start()
    {
        Pos = transform.position;
        UpMaxPos = Pos + new Vector3(0, 1.2f, 0);
        DownMaxPos = Pos + new Vector3(0, -1.2f, 0);
        vec = true;
    }

    private void Update()
    {
        UpDown();
        Spin();
        Timer += Time.deltaTime;
        if (Timer > AttackDelay && AttackRun)
        {
            Timer = 0;
            Attack = true;
            if (Attack)
            {
                if (_attackType == ChildAttackType.MakeBullet)
                {
                    AttackBullet();
                    Attack = false;
                }
                
                if (_attackType == ChildAttackType.MakeGround)
                {
                    BossGroundOn();
                    Attack = false;
                    AttackRun = false;
                }
            }
        }
    }
    
    private void AttackBullet()
    {
        var target =Instantiate(_playerTarget, _target.transform.position, _target.transform.rotation);
        Destroy(target,15f);
        
        foreach (var Bullet in BulletPoint)
        {
            if (!SpinBody)
            {
                var Obj = Instantiate(BulletPrefabs, Bullet.position, Bullet.rotation);
                Obj.GetComponent<BossBullet>().BulletPoint = target.gameObject.transform;
            }
            else
            {
                var Obj = Instantiate(PlayerBullet, Bullet.position, Bullet.rotation);
                Obj.GetComponent<BossBullet>().BulletPoint = target.gameObject.transform;
            }
        }
    }
    

    private void BossGroundOn()
    {
        Ground._Trigger = true;
    }
    public void BossGroundOff()
    {
        Ground.GroundRollBack();
    }

    private void UpDown()
    {
        if (vec)
        {
            transform.position = 
                Vector3.SmoothDamp(transform.position, UpMaxPos,ref velo, 2f);
        }
        else
        {
            transform.position =
                Vector3.SmoothDamp(transform.position, DownMaxPos,ref velo, 2f);
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
           transform.Rotate(new Vector3(0,BodySpinSpeed * Time.deltaTime, 0));
        }
    }
    
    public void ChangeTarget(bool target)
    {
        // true - 플레이어
        if (target)
        {
            _target = _player;
        }
        // false - 보스
        else
        {
            _target = _BossZombie;
        }
    }
}
