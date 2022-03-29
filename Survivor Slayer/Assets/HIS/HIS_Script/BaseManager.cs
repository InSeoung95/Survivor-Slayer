using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [Header("Base관리")]
    public Base[] bases = new Base[4];

    [Range(0,3)]
    [SerializeField]
    private int Current_BaseLevel=0; //현재 플레이어가 점령한 베이스 수


    [Header("라이트 관리")]
    public GameObject[] Dynamic_LightGroup; // 동적 라이트 오브젝트 
    [SerializeField]
    private List<Light> lights = new List<Light>();// 관리할 라이트들 리스트
    private float light_intencity;//삽입할 라이트 강도

    //베이스 레벨 별 적용할 라이트 강도
    [SerializeField]private float level0_intencity;
    [SerializeField]private float level1_intencity;
    [SerializeField]private float level2_intencity;
    [SerializeField] private float level3_intencity;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0;i<Dynamic_LightGroup.Length;++i)
        {
            lights.AddRange(Dynamic_LightGroup[i].GetComponentsInChildren<Light>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        BaseStateUpdate();
        LightUpdate();
    }
    public void BaseStateUpdate()
    {
        Current_BaseLevel = 0;

       for(int i=0;i<bases.Length;++i)
        {
            if(bases[i].state==Base.State.Player_Occupation)
            {
                Current_BaseLevel++;
            }
        }
    }
    public void LightUpdate() // 
    {
        switch(Current_BaseLevel)
        {
            case 0:
                light_intencity = level0_intencity;
                break;
            case 1:
                light_intencity = level1_intencity;
                break;
            case 2:
                light_intencity = level2_intencity;
                break;
            case 3:
                light_intencity = level3_intencity;
                break;
        }
      

        foreach (Light light in lights)
        {
            light.intensity = light_intencity;
            
        }
    }
}
