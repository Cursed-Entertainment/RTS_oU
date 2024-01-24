using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Selected : MonoBehaviour
{
    public bool isSelected = false;
    bool RenderOverlay;
    Texture2D Overlay;

    public bool hasBoxes = false;
    public int BoxAmount = 2;
    public int BoxSize = 4;

    public Color BoxColor;

    List<int[]> BoxPixels = new List<int[]>();
    List<int[]> BoxColorPixels = new List<int[]>();

    int GroupNumber = -1;

    public bool JustBeenSelected = false;
    float JustBeenSelectedTime = 1.0f;
    float JustBeenSelectedCounter = 0;

    int BlankSidesLength = 5;

    void Start()
    {
        if (hasBoxes)
        {
            Overlay = createSelectedTexture(20, 20, BoxAmount);
        }
        else
        {
            Overlay = createSelectedTexture(20, 20);
        }
    }

    void Update()
    {
        if (isSelected)
        {
            if (JustBeenSelected)
            {
                JustBeenSelectedCounter += Time.deltaTime;
                if (JustBeenSelectedCounter >= JustBeenSelectedTime)
                {
                    JustBeenSelected = false;
                }
            }
            else
            {
                JustBeenSelectedCounter = 0;
            }
        }
        else
        {
            JustBeenSelectedCounter = 0;
        }
    }

    public void removeFromGroup()
    {
        Selection_Controller.temp.RemoveFromGroup(gameObject, GroupNumber);
        GroupNumber = -1;
    }

    Texture2D createSelectedTexture(int w, int l)
    {
        Texture2D texture = new Texture2D(w, l, TextureFormat.ARGB32, false);
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < l; j++)
            {
                if ((i == 0 && j > BlankSidesLength && j < l - BlankSidesLength - 1) || (i == w - 1 && j > BlankSidesLength && j < l - BlankSidesLength - 1) || (j == 0 && i > BlankSidesLength && i < w - BlankSidesLength - 1))
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
                else if (i == 0 || i == w - 1 || j == l - 4 || j == 0 || j == l - 1)
                {
                    texture.SetPixel(i, j, Color.white);
                }
                else if (j == l - 2 || j == l - 3)
                {
                    if (GetComponent<Unit_Core>())
                    {
                        float h = (float)GetComponent<Unit_Core>().Stats.Health;
                        float hMax = (float)GetComponent<Unit_Core>().Stats.getMaxHealth();
                        float hRatio = h / hMax;

                        float tempI = (float)i;
                        float maxI = (float)(w - 1);
                        float iRatio = tempI / maxI;

                        if (iRatio < hRatio)
                        {
                            texture.SetPixel(i, j, new Color(0, 1, 0, 1));
                        }
                        else
                        {
                            texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                        }
                    }
                }
                else
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }
        }
        texture.Apply();
        return texture;
    }

    Texture2D createSelectedTexture(int w, int l, int boxes)
    {
        Texture2D texture = new Texture2D(w, l, TextureFormat.ARGB32, false);
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < l; j++)
            {
                if (i == 0 || i == w - 1 || j == l - 4 || j == 0 || j == l - 1)
                {
                    texture.SetPixel(i, j, Color.white);
                }
                else if (j == l - 2 || j == l - 3)
                {
                    float h = (float)GetComponent<Unit_Core>().Stats.Health;
                    float hMax = (float)GetComponent<Unit_Core>().Stats.getMaxHealth();
                    float hRatio = h / hMax;

                    float tempI = (float)i;
                    float maxI = (float)(w - 1);
                    float iRatio = tempI / maxI;

                    if (iRatio < hRatio)
                    {
                        texture.SetPixel(i, j, new Color(0, 1, 0, 1));
                    }

                    else
                    {
                        texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                    }
                }
                else if (hasBoxes)
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));

                    for (int k = 0; k < boxes; k++)
                    {
                        if ((j == BoxSize - 1 && i > w - 1 - (boxes * (BoxSize - 1))) || (i == (w - 4) - (k * (BoxSize - 1)) && j < BoxSize))
                        {
                            BoxPixels.Add(new int[] { i, j });
                        }
                        else if (j < BoxSize && i < (w - 1 - (k * (BoxSize - 1))) && i > (w - 4 - (k * (BoxSize - 1))))
                        {
                            BoxColorPixels.Add(new int[] { i, j, k });
                        }
                    }
                }
                else
                {
                    texture.SetPixel(i, j, new Color(0, 0, 0, 0));
                }
            }
        }

        foreach (int[] b in BoxPixels)
        {
            texture.SetPixel(b[0], b[1], Color.white);

            for (int h = BoxColorPixels.Count - 1; h > -1; h--)
            {
                if (BoxColorPixels[h][0] == b[0] && BoxColorPixels[h][1] == b[1])
                {
                    BoxColorPixels.RemoveAt(h);
                }
            }
        }

        foreach (int[] b in BoxColorPixels)
        {
            texture.SetPixel(b[0], b[1], BoxColor);
        }

        texture.Apply();
        return texture;
    }

    private Texture2D makeColor(float R, float G, float B, float A)
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, new Color(R, G, B, A));
        texture.Apply();
        return texture;
    }

    public Texture2D getOverlay()
    {
        return Overlay;
    }

    public void updateOverlay(float ratio)
    {
        for (float i = 1; i < 19.0f; i++)
        {
            for (int j = 17; j < 19; j++)
            {
                if ((i / 19.0f) < ratio)
                {
                    Overlay.SetPixel((int)i, j, Color.green);
                }
                else
                {
                    Overlay.SetPixel((int)i, j, new Color(0, 0, 0, 0));
                }
            }
        }
        Overlay.Apply();
    }

    public void addAmmo(int a)
    {
        foreach (int[] b in BoxColorPixels)
        {
            if (b[2] == a)
            {
                Overlay.SetPixel(b[0], b[1], BoxColor);
            }
        }

        Overlay.Apply();
    }

    public void removeAmmo(int a)
    {
        foreach (int[] b in BoxColorPixels)
        {
            if (b[2] == a)
            {
                Overlay.SetPixel(b[0], b[1], Color.clear);
            }
        }
        Overlay.Apply();
    }

    public void emptyAmmo()
    {
        foreach (int[] b in BoxColorPixels)
        {
            Overlay.SetPixel(b[0], b[1], Color.clear);
        }

        Overlay.Apply();
    }

    public void assignNumber(int number)
    {
        GroupNumber = number;
    }

    public int getGroupNumber()
    {
        return GroupNumber;
    }

    public void resetJustBeenSelected()
    {
        JustBeenSelected = true;
        JustBeenSelectedCounter = 0;
    }
}
