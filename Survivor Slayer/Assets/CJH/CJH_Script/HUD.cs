using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    /*
    [SerializeField] private GunController theGunController;
    [SerializeField] private Gun currentGun;

    [SerializeField] private GameObject go_BulletHUD;   // HUD?�출, HUD비활성화

    [SerializeField] private Text[] text_Bullet;
     */

    private bool GameStop = false;
    public Text pause;

    private void Start()
    {
        UIManager.instance.UpdateLeftEnemy(0);
    }
    // Update is called once per frame
    void Update()
    {
        UIManager.instance.CheckBullet();
        UIManager.instance.UpdateLeftEnemy(UIManager.instance.CurrentEnemyNum);

        if(Input.GetKeyDown(KeyCode.P))
        {
            GameStop = !GameStop;

            if (GameStop)
            {
                Time.timeScale = 0;
                pause.text = "Pause";
            }
            else
            {
                Time.timeScale = 1f;
                pause.text = "Playing";
            }
        }
    }

}
