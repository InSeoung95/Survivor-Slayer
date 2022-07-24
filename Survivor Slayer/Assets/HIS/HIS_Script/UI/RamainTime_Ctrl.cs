using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RamainTime_Ctrl : MonoBehaviour
{
    public TextMeshPro remainTxt;
    public int LimitTime;// 스테이지 2 제한시간. 초 단위로 입력.

    float second;//초
    int minute; // 분

    public void CheckTime()
    {
        second = Time.deltaTime;

        LimitTime -= (int)Time.deltaTime;

        minute = (LimitTime / 60);
        //second=(LimitTime)
        
        remainTxt.text = string.Format("{0:D2},{1:D2}", minute, (int)second);

        remainTxt.text = minute + " : " + (int)second;

        if((int)second>59)
        {
            minute++;
        }
    }
}
