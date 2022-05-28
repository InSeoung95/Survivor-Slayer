using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorOpen : MonoBehaviour
{
    private bool isOpen;
    private Animator anim;
    public PlayableDirector Stage1Start;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            isOpen = true;

            anim.SetBool("Open", true);
        }

        StartCoroutine(DoorClose());

        IEnumerator DoorClose()
        {
            yield return new WaitForSeconds(2f);
            anim.SetBool("Open", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Stage1Start.Play();
        }
    }
}
