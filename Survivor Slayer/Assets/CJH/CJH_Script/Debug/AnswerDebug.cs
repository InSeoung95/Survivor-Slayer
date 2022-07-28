using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerDebug : MonoBehaviour
{
    public Keypad_Ctrl _Keypad;
    public TextMeshProUGUI ClueUI_Txt;
    public GameObject _Answer;
    private bool active;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ClueUI_Txt.text = _Keypad.Answer;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ClueUI_Txt.text = _Keypad.Answer;
            _Answer.gameObject.SetActive(!active);
            active = !active;
        }
    }
}
