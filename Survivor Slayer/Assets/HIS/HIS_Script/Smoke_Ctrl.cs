using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Smoke_Ctrl : MonoBehaviour
{
    private VisualEffect SmokeEffect;
    [SerializeField]
    private bool OnSmoke; //연기가 나오고 있으면 true,아니면 false
    public float FlickerTime; //연기가 나왔다가 안나왓다가 할 시간.
    private BoxCollider SmokeCollier; // 스모크 이펙트 활성화 시 데미지 줄 콜라이더.
    public int Damage; // 스모크가 플레이어에게 입힐 데미지
    public float DamageDelay; //스모크 데미지 간격.
    public PlayerInfo player;
    private bool isDamage;// 현재 데미지 주는 중인지.
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        SmokeEffect = GetComponent<VisualEffect>();
        SmokeCollier = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnSmoke)
        {
            StartCoroutine(FlickerSmoke());
        }
           
        
    }

    IEnumerator FlickerSmoke()
    {
        //스모크 시작
        
        OnSmoke = true;
        SmokeEffect.Play();// 이펙트 재생
        SmokeCollier.enabled = true;// 콜라이더 활성화.
        audioSource.Play();
        yield return new WaitForSeconds(FlickerTime);
        
        //스모크 멈춤
        
        SmokeEffect.Stop();// 이펙트 멈춤
        SmokeCollier.enabled = false; // 콜라이더 비활성화
        audioSource.Stop();
        yield return new WaitForSeconds(FlickerTime);
        OnSmoke = false;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player"&&!isDamage)
        {
            StartCoroutine( OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        player.currenthealth -= Damage;
        UIManager.instance.PlayerAttacked();
        yield return new WaitForSeconds(DamageDelay);
        isDamage = false;
    }
}
