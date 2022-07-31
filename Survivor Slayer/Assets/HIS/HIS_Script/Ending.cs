using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Ending : MonoBehaviour
{
    private PlayableDirector ending;
    public GameObject endingUI;
    private bool isPlayed;
    private PlayerController player;

    [SerializeField] private AudioListener _listener;
    [SerializeField] private GameObject _Dlight;

    // Start is called before the first frame update
    void Start()
    {
        ending = GetComponent<PlayableDirector>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && isPlayed)
        {
            Application.Quit();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.gameObject.SetActive(false);
            _Dlight.gameObject.SetActive(true);
            _listener.enabled = true;
            ending.Play();
            if(!isPlayed)
            {
                isPlayed = true;
                StartCoroutine(EndingUI());

            }
        }

        
    }
    IEnumerator EndingUI()
    {
        yield return new WaitForSeconds(6f);
        endingUI.SetActive(true);
    }
}
