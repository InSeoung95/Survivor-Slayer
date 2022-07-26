using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public float TIME_CYCLE = 10f;
    
    public BossEnemy _Boss;
    public BossChild[] _BossChild;
    private GameObject _playerObj;
    private bool PatternStart;
    private float Timer;
    private float TimeCycle = 10f;
    private int bossPattern = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Boss.SpinBody = true;
            PatternStart = true;
            _playerObj = other.gameObject;
            foreach (var child in _BossChild)
            {
                child._player = _playerObj;
            }
        }
    }

    private void Update()
    {
        if (PatternStart)
        {
            Timer += Time.deltaTime;

            if (Timer > TimeCycle)
            {
                Timer = 0;
                TimeCycle += TIME_CYCLE;
                _Boss.CallAttack(bossPattern);
                bossPattern++;

                if (bossPattern > 2)
                {
                    foreach (var turret in  _Boss._turrets)
                    {
                        turret.BossType = true;
                    }
                    
                    Destroy(gameObject);
                }
            }
        }
    }
}
