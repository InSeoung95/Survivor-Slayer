using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public float maxHealth = 100;
    public float currenthealth = 100;
    public float maxPower = 100;
    public float currentPower = 0;
    public float maxPsycho = 100;
    public float currentPsycho = 0;
    
    public float currentBasePoint = 0;      // ???????–ì ™??ì”  ?¨ê¾©???ë³?¿¬???????ë’— ?ë¨?
        
    public Slider HPSlider;
    public Text HPText;
    //?¸ì„± ì¶”ê?
    public bool onDamaged = false; // ?Œë ˆ?´ì–´ê°€ ?°ë?ì§€ ë°›ëŠ” ?íƒœ?¸ì?
    //
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
