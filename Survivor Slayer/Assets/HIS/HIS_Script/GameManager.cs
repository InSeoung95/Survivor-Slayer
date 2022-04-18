using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance=null;
    public static GameManager instance
    {
        get
        {
            if(m_instance==null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    public int max_round; // 게임 최대 라운드
    public int current_round; // 현재 진행 중인 라운드

    public bool GameOver { get; private set; }

    private void Awake()
    {
        if(instance!=this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRound()
    {

    }
}
