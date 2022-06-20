using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubttitleTriger : MonoBehaviour
{
    public Subtitle subtitle;
    private bool isPlayed=false; // 한 번 재생된 적 있는지 

    public void TriggerSubtitle()
    {
        FindObjectOfType<SubtitleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&!isPlayed)
        {
            isPlayed = true;
            SubtitleManager.instance.StartSubtitle(subtitle);
        }
    }
}
