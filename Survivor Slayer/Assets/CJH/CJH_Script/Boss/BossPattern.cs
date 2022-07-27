using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public BossZombie _boss;
    public BossChild[] _BossChild;
    public BossFloor _floor;
    private GameObject _playerObj;
    private bool PatternStart;
    private float Timer;
    private float TimeCycle = 10f;
    private int bossPattern = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PatternStart = true;
            _floor._Trigger = true;
            _playerObj = other.gameObject;
            _boss.Target = _playerObj;
            foreach (var child in _BossChild)
            {
                child._player = _playerObj;
                child.ChangeTarget(true);
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
                Destroy(gameObject);
            }
        }
    }

}
