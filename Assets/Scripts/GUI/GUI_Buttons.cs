using UnityEngine;

[DisallowMultipleComponent]
public class GUI_Buttons : MonoBehaviour
{
    public bool GetFromResources = false;

    public Texture2D ConstructorNormal;
    public Texture2D ConstructorHover;
    public Texture2D ConstructorSelected;

    public Texture2D SupportNormal;
    public Texture2D SupportHover;
    public Texture2D SupportSelected;

    bool SpannerSelected = false;
    bool SellSelected = false;
}
