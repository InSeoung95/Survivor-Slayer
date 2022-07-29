using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieOnable : MonoBehaviour
{
    public GameObject _body;

    private void OnEnable()
    {
        _body.SetActive(true);
    }
}
