using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BossBase : MonoBehaviour
{
    public enum State  //인성 수정* BaseManager에서 사용하려고 public 선언.
    {
        Idle,                   // 게임 시작시 기본상태
        Player_Occupation,      // 플레이어가 점령한 상태
    }
    private float baseTimer;     //플레이어가 점령할때 필요한 시간타이머
    public State state { get; private set; } = State.Idle;
    public BossChild _BossChild;
    
    [SerializeField] private GameObject field; // 적이 점령시 보이는 베이스 필드.

    public Material player_occu; // 플레이어가 점령했을 때 메테리얼

    [SerializeField] private string occupying_sound;// 플레이어가 거점 점령 중일 때 나는 사운드
    [SerializeField] private string occupied_sound;// 플레이어가 거점 점령 완료 시 나는 사운드
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !(state == State.Player_Occupation))
        {
            SoundManager.instance.PlayEffectSound(occupying_sound, true); //점령 중 사운드 루프 재생
            
            _BossChild._player = other.gameObject;      //테스트용 나중에 지우기
            _BossChild.ChangeTarget(true);              //테스트용 나중에 지우기
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && state != State.Player_Occupation)
        {
            baseTimer += Time.deltaTime;
            UIManager.instance.BaseOccupationUI(baseTimer);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && state != State.Player_Occupation)
        {
            SoundManager.instance.StopEffectSound(occupying_sound);//점령 중 사운드 중지.
            StartCoroutine(BaseOccu_UI_Off());
        }
            
    }
    IEnumerator BaseOccu_UI_Off() // 0.5초 뒤에 UI 종료.
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.BaseOccu_UI.SetActive(false);
    }

    private void Update()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if (baseTimer > 5) // 
        {
            Debug.Log("플레이어 점령.");
            baseTimer = 0;
            state = State.Player_Occupation;
            Debug.Log("현재 상태"+state);
            UIManager.instance.BaseOccu_UI.SetActive(false);
            field.SetActive(false);

            _BossChild.SpinBody = true;
            _BossChild.ChangeTarget(false);

            if (_BossChild._attackType == BossChild.ChildAttackType.MakeGround)
            {
                _BossChild.Ground.ChangePoisonToHeal();
            }
            
            SoundManager.instance.StopEffectSound(occupying_sound);//점령 중 사운드 중지
            SoundManager.instance.PlayEffectSound(occupied_sound); // 점령 완료 사운드 출력
        }
    }
    
}
