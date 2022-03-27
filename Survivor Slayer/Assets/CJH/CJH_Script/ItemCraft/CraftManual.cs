using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject go_Prefab;                // 실제 설치된 프리팹
    public GameObject go_PreviewPrefab;         // 미리보기 프리팹
}
public class CraftManual : MonoBehaviour
{
    private bool isActive = false;
    private bool isPreviewActive = false;

    [SerializeField] private GameObject go_BaseUI;       //아이템 UI

    [SerializeField]private Craft[] _craft;

    private GameObject go_Preview;              // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab;               // 실제 생성될 프리팹을 담을 변수
    
    [SerializeField]private Transform Player;   // 플레이어가 설치하므로 플레이어 위치를 받음

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float range;

    public void SlotClick(int _obstacleNumber)
    {
        go_Preview = Instantiate(_craft[_obstacleNumber].go_PreviewPrefab, Player.position + Player.forward,
            quaternion.identity);
       
        go_Prefab = _craft[_obstacleNumber].go_Prefab;
        isPreviewActive = true;
        go_BaseUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPreviewActive)
            Window();
        
        if(isPreviewActive)
            PreviewPositionUpdate();

        if (Input.GetButtonDown("Fire2"))
            Build();
        
        if(Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    private void Build()
    {
        if (isPreviewActive && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, go_Preview.transform.rotation);
            Destroy(go_Preview);
            isActive = false;
            isPreviewActive = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(Player.position, Player.forward, out hitInfo, range, _layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
                
                Vector3 dir = go_Preview.transform.position - Player.position;
                dir.y = 0;
                dir.Normalize();
                Quaternion to = Quaternion.LookRotation(dir);
                go_Preview.transform.rotation = Quaternion.RotateTowards(go_Preview.transform.rotation, to, 1);

            }
        }
    }

    private void Window()
    {
        if(!isActive)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActive = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActive = false;
        go_BaseUI.SetActive(false);
    }

    private void Cancel()
    {
        if(isPreviewActive)
            Destroy(go_Preview);
        isActive = false;
        isPreviewActive = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }
}
