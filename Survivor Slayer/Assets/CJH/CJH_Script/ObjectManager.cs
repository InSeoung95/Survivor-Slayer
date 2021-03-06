using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
   public const int MAX_ITEM_OBJECT = 50;
   public const int MAX_BULLET = 30;
   public const int MAX_ENEMY = 30;
   public int stage;
   
   public GameObject Enemy_ZombiePrefab;
   public GameObject Enemy_FogZombiePrefab;
   public GameObject Enemy_TurretPrefab;
   public GameObject BulletPrefab;
   public GameObject Item_HealPackPrefab;
   public GameObject Item_AmmoPrefab;
   public GameObject Item_PowerGagePrefab;
   public GameObject Item_PsychoPrefab;
   
   private GameObject[] Enemy_Zombie;
   private GameObject[] Enemy_FogZombie;
   private GameObject[] Enemy_Turret;
   private GameObject[] Bullet;
   private GameObject[] Item_HealPack;
   private GameObject[] Item_Ammo;
   private GameObject[] Item_PowerGage;
   private GameObject[] Item_Psycho;

   [SerializeField] private Transform FirstPos;
   public GameObject[] targetPool;

   public Base[] EnemyTargetBase;
   public Transform Player;
   
   private void Awake()
   {
      Enemy_Zombie = new GameObject[MAX_ENEMY];
      Enemy_FogZombie = new GameObject[MAX_ENEMY];
      Enemy_Turret = new GameObject[MAX_ENEMY];
      Bullet = new GameObject[MAX_BULLET];
      Item_HealPack = new GameObject[MAX_ITEM_OBJECT];
      Item_Ammo = new GameObject[MAX_ITEM_OBJECT];
      Item_PowerGage = new GameObject[MAX_ITEM_OBJECT];
      Item_Psycho = new GameObject[MAX_ITEM_OBJECT];
      
      Generate();
   }

   private void Generate()
   {
      for (int index = 0; index < Enemy_Zombie.Length; index++)
      {
         Enemy_Zombie[index] = Instantiate(Enemy_ZombiePrefab,FirstPos.position,transform.rotation,transform);
         Enemy_Zombie[index].SetActive(false);
      }
      for (int index = 0; index < Enemy_FogZombie.Length; index++)
      {
         Enemy_FogZombie[index] = Instantiate(Enemy_FogZombiePrefab,FirstPos.position,transform.rotation,transform);
         Enemy_FogZombie[index].SetActive(false);
      }
      for (int index = 0; index < Enemy_Turret.Length; index++)
      {
         Enemy_Turret[index] = Instantiate(Enemy_TurretPrefab,FirstPos.position,transform.rotation,transform);
         Enemy_Turret[index].SetActive(false);
      }


      for (int index = 0; index < Bullet.Length; index++)
      {
         Bullet[index] = Instantiate(BulletPrefab,transform);
         Bullet[index].SetActive(false);
      }
      for (int index = 0; index < Item_HealPack.Length; index++)
      {
         Item_HealPack[index] = Instantiate(Item_HealPackPrefab,transform);
         Item_HealPack[index].SetActive(false);
      }
      for (int index = 0; index < Item_Ammo.Length; index++)
      {
         Item_Ammo[index] = Instantiate(Item_AmmoPrefab,transform);
         Item_Ammo[index].SetActive(false);
      }
      for (int index = 0; index < Item_PowerGage.Length; index++)
      {
         Item_PowerGage[index] = Instantiate(Item_PowerGagePrefab,transform);
         Item_PowerGage[index].SetActive(false);
      }
      for (int index = 0; index < Item_Psycho.Length; index++)
      {
         Item_Psycho[index] = Instantiate(Item_PsychoPrefab,transform);
         Item_Psycho[index].SetActive(false);
      }

   }

   public GameObject MakeObj(string type, Vector3 position, Quaternion rotation)
   {
      switch (type)
      {
         case "Enemy_Zombie" :
            targetPool = Enemy_Zombie;
            break;
         case "Enemy_Fog" :
            targetPool = Enemy_FogZombie;
            break;
         case "Enemy_Turret" :
            targetPool = Enemy_Turret;
            break;
         case "Bullet" :
            targetPool = Bullet;
            break;
         case "Item_HealPack" :
            targetPool = Item_HealPack;
            break;
         case "Item_Ammo" :
            targetPool = Item_Ammo;
            break;
         case "Item_PowerGage" :
            targetPool = Item_PowerGage;
            break;
         case "Item_Psycho" :
            targetPool = Item_Psycho;
            break;
      }

      for (int index = 0; index < targetPool.Length; index++)
      {
         if (!targetPool[index].activeSelf)
         {
            targetPool[index].transform.position = position;
            targetPool[index].transform.rotation = rotation;
            targetPool[index].SetActive(true);
            return targetPool[index];
         }
      }

      return null;
   }
}
