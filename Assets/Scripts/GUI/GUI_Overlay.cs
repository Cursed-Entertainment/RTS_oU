using UnityEngine;

[DisallowMultipleComponent]
public class GUI_Overlay : MonoBehaviour
{
    [HideInInspector]
    public Texture2D[] Overlays = new Texture2D[360];
    [HideInInspector]
    public static GUI_Overlay temp;
    public Color OverlayColor;
    public bool UseGUIColor = true;

    void Start()
    {
        if (UseGUIColor)
        {
            OverlayColor = GetComponent<GUI_Colors>().OverlayColor;
        }

        temp = this;
        for (int i = 0; i < 360; i++)
        {
            Overlays[i] = createTexture(i);
        }
    }

    Texture2D createTexture(int i)
    {
        Color blank = new Color(0, 0, 0, 0);
        Texture2D Texture = new Texture2D(50, 50, TextureFormat.ARGB32, false);
        float m, c, yCut;

        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                if (i < 90)
                {
                    if (x < 25 || y < 25)
                    {
                        Texture.SetPixel(x, y, OverlayColor);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Texture.SetPixel(x, y, OverlayColor);
                        }
                        else
                        {
                            m = Mathf.Tan((90 - i) * Mathf.Deg2Rad);
                            c = 25 - (25 * m);

                            yCut = (m * x) + c;

                            if (y < yCut)
                            {
                                Texture.SetPixel(x, y, OverlayColor);
                            }
                            else
                            {
                                Texture.SetPixel(x, y, blank);
                            }
                        }
                    }
                }
                else if (i < 180)
                {
                    if (x < 25)
                    {
                        Texture.SetPixel(x, y, OverlayColor);
                    }
                    else if (x >= 25 && y > 25)
                    {
                        Texture.SetPixel(x, y, blank);
                    }
                    else
                    {
                        if (i == 90)
                        {
                            Texture.SetPixel(x, y, OverlayColor);
                        }
                        else
                        {
                            m = Mathf.Tan((90 - i) * Mathf.Deg2Rad);
                            c = 25 - (25 * m);

                            yCut = (m * x) + c;

                            if (y < yCut)
                            {
                                Texture.SetPixel(x, y, OverlayColor);
                            }
                            else
                            {
                                Texture.SetPixel(x, y, blank);
                            }
                        }
                    }
                }
                else if (i < 270)
                {
                    if (x < 25 && y > 25)
                    {
                        Texture.SetPixel(x, y, OverlayColor);
                    }
                    else if (x >= 25)
                    {
                        Texture.SetPixel(x, y, blank);
                    }
                    else
                    {
                        if (i == 180)
                        {
                            Texture.SetPixel(x, y, OverlayColor);
                        }
                        else
                        {
                            m = Mathf.Tan((90 - i) * Mathf.Deg2Rad);
                            c = 25 - (25 * m);

                            yCut = (m * x) + c;

                            if (y < yCut)
                            {
                                Texture.SetPixel(x, y, blank);
                            }
                            else
                            {
                                Texture.SetPixel(x, y, OverlayColor);
                            }
                        }
                    }
                }
                else
                {
                    if (x >= 25 || y < 25)
                    {
                        Texture.SetPixel(x, y, blank);
                    }
                    else
                    {
                        if (i == 270)
                        {
                            Texture.SetPixel(x, y, OverlayColor);
                        }
                        else
                        {
                            m = Mathf.Tan((90 - i) * Mathf.Deg2Rad);
                            c = 25 - (25 * m);

                            yCut = (m * x) + c;

                            if (y < yCut)
                            {
                                Texture.SetPixel(x, y, blank);
                            }
                            else
                            {
                                Texture.SetPixel(x, y, OverlayColor);
                            }
                        }
                    }
                }
            }
        }

        Texture.Apply();
        return Texture;
    }
}
