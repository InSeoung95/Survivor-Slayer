using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Transform BulletPoint;       // 보스 총알 목적지
    [SerializeField] private float BULLET_DAMAGE = 10f;
    [SerializeField] private float BulletSpd;
    [SerializeField] private float Size;
    private PlayerInfo _player;
    public bool _type;                  // true - 플레이어를 공격 / false - 보스를 공격

    private void Start()
    {
        gameObject.transform.localScale = Vector3.one * Size;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, BulletPoint.position, BulletSpd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Player") && _type)
        {
            _player = other.gameObject.GetComponent<PlayerInfo>();
            _player.onDamaged = true;
            _player.OnDamage(BULLET_DAMAGE);
            UIManager.instance.PlayerAttacked();
            Destroy(gameObject);
        }
    }
}
