using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(GUI_GetFromResources))]
[RequireComponent(typeof(GUI_Screen))]
[RequireComponent(typeof(GUI_CursorSprites))]
[RequireComponent(typeof(GUI_Cursor))]
[RequireComponent(typeof(GUI_Buttons))]
[RequireComponent(typeof(GUI_Colors))]
[RequireComponent(typeof(GUI_Style))]
[RequireComponent(typeof(GUI_BuildingPlacement))]
[RequireComponent(typeof(GUI_Font))]
[RequireComponent(typeof(GUI_Radar))]
[RequireComponent(typeof(CursorDrag))]
public class GUI_Core : MonoBehaviour
{
    [HideInInspector]
    public GUI_Screen GUIScreen;
    [HideInInspector]
    public GUI_Cursor GUICursor;
    [HideInInspector]
    public GUI_CursorSprites GUISprites;
    [HideInInspector]
    public GUI_Colors GUIColors;
    [HideInInspector]
    public GUI_Buttons GUIButtons;
    [HideInInspector]
    public GUI_Style GUIStyle;
    [HideInInspector]
    public GUI_BuildingPlacement GUI_BuildingPlacement;
    [HideInInspector]
    public GUI_Font GUIFont;
    [HideInInspector]
    public GUI_Radar GUIRadar;
    public CursorDrag Drag;

    public Color invalidBuildingColor;

    public static GUI_Core temp;

    public string VictoryText = "VICTORY";
    public string LostText = "YOU LOST";

    public float mainMenuWidth = 300;

    [HideInInspector]
    public bool disableOnScreenMouse = false;

    public float[] miniMapBounds = new float[4];

    int mainMenuType = 1;
    public int selectedConstructor = 1;
    [HideInInspector]
    public int selectedSupport = 0;

    List<int> currentBuildings = new List<int>();
    List<DB_Item> currentItems = new List<DB_Item>();

    public Texture2D ConstructorNormal;
    public Texture2D ConstructorHover;
    public Texture2D ConstructorSelected;

    public Texture2D SupportNormal;
    public Texture2D SupportHover;
    public Texture2D SupportSelected;

    bool SpannerSelected = false;
    bool sellSelected = false;

    bool objectBeenPlaced = false;
    bool objectValid = false;
    GameObject objectToPlace;
    DB_Item itemBeenPlaced;
    Vector3 objectBounds;
    Vector3 objectOffset;

    float powerBarScale = 0.2f;

    Renderer objectRenderer;
    Material[] objectMaterials;

    float guiSelectedSize;

    List<Vector3[]> selectedBuildings = new List<Vector3[]>();

    float cameraZOffset;

    public GameObject buildDust;

    bool endGameWon = false;
    bool endGameLost = false;
    float endGameCounter = 0;

    bool alwaysDisplayRadar = false;
    public bool radarClosed = true;

    public Rect miniMapRect;

    bool ShowCost = false;
    string DisplayCost;

    float mouseX, mouseY;

    int ScreenHeight;
    int ScreenWidth;

    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        GUIScreen = GetComponent<GUI_Screen>();
        if (!GUIScreen)
        {
            GUIScreen = gameObject.AddComponent<GUI_Screen>();
        }

        GUISprites = GetComponent<GUI_CursorSprites>();
        if (!GUISprites)
        {
            GUISprites = gameObject.AddComponent<GUI_CursorSprites>();
        }

        GUIColors = GetComponent<GUI_Colors>();
        if (!GUIColors)
        {
            GUIColors = gameObject.AddComponent<GUI_Colors>();
        }

        GUICursor = GetComponent<GUI_Cursor>();
        if (!GUICursor)
        {
            GUICursor = gameObject.AddComponent<GUI_Cursor>();
        }

        GUIButtons = GetComponent<GUI_Buttons>();
        if (!GUIButtons)
        {
            GUIButtons = gameObject.AddComponent<GUI_Buttons>();
        }

        GUIStyle = GetComponent<GUI_Style>();
        if (!GUIStyle)
        {
            GUIStyle = gameObject.AddComponent<GUI_Style>();
        }

        GUIFont = GetComponent<GUI_Font>();
        if (!GUIFont)
        {
            GUIFont = gameObject.AddComponent<GUI_Font>();
        }

        GUIRadar = GetComponent<GUI_Radar>();
        if (!GUIRadar)
        {
            GUIRadar = gameObject.AddComponent<GUI_Radar>();
        }
        Drag = GetComponent<CursorDrag>();
        if (!Drag)
        {
            Drag = gameObject.AddComponent<CursorDrag>();
        }
    }

    void Start()
    {
        GUIScreen.InitScreen();
        temp = this;

        guiSelectedSize = 30;

        alwaysDisplayRadar = RTS_Player.temp.AlwaysDisplayRadar;

        ScreenHeight = GUIScreen.ScreenHeight;
        ScreenWidth = GUIScreen.ScreenWidth;
    }

    void Update()
    {
        if (endGameWon || endGameLost)
        {
            endGameCounter += Time.deltaTime;
        }

        CheckRadar();
    }

    private void CheckRadar()
    {
        if (!alwaysDisplayRadar)
        {
            if (RTS_Player.temp.TotalPower - RTS_Player.temp.TotalConsumption >= 0 && RTS_Player.temp.hasRadar())
            {
                radarClosed = false;

            }
            else
            {
                radarClosed = true;
            }
        }
    }

    public void EndGameWon()
    {
        endGameWon = true;
    }

    public void EndGameLost()
    {
        endGameLost = true;
    }

    void OnGUI()
    {
        Camera _Camera = Camera.main;

        GetInputs();

        if (endGameWon || endGameLost)
        {
            if (endGameWon)
            {
                GUI.Label(new Rect(0, 0, ScreenWidth, ScreenHeight), VictoryText, GUIStyle.EndGameStyle);
            }
            else
            {
                GUI.Label(new Rect(0, 0, ScreenWidth, ScreenHeight), LostText, GUIStyle.EndGameStyle);
            }
        }

        if (!alwaysDisplayRadar)
        {
            if (radarClosed)
            {
                if (Event.current.type.Equals(EventType.Repaint))
                {
                    Graphics.DrawTexture(miniMapRect, GUIColors.BlackTexture);
                }
            }
            else
            {

            }
        }

        foreach (GameObject g in Selection_Controller.temp.Objects)
        {
            Vector3 pos = _Camera.WorldToScreenPoint(g.transform.position);
            Selected _Selected = g.GetComponent<Selected>();

            int gNum = _Selected.getGroupNumber();
            if (Event.current.type.Equals(EventType.Repaint) && _Camera.WorldToScreenPoint(g.transform.position).x < ScreenWidth - GUI_Core.temp.mainMenuWidth)
            {
                Graphics.DrawTexture(new Rect(pos.x - (guiSelectedSize / 2), ScreenHeight - (pos.y + (guiSelectedSize / 2)), guiSelectedSize, guiSelectedSize), _Selected.getOverlay());
                if (gNum != -1) GUI.Label(new Rect(pos.x - 20, ScreenHeight - pos.y + 5, 20, 20), gNum.ToString());
            }
        }

        if (Selection_Controller.temp.BuildingObjects.Count > 0)
        {
            selectedBuildings.Clear();
            foreach (GameObject g in Selection_Controller.temp.BuildingObjects)
            {
                Building_Controller Building = g.GetComponent<Building_Controller>();

                Vector3 cameraPos = _Camera.transform.position;
                cameraPos.y = g.transform.position.y;
                cameraPos.z += Cam_CameraCore.temp.zOffset;
                if (Vector3.Distance(g.transform.position, cameraPos) < 50)
                {
                    Bounds tempBounds = g.GetComponent<BoxCollider>().bounds;
                    Vector3 center = tempBounds.center;
                    Vector3 size = tempBounds.size;
                    Vector3 ratio = new Vector3(Building.getHealthRatio(), Building.numberOfBoxes, 0);
                    selectedBuildings.Add(new Vector3[3] { center, size, ratio });
                    Cam_CameraCore.temp.glBuildings = selectedBuildings;
                }
            }
        }
        else
        {
            Cam_CameraCore.temp.glBuildings.Clear();
        }

        if (Event.current.type.Equals(EventType.Repaint))
        {
            Graphics.DrawTexture(new Rect(ScreenWidth - mainMenuWidth, ScreenHeight / 4, mainMenuWidth, 3 * ScreenHeight / 4), GUIColors.BlackTexture);
            Graphics.DrawTexture(new Rect(ScreenWidth - mainMenuWidth, 0, mainMenuWidth, (ScreenHeight / 4) - (miniMapBounds[2] - miniMapBounds[3])), GUIColors.BlackTexture);
            Graphics.DrawTexture(new Rect(miniMapBounds[1], 0, ScreenWidth - miniMapBounds[1], (ScreenHeight / 4)), GUIColors.BlackTexture);
            Graphics.DrawTexture(new Rect(miniMapBounds[0] - (ScreenWidth - miniMapBounds[1]), 0, ScreenWidth - miniMapBounds[1], (ScreenHeight / 4)), GUIColors.BlackTexture);
        }

        GUI.Label(new Rect(ScreenWidth - mainMenuWidth, 0, mainMenuWidth, (ScreenHeight / 4) - (miniMapBounds[2] - miniMapBounds[3])), RTS_Player.temp.getMoney().ToString(), GUIStyle.MoneyLabel);

        GUIStyle.PowerBarStyle.normal.background = GUIColors.GreenTexture;
        float tempPower = RTS_Player.temp.TotalPower;
        float tempConsumption = RTS_Player.temp.TotalConsumption;

        if (tempPower > 0)
        {
            GUI.Box(new Rect(miniMapBounds[0] - (ScreenWidth - miniMapBounds[1]) + 2, (ScreenHeight / 4) - (tempPower * powerBarScale), ScreenWidth - miniMapBounds[1] - 4, (tempPower * powerBarScale)), "", GUIStyle.PowerBarStyle);
        }

        if (tempConsumption > 0)
        {
            GUIStyle.PowerBarStyle.normal.background = GUIColors.RedTexture;
            GUI.Box(new Rect(miniMapBounds[0] - (ScreenWidth - miniMapBounds[1]) + 2, (ScreenHeight / 4) - (tempConsumption * powerBarScale), ScreenWidth - miniMapBounds[1] - 4, (tempConsumption * powerBarScale)), "", GUIStyle.PowerBarStyle);
        }
        //
        float b1x = miniMapBounds[0];
        float bWidth = (mainMenuWidth - (2 * (ScreenWidth - miniMapBounds[1]))) / 5.0f;
        float b1y = ScreenHeight / 4.0f;

        if (mainMenuType == 1)
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
        }
        else
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
        }

        if (GUI.Button(new Rect(b1x, b1y, bWidth, bWidth), "B", GUIStyle.TopButtonStyle))
        {
            mainMenuType = 1;
            currentBuildings = RTS_Player.temp.getBuildings(1);
            currentBuildings.Sort();
            selectedConstructor = 1;
            currentItems = RTS_Player.temp.getItems(1, selectedConstructor);
        }

        if (mainMenuType == 2)
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
        }
        else
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
        }

        if (GUI.Button(new Rect(b1x + bWidth, b1y, bWidth, bWidth), "S", GUIStyle.TopButtonStyle))
        {
            mainMenuType = 2;
            currentBuildings = RTS_Player.temp.getBuildings(2);
            currentBuildings.Sort();
            selectedConstructor = 1;
            currentItems = RTS_Player.temp.getItems(2, selectedConstructor);
        }

        if (mainMenuType == 3)
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
        }
        else
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
        }

        if (GUI.Button(new Rect(b1x + (2 * bWidth), b1y, bWidth, bWidth), "I", GUIStyle.TopButtonStyle))
        {
            mainMenuType = 3;
            currentBuildings = RTS_Player.temp.getBuildings(3);
            currentBuildings.Sort();
            selectedConstructor = 1;
        }

        if (mainMenuType == 4)
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
        }
        else
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
        }

        if (GUI.Button(new Rect(b1x + (3 * bWidth), b1y, bWidth, bWidth), "V", GUIStyle.TopButtonStyle))
        {
            mainMenuType = 4;
            currentBuildings = RTS_Player.temp.getBuildings(4);
            selectedConstructor = 1;
        }

        if (mainMenuType == 5)
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
        }
        else
        {
            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
        }

        if (GUI.Button(new Rect(b1x + (4 * bWidth), b1y, bWidth, bWidth), "A", GUIStyle.TopButtonStyle))
        {
            mainMenuType = 5;
            currentBuildings = RTS_Player.temp.getBuildings(5);
            selectedConstructor = 1;
        }

        GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
        GUIStyle.TopButtonStyle.hover.background = ConstructorHover;

        float sYval = ScreenHeight - 40;
        float sX1 = miniMapBounds[0];
        float sWidth = (mainMenuWidth - (2 * (ScreenWidth - miniMapBounds[1]))) / 2.0f;

        if (selectedSupport == 1)
        {
            GUIStyle.SupportButtonStyle.normal.background = SupportSelected;
            GUIStyle.SupportButtonStyle.hover.background = SupportSelected;
        }
        else
        {
            GUIStyle.SupportButtonStyle.normal.background = SupportNormal;
            GUIStyle.SupportButtonStyle.hover.background = SupportHover;
        }
       
        if (GUI.Button(new Rect(sX1, sYval, 30, 30), "$", GUIStyle.SupportButtonStyle))
        {
            if (selectedSupport == 1)
            {
                selectedSupport = 0;
            }
            else
            {
                selectedSupport = 1;
            }
        }

        if (selectedSupport == 2)
        {
            GUIStyle.SupportButtonStyle.normal.background = SupportSelected;
            GUIStyle.SupportButtonStyle.hover.background = SupportSelected;
        }
        else
        {
            GUIStyle.SupportButtonStyle.normal.background = SupportNormal;
            GUIStyle.SupportButtonStyle.hover.background = SupportHover;
        }

        if (GUI.Button(new Rect(sX1 + sWidth, sYval, 30, 30), "*", GUIStyle.SupportButtonStyle))
        {
            if (selectedSupport == 2)
            {
                selectedSupport = 0;
            }
            else
            {
                selectedSupport = 2;
            }
        }

        if (selectedSupport != 0 && Input.GetMouseButtonDown(1))
        {
            selectedSupport = 0;
        }

        GUIStyle.SupportButtonStyle.normal.background = SupportNormal;
        GUIStyle.SupportButtonStyle.hover.background = SupportHover;
        //
        switch (mainMenuType)
        {
            case 1:
                if (RTS_Player.temp.numberOfConstruction() > 0 && RTS_Player.temp.numberOfConstruction() <= 5)
                {
                    for (int i = 0; i < RTS_Player.temp.numberOfConstruction(); i++)
                    {
                        if (selectedConstructor > RTS_Player.temp.numberOfConstruction()) selectedConstructor = RTS_Player.temp.numberOfConstruction();
                        if (selectedConstructor == i + 1)
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
                        }
                        else
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
                        }

                        if (GUI.Button(new Rect(b1x + (i * bWidth), b1y + bWidth + 5, bWidth, 2 * bWidth / 3), (i + 1).ToString(), GUIStyle.TopButtonStyle))
                        {
                            selectedConstructor = i + 1;
                        }
                    }
                    currentItems = RTS_Player.temp.getItems(1, selectedConstructor);
                }
                else if (RTS_Player.temp.numberOfConstruction() == 0)
                {
                    currentItems = new List<DB_Item>();
                }
                else
                {

                }
                break;
            case 2:
                if (RTS_Player.temp.numberOfConstruction() > 0 && RTS_Player.temp.numberOfConstruction() <= 5)
                {
                    for (int i = 0; i < RTS_Player.temp.numberOfConstruction(); i++)
                    {
                        if (selectedConstructor == i + 1)
                        {
                           AssignButton();
                        }
                        else
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
                        }

                        if (GUI.Button(new Rect(b1x + (i * bWidth), b1y + bWidth + 5, bWidth, 2 * bWidth / 3), (i + 1).ToString(), GUIStyle.TopButtonStyle))
                        {
                            selectedConstructor = i + 1;
                        }
                    }
                    currentItems = RTS_Player.temp.getItems(2, selectedConstructor);
                }
                else
                {

                }
                break;

            case 3:
                if (RTS_Player.temp.numberOfInfantry() <= 5)
                {
                    for (int i = 0; i < RTS_Player.temp.numberOfInfantry(); i++)
                    {
                        if (selectedConstructor == i + 1)
                        {
                            AssignButton();
                        }
                        else
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
                        }
                        if (GUI.Button(new Rect(b1x + (i * bWidth), b1y + bWidth + 5, bWidth, 2 * bWidth / 3), (i + 1).ToString(), GUIStyle.TopButtonStyle))
                        {
                            selectedConstructor = i + 1;
                        }
                    }
                    currentItems = RTS_Player.temp.getItems(3, selectedConstructor);
                }
                else
                {

                }
                break;

            case 4:
                if (RTS_Player.temp.numberOfVehicles() <= 5)
                {
                    for (int i = 0; i < RTS_Player.temp.numberOfVehicles(); i++)
                    {
                        if (selectedConstructor == i + 1)
                        {
                            AssignButton();
                        }
                        else
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
                        }
                        if (GUI.Button(new Rect(b1x + (i * bWidth), b1y + bWidth + 5, bWidth, 2 * bWidth / 3), (i + 1).ToString(), GUIStyle.TopButtonStyle))
                        {
                            selectedConstructor = i + 1;
                        }
                    }
                    currentItems = RTS_Player.temp.getItems(4, selectedConstructor);
                }
                else
                {
                    currentItems = new List<DB_Item>();
                }
                break;

            case 5:
                if (RTS_Player.temp.numberOfAir() <= 5)
                {
                    for (int i = 0; i < RTS_Player.temp.numberOfAir(); i++)
                    {
                        if (selectedConstructor == i + 1)
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
                        }
                        else
                        {
                            GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
                            GUIStyle.TopButtonStyle.hover.background = ConstructorHover;
                        }
                        if (GUI.Button(new Rect(b1x + (i * bWidth), b1y + bWidth + 5, bWidth, 2 * bWidth / 3), (i + 1).ToString(), GUIStyle.TopButtonStyle))
                        {
                            selectedConstructor = i + 1;
                        }
                    }
                    currentItems = RTS_Player.temp.getItems(5, selectedConstructor);
                }
                else
                {
                    currentItems = new List<DB_Item>();
                }
                break;
        }

        GUIStyle.TopButtonStyle.normal.background = ConstructorNormal;
        GUIStyle.TopButtonStyle.hover.background = ConstructorHover;

        float buildingButtonWidth = (mainMenuWidth - (2 * (ScreenWidth - miniMapBounds[1]))) / 2.0f;
        float buildingButtonY = b1y + bWidth + 10 + (2 * bWidth / 3);
        int count = 0;

        bool itemBuilding = false;
        foreach (DB_Item i in currentItems)
        {
            if (i.isBuilding)
            {
                itemBuilding = true;
                break;
            }
        }

        ShowCost = false;

        switch (mainMenuType)
        {
            case 1:

                foreach (DB_Item b in currentItems)
                {
                    GUIStyle.ItemButtonStyle.normal.background = b.Sprite;
                    GUIStyle.ItemButtonStyle.hover.background = b.SpriteHover;

                    GUIStyle.ItemButtonStyle.alignment = TextAnchor.LowerCenter;
                    string details = b.Name;
                    if (b.isBuilding && !b.isFinished)
                    {
                    }
                    else if (b.isFinished)
                    {
                        details = MoveTextToCentre();
                    }

                    if (GUI.Button(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), details, GUIStyle.ItemButtonStyle) && !objectBeenPlaced)
                    {
                        if (Event.current.button == 0)
                        {
                            if (!itemBuilding)
                            {
                                b.startBuilding();
                            }
                            else if (b.isPaused())
                            {
                                b.carryOn();
                            }
                            else if (b.isFinished)
                            {
                                objectBeenPlaced = true;
                                objectToPlace = (GameObject)Instantiate(b.Unit);
                                objectToPlace.tag = "fBuilding";
                                itemBeenPlaced = b;
                                Cam_CameraCore.temp.enableMouseClicks = false;

                                objectRenderer = objectToPlace.GetComponent<Renderer>();
                                if (objectRenderer == null) objectRenderer = objectToPlace.GetComponentInChildren<Renderer>();

                                objectMaterials = objectRenderer.materials;
                                GUIColors.ObjectColors = new Color[objectMaterials.Length];

                                for (int i = 0; i < objectMaterials.Length; i++)
                                {
                                    if (objectMaterials[i].name.Contains("teamColor")) objectMaterials[i].color = RTS_Player.temp.Player_Material.color;
                                    GUIColors.ObjectColors[i] = objectMaterials[i].color;
                                }

                                objectBounds = objectRenderer.bounds.size;
                                float gSize = (float)grid.mGrid.width / (float)grid.mGrid.gridSize;

                                float xOffset;
                                float zOffset;

                                if (objectBounds.x > gSize * 3)
                                {
                                    xOffset = gSize / 2;
                                }
                                else if (objectBounds.x > gSize * 2)
                                {
                                    xOffset = 0;
                                }
                                else if (objectBounds.x > gSize)
                                {
                                    xOffset = gSize / 2;
                                }
                                else
                                {
                                    xOffset = 0;
                                }

                                if (objectBounds.z > gSize * 3)
                                {
                                    zOffset = -gSize / 2;
                                }
                                else if (objectBounds.z > gSize * 2)
                                {
                                    zOffset = 0;
                                }
                                else if (objectBounds.z > gSize)
                                {
                                    zOffset = -gSize / 2;
                                }
                                else
                                {
                                    zOffset = 0;
                                }

                                objectOffset = new Vector3(xOffset, 0, zOffset);
                            }
                            else
                            {

                            }
                        }
                        else if (Event.current.button == 1)
                        {
                            if (!itemBuilding)
                            {

                            }
                            else if (b.isFinished)
                            {
                                b.cancel(true);
                            }
                            else if (!b.isPaused())
                            {
                                b.pause();
                            }
                            else
                            {
                                b.cancel(true);
                            }
                        }
                    }

                    if (b.isBuilding && !b.isFinished)
                    {
                        GUI.DrawTexture(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), (Texture)GUI_Overlay.temp.Overlays[Mathf.Clamp((int)(b.getRatio() * 360), 0, 359)]);
                    }

                    if (new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth).Contains(new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0)))
                    {
                        ShowCost = true;
                        DisplayCost = b.Cost.ToString();
                    }

                    count++;

                    if (count != 0 && count % 2 == 0)
                    {
                        buildingButtonY += buildingButtonWidth;
                    }
                }
                break;

            case 2:
                foreach (DB_Item b in currentItems)
                {
                    GUIStyle.ItemButtonStyle.normal.background = b.Sprite;
                    GUIStyle.ItemButtonStyle.hover.background = b.SpriteHover;

                    GUIStyle.ItemButtonStyle.alignment = TextAnchor.LowerCenter;
                    string details = b.Name;
                    if (b.isBuilding && !b.isFinished)
                    {

                    }
                    else if (b.isFinished)
                    {
                        details = MoveTextToCentre();
                    }

                    if (GUI.Button(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), details, GUIStyle.ItemButtonStyle) && !objectBeenPlaced)
                    {
                        if (Event.current.button == 0)
                        {
                            if (!itemBuilding)
                            {
                                b.startBuilding();
                            }
                            else if (b.isFinished)
                            {
                                objectBeenPlaced = true;
                                objectToPlace = (GameObject)Instantiate(b.Unit);
                                objectToPlace.tag = "fBuilding";
                                itemBeenPlaced = b;
                                Cam_CameraCore.temp.enableMouseClicks = false;

                                objectRenderer = objectToPlace.GetComponent<Renderer>();
                                if (objectRenderer == null) objectRenderer = objectToPlace.GetComponentInChildren<Renderer>();

                                objectMaterials = objectRenderer.materials;
                                GUIColors.ObjectColors = new Color[objectMaterials.Length];

                                for (int i = 0; i < objectMaterials.Length; i++)
                                {
                                    GUIColors.ObjectColors[i] = objectMaterials[i].color;
                                }

                                objectBounds = objectRenderer.bounds.size;

                                float gSize = (float)grid.mGrid.width / (float)grid.mGrid.gridSize;

                                float xOffset;
                                float zOffset;

                                if (objectBounds.x > gSize * 3)
                                {
                                    xOffset = 3 * gSize / 2;
                                }
                                else if (objectBounds.x > gSize * 2)
                                {
                                    xOffset = gSize;
                                }
                                else if (objectBounds.x > gSize)
                                {
                                    xOffset = gSize / 2;
                                }
                                else
                                {
                                    xOffset = 0;
                                }

                                if (objectBounds.z > gSize * 3)
                                {
                                    zOffset = -3 * gSize / 2;
                                }
                                else if (objectBounds.z > gSize * 2)
                                {
                                    zOffset = -gSize;
                                }
                                else if (objectBounds.z > gSize)
                                {
                                    zOffset = -gSize / 2;
                                }
                                else
                                {
                                    zOffset = 0;
                                }

                                objectOffset = new Vector3(xOffset, 0, zOffset);
                            }
                            else
                            {
                            }
                        }
                        else if (Event.current.button == 1)
                        {
                            if (!itemBuilding)
                            {

                            }
                            else if (b.isFinished)
                            {
                                b.cancel(true);
                            }
                            else if (!b.isPaused())
                            {
                                b.pause();
                            }
                            else
                            {
                                b.cancel(true);
                            }
                        }
                    }

                    if (b.isBuilding && !b.isFinished)
                    {

                        GUI.DrawTexture(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), (Texture)GUI_Overlay.temp.Overlays[Mathf.Clamp((int)(b.getRatio() * 360), 0, 359)]);
                    }

                    if (new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth).Contains(new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0)))
                    {
                        ShowCost = true;
                        DisplayCost = b.Cost.ToString();

                    }

                    count++;

                    if (count != 0 && count % 2 == 0)
                    {
                        buildingButtonY += buildingButtonWidth;
                    }
                }
                break;

            case 3:
                foreach (DB_Item b in currentItems)
                {
                    GUIStyle.ItemButtonStyle.normal.background = b.Sprite;
                    GUIStyle.ItemButtonStyle.hover.background = b.SpriteHover;

                    GUIStyle.ItemButtonStyle.alignment = TextAnchor.LowerCenter;
                    string details = b.Name;
                    if (b.isBuilding && !b.isFinished)
                    {
                    }

                    if (GUI.Button(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), details, GUIStyle.ItemButtonStyle) && !objectBeenPlaced)
                    {
                        if (Event.current.button == 0)
                        {
                            if (!itemBuilding)
                            {
                                b.startBuilding();
                            }
                            else
                            {

                            }
                        }
                        else if (Event.current.button == 1)
                        {
                            if (!itemBuilding)
                            {

                            }
                            else if (b.isFinished)
                            {
                                b.cancel(true);
                            }
                            else if (!b.isPaused())
                            {
                                b.pause();
                            }
                            else
                            {
                                b.cancel(true);
                            }
                        }
                    }

                    if (b.isBuilding && !b.isFinished)
                    {

                        GUI.DrawTexture(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), (Texture)GUI_Overlay.temp.Overlays[Mathf.Clamp((int)(b.getRatio() * 360), 0, 359)]);
                    }

                    if (new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth).Contains(new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0)))
                    {
                        ShowCost = true;
                        DisplayCost = b.Cost.ToString();

                    }

                    count++;

                    if (count != 0 && count % 2 == 0)
                    {
                        buildingButtonY += buildingButtonWidth;
                    }
                }
                break;

            case 4:
                foreach (DB_Item b in currentItems)
                {
                    GUIStyle.ItemButtonStyle.normal.background = b.Sprite;
                    GUIStyle.ItemButtonStyle.hover.background = b.SpriteHover;

                    GUIStyle.ItemButtonStyle.alignment = TextAnchor.LowerCenter;
                    string details = b.Name;
                    if (b.isBuilding && !b.isFinished)
                    {
                    }

                    if (GUI.Button(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), details, GUIStyle.ItemButtonStyle) && !objectBeenPlaced)
                    {
                        if (Event.current.button == 0)
                        {
                            if (!itemBuilding)
                            {
                                b.startBuilding();
                            }
                            else
                            {
                            }
                        }
                        else if (Event.current.button == 1)
                        {
                            if (!itemBuilding)
                            {

                            }
                            else if (b.isFinished)
                            {
                                b.cancel(true);
                            }
                            else if (!b.isPaused())
                            {
                                b.pause();
                            }
                            else
                            {
                                b.cancel(true);
                            }
                        }
                    }

                    if (b.isBuilding && !b.isFinished)
                    {
                        GUI.DrawTexture(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), (Texture)GUI_Overlay.temp.Overlays[Mathf.Clamp((int)(b.getRatio() * 360), 0, 359)]);
                    }

                    if (new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth).Contains(new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0)))
                    {
                        ShowCost = true;
                        DisplayCost = b.Cost.ToString();

                    }

                    count++;

                    if (count != 0 && count % 2 == 0)
                    {
                        buildingButtonY += buildingButtonWidth;
                    }
                }
                break;

            case 5:
                foreach (DB_Item b in currentItems)
                {
                    bool cB = true;
                    if (RTS_Player.temp.Helipads[selectedConstructor - 1].CanBuild())
                    {
                        GUIStyle.ItemButtonStyle.normal.background = b.Sprite;
                        GUIStyle.ItemButtonStyle.hover.background = b.SpriteHover;
                    }
                    else
                    {
                        GUIStyle.ItemButtonStyle.normal.background = b.Sprite;
                        GUIStyle.ItemButtonStyle.hover.background = b.SpriteHover;

                        cB = false;
                    }

                    GUIStyle.ItemButtonStyle.alignment = TextAnchor.LowerCenter;
                    string details = b.Name;

                    if (GUI.Button(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), details, GUIStyle.ItemButtonStyle) && !objectBeenPlaced && cB)
                    {
                        if (Event.current.button == 0)
                        {
                            if (!itemBuilding)
                            {
                                b.startBuilding();
                            }
                            else
                            {
                            }
                        }
                        else if (Event.current.button == 1)
                        {
                            if (!itemBuilding)
                            {

                            }
                            else if (b.isFinished)
                            {
                                b.cancel(true);
                            }
                            else if (!b.isPaused())
                            {
                                b.pause();
                            }
                            else
                            {
                                b.cancel(true);
                            }
                        }
                    }

                    if (b.isBuilding && !b.isFinished)
                    {
                        GUI.DrawTexture(new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth), (Texture)GUI_Overlay.temp.Overlays[Mathf.Clamp((int)(b.getRatio() * 360), 0, 359)]);
                    }

                    if (new Rect(b1x + ((count % 2) * buildingButtonWidth), buildingButtonY, buildingButtonWidth, buildingButtonWidth).Contains(new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0)))
                    {
                        ShowCost = true;
                        DisplayCost = b.Cost.ToString();

                    }

                    count++;

                    if (count != 0 && count % 2 == 0)
                    {
                        buildingButtonY += buildingButtonWidth;
                    }
                }
                break;
        }

        if (ShowCost)
        {
            Vector2 costSize = GUIStyle.CostHoverStyle.CalcSize(new GUIContent(DisplayCost));
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y - costSize.y, costSize.x + 20, costSize.y), "$" + DisplayCost, GUIStyle.CostHoverStyle);
        }

        if (objectBeenPlaced)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Cam_CameraCore.temp.enableMouseClicks = true;
                objectBeenPlaced = false;
                Destroy(objectToPlace);
                objectToPlace = null;
                itemBeenPlaced = null;
                for (int i = 0; i < objectMaterials.Length; i++)
                {
                    Destroy(objectMaterials[i]);
                }
                return;
            }

            Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 11))
            {
                tile t = grid.mGrid.returnClosestTile(hit.point);
                if (t != null)
                {
                    objectToPlace.transform.position = t.center + objectOffset;
                    objectToPlace.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }
            }

            bool distanceValid;
            if (Physics.CheckSphere(objectToPlace.transform.position, RTS_Player.temp.BuildingDistance, 1 << 12))
            {
                distanceValid = true;
            }
            else
            {
                distanceValid = false;
            }

            float objectWidth = objectRenderer.bounds.size.x;
            float objectLength = objectRenderer.bounds.size.z;

            float objectX1 = objectToPlace.transform.position.x - (objectWidth / 2);
            float objectX2 = objectToPlace.transform.position.x + (objectWidth / 2);
            float objectZ1 = objectToPlace.transform.position.z - (objectLength / 2);
            float objectZ2 = objectToPlace.transform.position.z + (objectLength / 2);

            Vector3 bottomLeft = new Vector3(objectX1, 0, objectZ1);
            Vector3 bottomRight = new Vector3(objectX2, 0, objectZ1);
            Vector3 topRight = new Vector3(objectX2, 0, objectZ2);
            Vector3 topLeft = new Vector3(objectX1, 0, objectZ2);

            float bottomLeftHeight = Terrain.activeTerrain.SampleHeight(bottomLeft);
            float bottomRightHeight = Terrain.activeTerrain.SampleHeight(bottomRight);
            float topRightHeight = Terrain.activeTerrain.SampleHeight(topRight);
            float topLeftHeight = Terrain.activeTerrain.SampleHeight(topLeft);

            bool heightValid;

            float ahd = 2.5f;

            if (Mathf.Abs(bottomLeftHeight - bottomRightHeight) < ahd && Mathf.Abs(bottomLeftHeight - topRightHeight) < ahd && Mathf.Abs(bottomLeftHeight - topLeftHeight) < ahd && Mathf.Abs(bottomRightHeight - topRightHeight) < ahd && Mathf.Abs(bottomRightHeight - topLeftHeight) < ahd && Mathf.Abs(topRightHeight - topLeftHeight) < ahd)
            {
                heightValid = true;
            }
            else
            {
                heightValid = false;
            }

            bool tilesValid = true;

            if (grid.mGrid.returnClosestTile(topLeft) != null && grid.mGrid.returnClosestTile(topRight) != null && grid.mGrid.returnClosestTile(bottomLeft) != null && grid.mGrid.returnClosestTile(topLeft) != null)
            {
                int tileLeftI = grid.mGrid.returnClosestTile(topLeft).i;
                int tileRightI = grid.mGrid.returnClosestTile(topRight).i;
                int tileBottomJ = grid.mGrid.returnClosestTile(bottomLeft).j;
                int tileTopJ = grid.mGrid.returnClosestTile(topLeft).j;

                for (int i = tileLeftI; i <= tileRightI; i++)
                {
                    for (int j = tileBottomJ; j <= tileTopJ; j++)
                    {
                        if (grid.mGrid.mainGrid[i, j].status == 4 || grid.mGrid.mainGrid[i, j].hasOre)
                        {
                            tilesValid = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                tilesValid = false;
            }

            CheckValidity(distanceValid, heightValid, tilesValid);

            if (!objectValid)
            {
                for (int i = 0; i < objectMaterials.Length; i++)
                {
                    objectMaterials[i].color = invalidBuildingColor;
                }
            }
            else if (Input.GetMouseButtonDown(0) && objectValid && Input.mousePosition.x < ScreenWidth - mainMenuWidth)
            {
                PlaceBuilding(hit, bottomLeft, topRight);
            }
            else if (objectValid)
            {
                for (int i = 0; i < objectMaterials.Length; i++)
                {
                    objectMaterials[i].color = new Color(GUIColors.ObjectColors[i].r, GUIColors.ObjectColors[i].g, GUIColors.ObjectColors[i].b, invalidBuildingColor.a);
                }
            }
        }
    }

    private void AssignButton()
    {
        GUIStyle.TopButtonStyle.normal.background = ConstructorSelected;
        GUIStyle.TopButtonStyle.hover.background = ConstructorSelected;
    }

    private void GetInputs()
    {
        bool MouseButtonDown = Input.GetMouseButtonDown(0);
        Vector2 MousePosition = Input.mousePosition;
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
    }

    private void CheckValidity(bool distanceValid, bool heightValid, bool tilesValid)
    {
        if (tilesValid && heightValid && distanceValid)
        {
            objectValid = true;
        }
        else
        {
            objectValid = false;
        }
    }

    void PlaceBuilding(RaycastHit hit, Vector3 bottomLeft, Vector3 topRight)
    {
        for (int i = 0; i < objectMaterials.Length; i++)
        {
            objectMaterials[i].color = GUIColors.ObjectColors[i];
        }

        objectToPlace.layer = 12;
        Building_Controller BuildingController = objectToPlace.AddComponent<Building_Controller>();
        BuildingController.ID = itemBeenPlaced.ID;
        BuildingController.powerConsumption = itemBeenPlaced.PowerConsumption;
        BuildingController.health = itemBeenPlaced.Health;
        BuildingController.explosion = itemBeenPlaced.Explosion;
        BuildingController.materials = objectMaterials;
        BuildingController.placed();
        BuildingController.targetY = objectToPlace.transform.position.y;
        BuildingController.buildDust = buildDust;
        BuildingController.cost = (int)itemBeenPlaced.Cost;

        DB_UnitPaths ResourchPath = GetComponent<DB_UnitPaths>();

        if (itemBeenPlaced.Name == "Refinery")
        {
            objectToPlace.AddComponent<Refinery_Controller>();
        }
        else if (itemBeenPlaced.Name == "Power Plant")
        {
            objectToPlace.AddComponent<PowerPlantController>();
        }
        else if (itemBeenPlaced.Name == "Helipad")
        {
            objectToPlace.AddComponent<Helipad_Controller>();
        }
        else if (itemBeenPlaced.Name == "Light Defense")
        {
            objectToPlace.GetComponent<Defense_Controller>().enabled = true;
        }
        else if (itemBeenPlaced.Name == "Radar")
        {
            objectToPlace.AddComponent<Radar_Controller>();
            objectToPlace.GetComponent<Radar_Controller>().Dish = objectToPlace.transform.GetChild(0).gameObject;
        }

        objectBeenPlaced = false;
        objectToPlace = null;
        itemBeenPlaced.finishBuild();
        itemBeenPlaced = null;

        Vector3 bLterrainCoord = Vector3.zero;
        bLterrainCoord.x = bottomLeft.x / Terrain.activeTerrain.terrainData.size.x;
        bLterrainCoord.z = bottomLeft.z / Terrain.activeTerrain.terrainData.size.z;

        int bLx = (int)(bLterrainCoord.x * Terrain.activeTerrain.terrainData.heightmapWidth);
        int bLy = (int)(bLterrainCoord.z * Terrain.activeTerrain.terrainData.heightmapHeight);

        Vector3 tRterrainCoord = Vector3.zero;
        tRterrainCoord.x = topRight.x / Terrain.activeTerrain.terrainData.size.x;
        tRterrainCoord.z = topRight.z / Terrain.activeTerrain.terrainData.size.z;

        int tRx = (int)(tRterrainCoord.x * Terrain.activeTerrain.terrainData.heightmapWidth);
        int tRy = (int)(tRterrainCoord.z * Terrain.activeTerrain.terrainData.heightmapHeight);

        float[,] heightMap = Terrain.activeTerrain.terrainData.GetHeights(bLx, bLy, tRx - bLx + 1, tRy - bLy + 1);

        for (int x = 0; x < (tRx - bLx + 1); x++)
        {
            for (int y = 0; y < (tRy - bLy + 1); y++)
            {
                heightMap[y, x] = hit.point.y / Terrain.activeTerrain.terrainData.size.y;
            }
        }

        Terrain.activeTerrain.terrainData.SetHeights(bLx, bLy, heightMap);

        Cam_CameraCore.temp.enableMouseClicks = true;
    }

    string MoveTextToCentre()
    {
        string details;
        GUIStyle.ItemButtonStyle.alignment = TextAnchor.MiddleCenter;
        details = "Ready";
        return details;
    }

    public void setUnitSize(float s)
    {
        guiSelectedSize = s;
    }

    public float getUnitSize()
    {
        return guiSelectedSize;
    }

    public bool objBeenPlaced()
    {
        return objectBeenPlaced;
    }

    public int getSupportSelected()
    {
        return selectedSupport;
    }
}