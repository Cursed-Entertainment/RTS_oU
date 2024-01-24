using UnityEngine;

[DisallowMultipleComponent]
public class GUI_Screen : MonoBehaviour
{
    GUI_Core Core;

    [HideInInspector]
    public int ScreenHeight;
    [HideInInspector]
    public int ScreenWidth;

    void Start()
    {
        Core = GetComponent<GUI_Core>();        
    }

    public void InitScreen()
    {
        ScreenHeight = Screen.height;
        ScreenWidth = Screen.width;
    }
}
