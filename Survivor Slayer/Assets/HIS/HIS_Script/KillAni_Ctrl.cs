using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class KillAni_Ctrl : MonoBehaviour
{
    private bool isPlaying=false; // 애니메이션이 재생 중인지.
    [SerializeField]
    private Transform mainCamera;

    public PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
       
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

            Vector3 dir = other.transform.position - transform.position;
            dir.y = 0f;

            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            mainCamera.rotation = rot;
            transform.rotation = rot;

            playableDirector.Play();
            Debug.Log("정면 확정킬 애내 재생");
            StartCoroutine(playingTime());
        }
    }
    IEnumerator playingTime()
    {
        yield return new WaitForSeconds(1f);
        isPlaying = false;
        Debug.Log("is Playing: " + isPlaying);
    }
    public bool CheckIsPlaying()
    {
        return isPlaying;
    }
}
