using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCube : MonoBehaviour
{
    public Transform _toMove;
    public bool move;
    private PlayerInfo _player;
    private bool InPlayerAttack;
    private float Timer;
    [SerializeField]private float TimerMax = 1f;

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > TimerMax && InPlayerAttack)
        {
            Attack();
        }
        if(move)
            ToMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject.GetComponent<PlayerInfo>();
            InPlayerAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InPlayerAttack = false;
        }
    }

    private void Attack()
    {
        _player.onDamaged = true;
        _player.HitBomb(10);
        Timer = 0;

        UIManager.instance.PlayerAttacked();
    }

    private void ToMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, _toMove.position, 0.05f);
    }
}
