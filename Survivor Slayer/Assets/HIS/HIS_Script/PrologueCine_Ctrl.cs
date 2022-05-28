using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueCine_Ctrl : MonoBehaviour
{
    //public Transform MainCamera; // 카메라 초기 위치.

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finish()
    {
        Debug.Log("카메라 리셋");
        //MainCamera.transform.position = new Vector3(0, 0, 0); // 메인 카메라 위치 초기화
    }
}
