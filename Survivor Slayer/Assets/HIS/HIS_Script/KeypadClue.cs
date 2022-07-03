using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadClue : MonoBehaviour
{
    public string answer;
    private int RandomNum;

    public TextMeshProUGUI ClueUI_Txt;

    // Start is called before the first frame update
    void Start()
    {
        answer = "";
        RandomNum = Random.Range(1, 100);// 1부터 99까지 랜덤.
        if(RandomNum<10)
        {
            answer = "0";
        }
        answer += RandomNum.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            ClueUI_Txt.text = answer;
        }
    }
}
