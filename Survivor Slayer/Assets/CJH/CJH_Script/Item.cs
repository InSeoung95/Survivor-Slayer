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
    
    private float MovePos = 1f;           // 아이템이 이동할 값
    private float upMovMax = 5f;          // 아이템이 최대로 올라갈 값
    private float downMovMax = -5f;       // 아이템이 최대로 내려갈 값
    
}
