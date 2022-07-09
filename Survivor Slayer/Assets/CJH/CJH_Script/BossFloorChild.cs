using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloorChild : MonoBehaviour
{
    public MeshRenderer _Renderer;
    public bool FloorTrigger;
    private float effectTime = 10f;
    private float Timer;
    private Player_Info _info;

    private void Start()
    {
        _Renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (FloorTrigger)
        {
            Timer += Time.deltaTime;
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (FloorTrigger)
        {
            if (collisionInfo.gameObject.CompareTag("Player"))
            {
                Timer = 0;
                _info = collisionInfo.gameObject.GetComponent<Player_Info>();
                _info.currenthealth -= 10f;
            }
        }
    }

    public void ChangeMaterials(Material[] _materials)
    {
        _Renderer.sharedMaterials = _materials;
    }
    
}
