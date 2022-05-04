using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Crtl : MonoBehaviour
{
    public string click_sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject bomb;
    public Transform bombTarget;

    public void ButtonClick()
    {
        SoundManager.instance.PlayEffectSound(click_sound);
        Instantiate(bomb, bombTarget.position, bomb.transform.rotation);
    }
}
