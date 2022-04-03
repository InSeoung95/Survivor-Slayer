using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public AudioClip mainBGM;

    private AudioSource ad;
    void Start()
    {
        ad = GetComponent<AudioSource>();

        MainBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MainBGM()
    {
        ad.PlayOneShot(mainBGM);
    }
}
