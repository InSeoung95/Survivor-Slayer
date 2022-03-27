using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> _colliderList = new List<Collider>();

    [SerializeField] private int layerGround;       //Ground 레이어
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] private Material Green;        //설치 가능
    [SerializeField] private Material Red;          //설치 불가능


    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if(_colliderList.Count > 0)
            SetColor(Red);
        else
            SetColor(Green);
    }

    private void SetColor(Material mat)
    {
        foreach (Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            _colliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            _colliderList.Remove(other);
    }

    public bool isBuildable()
    {
        return _colliderList.Count == 0;
    }
}
