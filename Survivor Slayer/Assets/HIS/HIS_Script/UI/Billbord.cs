using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billbord : MonoBehaviour
{
    [SerializeField]
    private Transform MainCamera;

    private void Start()
    {
        MainCamera = FindObjectOfType<Camera>().transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position+MainCamera.forward);
    }
}
