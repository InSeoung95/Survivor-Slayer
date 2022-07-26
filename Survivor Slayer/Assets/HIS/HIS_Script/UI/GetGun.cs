using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGun : MonoBehaviour
{
    public GameObject gun;
    public GameObject dumpedGun;// 버려진 놈
    public GameObject Interact_txt;
    public string GetSound;

    //활성화 할 UI
    public GameObject bullet_UI;

    private bool ActiveInteract;

    void Update()
    {
        if (ActiveInteract && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("총 활성화");
           
            gun.SetActive(true);
            bullet_UI.SetActive(true);
            dumpedGun.SetActive(false);
            Interact_txt.SetActive(false);

            SoundManager.instance.PlayEffectSound(GetSound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (other.CompareTag("Player"))
            {

                Interact_txt.SetActive(true);
                ActiveInteract = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact_txt.SetActive(false);

            ActiveInteract = false;
        }
    }
}
