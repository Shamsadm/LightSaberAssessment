using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGrow : MonoBehaviour
{
    [SerializeField] private float time;
    private float defSize;
    void Awake()
    {
        
        GameManger.startGame += restartGrow;
        defSize = transform.localScale.x;
    }

    private void restartGrow()
    {
        transform.localScale= new Vector3(defSize,transform.localScale.y,defSize);
        StopAllCoroutines();
        StartCoroutine(Grow());
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Grow());
    }
    // function for grow the cyclinder
    IEnumerator Grow()
    {
        float ratio = defSize/time;
        while(transform.localScale.x<defSize*2)
        {
            float current = transform.localScale.x+(ratio*Time.deltaTime);
            // print(ratio*Time.deltaTime);
            transform.localScale = new Vector3(current,transform.localScale.y,current);
            yield return null;
        }
    }
    void OnDestroy()
    {
        GameManger.startGame -= restartGrow;
    }

    
}
