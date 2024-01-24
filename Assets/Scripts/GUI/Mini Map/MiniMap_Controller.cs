using UnityEngine;

[DisallowMultipleComponent]
public class MiniMap_Controller : MonoBehaviour
{
    Transform CameraTransform;
    Camera MiniMapCamera;
    float WorldWidth;
    float WorldHeight;
    float zOffset;

    float mapScreenLeft, mapScreenTop, mapScreenRight, mapScreenBottom;

    public static MiniMap_Controller temp;

    float ScreenWidth;
    float GuiWidthWorldSpace;

    Vector3 MiniMapBottomLeft;
    Vector3 MiniMapTopLeft;
    Vector3 MiniMapTopRight;
    Vector3 MiniMapBottomRight;

    Vector3[] miniMapCoords;

    Material viewportMaterial;

    float TestWidth;
    float TestHeight;

    public Texture viewportTexture;

    Texture2D unitColor;

    Vector3 gPos;
    Vector3 mapGPos;
    Rect UnitPos;

    Vector3[] mapBounds = new Vector3[2];

    Camera _Camera;
    MiniMap_Materials Materials;

    void Start()
    {
        _Camera = Camera.main;

        TestWidth = Screen.width;
        TestHeight = Screen.height;

        Materials = GetComponent<MiniMap_Materials>();
        viewportMaterial = Materials.viewportMaterial;

        temp = this;

        CameraTransform = _Camera.transform;
        SetUpCamera();

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float viewPortY = 3.0f / 4.0f;
        float viewPortHeight = 1.0f / 4.5f;

        float viewPortWidth = 1.0f / (4.5f * aspectRatio);
        float viewPortX = 1 - (0.25f / aspectRatio);

        MiniMapCamera.rect = new Rect(viewPortX, viewPortY, viewPortWidth, viewPortHeight);

        float miniMapX = _Camera.ViewportToScreenPoint(new Vector3(viewPortX, viewPortY, 0)).x;
        float miniMapX2 = _Camera.ViewportToScreenPoint(new Vector3(viewPortX + viewPortWidth, viewPortY, 0)).x;
        float miniMapWidth = miniMapX2 - miniMapX;
        float miniMapGap = Screen.width - miniMapX2;

        GUI_Core.temp.mainMenuWidth = miniMapWidth + (2 * miniMapGap);

        Vector3 v1;
        Vector3 v2;
        Vector3 v3;
        Vector3 v4;

        if (_Camera.orthographic)
        {
            Ray ray1 = _Camera.ScreenPointToRay(new Vector3(0, 0, 0));
            Ray ray2 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, Screen.height - 1, 0));
            Ray ray3 = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;

            Physics.Raycast(ray1, out hit, Mathf.Infinity, 1 << 16);
            v1 = hit.point;

            Physics.Raycast(ray2, out hit, Mathf.Infinity, 1 << 16);
            v2 = hit.point;

            Physics.Raycast(ray3, out hit, Mathf.Infinity, 1 << 16);
            zOffset = hit.point.z - CameraTransform.position.z;
            Cam_CameraCore.temp.zOffset = zOffset;

            Vector3 temp1 = MiniMapCamera.WorldToScreenPoint(v1);
            Vector3 temp2 = MiniMapCamera.WorldToScreenPoint(v2);

            WorldWidth = temp2.x - temp1.x;
            WorldHeight = temp2.y - temp1.y;

            ScreenWidth = v2.x - v1.x;
        }
        else
        {
            Ray ray1 = _Camera.ScreenPointToRay(new Vector3(0, 0, 0));
            Ray ray2 = _Camera.ScreenPointToRay(new Vector3(0, Screen.height - 1, 0));
            Ray ray3 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, Screen.height - 1, 0));
            Ray ray4 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, 0, 0));

            RaycastHit hit;
            Physics.Raycast(ray1, out hit, Mathf.Infinity, 1 << 16);
            v1 = hit.point;

            Physics.Raycast(ray3, out hit, Mathf.Infinity, 1 << 16);
            v2 = hit.point;

            Physics.Raycast(ray2, out hit, Mathf.Infinity, 1 << 16);
            v3 = hit.point;

            Physics.Raycast(ray4, out hit, Mathf.Infinity, 1 << 16);
            v4 = hit.point;

            MiniMapBottomLeft = MiniMapCamera.WorldToScreenPoint(v1);
            MiniMapTopLeft = MiniMapCamera.WorldToScreenPoint(v2);
            MiniMapTopRight = MiniMapCamera.WorldToScreenPoint(v3);
            MiniMapBottomRight = MiniMapCamera.WorldToScreenPoint(v4);

            Ray ray5 = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Physics.Raycast(ray5, out hit, Mathf.Infinity, 1 << 16);
            zOffset = hit.point.z - CameraTransform.position.z;
            Cam_CameraCore.temp.zOffset = zOffset;

            miniMapCoords = new Vector3[4] { MiniMapBottomLeft, MiniMapTopLeft, MiniMapTopRight, MiniMapBottomRight };

            Ray ray7 = MiniMapCamera.ViewportPointToRay(new Vector3(0, 0, 0));
            Ray ray8 = MiniMapCamera.ViewportPointToRay(new Vector3(1, 1, 0));

            Physics.Raycast(ray7, out hit, Mathf.Infinity, 1 << 16);
            mapBounds[0] = hit.point;

            Physics.Raycast(ray8, out hit, Mathf.Infinity, 1 << 16);
            mapBounds[1] = hit.point;

            Cam_CameraCore.temp.SetMaxValues(mapBounds[0].x, mapBounds[1].x, mapBounds[0].z, mapBounds[1].z);
        }

        mapScreenLeft = MiniMapCamera.ViewportToScreenPoint(new Vector3(0, 0, 0)).x;
        mapScreenBottom = MiniMapCamera.ViewportToScreenPoint(new Vector3(0, 0, 0)).y;
        mapScreenRight = MiniMapCamera.ViewportToScreenPoint(new Vector3(1, 1, 0)).x;
        mapScreenTop = MiniMapCamera.ViewportToScreenPoint(new Vector3(1, 1, 0)).y;

        Cam_CameraCore.temp.mapBounds = getMapBounds();
        GUI_Core.temp.miniMapBounds = getMapBounds();
        GUI_Core.temp.miniMapRect = getMapRect();

        RaycastHit hit2;
        Ray ray6 = _Camera.ScreenPointToRay(new Vector3(Screen.width - 1, Screen.height - 1, 0));
        Physics.Raycast(ray6, out hit2, Mathf.Infinity, 1 << 16);
        GuiWidthWorldSpace = hit2.point.x - v2.x;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > mapScreenLeft && Input.mousePosition.x < mapScreenRight && Input.mousePosition.y > mapScreenBottom && Input.mousePosition.y < mapScreenTop && (!GUI_Core.temp.radarClosed || RTS_Player.temp.AlwaysDisplayRadar))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray r1;
            RaycastHit h1;

            if (_Camera.orthographic)
            {
                if (mousePos.x > mapScreenLeft && mousePos.x < mapScreenLeft + (getCameraScreen().width / 2))
                {
                    mousePos.x = mapScreenLeft + (getCameraScreen().width / 2);
                }
                else if (mousePos.x < mapScreenRight && mousePos.x > mapScreenRight - (getCameraScreen().width / 2))
                {
                    mousePos.x = mapScreenRight - (getCameraScreen().width / 2) + 1;
                }

                if (mousePos.y > mapScreenBottom && mousePos.y < mapScreenBottom + (getCameraScreen().height / 2))
                {
                    mousePos.y = mapScreenBottom + (getCameraScreen().height / 2) - 1;
                }
                else if (mousePos.y < mapScreenTop && mousePos.y > mapScreenTop - (getCameraScreen().height / 2))
                {
                    mousePos.y = mapScreenTop - (getCameraScreen().height / 2);
                }

                r1 = MiniMapCamera.ScreenPointToRay(mousePos);

                Physics.Raycast(r1, out h1);
                CameraTransform.position = new Vector3(h1.point.x + (GuiWidthWorldSpace / 2), CameraTransform.position.y, h1.point.z - zOffset);
            }
            else
            {
                r1 = MiniMapCamera.ScreenPointToRay(mousePos);
                Physics.Raycast(r1, out h1, Mathf.Infinity, 1 << 16);

                CameraTransform.position = new Vector3(h1.point.x + (GuiWidthWorldSpace / 2), CameraTransform.position.y, h1.point.z - zOffset);
                Cam_CameraCore.temp.Position.CheckCameraPos();
            }
        }

        if (!_Camera.orthographic)
        {
            Vector3 v1;
            Vector3 v2;
            Vector3 v3;
            Vector3 v4;

            Ray ray1 = _Camera.ScreenPointToRay(new Vector3(0, 0, 0));
            Ray ray2 = _Camera.ScreenPointToRay(new Vector3(0, Screen.height - 1, 0));
            Ray ray3 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, Screen.height - 1, 0));
            Ray ray4 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, 0, 0));

            RaycastHit hit;
            Physics.Raycast(ray1, out hit, Mathf.Infinity, 1 << 16);
            v1 = hit.point;

            Physics.Raycast(ray2, out hit, Mathf.Infinity, 1 << 16);
            v2 = hit.point;

            Physics.Raycast(ray3, out hit, Mathf.Infinity, 1 << 16);
            v3 = hit.point;

            Physics.Raycast(ray4, out hit, Mathf.Infinity, 1 << 16);
            v4 = hit.point;

            MiniMapBottomLeft = MiniMapCamera.WorldToScreenPoint(v1);
            MiniMapTopLeft = MiniMapCamera.WorldToScreenPoint(v2);
            MiniMapTopRight = MiniMapCamera.WorldToScreenPoint(v3);
            MiniMapBottomRight = MiniMapCamera.WorldToScreenPoint(v4);

            miniMapCoords = new Vector3[4] { MiniMapBottomLeft, MiniMapTopLeft, MiniMapTopRight, MiniMapBottomRight };
        }
    }

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
        {
            RTS_TeamColors TeamColors = GetComponent<MiniMap_Core>().GameController.GetComponent<RTS_TeamColors>();

            foreach (GameObject g in RTS_Player.temp.Units)
            {
                gPos = g.transform.position;
                mapGPos = MiniMapCamera.WorldToScreenPoint(gPos);
                UnitPos = new Rect(mapGPos.x - 1, Screen.height - mapGPos.y - 1, 3, 3);

                bool isFriendly = g.tag == "Player" || g.tag == "fBuilding";

                if (isFriendly)
                {
                    unitColor = TeamColors.ColorToTexture(TeamColors.PlayerColor);
                    Graphics.DrawTexture(UnitPos, unitColor);
                }
                else
                {
                    unitColor = TeamColors.ColorToTexture(TeamColors.EnemyColor);
                    if (!grid.mGrid.returnClosestTile(g.transform.position).shrouded)
                    {
                        Graphics.DrawTexture(UnitPos, unitColor);
                    }
                }
            }

            foreach (GameObject g in RTS_Player.temp.Buildings)
            {
                gPos = g.transform.position;
                mapGPos = MiniMapCamera.WorldToScreenPoint(gPos);

                BoxCollider boxCollider = g.GetComponent<BoxCollider>();

                float wLength = boxCollider.size.z / 2;
                float wWidth = boxCollider.size.x / 2;

                Vector3 topLeft = MiniMapCamera.WorldToScreenPoint(gPos + new Vector3(-wWidth, 0, wLength));
                Vector3 bottomRight = MiniMapCamera.WorldToScreenPoint(gPos + new Vector3(wWidth, 0, -wLength));

                float mapWidth = bottomRight.x - topLeft.x;
                float mapLength = topLeft.y - bottomRight.y;

                UnitPos = new Rect(mapGPos.x - (mapWidth / 2), Screen.height - mapGPos.y - (mapLength / 2), mapWidth, mapLength);

                bool isFriendly = g.tag == "fBuilding";

                if (isFriendly)
                {
                    unitColor = TeamColors.ColorToTexture(TeamColors.PlayerColor);
                    Graphics.DrawTexture(UnitPos, unitColor);
                }
                else
                {
                    unitColor = TeamColors.ColorToTexture(TeamColors.EnemyColor);
                    if (!grid.mGrid.returnClosestTile(g.transform.position).shrouded)
                    {
                        Graphics.DrawTexture(UnitPos, unitColor);
                    }
                }
            }
        }

        if (_Camera.orthographic)
        {
            GUI.Box(getCameraScreen(), "");
        }
    }

    public Rect getCameraScreen()
    {
        Ray r1 = _Camera.ScreenPointToRay(new Vector3((Screen.width / 2) - (GUI_Core.temp.mainMenuWidth / 2), Screen.height / 2, 0));
        RaycastHit h1;

        Physics.Raycast(r1, out h1);
        Vector3 center = MiniMapCamera.WorldToScreenPoint(h1.point);

        float left = center.x - (WorldWidth / 2);
        float right = center.x + (WorldWidth / 2);
        float top = center.y + (WorldHeight / 2);
        float bottom = center.y - (WorldHeight / 2);

        Rect r = new Rect(left, Screen.height - top, (right - left), (top - bottom));
        return r;
    }

    public float[] getMapBounds()
    {
        float[] mapBounds = new float[4];
        mapBounds[0] = mapScreenLeft;
        mapBounds[1] = mapScreenRight;
        mapBounds[2] = mapScreenTop;
        mapBounds[3] = mapScreenBottom;
        return mapBounds;
    }

    public Rect getMapRect()
    {
        return new Rect(mapScreenLeft, Screen.height - mapScreenTop, mapScreenRight - mapScreenLeft, mapScreenTop - mapScreenBottom);
    }


    void OnPostRender()
    {
        if (!_Camera.orthographic)
        {
            GL.PushMatrix();
            viewportMaterial.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);

            GL.Vertex(new Vector3(miniMapCoords[0].x, miniMapCoords[0].y, 0));
            GL.Vertex(new Vector3(miniMapCoords[1].x, miniMapCoords[1].y, 0));

            GL.Vertex(new Vector3(miniMapCoords[1].x, miniMapCoords[1].y, 0));
            GL.Vertex(new Vector3(miniMapCoords[2].x, miniMapCoords[2].y, 0));

            GL.Vertex(new Vector3(miniMapCoords[2].x, miniMapCoords[2].y, 0));
            GL.Vertex(new Vector3(miniMapCoords[3].x, miniMapCoords[3].y, 0));

            GL.Vertex(new Vector3(miniMapCoords[3].x, miniMapCoords[3].y, 0));
            GL.Vertex(new Vector3(miniMapCoords[0].x, miniMapCoords[0].y, 0));

            GL.End();
            GL.PopMatrix();
        }
    }

    public void UpdateCameraSize()
    {
        if (_Camera.orthographic)
        {
            Ray ray1 = _Camera.ScreenPointToRay(new Vector3(0, 0, 0));
            Ray ray2 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, Screen.height - 1, 0));
            Ray ray3 = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;

            Vector3 v1;
            Vector3 v2;

            Physics.Raycast(ray1, out hit);
            v1 = hit.point;

            Physics.Raycast(ray2, out hit);
            v2 = hit.point;

            Physics.Raycast(ray3, out hit);
            zOffset = hit.point.z - CameraTransform.position.z;
            Cam_CameraCore.temp.zOffset = zOffset;

            Vector3 temp1 = MiniMapCamera.WorldToScreenPoint(v1);
            Vector3 temp2 = MiniMapCamera.WorldToScreenPoint(v2);

            WorldWidth = temp2.x - temp1.x;
            WorldHeight = temp2.y - temp1.y;

            ScreenWidth = v2.x - v1.x;
        }
    }

    Texture2D MakeColor(float R, float G, float B, float A)
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, new Color(R, G, B, A));
        texture.Apply();
        return texture;
    }

    public void SetUpCamera()
    {
        MiniMapCamera = GetComponent<Camera>();

        MiniMapCamera.transform.position = new Vector3(Terrain.activeTerrain.terrainData.size.x / 2, 90, Terrain.activeTerrain.terrainData.size.z / 2);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float viewPortY = 3.0f / 4.0f;
        float viewPortHeight = 1.0f / 4.5f;

        float viewPortWidth = 1.0f / (4.5f * aspectRatio);
        float viewPortX = 1 - (0.25f / aspectRatio);

        MiniMapCamera.rect = new Rect(viewPortX, viewPortY, viewPortWidth, viewPortHeight);
    }
}
