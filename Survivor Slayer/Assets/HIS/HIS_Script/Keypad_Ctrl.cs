using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad_Ctrl : MonoBehaviour
{
    [SerializeField]
    private GameObject KeypadUI; // 활성화할 키패드 UI. 이전 씬에서 넘어온 캔버스 할당
    public Text NumDisplayTxt; 
    public string Answer; // 정답
    private int[] InputedNum=new int[6]; // 씬에서 입력받을 숫자 배열. 6자리가 최대
    private string CompareNum; //정답과 비교할 숫자

    public bool Correct; // 정답을 맞출 때 true

    public InteractDoor Door;

    public KeypadClue clue1;
    public KeypadClue clue2;
    public KeypadClue clue3;

    public string UI_ClickSound;
    public string CorrectSoound;
    public string WrongSound;

    private void Start()
    {
        Answer = clue1.answer + clue2.answer + clue3.answer;
        //KeypadUI = GameObject.Find(" Keypad UI");
    }

    private void OnEnable() // 활성화 시 변수들 초기화
    {
       for(int i=0;i<InputedNum.Length;++i)
        {
            InputedNum[i] = -1;
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
            if(InputedNum[i]==-1)
            {
                InputedNum[i] = number;
                NumDisplayTxt.text += number.ToString();
                SoundManager.instance.PlayEffectSound(UI_ClickSound);
                return;
            }
        }
            return; // 여섯자리 넘으면 더 이상 입력 안되게
    }
  
    public void Delete() // 키패드 숫자 지울 때 
    {
        for(int i=InputedNum.Length-1;i>=0;--i)
        {
            if(InputedNum[i]!=-1)
            {
                InputedNum[i] = -1;
                NumDisplayTxt.text = "";
                SoundManager.instance.PlayEffectSound(UI_ClickSound);
                for (int j=0;j<i;++j)
                {

                    NumDisplayTxt.text += InputedNum[j].ToString();
                }
                return;
            }
        }
    }
    
    public void Enter() // 키패드 입력 숫자 확인.
    {
        SoundManager.instance.PlayEffectSound(UI_ClickSound);

        foreach (int num in InputedNum)
        {
            CompareNum += num;
        }

        Debug.Log("입력된 숫자: " + CompareNum);

        if(Answer==CompareNum)
        {
            Correct = true;
            Debug.Log("정답임다");
            //정답 UI 사운드
            SoundManager.instance.PlayEffectSound(CorrectSoound);
            //InteractDoor.GetComponent<Animator>().SetBool("Open", true);
            Door.Activate = true;
            Exit();
        }
        else
        {
            Debug.Log("틀렷슴다");
            SoundManager.instance.PlayEffectSound(WrongSound);
            CompareNum = ""; // 초기화
            //오답 UI 사운드
        }

    }

    public void Exit()
    {
        SoundManager.instance.PlayEffectSound(UI_ClickSound);

        KeypadUI.SetActive(false);
        Cursor.visible = false;
        UIManager.instance.OnInteract = false;
    }
}
