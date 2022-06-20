using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상호작용하는 것들 활성화.
public class TriggerInteract : MonoBehaviour
{
    public GameObject Interact_Object; //활성화할 오브젝트
    public GameObject Interact_UI; // 표시할 상호작용 UI

    //private bool isReach; // 거리가 닿는

    // Update is called once per frame
    void Update()
    {
        if(Interact_UI.activeInHierarchy&&Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상호작용 오브젝트 활성화");
            Interact_Object.SetActive(true);
            Interact_UI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Interact_UI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            Interact_UI.SetActive(false);
        }
    }
}
