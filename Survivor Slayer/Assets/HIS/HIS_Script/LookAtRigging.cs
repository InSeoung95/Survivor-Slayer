using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtRigging : MonoBehaviour
{
    private Rig rig;
    private float targetWeight;



    private void Awake()
    {
        rig = GetComponent<Rig>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * 10f);
    }
}
