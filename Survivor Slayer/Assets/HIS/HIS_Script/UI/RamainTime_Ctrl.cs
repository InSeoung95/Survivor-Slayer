using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RamainTime_Ctrl : MonoBehaviour
{
    public TextMeshProUGUI remainTxt;
    public float LimitTime;// 스테이지 2 제한시간. 초 단위로 입력.

    float second;//초
    int minute; // 분

    private void Update()
    {
        CheckTime();
    }

    public void CheckTime()
    {
        LimitTime -= Time.deltaTime;

        minute = (int)(LimitTime / 60);

        second = (LimitTime % 60);
        remainTxt.text = "0"+minute + " : " + (int)second;

        if (LimitTime < 120&&LimitTime>60)
            remainTxt.color = Color.yellow;
        else if (LimitTime < 60)
            remainTxt.color = Color.red;
    }
}
