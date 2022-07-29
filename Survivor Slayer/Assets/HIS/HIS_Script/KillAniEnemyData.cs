using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


[System.Serializable]
public class TargetInform
{
    public Transform CameraLookAt; // 카메라 타겟 정보
    public Transform GunTarget; // 총 타겟 
    public Transform LeftArmTarget;
    public Transform RifhtArmTarget;
    public Transform LeftLegTarget;
    public Transform RightLegTarget;
    public Transform ExtraPoint; // 예외적으로 포인트 필요한경우.
}
//확정킬 애니메이션 재생에 필요한 적 정보 받기위한 스크립트
public class KillAniEnemyData : MonoBehaviour
{
    public TargetInform[] targetInforms = new TargetInform[6];
    public ParticleSystem bloodEf;
    public bool isCrawl; // 좀비가 누워있는 상태인지.
    public bool isGroggy; // 좀비가 그로기 상태인지.
    public float GroggyTime; // 그로기 지속 시간.
    public bool Front;// 좀비가 정면을 보고 있는 상태인지.
    public bool isFront;//플레이어로부터 정면인지.
    private GameObject player;
    public PlayableDirector[] KillAniGroup;


    private void Start()
    {
        player = FindObjectOfType<PlayerInfo>().gameObject;
    }
    
    public enum KillAniType
    {
        Stand_Front,
        Stand_Back,
        Stnad_Side,
        Lie_Front,
        Lie_Back,
        Lie_Side
    }

    public void GetEnemyData(KillAniType aniType )
    {
        switch(aniType)
        {
            case KillAniType.Stand_Front:
                {

                    break;
                }
            case KillAniType.Stand_Back:
                {
                    break;
                }
            case KillAniType.Stnad_Side:
                {
                    break;
                }

            case KillAniType.Lie_Front:
                {

                    break;
                }
            case KillAniType.Lie_Back:
                {
                    break;
                }
            case KillAniType.Lie_Side:
                {
                    break;
                }
        }
    }

    public bool GetAngle()
    {
        Vector3 dir = player.transform.position - this.gameObject.transform.localPosition;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        Debug.Log("로컬 y값: " + this.gameObject.transform.rotation.eulerAngles.y);
        float angle = Mathf.Abs( rot.eulerAngles.y - this.gameObject.transform.rotation.eulerAngles.y);
        Debug.Log("플레이어로부터의 각도: "+angle);
        
        //if (angle < 90 || angle > 270)
        if(angle<90)
        {
            isFront = true;
            return true;
        }
        else
        {
            isFront = false;
            return false;
          
        }

    }
}
