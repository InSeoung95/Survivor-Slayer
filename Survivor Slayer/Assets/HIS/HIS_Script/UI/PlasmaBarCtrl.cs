using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlasmaBarCtrl : MonoBehaviour
{
    public Slider PlasmaUI;
    public Gradient Gradient;
    public Image Fill;

   

    public void SetPlasmaGage(int poewerGage)
    {
        PlasmaUI.value += poewerGage;

        Fill.color = Gradient.Evaluate(PlasmaUI.normalizedValue);
    }
    public void SetMaxGage(int value)// 플라즈마UI의 Max 게이지.
    {
        PlasmaUI.maxValue = value;
    }
}
