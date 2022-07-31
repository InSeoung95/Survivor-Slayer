using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Start : MonoBehaviour
{
    public GameObject Stage2_UICanvas;
    public GameObject Stage3_UICanvas;
    public GameObject RemainTime;
    public AudioClip CountDown;

    public AudioClip Stage2Clip;

    private bool playOnce;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!playOnce)
        {
            Stage2_UICanvas.SetActive(true);
            RemainTime.SetActive(true);
            SoundManager.instance.BGM_Sound.clip = Stage2Clip;
            SoundManager.instance.Audiosource_BGM.Stop();
            playOnce = true;
        }
    }
}
