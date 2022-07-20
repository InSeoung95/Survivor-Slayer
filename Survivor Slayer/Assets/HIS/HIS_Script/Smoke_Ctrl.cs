using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Smoke_Ctrl : MonoBehaviour
{
    private VisualEffect SmokeEffect;
    public float FlickerTime; //연기가 나왔다가 안나왓다가 할 시간.
    private BoxCollider SmokeCollier; // 스모크 이펙트 활성화 시 데미지 줄 콜라이더.


    // Start is called before the first frame update
    void Start()
    {
        SmokeEffect = GetComponent<VisualEffect>();
        SmokeCollier = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FlickerSmoke()
    {

    }
}
