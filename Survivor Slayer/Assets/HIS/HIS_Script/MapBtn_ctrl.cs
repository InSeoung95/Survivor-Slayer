using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBtn_ctrl : MonoBehaviour
{

    public GameObject door;
    private Button button;
    private bool active;
    public string click_sound;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    public void ButtonClick()
    {
        SoundManager.instance.PlayEffectSound(click_sound);
        door.GetComponent<Door_Ctrl>().Open();
        button.interactable = false;
        StartCoroutine(ButtonActive());
    }
    IEnumerator ButtonActive()
    {
        yield return new WaitForSeconds(door.GetComponent<Door_Ctrl>().Cool_time);
        button.interactable = true;
    }
}
