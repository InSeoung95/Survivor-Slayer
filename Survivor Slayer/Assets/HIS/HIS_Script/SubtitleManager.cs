using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;
    public string Robot_sound;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
            
    }

    public Queue<string> subtitles; // 자막들 담을 큐
    [SerializeField] float delayTime = 5f; // dalayTime만큼 지연 뒤 자동으로 뒤의 자막 재생
    public Text subtitleTxt;

    public GameObject subtitleUI;

    // Start is called before the first frame update
    void Start()
    {
        subtitles = new Queue<string>();
        subtitleUI.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayNextSentence();
        }
    }

    public void StartSubtitle(Subtitle subtitle)
    {
        Debug.Log("자막 온");
        subtitleUI.SetActive(true);

        subtitles.Clear(); // 큐 초기화

        foreach(string sentence in subtitle.sentences)
        {
            subtitles.Enqueue(sentence);
            SoundManager.instance.PlayEffectSound(Robot_sound);
        }

        PlayNextSentence();
    }

    public void PlayNextSentence()
    {
        if(subtitles.Count==0)
        {
            EndSubtitle();
            return;
        }

        string subtitle = subtitles.Dequeue();
        StopAllCoroutines();
        StartCoroutine(DelaySubtitle(subtitle));
    }
    
    IEnumerator DelaySubtitle(string subtitle)
    {
        subtitleTxt.text = "";

        foreach(char letter in subtitle.ToCharArray())
        {
            subtitleTxt.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(delayTime);
        PlayNextSentence();
    }

    void EndSubtitle()
    {
        Debug.Log("자막 끝");
        subtitleUI.SetActive(false);
    }
}
