using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Cam_Zoom))]
[RequireComponent(typeof(Cam_Position))]
public class Cam_CameraCore : MonoBehaviour
{
    Camera _Camera;
    [HideInInspector]
    public Cam_Zoom Zoom;
    [HideInInspector]
    public Cam_Position Position;

    public float HeightAboveGround = 10.0f;
    public float AngleOffset = 10.0f;

    Terrain terrain;
    Vector3 TerrainCenter;

    public bool Tracking = false;

    public static Cam_CameraCore temp;
    bool move = true;

    bool objectSelected = false;
    bool tUnitSelected = false;
    bool interactSelected = false;

    bool supportValid = false;

    public float zOffset = new float();

    public float[] mapBounds = new float[4];

    public float ScrollSpeed = 3.0f;
    public float CurrentScrollSpeed;
    public float zoomRate = 0.4f;

    public float xMin, xMax, yMin, yMax, cxMin, cxMax, cyMin, cyMax;

    public bool enableMouseClicks = true;

    public Vector3[] miniMapCoords;

    public GameObject startPoint;
    public float unitScreenSize = 10;

    public Material Material;
    public List<Vector3[]> glBuildings = new List<Vector3[]>();

    void Awake()
    {
        GetAndAddComponents();

        if (!startPoint)
        {
            startPoint = GameObject.FindWithTag("StartPoint");
        }
    }

    void GetAndAddComponents()
    {
        _Camera = GetComponent<Camera>();

        Position = GetComponent<Cam_Position>();
        if (!Position)
        {
            Position = gameObject.AddComponent<Cam_Position>();
        }

        Zoom = GetComponent<Cam_Zoom>();
        if (!Zoom)
        {
            Zoom = gameObject.AddComponent<Cam_Zoom>();
        }
    }

    void Start()
    {
        temp = this;
        InitializeTerrain();
        MoveToStart();

        Ray ray2 = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2))
        {
            Vector3 t1 = _Camera.WorldToScreenPoint(hit2.point + new Vector3(grid.mGrid.getGridSize() / 2, 0, 0));
            Vector3 t2 = _Camera.WorldToScreenPoint(hit2.point - new Vector3(grid.mGrid.getGridSize() / 2, 0, 0));
            unitScreenSize = t1.x - t2.x;
            GUI_Core.temp.setUnitSize(unitScreenSize);
        }
    }

    void InitializeTerrain()
    {
        terrain = GameObject.FindWithTag("Terrain").GetComponent<Terrain>();
        TerrainCenter = terrain.terrainData.size / 2;
        transform.position = new Vector3(terrain.terrainData.size.x / 2, HeightAboveGround, terrain.terrainData.size.z / 2);
        transform.LookAt(new Vector3((terrain.terrainData.size.x / 2), 0, (terrain.terrainData.size.z / 2) + AngleOffset));
    }

    void SetCameraPositionAndRotation()
    {
        Vector3 cameraPosition = new Vector3(terrain.terrainData.size.x / 2, HeightAboveGround, terrain.terrainData.size.z / 2);
        Vector3 lookAtPosition = new Vector3(terrain.terrainData.size.x / 2, 0, (terrain.terrainData.size.z / 2) + AngleOffset);

        transform.position = cameraPosition;
        transform.LookAt(lookAtPosition);
    }

    void SetUnitScreenSize()
    {
        Ray ray = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 t1 = _Camera.WorldToScreenPoint(hit.point + new Vector3(grid.mGrid.getGridSize() / 2, 0, 0));
            Vector3 t2 = _Camera.WorldToScreenPoint(hit.point - new Vector3(grid.mGrid.getGridSize() / 2, 0, 0));
            unitScreenSize = t1.x - t2.x;
            GUI_Core.temp.setUnitSize(unitScreenSize);
        }
    }

    void MoveToStart()
    {
        if (startPoint != null)
        {
            transform.position = new Vector3(startPoint.transform.position.x, HeightAboveGround, startPoint.transform.position.z - AngleOffset);
        }
        else
        {

        }
    }

    void Update()
    {
        Tracking = false;

        if (_Camera.orthographic)
        {
            Rect cameraRect = MiniMap_Controller.temp.getCameraScreen();
            Vector3 translation = Vector3.zero;

            HandleHorizontalMovement(cameraRect, ref translation);
            HandleVerticalMovement(cameraRect, ref translation);

            if (translation != Vector3.zero)
            {
                Tracking = true;
                transform.Translate(translation, Space.World);
            }
        }
        else
        {
            Ray mRay1 = _Camera.ScreenPointToRay(new Vector3(0, Screen.height, 0));
            Ray mRay2 = _Camera.ScreenPointToRay(new Vector3(Screen.width - GUI_Core.temp.mainMenuWidth, Screen.height, 0));
            Ray mRay3 = _Camera.ScreenPointToRay(new Vector3(Screen.width / 2, 0, 0));
            RaycastHit mHit;

            if (Physics.Raycast(mRay1, out mHit) || Physics.Raycast(mRay2, out mHit) || Physics.Raycast(mRay3, out mHit))
            {
                float hitX = mHit.point.x;
                float hitZ = mHit.point.z;

                if (Input.mousePosition.x == 0 && hitX > xMin && move)
                {
                    transform.Translate(new Vector3(-CurrentScrollSpeed * Time.deltaTime, 0, 0), Space.World);
                    Tracking = true;
                }
                else if (Input.mousePosition.x == Screen.width - 1 && hitX < xMax && move)
                {
                    transform.Translate(new Vector3(CurrentScrollSpeed * Time.deltaTime, 0, 0), Space.World);
                    Tracking = true;
                }

                if (Input.mousePosition.y == 0 && hitZ > yMin && move)
                {
                    transform.Translate(new Vector3(0, 0, -CurrentScrollSpeed * Time.deltaTime), Space.World);
                    Tracking = true;
                }
                else if (Input.mousePosition.y == Screen.height - 1 && hitZ < yMax && move)
                {
                    transform.Translate(new Vector3(0, 0, CurrentScrollSpeed * Time.deltaTime), Space.World);
                    Tracking = true;
                }
            }
        }

        Position.CheckCameraPos();

        Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int supSel = GUI_Core.temp.getSupportSelected();
        if (supSel != 0 && Physics.Raycast(ray, out hit))
        {
            if (supSel == 1)
            {
                if (hit.collider.tag == "fBuilding")
                {
                    supportValid = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.gameObject.GetComponent<Building_Controller>().Sell();
                    }
                }
                else
                {
                    supportValid = false;
                }
            }
            else if (supSel == 2)
            {
                if (hit.collider.tag == "fBuilding" && hit.collider.gameObject.GetComponent<Building_Controller>().getHealthRatio() < 1)
                {
                    supportValid = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.gameObject.GetComponent<Building_Controller>().Fix();
                    }
                }
                else
                {
                    supportValid = false;
                }
            }
        }
        else if (Input.mousePosition.x < Screen.width - GUI_Core.temp.mainMenuWidth && Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 17)) && enableMouseClicks)
        {
            if (hit.collider.tag == "fBuilding")
            {
                RaycastHit hit2;
                if (Physics.Raycast(ray, out hit2, Mathf.Infinity, 1 << 8))
                {
                    hit = hit2;
                }
            }

            SelectUnits(hit);
        }

        if (tUnitSelected)
        {
            if (Selection_Controller.temp.Objects.Count == 0) tUnitSelected = false;
        }

        interactSelected = false;
        foreach (GameObject g in Selection_Controller.temp.Objects)
        {
            if (g.GetComponent<Unit_Core>().Stats.isEngineer)
            {
                interactSelected = true;
            }
        }
        hit = SetCursorSprites(ray, supSel);
    }

    void SelectUnits(RaycastHit hit)
    {
        bool isMouseButtonDown, isRightMouseButtonDown, isShiftClick;
        GetInput(out isMouseButtonDown, out isRightMouseButtonDown, out isShiftClick);

        if (hit.collider.tag == "Player")
        {
            if (isShiftClick)
            {
                if (!hit.collider.gameObject.GetComponent<Selected>().isSelected)
                {
                    if (Selection_Controller.temp.Objects.Count > 0 && Selection_Controller.temp.Objects[0].tag == "npc")
                    {
                        Selection_Controller.temp.Deselect();
                    }

                    Selection_Controller.temp.addObject(hit.collider.gameObject);
                    objectSelected = true;
                    tUnitSelected = true;
                }
                else
                {
                    UnitDeselected(hit.collider.gameObject);
                }
            }
            else if (isMouseButtonDown)
            {
                if (hit.collider.gameObject.GetComponent<Selected>().isSelected && hit.collider.gameObject.GetComponent<Unit_Core>().Stats.Deployable)
                {

                    if (hit.collider.gameObject.GetComponent<Unit_Core>().deployUnit())
                    {

                    }
                }
                else
                {
                    Selection_Controller.temp.deselectAll();
                    Selection_Controller.temp.addObject(hit.collider.gameObject);
                    objectSelected = true;
                    tUnitSelected = true;
                }
            }
        }
        else if (hit.collider.tag == "npc")
        {
            if (isMouseButtonDown)
            {
                Selection_Controller.temp.deselectAll();
                Selection_Controller.temp.Objects.Add(hit.collider.gameObject);
                hit.collider.gameObject.GetComponent<Selected>().isSelected = true;
                objectSelected = true;

            }
            if (isRightMouseButtonDown && hit.collider.gameObject.layer == 9)
            {
                foreach (GameObject g in Selection_Controller.temp.Objects)
                {
                    if (g.GetComponent<Unit_Core>().Stats.isAir)
                    {
                        g.GetComponent<Aircraft_Movement>().setTarget(hit.collider.gameObject, true);
                    }
                    else
                    {
                        g.GetComponent<Unit_Core>().attack(hit.collider.gameObject, false);
                    }

                    g.GetComponent<Selected>().resetJustBeenSelected();
                }
            }
        }
        else if (hit.collider.tag == "fBuilding")
        {
            if (isMouseButtonDown && hit.collider.gameObject.GetComponent<Building_Controller>().beingPlaced())
            {

            }
            else if (isShiftClick)
            {
                Selection_Controller.temp.addBuilding(hit.collider.gameObject);
            }
            else if (isMouseButtonDown)
            {
                Selection_Controller.temp.deselectAll();
                Selection_Controller.temp.addBuilding(hit.collider.gameObject);
            }

            if (isRightMouseButtonDown && objectSelected && hit.collider.gameObject.GetComponent<Building_Controller>() != null)
            {
                int tempID = hit.collider.gameObject.GetComponent<Building_Controller>().ID;
                if (tempID == 3)
                {
                    foreach (GameObject g in Selection_Controller.temp.Objects)
                    {
                        if (g.GetComponent<Unit_Core>().Stats.isOreTruck)
                        {
                            g.GetComponent<OreUnit>().goToRefinery(hit.collider.gameObject.GetComponent<Refinery_Controller>());

                            g.GetComponent<Selected>().resetJustBeenSelected();
                        }
                    }
                }
            }
        }
        else if (hit.collider.tag == "eBuilding")
        {
            if (isShiftClick)
            {
                Selection_Controller.temp.addBuilding(hit.collider.gameObject);
                tUnitSelected = false;
            }
            else if (isMouseButtonDown)
            {
                Selection_Controller.temp.deselectAll();
                Selection_Controller.temp.addBuilding(hit.collider.gameObject);
                tUnitSelected = false;
            }

            if (isRightMouseButtonDown)
            {
                foreach (GameObject g in Selection_Controller.temp.Objects)
                {
                    if (g.tag == "Player" && g.GetComponent<Unit_Core>().Stats.isAir)
                    {
                        g.GetComponent<Aircraft_Movement>().setTarget(hit.collider.gameObject, true);
                    }
                    else if (g.tag == "Player")
                    {
                        g.GetComponent<Unit_Core>().attack(hit.collider.gameObject, true);
                    }
                    g.GetComponent<Selected>().resetJustBeenSelected();
                }
            }
        }
        else
        {
            if (isShiftClick)
            {
                Selection_Controller.temp.deselectAll();
                objectSelected = false;
                tUnitSelected = false;
            }

            if (isRightMouseButtonDown && objectSelected && Selection_Controller.temp.Objects[0].tag == "Player")
            {
                bool moreThan8 = false;
                int[,] orderToMove;
                int orderCounter = 0;

                List<float[]> distToTarget = new List<float[]>();
                if (Selection_Controller.temp.Objects.Count > 8)
                {
                    moreThan8 = true;
                    orderToMove = new int[Selection_Controller.temp.Objects.Count, 2];

                    foreach (GameObject g in Selection_Controller.temp.Objects)
                    {
                        int difOrderCounter = -1;
                        float dist = Vector3.Distance(g.transform.position, hit.point);
                        do
                        {
                            difOrderCounter++;
                            if (distToTarget.Count == 0 || distToTarget.Count == difOrderCounter)
                            {
                                distToTarget.Add(new float[2] { orderCounter, dist });
                            }
                            else
                            {
                                if (dist < distToTarget[difOrderCounter][1])
                                {
                                    distToTarget.Insert(difOrderCounter, new float[2] { orderCounter, dist });
                                }
                            }
                        }
                        while (dist > distToTarget[difOrderCounter][1]);
                        orderCounter++;
                    }
                }

                int sendOrder = 0;
                foreach (GameObject g in Selection_Controller.temp.Objects)
                {
                    Unit_Core UnitCore = g.GetComponent<Unit_Core>();

                    if (UnitCore.Stats.isAir)
                    {
                        g.GetComponent<Aircraft_Movement>().setTarget(hit.point, false);
                    }
                    else if (UnitCore.Stats.isOreTruck)
                    {
                        g.GetComponent<OreUnit>().setTarget(hit.point);
                    }
                    else
                    {
                        if (moreThan8)
                        {
                            Selection_Controller.temp.Objects[(int)distToTarget[sendOrder][0]].GetComponent<Unit_Movement>().SetTarget(hit.point);
                            sendOrder++;
                        }
                        else
                        {
                            UnitCore.UnitMovement.SetTarget(hit.point);
                        }


                        UnitCore.stopAttack();
                        UnitCore.stopDeploy();
                    }
                    g.GetComponent<Selected>().resetJustBeenSelected();
                }
            }
        }
    }

    static void GetInput(out bool isMouseButtonDown, out bool isRightMouseButtonDown, out bool isShiftClick)
    {
        bool isLeftShiftPressed = Input.GetKey(KeyCode.LeftShift);
        isMouseButtonDown = Input.GetMouseButtonDown(0);
        bool isMouseButton = Input.GetMouseButton(0);
        bool isMouseButtonUp = Input.GetMouseButtonUp(0);
        isRightMouseButtonDown = Input.GetMouseButtonDown(1);
        isShiftClick = isMouseButtonDown && isLeftShiftPressed;
        bool isRegularClick = isMouseButtonDown && !isLeftShiftPressed;
    }

    RaycastHit SetCursorSprites(Ray ray, int supSel)
    {
        GUI_Cursor GUICursor = GUI_Core.temp.GetComponent<GUI_Cursor>();
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.mousePosition.x < Screen.width - GUI_Core.temp.mainMenuWidth)
        {
            if (supSel != 0)
            {
                if (supSel == 1)
                {
                    if (supportValid)
                    {
                        GUICursor.SetCursorState("sell");
                    }
                    else
                    {
                        GUICursor.SetCursorState("sellInvalid");
                    }
                }
                else if (supSel == 2)
                {
                    if (supportValid)
                    {
                        GUICursor.SetCursorState("fix");
                    }
                    else
                    {
                        GUICursor.SetCursorState("fixInvalid");
                    }
                }
            }
            else if (hit.collider.tag == "Player")
            {
                Selected _Selected = hit.collider.gameObject.GetComponent<Selected>();
                Unit_Core UnitCore = hit.collider.gameObject.GetComponent<Unit_Core>();

                if (UnitCore.Stats.Deployable && _Selected.isSelected && UnitCore.canUnitDeploy())
                {
                    GUICursor.SetCursorState("deploy");
                }
                else if (UnitCore.Stats.Deployable && _Selected.isSelected)
                {
                    GUICursor.SetCursorState("invalidDeploy");
                }
                else if (_Selected.isSelected)
                {
                    GUICursor.SetCursorState("go");
                }
                else
                {
                    GUICursor.SetCursorState("hover");
                }
            }
            else if (hit.collider.tag == "fBuilding")
            {
                GUICursor.SetCursorState("hover");
            }
            else if (hit.collider.tag == "npc" || hit.collider.tag == "eBuilding")
            {
                if (tUnitSelected)
                {
                    if (interactSelected && hit.collider.tag == "eBuilding")
                    {
                        GUICursor.SetCursorState("bInt");
                    }
                    else if (interactSelected)
                    {
                        GUICursor.SetCursorState("hover");
                    }
                    else
                    {
                        GUICursor.SetCursorState("attack");
                    }
                }
                else
                {
                    GUICursor.SetCursorState("hover");
                }
            }
            else
            {
                if (tUnitSelected)
                {
                    GUICursor.SetCursorState("go");
                }
                else
                {
                    GUICursor.SetCursorState("normal");
                }
            }
        }
        else
        {
            GUICursor.SetCursorState("normal");
        }

        return hit;
    }

    bool HitIsDeployable(Unit_Core unitCore, Selected selected)
    {
        return unitCore.Stats.Deployable && selected.isSelected;
    }

    bool IsBuildingOrNPC(string tag)
    {
        return tag == "npc" || tag == "eBuilding";
    }

    public void EdgeMovement(bool b)
    {
        move = b;
    }

    public void UnitSelected(GameObject g)
    {
        Selection_Controller.temp.Objects.Add(g);
        g.GetComponent<Selected>().isSelected = true;
        objectSelected = true;
        tUnitSelected = true;
    }

    public void UnitDeselected(GameObject g)
    {
        Selection_Controller.temp.Objects.Remove(g);
        g.GetComponent<Selected>().isSelected = false;

        if (Selection_Controller.temp.Objects.Count == 0)
        {
            objectSelected = false;
            tUnitSelected = false;
        }
    }

    public float GetScreenUnitSize()
    {
        return unitScreenSize;
    }

    public void SetMaxValues(float xMin, float xMax, float yMin, float yMax)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;
    }

    void OnPostRender()
    {
        foreach (Vector3[] v in glBuildings)
        {
            Vector3 worldFBL = new Vector3(v[0].x - (v[1].x / 2), v[0].y - (v[1].y / 2), v[0].z - (v[1].z / 2));
            Vector3 worldFTL = new Vector3(v[0].x - (v[1].x / 2), v[0].y + (v[1].y / 2), v[0].z - (v[1].z / 2));
            Vector3 worldFTR = new Vector3(v[0].x + (v[1].x / 2), v[0].y + (v[1].y / 2), v[0].z - (v[1].z / 2));
            Vector3 worldFBR = new Vector3(v[0].x + (v[1].x / 2), v[0].y - (v[1].y / 2), v[0].z - (v[1].z / 2));

            Vector3 forwardBL = _Camera.WorldToScreenPoint(worldFBL);
            Vector3 forwardTL = _Camera.WorldToScreenPoint(worldFTL);
            Vector3 forwardTR = _Camera.WorldToScreenPoint(worldFTR);
            Vector3 forwardBR = _Camera.WorldToScreenPoint(worldFBR);

            Vector3 worldBBL = new Vector3(v[0].x - (v[1].x / 2), v[0].y - (v[1].y / 2), v[0].z + (v[1].z / 2));
            Vector3 worldBTL = new Vector3(v[0].x - (v[1].x / 2), v[0].y + (v[1].y / 2), v[0].z + (v[1].z / 2));
            Vector3 worldBTR = new Vector3(v[0].x + (v[1].x / 2), v[0].y + (v[1].y / 2), v[0].z + (v[1].z / 2));
            Vector3 worldBBR = new Vector3(v[0].x + (v[1].x / 2), v[0].y - (v[1].y / 2), v[0].z + (v[1].z / 2));

            Vector3 backBL = _Camera.WorldToScreenPoint(worldBBL);
            Vector3 backTL = _Camera.WorldToScreenPoint(worldBTL);
            Vector3 backTR = _Camera.WorldToScreenPoint(worldBTR);
            Vector3 backBR = _Camera.WorldToScreenPoint(worldBBR);

            forwardBL.z = 0;
            forwardTL.z = 0;
            forwardTR.z = 0;
            forwardBR.z = 0;

            backBL.z = 0;
            backTL.z = 0;
            backTR.z = 0;
            backBR.z = 0;

            float buildingWidth = v[1].x;
            float healthRatio = v[2].x;
            float numBoxes = v[2].y;

            float boxWidth = 0.7f;

            Vector3 worldCenter = worldBTL + new Vector3(boxWidth / 2, -boxWidth / 2, -boxWidth / 2);

            GL.PushMatrix();

            GL.LoadPixelMatrix();
            Material.SetPass(0);

            GL.Begin(GL.QUADS);

            float leftX1 = worldBTL.x;
            float leftX2 = worldBTL.x + (buildingWidth * healthRatio);
            float leftY1 = worldBTL.y;
            float leftY2 = worldBTL.y - boxWidth;
            float leftZ1 = worldBTL.z;
            float leftZ2 = worldBTL.z - boxWidth;

            Vector3 leftV1 = _Camera.WorldToScreenPoint(new Vector3(leftX1, leftY1, leftZ1));
            Vector3 leftV2 = _Camera.WorldToScreenPoint(new Vector3(leftX1, leftY2, leftZ1));
            Vector3 leftV3 = _Camera.WorldToScreenPoint(new Vector3(leftX1, leftY2, leftZ2));
            Vector3 leftV4 = _Camera.WorldToScreenPoint(new Vector3(leftX1, leftY1, leftZ2));

            Vector3 rightV1 = _Camera.WorldToScreenPoint(new Vector3(leftX2, leftY1, leftZ1));
            Vector3 rightV2 = _Camera.WorldToScreenPoint(new Vector3(leftX2, leftY2, leftZ1));
            Vector3 rightV3 = _Camera.WorldToScreenPoint(new Vector3(leftX2, leftY2, leftZ2));
            Vector3 rightV4 = _Camera.WorldToScreenPoint(new Vector3(leftX2, leftY1, leftZ2));

            leftV1.z = 0;
            leftV2.z = 0;
            leftV3.z = 0;
            leftV4.z = 0;

            rightV1.z = 0;
            rightV2.z = 0;
            rightV3.z = 0;
            rightV4.z = 0;

            if (healthRatio > 0.5f)
            {
                GL.Color(Color.Lerp(new Color(0, 1, 0, 1), new Color(1, 0.84f, 0, 1), 1 - ((healthRatio - 0.5f) * 2)));
            }
            else
            {
                GL.Color(Color.Lerp(new Color(1, 0.84f, 0, 1), new Color(1, 0, 0, 1), 1 - (healthRatio * 2)));
            }

            GL.Vertex(leftV1);
            GL.Vertex(leftV2);
            GL.Vertex(leftV3);
            GL.Vertex(leftV4);

            GL.Color(Color.Lerp(new Color(0, 0.7f, 0, 1), new Color(0.7f, 0, 0, 1), 1 - healthRatio));

            GL.Vertex(rightV1);
            GL.Vertex(rightV2);
            GL.Vertex(rightV3);
            GL.Vertex(rightV4);

            if (healthRatio > 0.5f)
            {
                GL.Color(Color.Lerp(new Color(0, 1, 0, 1), new Color(1, 0.84f, 0, 1), 1 - ((healthRatio - 0.5f) * 2)));
            }
            else
            {
                GL.Color(Color.Lerp(new Color(1, 0.84f, 0, 1), new Color(1, 0, 0, 1), 1 - (healthRatio * 2)));
            }

            GL.Vertex(leftV1);
            GL.Vertex(leftV4);
            GL.Vertex(rightV4);
            GL.Vertex(rightV1);

            GL.Color(Color.Lerp(new Color(0, 0.7f, 0, 1), new Color(0.7f, 0, 0, 1), 1 - healthRatio));

            GL.Vertex(leftV4);
            GL.Vertex(leftV3);
            GL.Vertex(rightV3);
            GL.Vertex(rightV4);

            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.white);

            GL.Vertex(forwardBL); GL.Vertex(forwardTL);
            GL.Vertex(forwardTL); GL.Vertex(forwardTR);
            GL.Vertex(forwardTR); GL.Vertex(forwardBR);
            GL.Vertex(forwardBR); GL.Vertex(forwardBL);

            GL.Vertex(backBL); GL.Vertex(backTL);
            GL.Vertex(backTL); GL.Vertex(backTR);
            GL.Vertex(backTR); GL.Vertex(backBR);
            GL.Vertex(backBR); GL.Vertex(backBL);

            GL.Vertex(forwardBL); GL.Vertex(backBL);
            GL.Vertex(forwardTL); GL.Vertex(backTL);
            GL.Vertex(forwardTR); GL.Vertex(backTR);
            GL.Vertex(forwardBR); GL.Vertex(backBR);

            GL.End();

            GL.PopMatrix();
        }

        foreach (GameObject g in Selection_Controller.temp.Objects)
        {
            Selected _Selected = g.GetComponent<Selected>();
            Unit_Core UnitCore = g.GetComponent<Unit_Core>();

            if (_Selected.JustBeenSelected && UnitCore.isMoving())
            {
                Vector3 startPos;
                startPos = _Camera.WorldToScreenPoint(g.transform.position);
                startPos.z = 0;
                Vector3 endPos;
                GL.PushMatrix();
                GL.LoadPixelMatrix();
                Material.SetPass(0);
                GL.Begin(GL.LINES);

                if (UnitCore.Stats.isAir)
                {
                    Aircraft_Movement AircraftMovement = g.GetComponent<Aircraft_Movement>();

                    if (AircraftMovement.isAttacking())
                    {
                        GL.Color(Color.red);
                        endPos = _Camera.WorldToScreenPoint(AircraftMovement.getTarget());
                        endPos.z = 0;
                    }
                    else
                    {
                        GL.Color(Color.green);
                        endPos = _Camera.WorldToScreenPoint(AircraftMovement.getTarget());
                        endPos.z = 0;
                    }
                }
                else if (UnitCore.isAttacking())
                {
                    GL.Color(Color.red);
                    try
                    {
                        endPos = _Camera.WorldToScreenPoint(UnitCore.getUnitToAttack().transform.position);
                        endPos.z = 0;
                    }
                    catch
                    {
                        endPos = startPos;
                    }
                }
                else
                {
                    GL.Color(Color.green);
                    endPos = _Camera.WorldToScreenPoint(UnitCore.UnitMovement.getArrivalTile().center);
                    endPos.z = 0;
                }

                GL.Vertex(startPos); GL.Vertex(endPos);

                GL.End();

                GL.Begin(GL.QUADS);
                GL.Vertex3(endPos.x - 3, endPos.y - 3, 0);
                GL.Vertex3(endPos.x - 3, endPos.y + 3, 0);
                GL.Vertex3(endPos.x + 3, endPos.y + 3, 0);
                GL.Vertex3(endPos.x + 3, endPos.y - 3, 0);
                GL.End();
                GL.PopMatrix();
            }
        }
    }

    void HandleHorizontalMovement(Rect cameraRect, ref Vector3 translation)
    {
        if (Input.mousePosition.x == 0 && cameraRect.xMin > mapBounds[0] && move)
        {
            translation.x = -CurrentScrollSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x == Screen.width - 1 && cameraRect.xMax < mapBounds[1] && move)
        {
            translation.x = CurrentScrollSpeed * Time.deltaTime;
        }
    }

    void HandleVerticalMovement(Rect cameraRect, ref Vector3 translation)
    {
        if (Input.mousePosition.y == 0 && (Screen.height - cameraRect.yMax) > mapBounds[3] && move)
        {
            translation.z = -CurrentScrollSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y == Screen.height - 1 && (Screen.height - cameraRect.yMin) < mapBounds[2] && move)
        {
            translation.z = CurrentScrollSpeed * Time.deltaTime;
        }
    }

    public void GroupObjects(bool b)
    {
        objectSelected = b;
        tUnitSelected = b;
    }
}