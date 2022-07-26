using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상호작용하는 것들 활성화.
public class TriggerInteract : MonoBehaviour
{
    public GameObject Interact_Object; //활성화할 오브젝트
    public GameObject Interact_txt; // 표시할 상호작용 UI
    private bool ActiveInteract;
    public bool GetObject;
    public bool isUse;// 사용할 오브젝트인지.

    //private bool isReach; // 거리가 닿는

    
    // Update is called once per frame
    void Update()
    {
        if(ActiveInteract&&Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상호작용 오브젝트 활성화");
            GetObject = true;
            Interact_Object.SetActive(true);
            Interact_txt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            Interact_txt.SetActive(true);
            ActiveInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Interact_txt.SetActive(false);
            
            ActiveInteract = false;


            if(Interact_Object.activeSelf&&!isUse) // 사용할 물건이 아닐 때.
            {
                Interact_Object.SetActive(false);
            }
            
        }
    }
}
