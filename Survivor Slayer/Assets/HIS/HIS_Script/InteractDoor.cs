using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : MonoBehaviour
{
    public bool Activate; // 상호작용 가능 여부.

    private Animator DoorAnim;

    // Start is called before the first frame update
    void Start()
    {
        DoorAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&Activate)
        {
            DoorAnim.SetBool("Open", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            DoorAnim.SetBool("Open", false);
            
        }
    }
}
