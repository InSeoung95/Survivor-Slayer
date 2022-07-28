using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Start : MonoBehaviour
{
    public GameObject Stage2_UICanvas;
    public GameObject Stage3_UICanvas;
    public GameObject RemainTime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Stage2_UICanvas.SetActive(true);
            RemainTime.SetActive(true);
        }
    }
}
