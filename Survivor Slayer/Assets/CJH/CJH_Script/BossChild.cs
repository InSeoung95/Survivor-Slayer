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
    [SerializeField] private GameObject BulletPrefabs;      // 보스용 총알 프리팹
    public Transform[] BulletPoint;                         // 보스용 총알 생성될 위치

    private void Update()
    {
        if (AttackRun)
        {
            if (_attackType == ChildAttackType.MakeBullet)
            {
                AttackBullet();
                AttackRun = false;
            }
        }
    }


    private void AttackBullet()
    {
        foreach (var Bullet in BulletPoint)
        {
            var Obj = Instantiate(BulletPrefabs, Bullet.position, Bullet.rotation);
            Obj.GetComponent<BossBullet>().BulletPoint = _player.gameObject.transform;
        }
    }
}
