using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    private PlayerInfo _playerInfo;
    
    
    private void Start()
    {
        _playerInfo = gameObject.GetComponent<PlayerInfo>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.itemType)
            {
                case Item.ItemType.Ammo :
                    currentGun.carryBulletCount += item.value;
                    if (currentGun.carryBulletCount > currentGun.maxBulletCount)
                        currentGun.carryBulletCount = currentGun.maxBulletCount;
                    break;
                case Item.ItemType.HealthPack :
                    _playerInfo.currenthealth += item.value;
                    if (_playerInfo.currenthealth > _playerInfo.maxHealth)
                        _playerInfo.currenthealth = _playerInfo.maxHealth;
                    break;
                case Item.ItemType.PowerGage :
                    _playerInfo.currentPower += item.value;
                    if (_playerInfo.currentPower > _playerInfo.maxPower)
                        _playerInfo.currentPower = _playerInfo.maxPower;
                    break;
                case Item.ItemType.Psycho:
                    _playerInfo.currentPsycho += item.value;
                    if (_playerInfo.currentPsycho > _playerInfo.maxPsycho)
                        _playerInfo.currentPsycho = _playerInfo.maxPsycho;
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }
}
