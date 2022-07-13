using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOM : MonoBehaviour
{
    private CameraShake _camera;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _camera = collision.gameObject.GetComponentInChildren<CameraShake>();
            StartCoroutine(_camera.Shake(2f,10f));
        }
    }
}
