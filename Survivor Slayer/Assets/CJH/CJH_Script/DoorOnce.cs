using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOnce : MonoBehaviour
{
    [SerializeField] private InteractDoor _door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _door.Activate = false;
        }
    }
}
