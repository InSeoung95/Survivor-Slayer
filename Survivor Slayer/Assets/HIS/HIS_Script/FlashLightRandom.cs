using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightRandom : MonoBehaviour
{
    private Light light;

    private float max_intencity;
    private float current_intencity;
    private float target_intencity;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        max_intencity = light.intensity;
        current_intencity = light.intensity;
        target_intencity = Random.Range(0f, max_intencity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(target_intencity-current_intencity)>=0.01)
        {
            if (target_intencity - current_intencity >= 0)
            {
                current_intencity += Time.deltaTime * 10f;
            }
            else
                current_intencity -= Time.deltaTime+10f;

            light.intensity = current_intencity;
        }
        else
        {
            target_intencity = Random.Range(0f, max_intencity);

        }
    }
}
