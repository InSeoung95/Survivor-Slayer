using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light light;

    public float min_time;
    public float max_time;
    private float Timer;

    public bool AudioUse=false;
    private AudioSource audioSource;
    public AudioClip FlickerSound;

    [SerializeField]
    private bool isMaterialOn=true;
    public int materailNum=0; // 변경할 메쉬 렌더러 머테리얼의 배열 숫자
    private Material[] materials;

    public Material On; //켜질 때 활성화할 머테리얼
    public Material Off; // 꺼질 때 활성화할 머테리얼

    // Start is called before the first frame update
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        Timer = Random.Range(min_time, max_time);
        materials = gameObject.GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        Flicker();
    }

    void Flicker()
    {
        if(Timer>0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            light.enabled = !light.enabled;
            Timer = Random.Range(min_time, max_time);
            if(AudioUse)
            {
                //audioSource.PlayOneShot(FlickerSound);
            }

            isMaterialOn = !isMaterialOn;

            if (isMaterialOn)
                materials[materailNum] = On;
            else
                materials[materailNum] = Off;

            gameObject.GetComponent<MeshRenderer>().materials = materials;

        }
    }
}
