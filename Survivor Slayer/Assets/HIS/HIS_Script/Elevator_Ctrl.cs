using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Ctrl : MonoBehaviour
{
    private Animator eleAnim; // 엘리베이터 애니메이터
    InteractDoor door;
    [SerializeField]
    private GameObject Player;

    [SerializeField] private GameObject _2rdHUD;
    private bool once=false;

    public ParticleSystem[] explosions;// 엘리베이터로 탈출 시 재생될 이펙트들
    public float BombDelay;// 폭발 이펙트 딜레이 시간
    [SerializeField] private AudioSource[] exploSounds;
    public bool end=false;

    private int count=0;

    private CameraShake _camera;

    public AudioSource Warning; 
    
    // Start is called before the first frame update
    void Start()
    {
        eleAnim = GetComponent<Animator>();
        door = GetComponentInChildren<InteractDoor>();
        Player = FindObjectOfType<PlayerController>().gameObject;
        exploSounds = new AudioSource[explosions.Length];
        for(int i=0;i<explosions.Length;++i)
        {
            exploSounds[i]= explosions[i].gameObject.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if(eleAnim.GetBool("Move")&&!end)
        {
            Player.transform.position = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);

            if(!once)
                StartCoroutine(MultiExplosion());
        }

        if (end && once) // 이것도 한번만 실행되게.
        {
            once = false;
            door.Activate = true;
        }
    }
    IEnumerator MultiExplosion() // 연쇄 폭발 코루틴
    {
        _2rdHUD.gameObject.SetActive(false); // 제한 시간 UI도 꺼지도록.
        Debug.Log("2번째 UI 꺼짐");
        Warning.Stop(); // 위험 알람음 정지.
        once = true;
        yield return new WaitForSeconds(2f);
        StartCoroutine(_camera.Shake(1f, 5f, 2f)); // 카메라 떨리게.
        //count++;

        for(int i=0;i<explosions.Length;++i)
        {
            explosions[i].Play(); // 폭발 이펙트 재생
            
            exploSounds[i].Play(); // 폭발 사운드 재생.
            
           
            yield return new WaitForSeconds(BombDelay);
            explosions[i].Stop(); // 폭발 이펙트 정지
        }
        /*
        foreach (var explosion in explosions)
        {
            explosion.Play();
            yield return new WaitForSeconds(BombDelay);
            explosion.Stop();
        }
         */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            door.Activate = false;
            _camera = other.gameObject.GetComponentInChildren<CameraShake>();
            eleAnim.SetBool("Move", true);
        }
    }
}
