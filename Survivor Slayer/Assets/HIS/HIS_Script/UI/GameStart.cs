using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void ClickStart()
    {
        SceneManager.LoadScene("Main_Game");
    }
    public void ClickExit()
    {
        Application.Quit();
    }
    
}
