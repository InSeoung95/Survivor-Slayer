using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWall : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider collider;
    public float ActivateTime; // 벽활성화 시간.
    public float NonActivateTime;// 벽 비활성화 시간.
    [SerializeField] private bool isActivate=false; // 현재 활성화된 상태인지

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActivate)
        {
            StartCoroutine(WallActivate());
        }
    }

    IEnumerator WallActivate()
    {
        isActivate = true;
        meshRenderer.enabled = true;
        collider.enabled = true;

        yield return new WaitForSeconds(ActivateTime);

        meshRenderer.enabled = false;
        collider.enabled = false;
        yield return new WaitForSeconds(NonActivateTime);
        isActivate = false;
    }
}
