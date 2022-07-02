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
    private Gun _gun;
    private UpgradeType _upgrade;

    private void Start()
    {
        _upgrade = (UpgradeType)Random.Range(0, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _gun = collision.gameObject.GetComponentInChildren<Gun>();
            _gun.GunUpgrade(_upgrade);
            Destroy(this.gameObject);
        }
    }
}
