using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class CreateFontTextureData : MonoBehaviour
{
    public string sCharactor = "ÎÒ°®ÄãÖÐ¹ú";
    public Font fontFile;
    public RawImage rawImage;
    public int atlasWidth = 512;
    public int atlasHeight = 512;
    public int padding = 5;

    // Start is called before the first frame update
    void Start()
    {
        rawImage.texture = CustomTextUtil.CreateFontAtlasTexture(fontFile, atlasWidth, atlasHeight, padding, sCharactor);
    }
}
