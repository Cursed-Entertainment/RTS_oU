using UnityEngine;

[DisallowMultipleComponent]
public class Mouse_Input : MonoBehaviour
{
    bool isLeftShiftPressed;
    bool isMouseButtonDown;
    bool isMouseButtonUp;
    bool isMouseButton;
    bool isRightMouseButtonDown;
    bool isShiftClick;
    bool isRegularClick;

    void Update()
    {
        isLeftShiftPressed = Input.GetKey(KeyCode.LeftShift);
        isMouseButtonDown = Input.GetMouseButtonDown(0);
        isMouseButton = Input.GetMouseButton(0);
        isMouseButtonUp = Input.GetMouseButtonUp(0);
        isRightMouseButtonDown = Input.GetMouseButtonDown(1);

        isShiftClick = isMouseButtonDown && isLeftShiftPressed;
        isRegularClick = isMouseButtonDown && !isLeftShiftPressed;
    }
}
