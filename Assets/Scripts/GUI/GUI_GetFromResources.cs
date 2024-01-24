using UnityEngine;

[DisallowMultipleComponent]
public class GUI_GetFromResources : MonoBehaviour
{
    GUI_Core Core;

    void Start()
    {
        Core = GetComponent<GUI_Core>();
        GetSprites();
    }

    public void GetSprites()
    {
        GUI_CursorSprites Sprites = Core.GUISprites;
        Sprites.NormalCursor = Resources.Load<Texture>("normalCursor");
        Sprites.HoverCursor = Resources.Load<Texture>("hoverCursor");
        Sprites.InvalidCursor = Resources.Load<Texture>("invalidCursor");
        Sprites.SellCursor = Resources.Load<Texture>("sellCursor");
        Sprites.FixCursor = Resources.Load<Texture>("fixCursor");
        Sprites.AttackCursor = LoadTextures("attackCursor", 3);
        Sprites.GoCursor = LoadTextures("goCursor", 3);
        Sprites.DeployCursor = LoadTextures("deployCursor", 3);
        Sprites.InteractCursor = LoadTextures("bInteractCursor", 3);
        Sprites.CursorMaterial = Resources.Load<Material>("cursorMaterial");
    }

    Texture[] LoadTextures(string folderName, int count)
    {
        Texture[] textures = new Texture[count];
        for (int i = 0; i < count; i++)
        {
            textures[i] = Resources.Load<Texture>(folderName + "/texture" + i);
        }
        return textures;
    }
}
