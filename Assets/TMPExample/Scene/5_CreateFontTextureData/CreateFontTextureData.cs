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
    public SpriteRenderer sprite;
    public int atlasWidth = 512;
    public int atlasHeight = 512;
    public int padding = 5;

    // Start is called before the first frame update
    void Start()
    {
        Texture2D texture2D = CustomTextUtil.CreateFontAtlasTexture(fontFile, atlasWidth, atlasHeight, padding, sCharactor);

        // sprite.material.SetTexture("_MainTex", texture2D);
        sprite.sprite = Sprite.Create(texture2D, new Rect(0, 0, atlasWidth, atlasHeight), new Vector2(0.5f, 0.5f));
        
    }
}
