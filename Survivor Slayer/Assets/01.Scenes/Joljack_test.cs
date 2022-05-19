using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joljack_test : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject GameOver;
    public GameObject button;
    public GameObject MapCanvas;
    public BaseManager _BaseManager;
    public Gun _Gun;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            button.SetActive(!button.active);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _Gun.carryBulletCount = 120;
        }
        
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if(_BaseManager.Current_BaseLevel<3)
                _BaseManager.Current_BaseLevel++;
        }
        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if(_BaseManager.Current_BaseLevel>0)
                _BaseManager.Current_BaseLevel--;
        }
    }
    
    public void OnPauseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0.0f : 1.0f;
        MapCanvas.SetActive(!isPaused);
        
        GameOver.SetActive(isPaused);
        
    }
}
