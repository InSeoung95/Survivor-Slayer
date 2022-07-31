using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Stage1CinePlay : MonoBehaviour
{
    public PlayableDirector Stage1Start;

    [SerializeField]
    bool isPlayed;

    [RuntimeInitializeOnLoadMethod]
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&!isPlayed)
        {
            isPlayed = true;
            Stage1Start.Play();
        }
    }
}
