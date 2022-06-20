using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Ctrl : MonoBehaviour
{
    public GameObject KeypadUI; // 활성화할 키패드 UI
    public Text NumDisplayTxt; 
    public string Answer; // 정답
    private int[] InputedNum=new int[6]; // 씬에서 입력받을 숫자 배열. 6자리가 최대
    private string CompareNum; //정답과 비교할 숫자

    private bool Correct; // 정답을 맞출 때 true

    public Animator doorAnim;
    

    private void OnEnable() // 활성화 시 변수들 초기화
    {
       for(int i=0;i<InputedNum.Length;++i)
        {
            InputedNum[i] = 0;
        }
        CompareNum = "";
        NumDisplayTxt.text = "";

        Cursor.visible = true;
        UIManager.instance.OnInteract = true;
        Debug.Log("키패드 켜짐");
    }
   

    public void NumOnClick(int number) // 키패드 숫자 눌릴 때
    {
        for(int i=0; i<InputedNum.Length;++i)
        {
            if(InputedNum[i]==0)
            {
                InputedNum[i] += number;
                NumDisplayTxt.text += number.ToString();
                return;
            }
        }
            return; // 여섯자리 넘으면 더 이상 입력 안되게
    }
  
    public void Delete() // 키패드 숫자 지울 때 
    {
        for(int i=InputedNum.Length-1;i>=0;--i)
        {
            if(InputedNum[i]!=0)
            {
                InputedNum[i] = 0;
                NumDisplayTxt.text = "";
                for(int j=0;j<i;++j)
                {
                    NumDisplayTxt.text += InputedNum[j].ToString();
                }
                return;
            }
        }
    }
    
    public void Enter() // 키패드 입력 숫자 확인.
    {
        foreach(int num in InputedNum)
        {
            CompareNum += num;
        }

        Debug.Log("입력된 숫자: " + CompareNum);

        if(Answer==CompareNum)
        {
            Correct = true;
            Debug.Log("정답임다");
            //정답 UI 사운드
            //InteractDoor.GetComponent<Animator>().SetBool("Open", true);
            doorAnim.SetBool("Open", true);
            Exit();
        }
        else
        {
            Debug.Log("틀렷슴다");
            CompareNum = ""; // 초기화
            //오답 UI 사운드
        }

    }

    public void Exit()
    {
        KeypadUI.SetActive(false);
        Cursor.visible = false;
        UIManager.instance.OnInteract = false;
    }
}
