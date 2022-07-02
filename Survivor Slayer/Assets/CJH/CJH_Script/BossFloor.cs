using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloor : MonoBehaviour
{
    private Player_Info _onPlayerInfo;
    [SerializeField] private Material[] _materials;
    private MeshRenderer _floorRenderer;
    public bool _Trigger;
    private int _state;                 // 발판에 따른 효과분류용
    private float Timer;
    [SerializeField]private float TimerMax = 10f;           // 

    private void Start()
    {
        _floorRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _onPlayerInfo = collision.gameObject.GetComponent<Player_Info>();
            _floorRenderer.sharedMaterials = _materials;
        }
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        if (_Trigger && Timer>TimerMax)
        {
            Timer = 0;
            OnEffect();
        }
    }

    private void OnEffect()
    {
        switch (_state)
        {
            case 1:
                _onPlayerInfo.currenthealth -= 10;
                break;
            case 2:
                break;
        }
    }

    public void FloorTriggerOn()
    {
        _Trigger = true;
    }
}