using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(GUI_GetFromResources))]
public class GUI_CursorSprites : MonoBehaviour
{
    GUI_GetFromResources GetFromResource;

    public bool GetFromResources = false;

    public Texture NormalCursor;
    public Texture HoverCursor;
    public Texture[] AttackCursor;
    public Texture[] GoCursor;
    public Texture[] DeployCursor;
    public Texture InvalidCursor;
    public Texture SellCursor;
    public Texture FixCursor;
    public Texture[] InteractCursor;
    public Material CursorMaterial;

    void Start()
    {
        if (GetFromResources)
        {
            GetFromResource = GetComponent<GUI_GetFromResources>();
        }
    }
}
