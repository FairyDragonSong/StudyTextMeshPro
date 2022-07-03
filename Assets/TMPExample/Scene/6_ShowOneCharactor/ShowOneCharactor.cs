using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class ShowOneCharactor : MonoBehaviour
{
    public string sCharactor = "�Ұ����й�";
    public Font fontFile;
    public int atlasWidth = 512;
    public int atlasHeight = 512;
    public int padding = 5;

    public string targetString = "��";

    // Start is called before the first frame update
    void Start()
    {
        List<Glyph> glyphsPacked = CustomTextUtil.GenerateGlyphs(fontFile, atlasWidth, atlasHeight, padding, sCharactor);
        Texture2D tex = CustomTextUtil.GenerateTexture2D(glyphsPacked, atlasWidth, atlasHeight, padding);
        uint tarIdx = CustomTextUtil.TryGetGlyphIndex(targetString[0]);
        Glyph tarGlyph = null;
        foreach (Glyph glyph in glyphsPacked)
        {
            if (glyph.index == tarIdx)
            {
                tarGlyph = glyph;
                break;
            }
        }
        PopulateFont(tarGlyph, tex);
    }

    void PopulateFont(Glyph tarGlyph, Texture2D tex)
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        // mesh.uv
        
        mesh.vertices = new Vector3[]
        {
            new Vector3(-1, -1),
            new Vector3(1, -1),
            new Vector3(-1, 1),
            new Vector3(1, 1),
        };

        mesh.triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3,
            0, 2, 1,
            2, 3, 1
        };

        // padding ����Ҫ���ϣ����߻���ּ��в�����������.
        float x1 = (tarGlyph.glyphRect.x - padding) * 1.0f / atlasWidth;
        float y1 = (tarGlyph.glyphRect.y - padding) * 1.0f / atlasHeight;
        float x2 = (tarGlyph.glyphRect.x + tarGlyph.glyphRect.width + padding) * 1.0f / atlasWidth;
        float y2 = (tarGlyph.glyphRect.y + tarGlyph.glyphRect.height + padding) * 1.0f / atlasHeight;
        mesh.uv = new Vector2[]
        {
            new Vector2(x1, y1),
            new Vector2(x2, y1),
            new Vector2(x1, y2),
            new Vector2(x2, y2),
        };

        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        // ע�� �Զ����shader��Ҫ���� Blend SrcAlpha OneMinusSrcAlpha ��ϱ�ǩ�����߳����лῴ����.
        Shader shader = Shader.Find("Unlit/ShowOneCharactor_SDF");
        // �����л�TMP���õ�shader
        // Shader shader = Shader.Find("TextMeshPro/Mobile/Bitmap");
        Material material = new Material(shader);
        
        material.SetTexture("_MainTex", tex);
        meshRenderer.material = material;
    }
}