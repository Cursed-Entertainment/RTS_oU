using UnityEngine;

[DisallowMultipleComponent]
public class Cam_Position : MonoBehaviour
{
    Camera _Camera;
    Cam_CameraCore Core;

    void Awake()
    {
        _Camera = GetComponent<Camera>();
        Core = GetComponent<Cam_CameraCore>();
    }

    void Start()
    {
        Core.CurrentScrollSpeed = Core.ScrollSpeed;
    }

    public void CheckCameraPos()
    {
        float xMin = Core.xMin;
        float xMax = Core.xMax;
        float yMin = Core.yMin;
        float yMax = Core.yMax;

        Ray r1 = _Camera.ViewportPointToRay(new Vector3(0, 1, 0));
        Ray r2 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, Screen.height - 1, 0));
        Ray r3 = _Camera.ViewportPointToRay(new Vector3(0, 0, 0));

        float left, right, top, bottom;

        RaycastHit h1;

        Physics.Raycast(r1, out h1, Mathf.Infinity, 1 << 16);
        left = h1.point.x;
        top = h1.point.z;

        Physics.Raycast(r2, out h1, Mathf.Infinity, 1 << 16);
        right = h1.point.x;

        Physics.Raycast(r3, out h1, Mathf.Infinity, 1 << 16);
        bottom = h1.point.z;

        if (left < Core.xMin)
        {
            _Camera.transform.Translate(new Vector3(xMin - left, 0, 0), Space.World);
        }
        else if (right > xMax)
        {
            _Camera.transform.Translate(new Vector3(xMax - right, 0, 0), Space.World);
        }

        if (bottom < yMin)
        {
            _Camera.transform.Translate(new Vector3(0, 0, yMin - bottom), Space.World);
        }
        else if (top > yMax)
        {
            _Camera.transform.Translate(new Vector3(0, 0, yMax - top), Space.World);
        }
    }
}
