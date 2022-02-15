using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currenthealth = 100;
    public Slider HPSlider;
    public Text HPText;

    private void Awake()
    {
        HPSlider.maxValue = maxHealth;
        HPSlider.value = currenthealth;
    }

    private void Update()
    {
        HPSlider.value = currenthealth;
        HPText.text = currenthealth.ToString();
    }
}
