using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class RTS_AI : MonoBehaviour
{
    public static RTS_AI temp;

    public enum Tactic
    {
        offensive,
        defensive,
    };

    public enum BuildPlacements
    {
        expand,
        close,
    };

    public Tactic tactic = Tactic.offensive;

    public BuildPlacements buildPlacement = BuildPlacements.expand;

    public GameObject rangeCenter;
    private Vector3 rangeCenterFinal;

    public int buildingAddCycle = 5;

    public float shortRange = 10.0f;
    public float midRange = 20.0f;
    public float longRange = 30.0f;

    public bool showShortRange = false;
    public bool showMidRange = false;
    public bool showLongRange = false;

    List<GameObject> Buildings = new List<GameObject>();
    List<GameObject> Defense = new List<GameObject>();
    List<GameObject> LightVehicles = new List<GameObject>();
    List<GameObject> HeavyVehicles = new List<GameObject>();
    List<GameObject> SpecialVehicles = new List<GameObject>();
    List<GameObject> Harvesters = new List<GameObject>();
    List<GameObject> OffensiveInfantry = new List<GameObject>();
    List<GameObject> Engineers = new List<GameObject>();
    List<GameObject> OffensiveAircraft = new List<GameObject>();

    List<int> availableBuildings = new List<int>();
    List<int> availableEnemyBuildings = new List<int>();
    List<int> availableSupport = new List<int>();
    List<int> availableEnemySupport = new List<int>();
    List<int> availableInfantry = new List<int>();
    List<int> availableEnemyInfantry = new List<int>();
    List<int> availableVehicles = new List<int>();
    List<int> availableEnemyVehicles = new List<int>();
    List<int> availableAircraft = new List<int>();
    List<int> availableEnemyAircraft = new List<int>();

    List<int> BuildingQueue = new List<int>();
    List<int> DefenseQueue = new List<int>();
    List<int> InfantryQueue = new List<int>();
    List<int> VehicleQueue = new List<int>();
    List<int> AircraftQueue = new List<int>();

    List<List<DB_Item>> CurrentBuildingItems = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentDefenseItems = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentInfantryItems = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentVehicleItems = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentAircraftItems = new List<List<DB_Item>>();

    List<bool> BuildingFriendly = new List<bool>();
    List<bool> DefenseFriendly = new List<bool>();
    List<bool> InfantryFriendly = new List<bool>();
    List<bool> VehicleFriendly = new List<bool>();
    List<bool> AircraftFriendly = new List<bool>();

    List<GameObject> VehicleFactories = new List<GameObject>();
    List<GameObject> InfantryFactories = new List<GameObject>();
    List<GameObject> AircraftFactories = new List<GameObject>();
    List<GameObject> BuildingFactories = new List<GameObject>();

    public List<DB_Item> ItemsToUpdate = new List<DB_Item>();

    bool removeItems = false;
    List<DB_Item> itemsToRemove = new List<DB_Item>();

    List<tile> oreTiles = new List<tile>();
    public List<Refinery_Controller> Refineries = new List<Refinery_Controller>();
    public List<PowerPlantController> PowerPlants = new List<PowerPlantController>();

    List<Building_Controller> myBuildings = new List<Building_Controller>();
    List<int> myBuildingIDs = new List<int>();

    int NumberConstruction = 0, NumberInfantry = 0, NumberVehicles = 0, NumberAircraft = 0;

    int Money;

    public int TechLevel = 10;
    public int TotalPower = 0;
    public int TotalConsumption = 0;

    public float FixRate = 10.0f;
    public float FixCostRate = 10.0f;

    bool FirstRun = true;

    public GameObject MCVs;

    tile[,] tempGrid;

    List<int> BuildingBuildOrder = new List<int>();
    List<int> VehicleBuildOrder = new List<int>();

    bool showArea = false;
    tile aTile;
    Vector3 aSize;

    bool CreatingBuilding = false;
    bool CreatingDefense = false;
    bool CreatingInfantry = false;
    bool CreatingVehicle = false;
    bool CreatingAircraft = false;

    DB_Item currentBuildingItem;
    DB_Item currentDefenseItem;
    DB_Item currentInfantryItem;
    DB_Item CurrentVehicleItem;
    DB_Item currentAircraftItem;

    GameObject CommandCentre;

    public GameObject PowerPlant;
    public GameObject Barracks;
    public GameObject Refinery;
    public GameObject WarFactory;
    public GameObject Radar;

    GameObject objTemp;

    public GameObject ident;

    List<MediumLevel> groupAIs = new List<MediumLevel>();
    List<MediumLevel> defendAIs = new List<MediumLevel>();
    List<MediumLevel> patrolAIs = new List<MediumLevel>();
    List<MediumLevel> exploreAIs = new List<MediumLevel>();
    List<MediumLevel> attackAIs = new List<MediumLevel>();

    List<MediumLevel> requestAIs = new List<MediumLevel>();

    public int midDefenseLimit = 2;
    public int midPatrolLimit = 3;
    public int midExploreLimit = 3;
    public int midAttackLimit = 3;

    int numMidDefense = 1;
    int numMidPatrol = 1;
    int numMidExplore = 1;
    int numMidAttack = 1;

    Collider[] lowThreat;
    Collider[] midThreat;
    Collider[] highThreat;

    public List<ThreatInfo> allThreatInfos = new List<ThreatInfo>();

    void Awake()
    {
        StartCoroutine("addBuildingOrder");
        StartCoroutine("addVehicleOrder");
    }

    void Start()
    {
        rangeCenterFinal = rangeCenter.transform.position;
        tempGrid = grid.mGrid.getGrid();        

        Money = GetComponent<RTS_Core>().StartingMoney;

        temp = this;

        defendAIs.Add(gameObject.AddComponent<MediumLevel>());

        defendAIs[0].setAreaToDefend(rangeCenterFinal);
        defendAIs[0].giveOrder(1);

        patrolAIs.Add(gameObject.AddComponent<MediumLevel>());
        patrolAIs[0].giveOrder(2);

        exploreAIs.Add(gameObject.AddComponent<MediumLevel>());
        exploreAIs[0].giveOrder(2);

        attackAIs.Add(gameObject.AddComponent<MediumLevel>());
        attackAIs[0].giveOrder(4);
    }

    IEnumerator addBuildingOrder()
    {
        BuildingBuildOrder.Add(100);
        BuildingBuildOrder.Add(101);
        BuildingBuildOrder.Add(102);
        BuildingBuildOrder.Add(103);
        BuildingBuildOrder.Add(105);
        BuildingBuildOrder.Add(106);

        while (buildingAddCycle > 0)
        {
            buildingAddCycle--;
            yield return new WaitForSeconds(1);
            BuildingBuildOrder.Add(101);
            BuildingBuildOrder.Add(101);
            BuildingBuildOrder.Add(103);

            if (buildingAddCycle > 3)
            {
                BuildingBuildOrder.Add(105);
            }

            if (buildingAddCycle > 0)
            {
                StartCoroutine(addBuildingOrder());
            }
        }
    }

    IEnumerator addVehicleOrder()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            VehicleBuildOrder.Add(141);
        }
    }

    void Update()
    {
        if (FirstRun)
        {
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("eBuilding");
            foreach (GameObject g in allObjects)
            {
                int bID = g.GetComponent<Building_Controller>().ID;
                if (bID == 120 || bID == 121 || bID == 122 || bID == 123)
                {
                    Defense.Add(g);
                }
                else
                {
                    if (bID == 100)
                    {
                        CommandCentre = g;
                    }
                    Buildings.Add(g);
                }

                addBuilding(g.GetComponent<Building_Controller>());

                BuildingBuildOrder.Remove(bID);
            }

            allObjects = GameObject.FindGameObjectsWithTag("npc");
            foreach (GameObject g in allObjects)
            {
                if (g.GetComponent<Unit_Core>().Stats.isEngineer)
                {
                    Engineers.Add(g);
                }
                else if (g.GetComponent<Unit_Core>().Stats.isOreTruck)
                {
                    Harvesters.Add(g);
                }
                else if (g.GetComponent<Unit_Core>().Stats.isInfantry)
                {
                    OffensiveInfantry.Add(g);
                }
                else if (g.GetComponent<Unit_Core>().Stats.isVehicle)
                {
                    DB_Item iTemp = g.GetComponent<Unit_Core>().getItem();
                    if (iTemp.ID == 141)
                    {
                        LightVehicles.Add(g);
                        VehicleBuildOrder.Remove(141);
                    }
                    else if (iTemp.ID == 142)
                    {
                    }
                    else if (iTemp.ID == 143)
                    {
                        HeavyVehicles.Add(g);
                        VehicleBuildOrder.Remove(143);
                    }
                    else if (iTemp.ID == 44)
                    {
                        SpecialVehicles.Add(g);
                    }
                }
                else
                {
                    OffensiveAircraft.Add(g);
                }
            }

            if (MCVs)
            {

            }
            else
            {
                if (BuildingBuildOrder[0] == 100)
                {

                }
            }

            FirstRun = false;
        }
        lowThreat = Physics.OverlapSphere(rangeCenterFinal, longRange, 1 << 8);
        foreach (MediumLevel m in defendAIs)
        {
            if (m.underOrders() && !m.hasTarget())
            {
                m.setDestination(m.getAreaToDefend(), 1);
            }
        }

        if (lowThreat.Length > 0)
        {
            foreach (Collider c in lowThreat)
            {
                if (!c.gameObject.GetComponent<ThreatInfo>())
                {
                    c.gameObject.AddComponent<ThreatInfo>();
                }

                if (c.gameObject.GetComponent<ThreatInfo>().handlerAI == null)
                {
                    MediumLevel closestAI = null;
                    float dist = 100000.0f;
                    foreach (MediumLevel m in defendAIs)
                    {
                        if (!m.underOrders() && Vector3.Distance(m.GetLocation(), c.transform.position) < dist)
                        {
                            dist = Vector3.Distance(m.GetLocation(), c.transform.position);
                            closestAI = m;
                        }
                    }

                    if (closestAI != null)
                    {
                        closestAI.setAttackTarget(c.gameObject, false, 1);
                    }
                }
            }
        }

        if (!CreatingBuilding && BuildingBuildOrder.Count > 0 && availableEnemyBuildings.Contains(BuildingBuildOrder[0]) && CurrentBuildingItems.Count > 0)
        {
            CreatingBuilding = true;
            foreach (DB_Item i in CurrentBuildingItems[0])
            {
                if (i.ID == BuildingBuildOrder[0])
                {
                    i.startBuilding();
                    currentBuildingItem = i;
                    break;
                }
            }
        }
        else if (CreatingBuilding && CurrentBuildingItems.Count > 0)
        {
            if (currentBuildingItem.isFinished)
            {
                CreatingBuilding = false;

                float width = currentBuildingItem.Unit.GetComponent<Renderer>().bounds.size.x;
                float length = currentBuildingItem.Unit.GetComponent<Renderer>().bounds.size.z;

                int gridWidth = (int)(width / 2.5f);
                int gridLength = (int)(length / 2.5f);

                gridWidth++;
                gridLength++;

                int[] tempLoc = new int[2];
                tempLoc[0] = grid.mGrid.returnClosestTile(CommandCentre.transform.position).i;
                tempLoc[1] = grid.mGrid.returnClosestTile(CommandCentre.transform.position).j;

                int[] tempInt;
                if (currentBuildingItem.ID == 103)
                {
                    tile tempTile = grid.mGrid.findClosestAvailableTileWithOre(CommandCentre.transform.position, grid.mGrid.returnClosestTile(CommandCentre.transform.position).navMesh);
                    tempInt = findAvailableSpace(tempTile.i, tempTile.j, gridWidth, gridLength);
                }
                else
                {
                    tempInt = findAvailableSpace(tempLoc[0], tempLoc[1], gridWidth, gridLength);
                }


                float xOffset = 0;
                float zOffset = 0;
                if (gridWidth % 2 == 0)
                {
                    xOffset = 1.25f;
                }

                if (gridLength % 2 == 0)
                {
                    zOffset = -1.25f;
                }
                if (currentBuildingItem.ID == 101)
                {
                    objTemp = (GameObject)Instantiate(PowerPlant, grid.mGrid.mainGrid[tempInt[0], tempInt[1]].center + new Vector3(xOffset, 0, zOffset), PowerPlant.transform.rotation);
                }
                else if (currentBuildingItem.ID == 102)
                {
                    objTemp = (GameObject)Instantiate(Barracks, grid.mGrid.mainGrid[tempInt[0], tempInt[1]].center + new Vector3(xOffset, 0, zOffset), Barracks.transform.rotation);
                }
                else if (currentBuildingItem.ID == 103)
                {
                    objTemp = (GameObject)Instantiate(Refinery, grid.mGrid.mainGrid[tempInt[0], tempInt[1]].center + new Vector3(xOffset, 0, zOffset), Refinery.transform.rotation);
                }
                else if (currentBuildingItem.ID == 104)
                {

                }
                else if (currentBuildingItem.ID == 105)
                {
                    objTemp = (GameObject)Instantiate(WarFactory, grid.mGrid.mainGrid[tempInt[0], tempInt[1]].center + new Vector3(xOffset, 0, zOffset), Refinery.transform.rotation);
                }
                else if (currentBuildingItem.ID == 106)
                {
                    objTemp = (GameObject)Instantiate(Radar, grid.mGrid.mainGrid[tempInt[0], tempInt[1]].center + new Vector3(xOffset, 0, zOffset), Radar.transform.rotation);
                }

                objTemp.GetComponent<Building_Controller>().placed();
                objTemp.GetComponent<Building_Controller>().targetY = objTemp.transform.position.y;
                addBuilding(objTemp.GetComponent<Building_Controller>());
                BuildingBuildOrder.Remove(currentBuildingItem.ID);
                currentBuildingItem.finishBuild();
            }
        }

        if (VehicleFactories.Count > 0)
        {

            if (VehicleBuildOrder.Count > 0 && !CreatingVehicle)
            {
                foreach (DB_Item i in CurrentVehicleItems[0])
                {
                    if (i.ID == VehicleBuildOrder[0])
                    {
                        CurrentVehicleItem = i;
                        CurrentVehicleItem.startBuilding();
                        VehicleBuildOrder.Remove(i.ID);
                        CreatingVehicle = true;
                        break;
                    }
                }
            }
            else if (CreatingVehicle)
            {
                if (!ItemsToUpdate.Contains(CurrentVehicleItem)) CreatingVehicle = false;
            }
            else
            {

            }
        }


        foreach (DB_Item i in ItemsToUpdate)
        {
            i.updateItem();
        }

        if (removeItems)
        {
            foreach (DB_Item i in itemsToRemove)
            {
                ItemsToUpdate.Remove(i);
            }
            itemsToRemove.Clear();
            removeItems = false;
        }
    }

    void OnDrawGizmos()
    {
        if (rangeCenter)
        {
            Gizmos.color = Color.red;
            if (showShortRange) Gizmos.DrawWireSphere(rangeCenter.transform.position, shortRange);
            Gizmos.color = Color.yellow;
            if (showMidRange) Gizmos.DrawWireSphere(rangeCenter.transform.position, midRange);
            Gizmos.color = Color.green;
            if (showLongRange) Gizmos.DrawWireSphere(rangeCenter.transform.position, longRange);

            if (showArea)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(aTile.center, aSize);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(aTile.center, new Vector3(2.5f, 0.1f, 2.5f));
            }
        }
    }

    public void addBuilding(GameObject g)
    {
        Buildings.Add(g);
    }

    public void addSupportBuilding(GameObject g)
    {
        Defense.Add(g);
    }

    public void removeSupportBuilding(GameObject g, bool sold)
    {
        Defense.Remove(g);

        if (!sold)
        {
            DefenseQueue.Add(g.GetComponent<Building_Controller>().ID);
        }
    }

    private int[] findAvailableSpace(int x, int y, int width, int length)
    {
        int tempX = x;
        int tempY = y;

        int checkLeft;
        int checkRight;
        int checkUp;
        int checkDown;

        int[] tempCoords = new int[2];

        int counter = 0;
        int limit = 1;
        int dir = 1;

        if (width == 1)
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkLeft = -1;
                checkRight = 1;
            }
            else
            {
                checkLeft = -2;
                checkRight = 2;
            }
        }
        else if (width == 2)
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkLeft = -1;
                checkRight = 2;
            }
            else
            {
                checkLeft = -2;
                checkRight = 3;
            }
        }
        else if (width == 3)
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkLeft = -2;
                checkRight = 2;
            }
            else
            {
                checkLeft = -3;
                checkRight = 3;
            }
        }
        else
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkLeft = -2;
                checkRight = 3;
            }
            else
            {
                checkLeft = -3;
                checkRight = 4;
            }
        }

        if (length == 1)
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkUp = 1;
                checkDown = -1;
            }
            else
            {
                checkUp = 2;
                checkDown = -2;
            }
        }
        else if (length == 2)
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkUp = 1;
                checkDown = -2;
            }
            else
            {
                checkUp = 2;
                checkDown = -3;
            }
        }
        else if (length == 3)
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkUp = 2;
                checkDown = -2;
            }
            else
            {
                checkUp = 3;
                checkDown = -3;
            }
        }
        else
        {
            if (buildPlacement == BuildPlacements.close)
            {
                checkUp = 2;
                checkDown = -3;
            }
            else
            {
                checkUp = 3;
                checkDown = -4;
            }
        }

        bool found = false;
        List<tile> checkedTiles = new List<tile>();
        do
        {
            if (tempGrid[tempX, tempY].status == 3)
            {
                tempCoords[0] = tempX;
                tempCoords[1] = tempY;
                tempGrid[tempX, tempY].buildCheck = true;
                checkedTiles.Add(tempGrid[tempX, tempY]);

                bool go = true;
                for (int i = checkLeft; i <= checkRight; i++)
                {
                    for (int j = checkDown; j <= checkUp; j++)
                    {
                        tile tTemp = tempGrid[tempX + i, tempY + j];
                        if (tTemp.status != 3 || tTemp.hasOre || (tTemp.occupied != null && tTemp.occupied.GetComponent<Faction>().isFriend()))
                        {
                            go = false;
                        }
                    }
                }

                if (go)
                {
                    foreach (tile ti in checkedTiles)
                    {
                        ti.buildCheck = false;
                    }
                    return tempCoords;
                }
            }

            tile t = findClosestAvailableTile(grid.mGrid.mainGrid[x, y], 0, 1, 0, grid.mGrid.mainGrid[tempX, tempY].navMesh);
            tempX = t.i;
            tempY = t.j;

        }
        while (!found);

        return null;
    }

    public void AddMoney(int m)
    {
        Money += m;
    }

    public void costMoney(int m)
    {
        Money -= m;
    }

    public int getMoney()
    {
        return Money;
    }

    public void requestAI(GameObject unit)
    {
        int nDefend = defendAIs.Count;
        int nExplore = exploreAIs.Count;
        int nPatrol = patrolAIs.Count;
        int nAttack = attackAIs.Count;
        if (requestAIs.Count > 0)
        {
            requestAIs[0].assignUnit(unit);
            requestAIs.RemoveAt(0);
        }
        else if (nDefend - 2 <= nExplore && nDefend - 2 <= nPatrol && nDefend - 2 <= nAttack)
        {
            defendAIs[numMidDefense - 1].assignUnit(unit);

            if (defendAIs[numMidDefense - 1].Units.Count >= midDefenseLimit)
            {
                defendAIs.Add(gameObject.AddComponent<MediumLevel>());
                numMidDefense++;

                float dist = 17;
                Vector3 area = new Vector3();

                int distMultiplier = (int)((numMidDefense / 8) + 1);
                int rotation = numMidDefense * 45;

                area.x = distMultiplier * dist * Mathf.Sin(rotation * Mathf.Deg2Rad);
                area.z = distMultiplier * dist * Mathf.Cos(rotation * Mathf.Deg2Rad);

                defendAIs[numMidDefense - 1].setAreaToDefend(rangeCenterFinal + area);

                defendAIs[numMidDefense - 1].giveOrder(1);
            }
        }
        else if (nPatrol <= nExplore && nPatrol <= nAttack)
        {
            patrolAIs[numMidPatrol - 1].assignUnit(unit);

            if (patrolAIs[numMidPatrol - 1].Units.Count >= midPatrolLimit)
            {
                patrolAIs.Add(gameObject.AddComponent<MediumLevel>());
                numMidPatrol++;
                patrolAIs[numMidPatrol - 1].giveOrder(2);
            }
        }
        else if (nExplore <= nAttack - 1)
        {
            exploreAIs[numMidExplore - 1].assignUnit(unit);

            if (exploreAIs[numMidExplore - 1].Units.Count >= midExploreLimit)
            {
                exploreAIs.Add(gameObject.AddComponent<MediumLevel>());
                numMidExplore++;
                exploreAIs[numMidExplore - 1].giveOrder(2);
            }
        }
        else
        {
            attackAIs[numMidAttack - 1].assignUnit(unit);

            if (attackAIs[numMidAttack - 1].Units.Count >= midAttackLimit)
            {
                attackAIs.Add(gameObject.AddComponent<MediumLevel>());

                numMidAttack++;
                attackAIs[numMidAttack - 1].giveOrder(4);
            }
        }
    }

    private tile findClosestAvailableTile(tile t, int counter, int limit, int direction, int navMesh)
    {
        if (t.status == 3 && t.navMesh == navMesh && !t.buildCheck)
        {
            return t;
        }

        counter++;

        if (direction == 0)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
            }

            return findClosestAvailableTile(grid.mGrid.mainGrid[t.i, t.j + 1], counter, limit, direction, navMesh);
        }
        else if (direction == 1)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
                limit++;
            }
            return findClosestAvailableTile(grid.mGrid.mainGrid[t.i + 1, t.j], counter, limit, direction, navMesh);
        }
        else if (direction == 2)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
            }
            return findClosestAvailableTile(grid.mGrid.mainGrid[t.i, t.j - 1], counter, limit, direction, navMesh);
        }
        else
        {
            if (counter == limit)
            {
                direction = 0;
                counter = 0;
                limit++;
            }
            return findClosestAvailableTile(grid.mGrid.mainGrid[t.i - 1, t.j], counter, limit, direction, navMesh);
        }
    }

    public void addBuilding(Building_Controller b)
    {
        DB_Item iTemp;
        if (b.ID == 0)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableBuildings.Add(1);
                availableBuildings.Add(3);
            }

            CurrentBuildingItems.Add(new List<DB_Item>());
            CurrentDefenseItems.Add(new List<DB_Item>());

            BuildingFriendly.Add(true);
            DefenseFriendly.Add(true);

            foreach (int i in availableBuildings)
            {
                iTemp = DB_Database.temp.returnItem(1, i);
                iTemp.BuiltByAI = true;
                CurrentBuildingItems[NumberConstruction].Add(iTemp);
            }

            CurrentBuildingItems[NumberConstruction].Sort();

            foreach (int i in availableSupport)
            {
                iTemp = DB_Database.temp.returnItem(2, i);
                iTemp.BuiltByAI = true;
                CurrentDefenseItems[NumberConstruction].Add(iTemp);
            }

            CurrentDefenseItems[NumberConstruction].Sort();

            NumberConstruction++;
            b.buildingID = NumberConstruction;
        }

        else if (b.ID == 1)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableBuildings.Add(2);
                availableSupport.Add(1);

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(1, 2));
                        l.Sort();
                    }
                }

                foreach (List<DB_Item> l in CurrentDefenseItems)
                {
                    if (DefenseFriendly[CurrentDefenseItems.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(2, 1));
                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 2)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableInfantry.Add(1);
                availableInfantry.Add(2);
                availableSupport.Add(2);

                if (myBuildingIDs.Contains(3))
                {
                    availableBuildings.Add(4);
                }

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(3))
                        {
                            l.Add(DB_Database.temp.returnItem(1, 4));
                        }
                        l.Sort();
                    }
                }

                foreach (List<DB_Item> l in CurrentDefenseItems)
                {
                    if (DefenseFriendly[CurrentDefenseItems.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(2, 2));
                    }
                }
            }

            CurrentInfantryItems.Add(new List<DB_Item>());

            InfantryFriendly.Add(true);

            foreach (int i in availableInfantry)
            {
                iTemp = DB_Database.temp.returnItem(3, i);
                iTemp.Parent = b.gameObject;
                CurrentInfantryItems[NumberInfantry].Add(iTemp);
            }

            NumberInfantry++;
            b.buildingID = NumberInfantry;
        }

        else if (b.ID == 3)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableBuildings.Add(5);
                availableBuildings.Add(6);

                if (myBuildingIDs.Contains(2))
                {
                    availableBuildings.Add(4);
                }

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(2))
                        {
                            l.Add(DB_Database.temp.returnItem(1, 4));
                        }
                        l.Add(DB_Database.temp.returnItem(1, 5));
                        l.Add(DB_Database.temp.returnItem(1, 6));
                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 4)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableVehicles.Add(1);
                availableVehicles.Add(2);

                if (myBuildingIDs.Contains(6))
                {
                    availableBuildings.Add(7);
                }

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(6))
                        {
                            l.Add(DB_Database.temp.returnItem(1, 7));
                        }

                        l.Sort();
                    }
                }
            }

            CurrentVehicleItems.Add(new List<DB_Item>());
            VehicleFactories.Add(b.gameObject);
            VehicleFriendly.Add(true);

            foreach (int i in availableVehicles)
            {
                iTemp = DB_Database.temp.returnItem(4, i);
                iTemp.Parent = b.gameObject;
                CurrentVehicleItems[NumberVehicles].Add(iTemp);
            }

            NumberVehicles++;
            b.buildingID = NumberVehicles;
        }

        else if (b.ID == 5)
        {

        }

        else if (b.ID == 6)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableVehicles.Add(4);

                if (myBuildingIDs.Contains(4))
                {
                    availableBuildings.Add(7);
                }

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(1, 8));

                        if (myBuildingIDs.Contains(4))
                        {
                            l.Add(DB_Database.temp.returnItem(1, 7));
                        }
                    }
                    l.Sort();
                }

                int buildingCounter = 0;
                foreach (List<DB_Item> l in CurrentVehicleItems)
                {
                    if (VehicleFriendly[CurrentVehicleItems.IndexOf(l)])
                    {
                        iTemp = DB_Database.temp.returnItem(4, 4);
                        iTemp.Parent = VehicleFactories[buildingCounter];
                        l.Add(iTemp);
                    }
                    buildingCounter++;
                    l.Sort();
                }
            }
        }

        else if (b.ID == 7)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableVehicles.Add(3);
            }

            int buildingCounter = 0;
            foreach (List<DB_Item> l in CurrentVehicleItems)
            {
                if (VehicleFriendly[CurrentVehicleItems.IndexOf(l)])
                {
                    iTemp = DB_Database.temp.returnItem(4, 3);
                    iTemp.Parent = VehicleFactories[buildingCounter];
                    l.Add(iTemp);
                }
                buildingCounter++;
                l.Sort();
            }
        }

        else if (b.ID == 8)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableAircraft.Add(1);
            }

            CurrentAircraftItems.Add(new List<DB_Item>());

            AircraftFriendly.Add(true);

            foreach (int i in availableAircraft)
            {
                iTemp = DB_Database.temp.returnItem(5, i);
                iTemp.Parent = b.gameObject;
                CurrentAircraftItems[NumberAircraft].Add(iTemp);
            }

            NumberAircraft++;
            b.buildingID = NumberAircraft;
        }

        else if (b.ID == 100)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableEnemyBuildings.Add(101);
            }

            CurrentBuildingItems.Add(new List<DB_Item>());
            CurrentDefenseItems.Add(new List<DB_Item>());

            BuildingFriendly.Add(false);
            DefenseFriendly.Add(false);

            foreach (int i in availableEnemyBuildings)
            {
                iTemp = DB_Database.temp.returnItem(1, i);
                iTemp.BuiltByAI = true;
                CurrentBuildingItems[NumberConstruction].Add(iTemp);
            }

            CurrentBuildingItems[NumberConstruction].Sort();

            foreach (int i in availableEnemySupport)
            {
                iTemp = DB_Database.temp.returnItem(2, i);
                iTemp.BuiltByAI = true;
                CurrentDefenseItems[NumberConstruction].Add(iTemp);
            }

            CurrentDefenseItems[NumberConstruction].Sort();

            NumberConstruction++;
            b.buildingID = NumberConstruction;
            BuildingFactories.Add(b.gameObject);
        }

        else if (b.ID == 101)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableEnemyBuildings.Add(102);
                availableEnemyBuildings.Add(103);

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (!BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        iTemp = DB_Database.temp.returnItem(1, 102);
                        iTemp.BuiltByAI = true;
                        l.Add(iTemp);

                        iTemp = DB_Database.temp.returnItem(1, 103);
                        iTemp.BuiltByAI = true;
                        l.Add(iTemp);

                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 102)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableEnemyBuildings.Add(105);

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (!BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        iTemp = DB_Database.temp.returnItem(1, 105);
                        iTemp.BuiltByAI = true;
                        iTemp.Parent = b.gameObject;
                        l.Add(iTemp);

                        l.Sort();
                    }
                }
            }

            InfantryFactories.Add(b.gameObject);
        }

        else if (b.ID == 103)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                availableEnemyBuildings.Add(106);

                foreach (List<DB_Item> l in CurrentBuildingItems)
                {
                    if (!BuildingFriendly[CurrentBuildingItems.IndexOf(l)])
                    {
                        iTemp = DB_Database.temp.returnItem(1, 106);
                        iTemp.BuiltByAI = true;
                        l.Add(iTemp);

                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 104)
        {

        }

        else if (b.ID == 105)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                if (myBuildingIDs.Contains(106))
                {
                    availableBuildings.Add(107);
                }

                availableVehicles.Add(141);
                availableVehicles.Add(142);
            }

            CurrentVehicleItems.Add(new List<DB_Item>());
            VehicleFriendly.Add(false);
            foreach (int i in availableVehicles)
            {
                iTemp = DB_Database.temp.returnItem(4, i);
                iTemp.BuiltByAI = true;
                iTemp.Parent = b.gameObject;
                CurrentVehicleItems[NumberVehicles].Add(iTemp);
            }


            VehicleFactories.Add(b.gameObject);
            NumberVehicles++;
        }
        else if (b.ID == 106)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                if (myBuildingIDs.Contains(105))
                {
                    availableBuildings.Add(107);
                }
            }
        }

        TotalConsumption += b.powerConsumption;
        myBuildings.Add(b);
        myBuildingIDs.Add(b.ID);
    }

    public void removeBuilding(Building_Controller b, bool sold)
    {
        if (b.ID == 0 || b.ID == 100)
        {

            foreach (DB_Item i in CurrentBuildingItems[b.buildingID - 1])
            {
                if (i.isBuilding || i.isFinished)
                {
                    i.cancel(sold);
                }
            }

            CurrentBuildingItems.RemoveAt(b.buildingID - 1);
            CurrentDefenseItems.RemoveAt(b.buildingID - 1);

            BuildingFriendly.RemoveAt(b.buildingID - 1);
            DefenseFriendly.RemoveAt(b.buildingID - 1);

            NumberConstruction--;

            foreach (Building_Controller build in myBuildings)
            {
                if (build.ID == 0 || build.ID == 100)
                {
                    if (build.buildingID > b.buildingID) build.buildingID -= 1;
                }
            }

            myBuildingIDs.Remove(b.ID);
            myBuildings.Remove(b);

            if (b.ID == 0 && !myBuildingIDs.Contains(0))
            {
                availableBuildings.Remove(1);
                availableBuildings.Remove(3);
                availableBuildings.Remove(8);
            }

            if (b.ID == 100 && !myBuildingIDs.Contains(100))
            {
                availableEnemyBuildings.Remove(101);
            }

        }
    }

    public void removeUpdateItem(DB_Item i)
    {
        removeItems = true;
        itemsToRemove.Add(i);
    }

    public Collider[] getLowThreats()
    {
        return lowThreat;
    }

    public Collider[] getMidThreats()
    {
        return midThreat;
    }

    public Collider[] getHighThreats()
    {
        return highThreat;
    }

    public void removeUnit(GameObject g)
    {
        if (g.GetComponent<Unit_Core>().getItem().ID == 141)
        {
            LightVehicles.Remove(g);
        }
        if (g.GetComponent<Unit_Core>().getItem().ID == 142)
        {
            Harvesters.Remove(g);
            VehicleBuildOrder.Add(142);
        }
    }

    public void removeBuilding(GameObject g)
    {
        if (g.GetComponent<Building_Controller>().ID == 100)
        {
            removeBuilding(g.GetComponent<Building_Controller>(), false);

        }
        else if (g.GetComponent<Building_Controller>().ID == 103)
        {
            BuildingBuildOrder.Add(103);
        }
    }

    public List<MediumLevel> getDefendAIs()
    {
        return defendAIs;
    }

    public List<MediumLevel> getPatrolAIs()
    {
        return patrolAIs;
    }

    public List<MediumLevel> getExploreAIs()
    {
        return exploreAIs;
    }

    public List<MediumLevel> getAttackAIs()
    {
        return attackAIs;
    }

    public void RequestUnit(MediumLevel controller)
    {
        requestAIs.Add(controller);

        List<MediumLevel> occurances = new List<MediumLevel>();
        List<int> nIndexes = new List<int>();
        List<int> allIndexes = new List<int>();

        for (int i = 0; i < requestAIs.Count; i++)
        {
            if (controller.action == requestAIs[i].action)
            {
                if (occurances.Contains(requestAIs[i]))
                {
                    nIndexes[occurances.IndexOf(requestAIs[i])]++;
                }
                else
                {
                    occurances.Add(requestAIs[i]);
                    nIndexes.Add(1);
                }

                allIndexes.Add(i);
            }
        }

        int counter = 0;

        while (counter < occurances.Count)
        {
            int currentIndex = nIndexes.IndexOf(nIndexes.Min());
            int numberOfTimes = nIndexes[currentIndex];
            MediumLevel lowestAI = occurances[currentIndex];

            for (int i = 0; i < numberOfTimes; i++)
            {
                requestAIs[allIndexes[0]] = lowestAI;
                allIndexes.RemoveAt(0);
            }

            occurances.Remove(lowestAI);
            nIndexes.Remove(nIndexes.Min());


            counter++;
        }


    }

    public void addThreatInfo(ThreatInfo tI)
    {
        allThreatInfos.Add(tI);
    }

    public void removeThreatInfo(ThreatInfo tI)
    {
        allThreatInfos.Remove(tI);
    }

    public bool isThreat(Collider c)
    {
        if (lowThreat.Contains(c))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}