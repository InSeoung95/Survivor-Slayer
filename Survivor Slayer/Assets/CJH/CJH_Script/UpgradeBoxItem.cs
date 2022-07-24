using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum UpgradeType
{
    Damage, Bullet, GunGage
}
public class UpgradeBoxItem : MonoBehaviour
{
    [SerializeField]private UpgradeType _upgrade;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var _gun = collision.gameObject.GetComponentInChildren<Gun>();
            _gun.GunUpgrade(_upgrade);
            Destroy(this.gameObject);
        }
    }
}
