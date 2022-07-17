using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMapStart : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private void Start()
    {
        var _player = GameObject.Find("Player");
        _player.transform.position = gameObject.transform.position;
        if(_player)
            Destroy(gameObject,2);
    }

}
