using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlasmaBarCtrl : MonoBehaviour
{
    public Slider PlasmaUI;
    public Gradient Gradient;
    public Image Fill;
    public GunController gunController;
    public Gun gun;
   

    public void SetPlasmaGage(int poewerGage)
    {
        PlasmaUI.value += poewerGage;

        Fill.color = Gradient.Evaluate(PlasmaUI.normalizedValue);

        if(PlasmaUI.value==20) // 현재 슬라이드 밸류가 값이 20이 나올 때  
        {
            Debug.Log("gun.upgradeRate: "+gun.upgradeRate[2]);

        }
    }
    public void SetMaxGage(int value)// 플라즈마UI의 Max 게이지.
    {
        PlasmaUI.maxValue = value;
    }
}
