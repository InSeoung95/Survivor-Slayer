using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //싱글톤 관리
    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    [SerializeField] private GunController theGunController;
    [SerializeField] private Gun currentGun;

    [SerializeField] private GameObject go_BulletHUD;

    [SerializeField] private Text[] text_Bullet;

    public Text RoundNumber; // 현재 진행 라운드
    public Text LeftEnemy;   // 남은 적 수
    public GameObject BaseOccu_UI; // 베이스 점령 시 보이는 UI
    public GameObject gameOver_UI; // 게임 패배시 보이는 UI
    public Slider occu_slider;
    public Text occu_txt;
    public GameObject PlayerAttackedImage; // 플레이어 맞을 때 뜨는 이미지.
    public GameObject Map_panel; // 문 컨트롤 할 수 있는 패널.
    public bool mapActive=false;
    public int CurrentEnemyNum=0;// 현재 적수

    private void Awake()
    {
        //occu_slider = BaseOccu_UI.GetComponent<Slider>();
        PlayerAttackedImage.SetActive(false);
        mapActive = false;
        Cursor.visible = false;
        Map_panel.SetActive(true);
        Map_panel.GetComponent<Canvas>().enabled = mapActive;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            mapActive = !mapActive;
            Cursor.visible = !Cursor.visible;
            Map_panel.GetComponent<Canvas>().enabled = mapActive;
            FindObjectOfType<Map_Ctrl>().ClearImage.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.currentBulletCount.ToString();
        text_Bullet[1].text = currentGun.carryBulletCount.ToString();
    }

    public void UpdateRound(int current_round)
    {
        RoundNumber.text = "Stage: " + current_round;
    }
    public void UpdateLeftEnemy(int left_enemy)
    {
        LeftEnemy.text = "남은 적: " + left_enemy;
    }
    
    public void UpdateGameOverUI(bool game_end) // 게임 오버 활성화 함수
    {
        gameOver_UI.SetActive(game_end);
    }

    public void GoStartScene() // 처음 시작 메인 화면으로 돌아가는 함수
    {
        SceneManager.LoadScene("GameStart");
    }

    public void Restart() // 1라운드부터 다시 시작하는 함수
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BaseOccupationUI(float occu_time) // 플레이어가 베이스 점령 시 보이는 UI 활성화
    {
        BaseOccu_UI.SetActive(true);
        occu_slider.value = occu_time / 5;
        int percent = (int)(occu_slider.value * 100);
        occu_txt.text = percent.ToString() + " %";
    }

    public void PlayerAttacked()
    {
        StartCoroutine("Flash"); // 0.1초 동안 보이도록.
    }

    IEnumerator Flash()
    {
        PlayerAttackedImage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        PlayerAttackedImage.SetActive(false);
    }
}
