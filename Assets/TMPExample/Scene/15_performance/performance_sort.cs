using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class performance_sort : MonoBehaviour
{

    private int iPerLine = 10;
    private int iInterval = 2;

    private int iStart = -20;

    private int numMax = 50;
    // Start is called before the first frame update
    void Start()
    {
        Transform tr;
        Transform obj = transform.GetChild(0);
        for (int i = 0; i < numMax; i++)
        {
            tr = GameObject.Instantiate(obj);
            tr.parent = transform;
            tr.localPosition =
                new Vector3((i % iPerLine) * iInterval + iStart, (i / iPerLine) * iInterval + iStart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
