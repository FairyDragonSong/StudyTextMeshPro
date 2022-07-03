using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂在text组件下，借助rawImage 查看text的maintexture图片
/// </summary>
public class text_property_mainTexture : MonoBehaviour
{
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        rawImage.texture = this.GetComponent<Text>().mainTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
