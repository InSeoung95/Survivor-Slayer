using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Ammo,
        HealthPack,
        PowerGage,
        Psycho,
    };

    public ItemType itemType;
    public int value;

    private const float ITEM_MOVE_MAX = 3f;     // 아이템 상하이동 최대값
    private Vector3 UpMaxPos;
    private Vector3 DownMaxPos;
    private Vector3 Pos;
    Vector3 velo = Vector3.zero;
    
    private bool vec;       // up : true, down : false

    private void OnEnable()
    {
        Pos = transform.position;
        UpMaxPos = Pos + new Vector3(0, ITEM_MOVE_MAX, 0);
        DownMaxPos = Pos + new Vector3(0, -ITEM_MOVE_MAX, 0);
        vec = true;
    }

    private void Update()
    {
        
        if (vec)
        {
            transform.position = Vector3.SmoothDamp(transform.position, UpMaxPos,ref velo, 0.9f);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, DownMaxPos,ref velo, 0.9f);
        }

        if (transform.position.y >= UpMaxPos.y -1)
            vec = false;
        else if (transform.position.y <= DownMaxPos.y +1)
            vec = true;

    }
    
}
