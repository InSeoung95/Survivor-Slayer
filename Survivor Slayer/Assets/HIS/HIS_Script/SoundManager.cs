using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name; // 사운드 이름
    public AudioClip clip; // 사운드 클립.
}


public class SoundManager : MonoBehaviour
{
    //싱글톤 관리
    public static SoundManager instance;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    

    private AudioSource Audiosource_BGM;
    [Header("BGM 관리")]
    public Sound BGM_Sound;
    public bool DoPlay=true;

    private AudioSource[] Audiosource_Effect=new AudioSource[5];  //5개 정도면 충분할 것 같음.
    [Header("Effect Sound 관리")]
    public Sound[] Effect_Sounds;

    private List<Sound> PlayingList=new List<Sound>(); // 현재 재생 중인 사운드 리스트

    void Start()
    {
        Audiosource_BGM=gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < Audiosource_Effect.Length; i++)
        {
            Audiosource_Effect[i]=gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayBGM();
    }

    public void PlayEffectSound(string _name, bool _loop=false) // 재생할 사운드 이름과 loop 여부.
    {
        for (int i = 0; i < Effect_Sounds.Length; i++)
        {
            if(Effect_Sounds[i].name==_name)
            {
                for (int j = 0; j < Audiosource_Effect.Length; j++)
                {
                    if(!Audiosource_Effect[j].isPlaying)
                    {
                        Audiosource_Effect[j].clip = Effect_Sounds[i].clip;
                        Audiosource_Effect[j].loop = _loop;
                        Audiosource_Effect[j].Play();

                        PlayingList.Insert(j,Effect_Sounds[i]); // 재생한 사운드 리스트에 추가
                        return;
                    }
                }
                Debug.Log("사용가능한 AudioSource가 없습니다.");
            }
        }
        Debug.Log(_name + "란 이름의 사운드를 찾을 수 없습니다.");
    }
    public void PlayBGM()
    {
        if (DoPlay)
        {
            if (!Audiosource_BGM.isPlaying)
            {
                Audiosource_BGM.clip = BGM_Sound.clip;
                Audiosource_BGM.volume = 0.5f;
                Audiosource_BGM.loop = true;
                Audiosource_BGM.Play();
            }

        }
        else
            Audiosource_BGM.Stop();
    }
    public void StopEffectSound(string _name) //선택한 이펙트 사운드 중지
    {
        for (int i = 0; i < PlayingList.Count; i++)
        {
            if(PlayingList[i].name==_name)
            {
                Audiosource_Effect[i].Stop();
                return;
            }
        }
        Debug.Log("Stop 요청한 사운드가 플레이 중이 아니거나 찾을 수 없습니다");
    }

    public void StopAllSound() // 모든 사운드 일괄 중지
    {
        Audiosource_BGM.Stop();

        foreach(AudioSource audioSource in Audiosource_Effect)
        {
            audioSource.Stop();
        }
    }
}
