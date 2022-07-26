using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : MonoBehaviour
{
    public bool Activate; // 상호작용 가능 여부.
    private bool _Indoor;
    private bool _OpenTrigger;
    private bool _DoorOpenTrigger;

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
            FrontLight.GetComponentInChildren<Light>().color = Color.green;
            BackLight.GetComponentInChildren<Light>().color = Color.green;
        }
        else
        {
            FrontLight.material = NotActivate;
            BackLight.material = NotActivate;
            FrontLight.GetComponentInChildren<Light>().color = Color.red;
            BackLight.GetComponentInChildren<Light>().color = Color.red;
        }

        if (_Indoor && Activate && _OpenTrigger)
        {
            DoorAnim.SetBool("Open", true);
            ad.clip = DoorOpen;
            ad.Play();
            _OpenTrigger = false;
        }

        if (!_Indoor && Activate && _OpenTrigger)
        {
            DoorAnim.SetBool("Open", false);
            ad.clip = DoorClose;
            ad.Play();
            _OpenTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _Indoor = true;
            _OpenTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _Indoor = false;
            _OpenTrigger = true;
        }
    }

    IEnumerator DelayOpen()
    {
        yield return new WaitForSeconds(3);
        if(!_Indoor)
            _OpenTrigger = true;
    }
}
