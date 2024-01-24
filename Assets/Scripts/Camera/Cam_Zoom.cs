using UnityEngine;

[DisallowMultipleComponent]
public class Cam_Zoom : MonoBehaviour
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

    void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        float ScrollSpeed = Core.ScrollSpeed;
        float CurrentScrollSpeed = Core.CurrentScrollSpeed;
        float zoomRate = Core.zoomRate;
        bool Tracking = Core.Tracking;

        if (Tracking)
        {
            if (CurrentScrollSpeed <= 20)
            {
                CurrentScrollSpeed *= 1.01f;
            }

            Core.Position.CheckCameraPos();
        }
        else
        {
            CurrentScrollSpeed = ScrollSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (scrollWheel > 0)
            {
                if (_Camera.orthographic)
                {
                    _Camera.orthographicSize = Mathf.Max(10, _Camera.orthographicSize - zoomRate);

                    if (MiniMap_Controller.temp != null)
                    {
                        MiniMap_Controller.temp.UpdateCameraSize();
                    }
                }
                else
                {
                    _Camera.fieldOfView = Mathf.Max(10, _Camera.fieldOfView - zoomRate);
                }
            }
            else
            {
                if (_Camera.orthographic)
                {
                    _Camera.orthographicSize = Mathf.Min(30, _Camera.orthographicSize + zoomRate * 2);

                    if (MiniMap_Controller.temp != null)
                    {
                        MiniMap_Controller.temp.UpdateCameraSize();
                    }
                }
                else
                {
                    _Camera.fieldOfView = Mathf.Min(50, _Camera.fieldOfView + zoomRate * 2);
                }
            }

            Ray ray2 = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2))
            {
                Vector3 t1 = _Camera.WorldToScreenPoint(hit2.point + new Vector3(grid.mGrid.getGridSize() / 2, 0, 0));
                Vector3 t2 = _Camera.WorldToScreenPoint(hit2.point - new Vector3(grid.mGrid.getGridSize() / 2, 0, 0));
                Core.unitScreenSize = t1.x - t2.x;
                GUI_Core.temp.setUnitSize(Core.unitScreenSize);
            }
        }
    }
}
