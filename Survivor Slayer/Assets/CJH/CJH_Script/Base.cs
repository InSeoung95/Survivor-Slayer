using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    enum State
    {
        Idle,                   // 게임 시작시 기본상태
        Player_Occupation,      // 플레이어가 점령한 상태
        Enemy_Occupation        // 적이 점령한 상태
    }
    private float baseTimer;     //플레이어가 점령할때 필요한 시간타이머
    public float baseHealth = 100;    //적이 거점점령할때 필요한 체력   *테스트용으로 100으로 설정 나중에 수정필요
    [SerializeField]private State state = State.Idle;
    public PlayerInfo _PlayerInfo;      // 거점 점령시 플레이어에게 재화를 넘겨줌
    private bool baseRun = false;       // 플레이어가 거점 점령시 재화를 얻기 시작하는 판정
    [SerializeField]private Material test_mat;      //단순테스트용 마테리얼;

    private void Awake()
    {
        test_mat.color = Color.white;
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
            test_mat.color = Color.blue;
        }
        else if (baseHealth <= 0)
        {
            baseHealth = 100;
            state = State.Enemy_Occupation;
            test_mat.color = Color.red;
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
            if(baseRun)
                _PlayerInfo.currentBasePoint++;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
