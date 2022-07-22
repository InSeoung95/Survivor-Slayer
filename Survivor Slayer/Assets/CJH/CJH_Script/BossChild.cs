using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject _player;
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private BossAttackCube CubePrefabs;    // 보스용 공격큐브
    [SerializeField] private GameObject BulletPrefabs;      // 보스용 총알 프리팹
    public Transform[] BulletPoint;                         // 보스용 총알 생성될 위치
    public Transform[] CubeSpawnPoint;                      // 공격 큐브 생성위치
    public Transform[] CubeTargetPoint;                     // 공격 큐브 움직일 위치
    private bool cubeAttack;                                // 홀짝으로 공격할 패턴 2분화

    public BossFloor Ground;                             // 장판효과
    

    private void Update()
    {
        if (AttackRun)
        {
            if (_attackType == ChildAttackType.MakeBullet)
            {
                AttackBullet();
                AttackRun = false;
            }

            if (_attackType == ChildAttackType.MakeCube)
            {
                AttackCube(cubeAttack);
                AttackRun = false;
                cubeAttack = !cubeAttack;
            }
            
            if (_attackType == ChildAttackType.MakeGround)
            {
                Ground._Trigger = true;
                AttackRun = false;
            }
        }
    }


    private void AttackBullet()
    {
        var target =Instantiate(_playerTarget, _player.transform.position, _player.transform.rotation);
        Destroy(target,10f);
        
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
}
