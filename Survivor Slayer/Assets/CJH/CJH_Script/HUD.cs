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

    // Update is called once per frame
    void Update()
    {
        UIManager.instance.CheckBullet();
        //CheckBullet();
    }

}
