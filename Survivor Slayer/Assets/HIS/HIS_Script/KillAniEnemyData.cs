using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TargetInform
{
    public Transform CameraLookAt;
    public Transform GunTarget;
    public Transform LeftArmTarget;
    public Transform RifhtArmTarget;
    public Transform LeftLegTarget;
    public Transform RightLegTarget;
}
//확정킬 애니메이션 재생에 필요한 적 정보 받기위한 스크립트
public class KillAniEnemyData : MonoBehaviour
{
    public TargetInform[] targetInforms = new TargetInform[6];
    public ParticleSystem bloodEf;

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
}
