using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    [SerializeField] private GameObject _3rdUI;
    
    public BossZombie _boss;
    public BossChild[] _BossChild;
    public BossFloor _floor;
    private GameObject _playerObj;
    private CameraShake _camera;
    
    private bool PatternStart;
    private float Timer;
    private float TimeCycle = 10f;
    private int bossPattern = 0;

    public AudioClip BossBGM;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.BGM_Sound.clip = BossBGM;
            SoundManager.instance.Audiosource_BGM.Stop();
            SoundManager.instance.Audiosource_BGM.volume = 0.4f;
            PatternStart = true;
            _playerObj = other.gameObject;
            _3rdUI.gameObject.SetActive(true);
            _camera = other.gameObject.GetComponentInChildren<CameraShake>();
            _boss.Target = _playerObj;
            foreach (var child in _BossChild)
            {
                child._player = _playerObj;
                child.ChangeTarget(true);
                child.AttackRun = true;
            }
            
            StartCoroutine(_boss.FirstJump());
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
