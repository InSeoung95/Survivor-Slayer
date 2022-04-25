using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door_Ctrl : MonoBehaviour
{
    public bool Active; // 플레이어에 반응하는 문인지
    private bool opened=true; // 문이 열린 상태인지.
    private Animator animator;
    public int Cool_time; //문이 잠겨있는 시간.
    private AudioSource audioSource;
    public AudioClip DoorClose_sound;
    public Button UIbutton;
  
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Open()
    {
        
        animator.SetBool("opened", false);
        audioSource.PlayOneShot(DoorClose_sound);
        Debug.Log("문닫힘");
        StartCoroutine(delayTime());
    }

    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(Cool_time);
        Close();
        
    }
    public void Close()
    {
        animator.SetBool("opened", true);
        Debug.Log("문열림");
        
    }
}
