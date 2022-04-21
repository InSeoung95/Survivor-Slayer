using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(flash());
    }
    IEnumerator flash()
    {
        while(true)
        {
            text.text = "";
            yield return new WaitForSeconds(0.5f);
            text.text = "점령 중...";
            yield return new WaitForSeconds(0.5f);
        }
    }
   
}
