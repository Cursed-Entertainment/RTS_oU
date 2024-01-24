using UnityEngine;

[DisallowMultipleComponent]
public class GUI_Style : MonoBehaviour
{
    GUI_Core Core;

    public GUIStyle DragBoxStyle = new GUIStyle();
    public GUIStyle PowerBarStyle = new GUIStyle();
    public GUIStyle BackgroundStyle;
    public GUIStyle TopButtonStyle; 
    public GUIStyle ItemButtonStyle;
    public GUIStyle MoneyLabel;
    public GUIStyle SupportButtonStyle;
    public GUIStyle EndGameStyle;
    public GUIStyle CostHoverStyle;

    public Font _Font;

    public bool UseGUIColor = true;
    public bool UseGUIFont = true;
    public bool UseGUIFont_Size = true;

    void Awake()
    {
        Core = GetComponent<GUI_Core>();
    }

    void Start()
    {
        if (UseGUIFont)
        {
            TopButtonStyle.font = _Font;
        }

        if (UseGUIFont_Size)
        {
            SetFontSize();
        }        

        AlignText();

        if (UseGUIColor)
        {
            InitItemButtonStyle();
            InitTopBottonStyle();
            InitMoneyLabelStyle();
            InitSupportButtonStyle();
        }

        InitDragBoxStyle();
        InitBackgroundStyle();
        InitPowerBarStyle();
    }

    void SetFontSize()
    {
        TopButtonStyle.fontSize = 20;
        SupportButtonStyle.fontSize = 10;
    }

    void AlignText()
    {
        TopButtonStyle.alignment = TextAnchor.MiddleCenter;
        SupportButtonStyle.alignment = TextAnchor.MiddleCenter;
        ItemButtonStyle.alignment = TextAnchor.MiddleCenter;
        MoneyLabel.alignment = TextAnchor.MiddleCenter;
        SupportButtonStyle.alignment = TextAnchor.MiddleCenter;
    }

    void InitMoneyLabelStyle()
    {
        Color _Color = Core.GUIColors.MoneyLabelText;

        MoneyLabel.normal.textColor = _Color;
        MoneyLabel.hover.textColor = _Color;
        MoneyLabel.active.textColor = _Color;
    }

    void InitTopBottonStyle()
    {
        Color _Color = Core.GUIColors.TopButtonText;
        
        TopButtonStyle.normal.textColor = _Color;
        TopButtonStyle.hover.textColor = _Color;
        TopButtonStyle.active.textColor = _Color;
    }

    void InitSupportButtonStyle()
    {
        Color _Color = Core.GUIColors.SupportButtonText;

        SupportButtonStyle.normal.textColor = _Color;
        SupportButtonStyle.hover.textColor = _Color;
        SupportButtonStyle.active.textColor = _Color;
    }

    void InitItemButtonStyle()
    {
        Color _Color = Core.GUIColors.ItemButtonText;

        ItemButtonStyle.normal.textColor = _Color;
        ItemButtonStyle.hover.textColor = _Color;
        ItemButtonStyle.active.textColor = _Color;
    }

    void InitPowerBarStyle()
    {
        PowerBarStyle.border.bottom = 2;
        PowerBarStyle.border.top = 2;
        PowerBarStyle.border.left = 2;
        PowerBarStyle.border.right = 2;
    }

    void InitBackgroundStyle()
    {
        BackgroundStyle.normal.background = Core.GUIColors.MakeColor(0.3f, 0.3f, 0.9f, 1.0f);
        BackgroundStyle.border.bottom = 1;
        BackgroundStyle.border.top = 1;
        BackgroundStyle.border.left = 1;
        BackgroundStyle.border.right = 1;
    }

    void InitDragBoxStyle()
    {
        DragBoxStyle.normal.background = Core.GUIColors.MakeColor(0.8f, 0.8f, 0.8f, 0.3f);
        DragBoxStyle.border.bottom = 1;
        DragBoxStyle.border.top = 1;
        DragBoxStyle.border.left = 1;
        DragBoxStyle.border.right = 1;
    }
}
