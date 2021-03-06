using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.Universal;
//using UnityEngine.VFX;

public class BaseManager : MonoBehaviour
{
    [Header("Base관리")]

    public Base[] bases = new Base[4];

    [Range(0, 3)]

    public int Current_BaseLevel = 0; //현재 플레이어가 점령한 베이스 수


    [Header("라이트 관리")]
    public GameObject Directional_Light;
    public GameObject[] Dynamic_LightGroup; // 동적 라이트 오브젝트 그룹
    [SerializeField]
    private List<Light> lights = new List<Light>();// 관리할 라이트들 리스트
    private float light_intencity;//삽입할 라이트 강도

    //베이스 레벨 별 적용할 라이트 강도
    [SerializeField] private float lv0_intencity;
    [SerializeField] private float lv1_intencity;
    [SerializeField] private float lv2_intencity;
    [SerializeField] private float lv3_intencity;

    [Header("포스트 프로세싱 관리")]
    public Volume Gv;// 글로벌 볼륨
    private Vignette vignette;
    public ReflectionProbe gRp;// 글로벌 반사 프로브

    [Header("이펙트 관리")]

    public GameObject[] fog_group;
    private ParticleSystem[] fog_effect;

    //빌드 위한 임시 코딩. 추후 삭제 및 수정
    public int PlayerOcuupy; //플레이어가 점령한 거점 개수.
    public GameObject Clear; // 클리어 활성화
    public GameObject ActivateDoor; // 활성화시킬 문.
    public GameObject LeftDoor;
    public GameObject RightDoor;

    private void Awake()
    {

        for (int i = 0; i < Dynamic_LightGroup.Length; ++i)
        {
            lights.AddRange(Dynamic_LightGroup[i].GetComponentsInChildren<Light>());
        
        }
        Directional_Light.SetActive(false); //디렉셔널 라이트 끄기

        if(Gv.profile.TryGet<Vignette>(out vignette))
        {

        }
        fog_effect = new ParticleSystem[fog_group.Length];

        for(int i=0;i<fog_group.Length;++i)
        {
            fog_effect[i]= fog_group[i].GetComponentInChildren<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //BaseLvUpdate();
        LightUpdate();
        PostProcessingUpdate();
        EffectUpdate();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)&&Current_BaseLevel<3)
        {
            Current_BaseLevel++;
            Debug.Log("Base LV UP: " + Current_BaseLevel);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus)&&Current_BaseLevel>0)
        {
            Current_BaseLevel--;
            Debug.Log("Base LV Down: " + Current_BaseLevel);
        }

        if(PlayerOcuupy>=2) // 플레이어가 거점 2개 이상 점령 시
        {
            //Debug.Log("스테이지 1 클리어!!");
            //Clear.SetActive(true);
            ActivateDoor.GetComponent<InteractDoor>().Activate = true;// 상호작용 가능하게 활성화.
            LeftDoor.layer = 11;//SeeTrough 레이어로 변경.
            RightDoor.layer = 11;
        }
            
    }

    public void LightUpdate() // 라이트 변수들 업데이트
    {
        switch (Current_BaseLevel)
        {
            case 0:
                {
                    light_intencity = lv0_intencity;
                }
                
                break;
            case 1:
                {
                    light_intencity = lv1_intencity;
                }
               
                break;
            case 2:
                {
                    light_intencity = lv2_intencity;
                }
                break;
            case 3:
                {
                    light_intencity = lv3_intencity;
                }
                break;
        }

        if (Current_BaseLevel < 3)
            Directional_Light.SetActive(false);
        else
            Directional_Light.SetActive(true);

        foreach (Light light in lights)
        {
            light.intensity = light_intencity;

        }
    }

    public void PostProcessingUpdate() // 포스트 프로세싱 관련 업데이트
    {
       
        switch (Current_BaseLevel)
        {
            case 0:
                {
                    vignette.intensity.value = 0.7f;
                    gRp.intensity = 0.5f;
                    break;
                }
                
            case 1:
                {
                    vignette.intensity.value = 0.5f;
                    gRp.intensity = 0.7f;
                    break;
                }
               
            case 2:
                {
                    vignette.intensity.value = 0.3f;
                    gRp.intensity = 1f;
                    break;
                }
                
            case 3:
                {
                    vignette.intensity.value = 0f;
                    gRp.intensity = 1f;
                    break;
                   
                }
                
        }
    }

    public void EffectUpdate()// 이펙트 관련 업데이트
    {
        Color fog_effect_color;
        for (int i=0; i<fog_group.Length;++i)
        {
           
            fog_effect_color = fog_effect[0].startColor;

            if (Current_BaseLevel != 3)
                fog_effect[i].Play();
            switch (Current_BaseLevel)
            {
                case 0:
                    fog_effect_color.a = 1f;
                    break;
                case 1:
                    fog_effect_color.a = 0.5f;
                    break;
                case 2:
                    fog_effect_color.a = 0.2f;
                    break;
                case 3:
                    fog_effect[i].Stop();
                    break;
            }
            fog_effect[i].startColor = fog_effect_color;
        }
        

    }
}
