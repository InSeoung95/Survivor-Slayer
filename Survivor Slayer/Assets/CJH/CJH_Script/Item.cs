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
}
