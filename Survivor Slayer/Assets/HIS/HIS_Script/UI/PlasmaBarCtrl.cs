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

        if(PlasmaUI.value==PlasmaUI.maxValue) // 현재 맥스 Value값에 달하면  
        {
            if(gun.PlasmaBombCount[0]>gunController.PlasmaFireRate)
            {
                gunController.PlasmaFireRate++;
            }
            
        }
    }
    public void SetMaxGage(int value)// 플라즈마UI의 Max 게이지.
    {
        PlasmaUI.maxValue = value;
    }
}
