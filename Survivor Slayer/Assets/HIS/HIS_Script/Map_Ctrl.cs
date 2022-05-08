using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Ctrl : MonoBehaviour
{
   
    public GameObject[] Ctrl_Doors; // 맵에서 컨트롤 해야되는 문들.
    [SerializeField]
    private List<Button> doorCtrl_btns;

    public bool isPause=false;

    //public GameObject door;
    //private Button button;
    private bool active;
    public GameObject ClearImage;

    private void Awake()
    {
        doorCtrl_btns.AddRange(this.GetComponentsInChildren<Button>());
    }
    public void GameClear()
    {
        Time.timeScale = 0.0f;
        ClearImage.SetActive(true);
    }
}
