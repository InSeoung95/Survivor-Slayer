using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFind : MonoBehaviour
{
    public GameObject _GameObject;

    [RuntimeInitializeOnLoadMethod]
    private void Start()
    {
        _GameObject = GameObject.Find("DRONE");
    }
}
