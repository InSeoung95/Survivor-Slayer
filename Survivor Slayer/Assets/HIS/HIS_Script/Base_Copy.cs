using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Copy : MonoBehaviour
{
    public enum State  //인성 수정* BaseManager에서 사용하려고 public 선언.
    {
        Idle,                   // 게임 시작시 기본상태
        Player_Occupation,      // 플레이어가 점령한 상태
        Enemy_Occupation        // 적이 점령한 상태
    }
    private float baseTimer;     //플레이어가 점령할때 필요한 시간타이머
    public float baseHealth = 100;    //적이 거점점령할때 필요한 체력   *테스트용으로 100으로 설정 나중에 수정필요
    //[SerializeField] private State state = State.Idle; //재혁님이 작성하신 코드. 전 다른 클래스에서 사용하게 pulic으로 선언해서 사용 좀 할게요.
    //인성 수정
    public State state { get; private set; } = State.Idle;

    public PlayerInfo _PlayerInfo;      // 거점 점령시 플레이어에게 재화를 넘겨줌
    private bool baseRun = false;       // 플레이어가 거점 점령시 재화를 얻기 시작하는 판정
    //[SerializeField]private Material test_mat;      //단순테스트용 마테리얼;
    //인성 추가
    [SerializeField] private GameObject shield; // 플레이어 점령시 보이는 쉴드 효과
    [SerializeField] private GameObject field; // 적이 점령시 보이는 베이스 필드.

    public Material player_occu; // 플레이어가 점령했을 때 메테리얼
    public Material enemy_occu;  // 적이 점령했을 때 메테리얼

    private void Awake()
    {
        //test_mat.color = Color.white;
        //mainRender = GetComponent<MeshRenderer>().material;
        StartCoroutine(BasePointTime());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !(state == State.Player_Occupation))
            baseTimer += Time.deltaTime;
    }

    private void Update()
    {
        ChangeState();
        BasePointUP();
    }

    private void ChangeState()
    {
        if (baseTimer > 15)
        {
            baseTimer = 0;
            state = State.Player_Occupation;
            //test_mat.color = Color.blue;
            //인성 추가
            this.GetComponent<MeshRenderer>().material = player_occu;
            shield.SetActive(true);
            field.SetActive(false);
            this.gameObject.layer = 0;// default 레이어로 변경
        }
        else if (baseHealth <= 0)
        {
            Debug.Log("Enemy 점령");
            baseHealth = 100;
            state = State.Enemy_Occupation;
            //test_mat.color = Color.red;
            //인성 추가
            this.GetComponent<MeshRenderer>().material = enemy_occu;
            shield.SetActive(false);
            field.SetActive(true);
            this.gameObject.layer = 11;//SeeThrough 레이어로 변경.
        }
    }

    private void BasePointUP()
    {
        if (state == State.Player_Occupation)
            baseRun = true;
        else
            baseRun = false;
    }

    IEnumerator BasePointTime()
    {
        while (true)
        {
            if (baseRun)
                _PlayerInfo.currentBasePoint++;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
