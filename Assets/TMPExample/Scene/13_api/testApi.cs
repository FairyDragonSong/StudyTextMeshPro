using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testApi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            test3();


        }
    }

    /// <summary>
    /// 根据屏幕位置给出字符的下标
    /// </summary>
    void test1()
    {
        int idx = TMP_TextUtilities.FindIntersectingCharacter(this.GetComponent<TMP_Text>(), Input.mousePosition, null, true);
        Debug.Log(idx);
    }

    /// <summary>
    /// 根据屏幕位置给出在的行数
    /// </summary>
    void test2()
    {
        int idx = TMP_TextUtilities.FindIntersectingLine(this.GetComponent<TMP_Text>(), Input.mousePosition, null);
        Debug.Log(idx);
    }

    void test3()
    {
        int idx = TMP_TextUtilities.FindIntersectingLink(this.GetComponent<TMP_Text>(), Input.mousePosition, null);
        Debug.Log(idx);
    }
}
