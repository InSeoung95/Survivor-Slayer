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
    private CameraShake _camera;
    
    private bool PatternStart;
    private float Timer;
    private float TimeCycle = 10f;
    private int bossPattern = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PatternStart = true;
            _playerObj = other.gameObject;
            _camera = other.gameObject.GetComponentInChildren<CameraShake>();
            _boss.Target = _playerObj;
            foreach (var child in _BossChild)
            {
                child._player = _playerObj;
                child.ChangeTarget(true);
                child.AttackRun = true;
            }

            StartCoroutine(_camera.Shake(1f,2f,1.5f));
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
