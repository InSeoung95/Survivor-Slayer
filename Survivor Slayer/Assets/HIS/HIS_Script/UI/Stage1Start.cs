using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Stage1Start : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public BaseManager baseManager;
    public GameObject Stage1_UI;
    public EnemySpawn enemySpawn;

    private bool isPlayed = false;
    // Start is called before the first frame update

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            playableDirector.Stop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!isPlayed)
        {
            isPlayed = true;
            baseManager.gameObject.SetActive(true);
            Stage1_UI.SetActive(true);
            playableDirector.Play();
            enemySpawn.sinematicTriger = true;
        }
    }
}
