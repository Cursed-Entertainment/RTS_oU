using UnityEngine;

[DisallowMultipleComponent]
public class GUI_Colors : MonoBehaviour
{
    public Color ColorScheme_Color00 = new Color(255f / 255f, 130f / 255f, 0f);
    public Color ColorScheme_Color01 = new Color(4f / 255f, 30f / 255f, 66f / 255f);
    public Color ColorScheme_Color02 = new Color(127f / 255f, 106f / 255f, 83f / 255f);
    public Color ColorScheme_Color03 = new Color(198f / 255f, 189f / 255f, 180f / 255f);
    public Color ColorScheme_Color04 = new Color(238f / 255f, 234f / 255f, 229f / 255f);

    public Color ColorScheme_Color_Transparent_00 = new Color(255f / 255f, 130f / 255f, 0f, 0.25f);
    public Color ColorScheme_Color_Transparent_01 = new Color(4f / 255f, 30f / 255f, 66f / 255f, 0.25f);
    public Color ColorScheme_Color_Transparent_02 = new Color(127f / 255f, 106f / 255f, 83f / 255f, 0.25f);
    public Color ColorScheme_Color_Transparent_03 = new Color(198f / 255f, 189f / 255f, 180f / 255f, 0.25f);
    public Color ColorScheme_Color_25_04 = new Color(238f / 255f, 234f / 255f, 229f / 255f, 0.25f);

  public  bool UseColorScheme = false;

    public Color[] ObjectColors;

    [HideInInspector]
    public Texture2D BlackTexture;
    [HideInInspector]
    public Texture2D GreenTexture;
    [HideInInspector]
    public Texture2D RedTexture;

    public Color OverlayColor = new Color(0, 0, 1, 0.2f);

    public Color Backround = Color.grey;

    public Color TopButtonText = Color.white;
    public Color ItemButtonText = Color.white;
    public Color MoneyLabelText = Color.white;
    public Color SupportButtonText = Color.white;

    void Start()
    {
        BlackTexture = MakeColor(0, 0, 0, 1);
        GreenTexture = MakeColor(0, 1, 0, 1);
        RedTexture = MakeColor(1, 0, 0, 1);

        if (UseColorScheme)
        {
            OverlayColor = ColorScheme_Color_Transparent_00;

            TopButtonText = ColorScheme_Color04;
            ItemButtonText = ColorScheme_Color04;
            MoneyLabelText = ColorScheme_Color04;
            SupportButtonText = ColorScheme_Color04;
        }
    }

    public Texture2D MakeColor(float R, float G, float B, float A)
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, new Color(R, G, B, A));
        texture.Apply();
        return texture;
    }
}
