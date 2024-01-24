
using UnityEngine;

[DisallowMultipleComponent]
public class CursorDrag : MonoBehaviour
{
    GUI_Core Core;

    Vector2 dragStart, dragEnd;
    [HideInInspector]
    public bool mouseDrag = false;

    Vector3 selectBoxStart = new Vector3();
    Vector3 selectBoxEnd = new Vector3();
    Vector3[] dLoc = new Vector3[2];

    float mouseX, mouseY;

    void Awake()
    {
        Core = GetComponent<GUI_Core>();
    }

    void Update()
    {
        bool MouseButtonDown, MouseButton, MouseButtonUp;
        Vector2 MousePosition;
        GetInput(out MouseButtonDown, out MouseButton, out MouseButtonUp, out MousePosition);

        float ScreenHeight = Core.GUIScreen.ScreenHeight;
        float ScreenWidth = Core.GUIScreen.ScreenWidth;
        float mainMenuWidth = Core.mainMenuWidth;

        if (MouseButtonDown && Cam_CameraCore.temp.enableMouseClicks && Core.selectedSupport == 0)
        {
            if (MousePosition.x < ScreenWidth - mainMenuWidth)
            {
                dragStart = MousePosition;
                Cam_CameraCore.temp.EdgeMovement(false);
                mouseX = MousePosition.x;
                mouseY = MousePosition.y;
            }
            else
            {
                Core.disableOnScreenMouse = true;
            }
        }
        else if (MouseButton && !Core.disableOnScreenMouse && Cam_CameraCore.temp.enableMouseClicks && Core.selectedSupport == 0)
        {
            dragEnd = MousePosition;
            if (dragEnd.x > ScreenWidth - mainMenuWidth) dragEnd.x = ScreenWidth - mainMenuWidth;
            if (!(Mathf.Abs(MousePosition.x - mouseX) < 4 && Mathf.Abs(MousePosition.y - mouseY) < 4))
            {
                mouseDrag = true;
            }
        }
        else if (MouseButtonUp && Cam_CameraCore.temp.enableMouseClicks && Core.selectedSupport == 0)
        {
            mouseDrag = false;
            Cam_CameraCore.temp.EdgeMovement(true);
            Core.disableOnScreenMouse = false;
        }

        if (mouseDrag)
        {
            if (dragStart.y < dragEnd.y && dragStart.x > dragEnd.x)
            {
                selectBoxStart = dragStart;
                selectBoxEnd = dragEnd;
            }
            else if (dragStart.y < dragEnd.y)
            {
                selectBoxStart.y = dragStart.y;
                selectBoxEnd.y = dragEnd.y;
                selectBoxStart.x = dragEnd.x;
                selectBoxEnd.x = dragStart.x;
            }
            else if (dragStart.x > dragEnd.x)
            {
                selectBoxStart.y = dragEnd.y;
                selectBoxEnd.y = dragStart.y;
                selectBoxStart.x = dragStart.x;
                selectBoxEnd.x = dragEnd.x;
            }
            else
            {
                selectBoxStart = dragEnd;
                selectBoxEnd = dragStart;
            }

            Vector3 dTemp1 = new Vector3(Mathf.Min(selectBoxStart.x, selectBoxEnd.x), Mathf.Min(selectBoxStart.y, selectBoxEnd.y), 0);
            Vector3 dTemp2 = new Vector3(Mathf.Max(selectBoxStart.x, selectBoxEnd.x), Mathf.Max(selectBoxStart.y, selectBoxEnd.y), 0);

            dLoc[0] = dTemp1;
            dLoc[1] = dTemp2;

        }
    }

    static void GetInput(out bool MouseButtonDown, out bool MouseButton, out bool MouseButtonUp, out Vector2 MousePosition)
    {
        MouseButtonDown = Input.GetMouseButtonDown(0);
        MouseButton = Input.GetMouseButton(0);
        MouseButtonUp = Input.GetMouseButtonUp(0);
        MousePosition = Input.mousePosition;
    }

    public Vector3[] dragLocations()
    {
        return dLoc;
    }

    void OnGUI()
    {
        if (mouseDrag)
        {
            float ScreenHeight = Core.GUIScreen.ScreenHeight;
            GUI.Box(new Rect(selectBoxStart.x, ScreenHeight - selectBoxStart.y, selectBoxEnd.x - selectBoxStart.x, selectBoxStart.y - selectBoxEnd.y), "", Core.GUIStyle.DragBoxStyle);
        }
    }
}
