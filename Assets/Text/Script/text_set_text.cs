using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_set_text : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine("SetText", "2");
    }

    IEnumerator SetText(string sText)
    {
        yield return new WaitForSeconds(2);
        text.text = sText;
    }
}
