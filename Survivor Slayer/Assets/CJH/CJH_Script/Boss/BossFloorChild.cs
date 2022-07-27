using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloorChild : MonoBehaviour
{
    public MeshRenderer _Renderer;
    public bool FloorTrigger;
    private bool _inPlayer;
    public bool floorType;     // true = Heal, false - Poison
    private bool AttackTrigger;
    private float effectTime = 1f;
    private float Timer;
    private PlayerInfo _info;

    private void Start()
    {
        _Renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (_inPlayer && FloorTrigger)
        {
            Timer += Time.deltaTime;
            if (Timer > effectTime)
            {
                AttackTrigger = true;
                Timer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _info = other.gameObject.GetComponent<PlayerInfo>();
            _inPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _inPlayer = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (AttackTrigger)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (floorType)
                {
                    if (_info.currenthealth < 99)
                        _info.currenthealth += 2f;
                }
                else
                {
                    _info.currenthealth -= 2f;
                    UIManager.instance.PlayerAttacked();
                }

                AttackTrigger = false;
                
            }
        }
    }

    public void ChangeMaterials(Material[] _materials)
    {
        _Renderer.sharedMaterials = _materials;
    }
    
}
