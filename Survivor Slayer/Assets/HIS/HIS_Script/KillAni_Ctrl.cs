using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.Animations.Rigging;

public class KillAni_Ctrl : MonoBehaviour
{
    public bool isPlaying=false; // 애니메이션이 재생 중인지.
    public bool isEnemyFront;// 적이 정면을 보고 있는지. rotation.y값이 -90~90일때.
    [SerializeField]
    private Transform mainCamera; //컨트롤 할 메인 카메라 좌표.
    [SerializeField] private Transform Default_MainCam_Transform;// 저장할 카메라 좌표의 디폴트 트랜트폼 값.
    public Transform gun;
    private Transform Default_Gun_Transform;// 저장할 총 좌표 디폴트 트랜스폼 값.
    private ParticleSystem bloodEf;

    private Rigidbody playerRigid;
    private PlayableDirector playableDirector;

    public PlayableDirector[] KillAniGroup;
    private KillAniType aniType;



    public CinemachineVirtualCamera playerCam; // 컨트롤 할 플레이어 가상 카메라

    public GameObject Player_Model; // 플레이어 캐릭터. 평소에 비활성화. 확정킬 재생 때 활성화.
    public Transform LeftArmIK; 
    public Transform RightArmIK;

    public Transform GunLeftHandle;
    public Transform GunRightHandle;

    public Transform LeftLegIK;
    public Transform RightLegIK;

    public TimelineAsset[] KillAniTimelines=new TimelineAsset[6]; // 재생할 확정킬 애니메이션들.

    public GameObject CybogModel;//플레이어 사이보그 모델링. 카메라가 이걸 가릴 것 같을 때 layer를 player로. 신체가 비추는 것이 필요할 땐 layer를 default로.

    public Rig rig1;

    public GameObject Sword;

    public enum KillAniType //확정킬 애니메이션 종류
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
        CybogModel.layer = 3; // player 레이어로.

        //gun = FindObjectOfType<Gun>();
        Default_MainCam_Transform = mainCamera;
        Default_Gun_Transform = gun.transform;

        playerRigid = GetComponent<Rigidbody>();

        Sword.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {
        /* //콜라이더로 충돌체크해서 킬애니 재생하는 시스템
        if(other.tag == "FrontKillAni"|| other.tag == "BackKillAni"|| other.tag == "CrawlKillAni")
        {
            if (other.tag == "FrontKillAni")
            {
                aniType = KillAniType.Stand_Front;
            }
            else if (other.tag == "BackKillAni")
            {
                aniType = KillAniType.Stand_Back;
            }
            else if (other.tag == "CrawlKillAni")
            {
                aniType = KillAniType.Lie_Back;
            }
            Debug.Log("KillAniType: " + aniType);
        }

        KillAniEnemyData enemyInform = other.GetComponentInParent<KillAniEnemyData>(); //적으로부터 KillAniEnemyDate 클래스 정보 받아온다.

        if(enemyInform.isGroggy)
        {
            isPlaying = true; //애니메이션 재생 중으로 변경.
            Player_Model.SetActive(true); // 플레이어 모델링 활성화.
        }
        Vector3 dir = enemyInform.transform.position - transform.position;
        //Debug.Log("로컬 포지션:"+enemyInform.transform.TransformPoint(enemyInform.transform.position));
        //Vector3 dir = other.transform.position - transform.position;
        dir.y = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);

        //카메라, 플레이어 정면 보도록.
        mainCamera.rotation = rot;
        transform.rotation = rot;

        bloodEf = enemyInform.bloodEf;


        //확정킬 애니 재생
        PlayKillAni(aniType, enemyInform); // 받아온 애니 타입에 맞는 킬애니 재생
        
        playableDirector.Play();
         */

//적의 각도 구해서 킬애니 시스템 
//조건: 적이 1.부위파괴로인한 그로기 상태인지, 2.플레이어와 적 확정킬 콜라이더에 닿았는지, 3.확정킬 애니메이션 재생 중 아닌지. 4.적이 누워있는 상태인지
        if (other.CompareTag("KillAni") && !isPlaying)
        {
            KillAniEnemyData
                enemyInform = other.GetComponentInParent<KillAniEnemyData>(); //적으로부터 KillAniEnemyDate 클래스 정보 받아온다.

            var isdeath = other.GetComponentInParent<Enemy_test>().isDeath;
            
            if (enemyInform.isGroggy && !isdeath) // 적이 현재 그로기 상태인지.
            {
                KillAniType aniType;

                isPlaying = true; //애니메이션 재생 중으로 변경.
                Player_Model.SetActive(true); // 플레이어 모델링 활성화.
                //확정킬 애니 타입 체크
                float enemyAngle;
                enemyAngle = enemyInform.transform.eulerAngles.y;
                Debug.Log("적 각도:" + enemyAngle);
                if (enemyAngle < 90 && enemyAngle > -90)
                    isEnemyFront = true;
                else
                    isEnemyFront = false;

                bloodEf = enemyInform.bloodEf;

                Vector3 dir = enemyInform.transform.position - transform.position;
                //Debug.Log("로컬 포지션:"+enemyInform.transform.TransformPoint(enemyInform.transform.position));
                //Vector3 dir = other.transform.position - transform.position;
                dir.y = 0f;

                Quaternion rot = Quaternion.LookRotation(dir.normalized);

                //카메라, 플레이어 정면 보도록.
                mainCamera.rotation = rot;
                transform.rotation = rot;


                //각도 구하기 첫번째 방법.


                Transform enemyTransform = enemyInform.transform;
                float angle = Mathf.Atan2(enemyTransform.position.z - transform.position.z,
                    enemyTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
                ;

                if (!isEnemyFront) //적이 뒤돌아 있을 때 
                    angle = -angle;


                //누워있을 때는 적 z축을 y로.

                //각도 구하기 하나 더
                //enemyInform.GetAngle();

                Debug.Log("anlge: " + angle);

                //어느 타입 애니메이션 재생할지
                if (enemyInform.isCrawl)
                {
                    aniType = KillAniType.Lie_Back;
                }

                else if (enemyInform.GetAngle())
                {
                    aniType = KillAniType.Stand_Front;
                }
                else
                {
                    aniType = KillAniType.Stand_Back;
                }


                /*
                else
                 { 
                    aniType = GetAniType(angle); // 재생할 애니 타입 받아오고 // 각도로 킬애니 타입 구하는 것 봉인.
                 }
                 */



                Debug.Log("AniType: " + aniType);
                //aniType = GetAniType(other,enemyInform); // 콜라이더 체크로 킬애니 타입 구하기.
                PlayKillAni(aniType, enemyInform); // 받아온 애니 타입에 맞는 킬애니 재생

                //playableDirector.Play();

            }


        }

    }

    public bool CheckIsPlaying()
    {
        return isPlaying;
    }
    
    public KillAniType GetAniType(float _angle) // 플레이어와 적의 각도에 따라 재생할 확정킬 애니 결정
    {
        KillAniType aniType;
       
        if (_angle > 0 && _angle <= 180)
        {
            aniType = KillAniType.Stand_Back;
        }
        else
            aniType = KillAniType.Stand_Front;
 
        return aniType;
    }
    
    /*
    public KillAniType GetAniType(Collider _other,KillAniEnemyData _enemyData)
    {
        KillAniType aniType;

        if (_other.tag == "FrontKillAni")
        {
            aniType = KillAniType.Stand_Front;
        }
        else if (_other.tag == "BackKillAni")
        {
            aniType = KillAniType.Stand_Back;
        }
        else if(_enemyData.isCrawl)
        {
            aniType = KillAniType.Lie_Front;
        }
        else
        {
            aniType = KillAniType.Lie_Side;
        }

        Debug.Log("킬애니 타입: " + aniType);
        return aniType;
    }
     */
    public void PlayKillAni(KillAniType _aniType, KillAniEnemyData _enemyInform) // 확정킬 애니 재생함수
    {
        Default_MainCam_Transform = mainCamera; // 킬애니 실행 전 현재 카메라 트랜스폼 정보 저장.

        playerRigid.isKinematic = true; // 물리 영향 받지 않게.

        switch (_aniType)
        {
            case KillAniType.Stand_Front:
                {
                    CybogModel.layer = 3; // player 레이어로.
                    playerCam.LookAt = _enemyInform.targetInforms[0].CameraLookAt;

                    //카메라가 돌아가는 것 y,z축 고정해야 된다.
                    //playerCam.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //playerCam.transform.rotation=

                    //playerCam.LookAt.position = Mathf.Lerp(playerCa, _enemyInform.targetInforms[0].CameraLookAt.position, Time.deltaTime);
                    //playableDirector.Play(KillAniTimelines[0]);
                    //playerCam.LookAt.position = Vector3.Lerp(new Vector3(0,0,0), _enemyInform.targetInforms[0].CameraLookAt.position, Time.deltaTime * 1);
                    //LeftArmIK.position = _enemyInform.targetInforms[0].LeftArmTarget.position;
                    Debug.Log("LeftArmIK: " + LeftArmIK);
                    //RightArmIK.position = _enemyInform.targetInforms[0].RifhtArmTarget.position;
                    //KillAniGroup[0].Play();
                    LeftArmIK.gameObject.GetComponent<UpdateRigTarget>().target = GunLeftHandle;
                    RightArmIK.gameObject.GetComponent<UpdateRigTarget>().target = GunRightHandle;
                    _enemyInform.KillAniGroup[0].Play();
                    break;
                }
            case KillAniType.Stand_Back:
                {
                    CybogModel.layer = 0; // default 레이어로.
                    playerCam.LookAt = _enemyInform.targetInforms[1].CameraLookAt;

                    //RightArmIK = _enemyInform.targetInforms[1].RifhtArmTarget;
                    //LeftArmIK = _enemyInform.targetInforms[1].LeftArmIKTarget;

                    //LeftArmIK.GetComponent<UpdateRigTarget>().target= _enemyInform.targetInforms[1].LeftArmIKTarget;
                    //RightArmIK.GetComponent<UpdateRigTarget>().target= _enemyInform.targetInforms[1].RifhtArmTarget;
                    //LeftArmIK.position = _enemyInform.targetInforms[1].LeftArmTarget.position;
                    Debug.Log("LeftArmIK: " + LeftArmIK);
                    //RightArmIK.position = _enemyInform.targetInforms[1].RifhtArmTarget.position;
                    LeftArmIK.gameObject.GetComponent<UpdateRigTarget>().target = _enemyInform.targetInforms[1].LeftArmTarget;
                    RightArmIK.gameObject.GetComponent<UpdateRigTarget>().target = _enemyInform.targetInforms[1].RifhtArmTarget;


                    // 적 머리가 플레이어를 바라보도록.
                    //Debug.Log("현재 대가리 위치: " + _enemyInform.targetInforms[1].ExtraPoint.position);
                    //_enemyInform.targetInforms[1].ExtraPoint.LookAt(this.transform);
                    //Debug.Log("돌린 대가리 위치: " + _enemyInform.targetInforms[1].ExtraPoint.position);
                    //Vector3 dir = transform.position - _enemyInform.targetInforms[1].ExtraPoint.position;
                    //Quaternion rot = Quaternion.LookRotation(dir.normalized);

                    //_enemyInform.targetInforms[1].ExtraPoint.rotation = rot;
                    //_enemyInform.GetComponentInChildren<LookAt>().LookAtTarget(gameObject.transform);

                    //_enemyInform.targetInforms[1].ExtraPoint

                    //playableDirector.Play(KillAniTimelines[1]);

                    KillAniGroup[1].Play();
                    _enemyInform.GetComponentInChildren<PlayableDirector>().Play(); // 머리 돌리는 타임 라인.
                    //_enemyInform.KillAniGroup[1].Play();
                    
                    Debug.Log("대가리 돌리기");
                    //_enemyInform.GetComponentInChildren<Animator>().SetTrigger("HeadRolling");
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
                    CybogModel.layer = 0; // default 레이어로.

                    playerCam.LookAt = _enemyInform.targetInforms[2].CameraLookAt;
                    RightLegIK.position = _enemyInform.targetInforms[2].RightLegTarget.position;
                    LeftArmIK.position = _enemyInform.targetInforms[2].LeftArmTarget.position;
                    Sword.SetActive(true);
                    KillAniGroup[2].Play();
                    break;
                }
            case KillAniType.Lie_Side:
                {
                    break;
                }
        }
        Debug.Log("확정킬 애내 재생");
        StartCoroutine(playingTime(_enemyInform));
        
    }
    IEnumerator playingTime(KillAniEnemyData _enemyInform) // 확정킬 재생 이후에 각종 설정들 초기화 시켜주는 함수
    {
        yield return new WaitForSeconds(2f);
        isPlaying = false; // 확정킬 애니 재생 중 false;
        //플레이어 설정
        LeftArmIK.position = new Vector3(0, 0, 0);
        RightArmIK.position = new Vector3(0, 0, 0);
        
        playerCam.LookAt = null;
        mainCamera = Default_MainCam_Transform;// 메인 카메라 트랜스폼 초기화.
        gun = Default_Gun_Transform;// 총 위치도 원래대로 초기화
        playerRigid.isKinematic = false; // 다시 물리 영향 받도록.
        rig1.weight = 1f;
        Sword.SetActive(false);

        //적 설정
        _enemyInform.GetComponent<Enemy_Dest>().currentHealth = 0; // 적 죽이기.
        _enemyInform.isGroggy = false;
        CybogModel.layer = 3; // player 레이어로.
        Player_Model.SetActive(false); // 플레이어 모델 다시 비활성화
        Debug.Log("is Playing: " + isPlaying);

        //각 변수들 초기화.
        
        //LeftArmIK.GetComponent<UpdateRigTarget>().target = GunLeftHandle;
        //RightArmIK.GetComponent<UpdateRigTarget>().target = GunRightHandle;
        //LeftArmIK;
        //RightArmIK =;
        //playerCam.transform.rotation= Quaternion.Euler(mainCamera.transform.rotation.x,0,0);
        //mainCamera.transform.localRotation= Quaternion.Euler(mainCamera.transform.rotation.x, 0, 0);
        //mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    }
    public void PlayBloodEffect()
    {
        bloodEf.Play();
    }
}
