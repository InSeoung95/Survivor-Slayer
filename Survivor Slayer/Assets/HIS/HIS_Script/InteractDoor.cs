using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : MonoBehaviour
{
    public bool Activate; // 상호작용 가능 여부.

    private Animator DoorAnim;

    private AudioSource ad;
    public AudioClip DoorOpen;
    public AudioClip DoorClose;

    public MeshRenderer FrontLight;
    public MeshRenderer BackLight;

    public Material OnActivate;
    public Material NotActivate;

    // Start is called before the first frame update
    void Start()
    {
        DoorAnim = GetComponent<Animator>();
        ad = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Activate)
        {
            FrontLight.material = OnActivate;
            BackLight.material = OnActivate;
        }
        else
        {
            FrontLight.material = NotActivate;
            BackLight.material = NotActivate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&Activate)
        {
            DoorAnim.SetBool("Open", true);
            ad.clip = DoorOpen;
            ad.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            DoorAnim.SetBool("Open", false);
            ad.clip = DoorClose;
            ad.Play();
        }
    }
}
