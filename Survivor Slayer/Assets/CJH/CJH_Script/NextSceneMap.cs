using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneMap : MonoBehaviour
{
    public Image _Image;
    private bool IN;
    private bool Out;
    private GameObject NextTransForm;
    private GameObject _player;
    [SerializeField] private ObjectManager _objectManager;

    [SerializeField] private AudioSource _audio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player = other.gameObject;
            _objectManager.stage = 2;
            _Image.gameObject.SetActive(true);
            _audio.Play();
            StartCoroutine(FadeIn());
           
        }
    }

    private IEnumerator FadeIn()
    {
        float fadeCount = 0;
        while (fadeCount < 1f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            _Image.color = new Color(0, 0, 0, fadeCount);
        }

        if (fadeCount >= 1f)
            IN = true;
        if (IN)
        {
            SceneManager.LoadScene("Main_Stage2");
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float fadeCount = 1;
        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            _Image.color = new Color(0, 0, 0, fadeCount);
        }

        if (fadeCount < 0)
            Out = true;
        if (Out)
        {
            _Image.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

    }
    
}
