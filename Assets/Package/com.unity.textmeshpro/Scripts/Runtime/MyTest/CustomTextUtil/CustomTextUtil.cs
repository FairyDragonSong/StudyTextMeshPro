
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

public class CustomTextUtil
{
    static GlyphRenderMode glyphRenderMode = GlyphRenderMode.SDFAA;
    enum FontPackingModes { Fast = 0, Optimum = 4 };
    public static Texture2D CreateFontAtlasTexture(Font fontFile, int atlasWidth, int atlasHeight, int padding, string readyPackCharacter)
    {
        List<Glyph> glyphsPacked = GenerateGlyphs(fontFile, atlasWidth, atlasHeight, padding, readyPackCharacter);
        return GenerateTexture2D(glyphsPacked, atlasWidth, atlasHeight, padding);
    }


    // 生成图片
    public static Texture2D GenerateTexture2D(List<Glyph> glyphsPacked, int atlasWidth, int atlasHeight, int padding)
    {
        byte[] atlasTextureBuffer;
        atlasTextureBuffer = new byte[atlasWidth * atlasHeight];
        FontEngine.RenderGlyphsToTexture(glyphsPacked, padding, glyphRenderMode, atlasTextureBuffer, atlasWidth, atlasHeight);
        Texture2D fontAtlasTexture = new Texture2D(atlasWidth, atlasHeight, TextureFormat.Alpha8, false, true);
        Color32[] colors = new Color32[atlasWidth * atlasHeight];
        for (int i = 0; i < colors.Length; i++)
        {
            byte c = atlasTextureBuffer[i];
            colors[i] = new Color32(c, c, c, c);
        }
        atlasTextureBuffer = null;
        if ((glyphRenderMode & GlyphRenderMode.RASTER) == GlyphRenderMode.RASTER || (glyphRenderMode & GlyphRenderMode.RASTER_HINTED) == GlyphRenderMode.RASTER_HINTED)
            fontAtlasTexture.filterMode = FilterMode.Point;
        fontAtlasTexture.SetPixels32(colors, 0);
        fontAtlasTexture.Apply(false, false);

        return fontAtlasTexture;
    }

    public static List<Glyph> GenerateGlyphs(Font fontFile, int atlasWidth, int atlasHeight, int padding, string readyPackCharacter)
    {
        // 初始化
        FontEngineError errorCode = FontEngine.InitializeFontEngine();
        if (errorCode != FontEngineError.Success)
        {
            Debug.Log(errorCode);
        }


        // 加载字体
        errorCode = FontEngine.LoadFontFace(fontFile);
        if (errorCode != FontEngineError.Success)
        {
            Debug.Log(errorCode, fontFile);
        }

        // todo 不同的loadFlag的区别
        GlyphLoadFlags glyphLoadFlags = ((GlyphRasterModes)glyphRenderMode & GlyphRasterModes.RASTER_MODE_HINTED) == GlyphRasterModes.RASTER_MODE_HINTED
            ? GlyphLoadFlags.LOAD_RENDER
            : GlyphLoadFlags.LOAD_RENDER | GlyphLoadFlags.LOAD_NO_HINTING;

        glyphLoadFlags = ((GlyphRasterModes)glyphRenderMode & GlyphRasterModes.RASTER_MODE_MONO) == GlyphRasterModes.RASTER_MODE_MONO
            ? glyphLoadFlags | GlyphLoadFlags.LOAD_MONOCHROME
            : glyphLoadFlags;


        // 设置字体大小
        FontEngine.SetFaceSize(204);

        // 获取所有需要打包字体的Unicode值
        uint[] characterSet = new uint[readyPackCharacter.Length];
        for (int i = 0; i < readyPackCharacter.Length; i++)
        {
            uint unicode = readyPackCharacter[i];
            characterSet[i] = unicode;
        }


        // 从unicode找到字体中的 字形（glyph）数据标记
        List<uint> glyphsIdxList = new List<uint>();
        for (int i = 0; i < characterSet.Length; i++)
        {
            uint unicode = characterSet[i];
            uint glyphIndex;

            if (FontEngine.TryGetGlyphIndex(unicode, out glyphIndex))
            {
                glyphsIdxList.Add(glyphIndex);
            }
        }

        List<Glyph> glyphsToPack = new List<Glyph>(); //打包前的数据
        for (int i = 0; i < glyphsIdxList.Count; i++)
        {
            uint glyphIndex = glyphsIdxList[i];
            Glyph glyph;

            if (FontEngine.TryGetGlyphWithIndexValue(glyphIndex, glyphLoadFlags, out glyph))
            {
                if (glyph.glyphRect.width > 0 && glyph.glyphRect.height > 0)
                {
                    glyphsToPack.Add(glyph);
                }
            }
        }

        List<Glyph> glyphsPacked = new List<Glyph>(); //打包好了后的数据
        FontPackingModes packingMode = FontPackingModes.Fast;
        List<GlyphRect> freeGlyphRects = new List<GlyphRect>();
        freeGlyphRects.Add(new GlyphRect(0, 0, atlasWidth - 1, atlasHeight - 1));
        List<GlyphRect> usedGlyphRects = new List<GlyphRect>();
        FontEngine.TryPackGlyphsInAtlas(glyphsToPack, glyphsPacked, padding, (GlyphPackingMode)packingMode, glyphRenderMode, atlasWidth, atlasHeight, freeGlyphRects, usedGlyphRects);

        return glyphsPacked;
    }

    public static uint TryGetGlyphIndex(uint unicode)
    {
        uint idx = 0;
        if (FontEngine.TryGetGlyphIndex(unicode, out idx))
        {
            return idx;
        }

        Debug.LogError("找不到");
        return 0;
    }

}
