using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAround : MonoBehaviour
{
    public bool InPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InPlayer = false;
        }
    }
}
