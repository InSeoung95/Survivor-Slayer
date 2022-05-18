using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

public class KillAni_Ctrl : MonoBehaviour
{
    private bool isPlaying=false; // 애니메이션이 재생 중인지.
    [SerializeField]
    private Transform mainCamera;

    private PlayableDirector playableDirector;
    public CinemachineVirtualCamera playerCam; // 플레이어 가상 캠

    public TimelineAsset[] KillAniTimelines=new TimelineAsset[6];

    public enum KillAniType
    {
        Stand_Front,
        Stand_Back,
        Stnad_Side,
        Lie_Front,
        Lie_Back,
        Lie_Side
    }
    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="KillAni"&&!isPlaying)
        {
            isPlaying = !isPlaying;

            KillAniEnemyData enemyInform = other.GetComponentInParent<KillAniEnemyData>();

            Vector3 dir = enemyInform.transform.position - transform.position;
            //Vector3 dir = other.transform.position - transform.position;
            dir.y = 0f;

            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            mainCamera.rotation = rot;
            transform.rotation = rot;

            //어느 타입 애니메이션 재생할지
            GetEnemyData(0, enemyInform);
            /*
            playableDirector.Play();
            */
            
        }
    }
    IEnumerator playingTime()
    {
        yield return new WaitForSeconds(1f);
        isPlaying = false;
        Debug.Log("is Playing: " + isPlaying);

        //각 변수들 초기화.
        playerCam.LookAt = null;
    }
    public bool CheckIsPlaying()
    {
        return isPlaying;
    }

    public void GetEnemyData(KillAniType aniType, KillAniEnemyData _enemyInform)
    {
        switch (aniType)
        {
            case KillAniType.Stand_Front:
                {
                    playerCam.LookAt = _enemyInform.targetInforms[0].CameraLookAt;
                    //playerCam.LookAt.position = Mathf.Lerp(playerCa, _enemyInform.targetInforms[0].CameraLookAt.position, Time.deltaTime);
                    playableDirector.Play(KillAniTimelines[0]);
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
        Debug.Log("정면 확정킬 애내 재생");
        StartCoroutine(playingTime());
       
    }
}
