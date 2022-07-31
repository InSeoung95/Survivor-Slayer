using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextMapStart : MonoBehaviour
{
    public GameObject _baseOccu;            // 할당할 baseOcc
    public Slider _baseSlider;
    public TextMeshProUGUI _baseTxt;
    private float time;
    
    
    [RuntimeInitializeOnLoadMethod]
    private void Awake()
    {
        var _player = GameObject.Find("Player");
        _player.transform.position = gameObject.transform.position;

        var _UI = GameObject.Find("Main HUD Canvas").GetComponent<UIManager>();
        _UI.BaseOccu_UI = _baseOccu;
        _UI.occu_slider = _baseSlider;
        _UI.occu_txt = _baseTxt;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time < 1)
        {
            var _player = GameObject.Find("Player");
            _player.transform.position = gameObject.transform.position;
        }

        if(time>1)
            Destroy(gameObject,2);
    }
}
