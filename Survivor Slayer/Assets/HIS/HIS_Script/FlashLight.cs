using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    //총기 발사 이펙트시 잠시 라이트 번쩍 거리게
    private Light light;
    public float max_intencity; // 최대 밝기
    private float current_intencity;//현재 밝기
    
    [SerializeField]
    [Range(0.1f,10f)]
    private float flashTime; // 플래시가 터지는 시간.
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.intensity = 0f;
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }
    IEnumerator FlashCoroutine()
    {
        light.intensity = 0f;
        current_intencity = 0f;
        float currentTime = 0f;
        float percent = 0f;

        while(percent<1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / flashTime;

            current_intencity = Mathf.Lerp(0, max_intencity, percent);
            light.intensity = current_intencity;

            yield return null;
        }
        currentTime = 0f;
        percent = 0f;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / flashTime;

            current_intencity = Mathf.Lerp(max_intencity, 0, percent);
            light.intensity = current_intencity;

            yield return null;
        }
    }
   
}
