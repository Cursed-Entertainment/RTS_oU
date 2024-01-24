using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Building_Controller : MonoBehaviour
{
    public int ID = 1;
    public int buildingID = 0;
    public int powerConsumption = 0;
    public int health = 100;
    int maxHealth;
    public int armour = 10;
    public int numberOfBoxes = 10;
    bool justBeenPlaced = false;
    bool lowered = false;
    public float targetY;
    float riseDistance = 5;
    float riseSpeed = 2;
    public int cost = 0;

    int tLi, tRi, tBj, tTj;

    public GameObject explosion;
    public Material[] materials;

    public GameObject buildDust;
    GameObject[] Dust = new GameObject[4];

    bool _Sell = false;
    bool _Fix = false;

    float FixCostSoFar = 0;
    float FixRateSoFar = 0;

    public bool AlreadyCreated = false;
    public bool CanBeCaptured = true;

    void Start()
    {
        maxHealth = health;
        CreateMaterials();

        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null) objectRenderer = GetComponentInChildren<Renderer>();

        float objectWidth = objectRenderer.bounds.size.x;
        float objectLength = objectRenderer.bounds.size.z;

        float objectX1 = transform.position.x - (objectWidth / 2);
        float objectX2 = transform.position.x + (objectWidth / 2);
        float objectZ1 = transform.position.z - (objectLength / 2);
        float objectZ2 = transform.position.z + (objectLength / 2);

        Vector3 bottomLeft = new Vector3(objectX1, 0, objectZ1);
        Vector3 bottomRight = new Vector3(objectX2, 0, objectZ1);
        Vector3 topRight = new Vector3(objectX2, 0, objectZ2);
        Vector3 topLeft = new Vector3(objectX1, 0, objectZ2);

        tLi = grid.mGrid.returnClosestTile(topLeft).i;
        tRi = grid.mGrid.returnClosestTile(topRight).i;
        tBj = grid.mGrid.returnClosestTile(bottomLeft).j;
        tTj = grid.mGrid.returnClosestTile(topLeft).j;

        List<GameObject> tList = new List<GameObject>();
        for (int i = tLi; i <= tRi; i++)
        {
            for (int j = tBj; j <= tTj; j++)
            {
                if (grid.mGrid.mainGrid[i, j].occupied)
                {
                    tList.Add(grid.mGrid.mainGrid[i, j].occupied);
                }
                grid.mGrid.mainGrid[i, j].status = 4;
            }
        }

        foreach (GameObject g in tList)
        {
            g.GetComponent<Unit_Movement>().SetTarget(grid.mGrid.findClosestAvailableTile(transform.position, g.GetComponent<Unit_Movement>().getNavMesh()).center);
        }      

        if (gameObject.layer == 12)
        {
            RTS_Player.temp.addBuilding(this);
        }

        RTS_Player.temp.Buildings.Add(gameObject);

        float bottomLeftHeight = Terrain.activeTerrain.SampleHeight(bottomLeft);
        float bottomRightHeight = Terrain.activeTerrain.SampleHeight(bottomRight);
        float topRightHeight = Terrain.activeTerrain.SampleHeight(topRight);
        float topLeftHeight = Terrain.activeTerrain.SampleHeight(topLeft);

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
                heightMap[y, x] = Terrain.activeTerrain.SampleHeight(transform.position) / Terrain.activeTerrain.terrainData.size.y;
            }
        }

        Terrain.activeTerrain.terrainData.SetHeights(bLx, bLy, heightMap);
    }

    void Update()
    {
        if (justBeenPlaced)
        {
            if (!lowered)
            {
                if (buildDust)
                {
                    Vector3 extents = GetComponent<BoxCollider>().bounds.extents;
                    Vector3 topLeft = transform.position + new Vector3(-extents.x, 0, extents.z) / 2;
                    Vector3 topRight = transform.position + new Vector3(extents.x, 0, extents.z) / 2;
                    Vector3 bottomRight = transform.position + new Vector3(extents.x, 0, -extents.z) / 2;
                    Vector3 bottomLeft = transform.position + new Vector3(-extents.x, 0, -extents.z) / 2;
                    Dust[0] = (GameObject)Instantiate(buildDust, topLeft, buildDust.transform.rotation);
                    Dust[1] = (GameObject)Instantiate(buildDust, topRight, buildDust.transform.rotation);
                    Dust[2] = (GameObject)Instantiate(buildDust, bottomRight, buildDust.transform.rotation);
                    Dust[3] = (GameObject)Instantiate(buildDust, bottomLeft, buildDust.transform.rotation);
                }

                transform.Translate(0, -riseDistance, 0);
                lowered = true;
            }

            transform.Translate(0, riseSpeed * Time.deltaTime, 0);

            if (transform.position.y >= targetY)
            {
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                justBeenPlaced = false;
                Dust[0].GetComponent<ParticleEmitter>().minEmission = 0;
                Dust[0].GetComponent<ParticleEmitter>().maxEmission = 0;
                Dust[1].GetComponent<ParticleEmitter>().minEmission = 0;
                Dust[1].GetComponent<ParticleEmitter>().maxEmission = 0;
                Dust[2].GetComponent<ParticleEmitter>().minEmission = 0;
                Dust[2].GetComponent<ParticleEmitter>().maxEmission = 0;
                Dust[3].GetComponent<ParticleEmitter>().minEmission = 0;
                Dust[3].GetComponent<ParticleEmitter>().maxEmission = 0;
            }
        }

        if (_Fix)
        {
            FixBuilding();
        }
    }

    void FixBuilding()
    {
        if (health < maxHealth)
        {
            FixRateSoFar += RTS_Player.temp.FixRate * Time.deltaTime;
            if ((int)FixRateSoFar >= 1)
            {
                health += (int)FixRateSoFar;
                FixRateSoFar = 0;
            }

            FixCostSoFar += RTS_Player.temp.FixCostRate * Time.deltaTime;
            if ((int)FixCostSoFar >= 1)
            {
                RTS_Player.temp.costMoney((int)FixCostSoFar);
                FixCostSoFar = 0;
            }
        }
        else
        {
            _Fix = false;
            health = maxHealth;
        }
    }

    void OnGUI()
    {
        if (_Fix && Event.current.type.Equals(EventType.Repaint))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Graphics.DrawTexture(new Rect(screenPos.x - 15, Screen.height - (screenPos.y + 15), 30, 30), GUI_Core.temp.GUISprites.FixCursor);
        }
    }

    void CreateMaterials()
    {
        if (materials.Length == 0) materials = GetComponent<Renderer>().materials;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name == "glass (Instance)")
            {
                materials[i].shader = Shader.Find("Specular");
                materials[i].SetFloat("_Shininess", 0.04f);
            }
            else
            {
                materials[i].shader = Shader.Find("Diffuse");
            }
        }
    }

    public void takeDamage(int d)
    {
        health -= d / armour;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health > 0)
        {
            return;
        }
        else if (health <= 0)
        {
            if (gameObject.layer == 12)
            {
                RTS_Player.temp.removeBuilding(this, false);
            }

            if (gameObject.layer == 13)
            {
                RTS_AI.temp.removeBuilding(gameObject);
            }

            DestroyUnit();
        }
    }

    public float getHealthRatio()
    {
        return (float)health / (float)maxHealth;
    }

    public void DestroyUnit()
    {
        RTS_Player.temp.TotalConsumption -= powerConsumption;
        RTS_Player.temp.Buildings.Remove(gameObject);

        if (Selection_Controller.temp.BuildingObjects.Contains(gameObject))
        {
            Selection_Controller.temp.BuildingObjects.Remove(gameObject);
        }

        for (int i = tLi; i <= tRi; i++)
        {
            for (int j = tBj; j <= tTj; j++)
            {
                grid.mGrid.mainGrid[i, j].status = 3;
            }
        }

        if (ID == 3)
        {
            RTS_Player.temp.Refineries.Remove(GetComponent<Refinery_Controller>());
        }

        if (explosion)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (materials != null)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                Destroy(materials[i]);
            }
        }
    }

    public void placed()
    {
        justBeenPlaced = true;
    }

    public bool beingPlaced()
    {
        return justBeenPlaced;
    }

    public void Sell()
    {
        if (!_Sell)
        {
            _Sell = true;
            RTS_Player.temp.AddMoney((int)(cost / 2));
            RTS_Player.temp.removeBuilding(this, true);
            DestroyUnit();
        }
    }

    public void Fix()
    {
        if (_Fix)
        {
            _Fix = false;
        }
        else
        {
            _Fix = true;
        }
    }

    public void takeOver(bool friendly)
    {
        if (gameObject.layer == 12)
        {
            if (friendly)
            {

            }
            else
            {

            }
        }
        else
        {
            if (friendly)
            {
                gameObject.layer = 12;
                gameObject.tag = "fBuilding";
                BroadcastMessage("swapTeams", SendMessageOptions.DontRequireReceiver);

                RTS_Player.temp.addBuilding(this);

                foreach (Material matt in materials)
                {
                    if (matt.name.Contains("teamColor"))
                    {
                        matt.color = RTS_Player.temp.Player_Material.color;
                        return;
                    }
                }
            }
            else
            {
            }
        }
    }
}
