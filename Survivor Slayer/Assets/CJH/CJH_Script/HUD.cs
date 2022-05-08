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

    //이것들 다 빌드 위해서 추후 삭제
    private bool GameStop = false;

    public Text pause;
    public Text Goal;//
    public BaseManager baseManager;


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
        Goal.text = "현재 점령 수: " + baseManager.PlayerOcuupy;
    }

}
