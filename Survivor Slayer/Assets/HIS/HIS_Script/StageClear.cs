using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear : MonoBehaviour
{
    public string GameClearSound;

    private void OnEnable()
    {
        StartCoroutine(Quit());
    }
    IEnumerator Quit()
    {
        Time.timeScale = 0;
        SoundManager.instance.DoPlay = false;



        //Debug.Log("스테이지 클리어!!");
       

        SoundManager.instance.PlayEffectSound(GameClearSound);
        yield return new WaitForSeconds(5f);
        Debug.Log("게임 종료");
        Application.Quit();
    }
  
    /*
    public void GameBack()
    {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
    }
    public void GameOfffff()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
     */
}
