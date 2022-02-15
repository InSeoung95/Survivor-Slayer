using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GunController theGunController;
    [SerializeField] private Gun currentGun;

    [SerializeField] private GameObject go_BulletHUD;   // HUD호출, HUD비활성화

    [SerializeField] private Text[] text_Bullet;
    
    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.currentBulletCount.ToString();
        text_Bullet[1].text = currentGun.carryBulletCount.ToString();
    }
}
