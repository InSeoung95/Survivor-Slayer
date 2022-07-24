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

    [SerializeField] private ChildAttackType _attackType;
    public bool AttackRun;
    public bool Attack;
    public GameObject _player;
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private BossAttackCube CubePrefabs;    // 보스용 공격큐브
    [SerializeField] private GameObject BulletPrefabs;      // 보스용 총알 프리팹
    public Transform[] BulletPoint;                         // 보스용 총알 생성될 위치
    public Transform[] CubeSpawnPoint;                      // 공격 큐브 생성위치
    public Transform[] CubeTargetPoint;                     // 공격 큐브 움직일 위치
    private bool cubeAttack;                                // 홀짝으로 공격할 패턴 2분화
    public BossFloor Ground;                             // 장판효과

    private float Timer;
    [SerializeField]private float AttackDelay = 10f;

    public float Health = 250f;
    public bool Back;
    private bool HealthOut;

    //체력 UI
    [Header("체력 UI")]
    public Slider HP_Ui;
    public Image Fill;
    public Gradient gradient;

    private void Start()
    {
        HP_Ui.maxValue = Health;
        HP_Ui.value = Health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            var BulletDamage = collision.gameObject.GetComponent<Bullet>()
                .Damage[collision.gameObject.GetComponent<Bullet>().UpgradeRate];
            Health -= BulletDamage;

            if (Health > 0)
            {
                HP_Ui.value = Health;
                Fill.color = gradient.Evaluate(HP_Ui.normalizedValue);
            }
            else
                HP_Ui.value = 0;


            collision.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
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

                if (_attackType == ChildAttackType.MakeCube)
                {
                    AttackCube(cubeAttack);
                    Attack = false;
                    cubeAttack = !cubeAttack;
                }

                if (_attackType == ChildAttackType.MakeGround)
                {
                    BossGroundOn();
                    Attack = false;
                }
            }
        }

        if (Health < 0 && !HealthOut)
        {
            Back = true;
            HealthOut = true;
        }
    }


    private void AttackBullet()
    {
        var target =Instantiate(_playerTarget, _player.transform.position, _player.transform.rotation);
        Destroy(target,15f);
        
        foreach (var Bullet in BulletPoint)
        {
            var Obj = Instantiate(BulletPrefabs, Bullet.position, Bullet.rotation);
            Obj.GetComponent<BossBullet>().BulletPoint = target.gameObject.transform;
        }
    }

    private void AttackCube(bool pattern)
    {
        for (int i = 0; i < CubeSpawnPoint.Length; i++)
        {
            if (pattern && i % 2 == 0)
            {
                var obj = Instantiate(CubePrefabs, CubeSpawnPoint[i].position, CubeSpawnPoint[i].rotation);
                obj._toMove = CubeTargetPoint[i];
                Destroy(obj.gameObject,15f);
            }
            else if(!pattern && i % 2 != 0)
            {
                var obj = Instantiate(CubePrefabs, CubeSpawnPoint[i].position, CubeSpawnPoint[i].rotation);
                obj._toMove = CubeTargetPoint[i];
                Destroy(obj.gameObject,15f);
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
}
