using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class RTS_Player : MonoBehaviour
{
    public static RTS_Player temp;

    public List<GameObject> Units = new List<GameObject>();
    public List<GameObject> Buildings = new List<GameObject>();

    List<Building_Controller> myBuildings = new List<Building_Controller>();
    List<int> myBuildingIDs = new List<int>();

    int numberConstruction = 0, numberInfantry = 0, numberVehicles = 0, numberAir = 0;

    List<int> Available_Player_Buildings = new List<int>();
    List<int> Available_Player_Defense = new List<int>();
    List<int> Available_Player_Infantry = new List<int>();
    List<int> Available_Player_Vehicles = new List<int>();
    List<int> Available_Player_Aircraft = new List<int>();

    List<int> Available_AI_Buildings = new List<int>();
    List<int> Available_AI_Defense = new List<int>();
    List<int> Available_AI_Infantry = new List<int>();
    List<int> Available_AI_Vehicles = new List<int>();
    List<int> Available_AI_Aircraft = new List<int>();

    List<List<DB_Item>> CurrentBuildings = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentDefense = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentInfantry = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentVehicles = new List<List<DB_Item>>();
    List<List<DB_Item>> CurrentAircraft = new List<List<DB_Item>>();

    List<bool> FriendlyBuilding = new List<bool>();
    List<bool> FriendlyDefense = new List<bool>();
    List<bool> FriendlyInfantry = new List<bool>();
    List<bool> FriendlyVehicles = new List<bool>();
    List<bool> FriendlyAircraft = new List<bool>();

    public List<DB_Item> ItemsToUpdate = new List<DB_Item>();

    bool RemoveItems = false;
    List<DB_Item> ItemsToRemove = new List<DB_Item>();

    List<tile> OreTiles = new List<tile>();
    public List<Refinery_Controller> Refineries = new List<Refinery_Controller>();
    public List<PowerPlantController> PowerPlants = new List<PowerPlantController>();
    public List<Helipad_Controller> Helipads = new List<Helipad_Controller>();

    List<GameObject> VehicleFactories = new List<GameObject>();
    List<GameObject> InfantryFactories = new List<GameObject>();
    List<GameObject> AircraftFactories = new List<GameObject>();
    List<GameObject> BuildingFactories = new List<GameObject>();

    public float BuildingDistance = 5.0f;

    int Money;

    public int TechLevel = 10;
    public int TotalPower = 0;
    public int TotalConsumption = 0;

    public float FixRate = 10.0f;
    public float FixCostRate = 10.0f;

    public Material AI_Material;
    public Material Player_Material;

    bool _won = false;
    public bool won
    {
        get { return _won; }
    }

    bool _lost = false;
    public bool lost
    {
        get { return _lost; }
    }

    public bool AlwaysDisplayRadar = false;

    void Start()
    {
        temp = this;
        Money = GetComponent<RTS_Core>().StartingMoney;
    }

    void Update()
    {
        foreach (DB_Item i in ItemsToUpdate)
        {
            i.updateItem();
        }

        if (RemoveItems)
        {
            foreach (DB_Item i in ItemsToRemove)
            {
                ItemsToUpdate.Remove(i);
            }
            ItemsToRemove.Clear();
            RemoveItems = false;
        }

        GameObject[] cBuildings = GameObject.FindGameObjectsWithTag("npc");
        GameObject[] cUnits = GameObject.FindGameObjectsWithTag("eBuilding");

        if (cBuildings.Length == 0 && cUnits.Length == 0)
        {
            _won = true;
            BroadcastMessage("EndGameWon", SendMessageOptions.DontRequireReceiver);
        }

        cBuildings = GameObject.FindGameObjectsWithTag("Player");
        cUnits = GameObject.FindGameObjectsWithTag("fBuilding");

        if (cBuildings.Length == 0 && cUnits.Length == 0)
        {
            _lost = true;
            BroadcastMessage("EndGameLost", SendMessageOptions.DontRequireReceiver);
        }

    }

    public void addBuilding(Building_Controller b)
    {
        if (b.ID == 0)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Buildings.Add(1);
                Available_Player_Buildings.Add(3);
            }

            CurrentBuildings.Add(new List<DB_Item>());
            CurrentDefense.Add(new List<DB_Item>());

            FriendlyBuilding.Add(true);
            FriendlyDefense.Add(true);

            int currentBuildingIndex = numberConstruction++;

            foreach (int i in Available_Player_Buildings)
            {
                CurrentBuildings[currentBuildingIndex].Add(DB_Database.temp.returnItem(1, i));
            }

            CurrentBuildings[currentBuildingIndex].Sort();

            foreach (int i in Available_Player_Defense)
            {
                CurrentDefense[currentBuildingIndex].Add(DB_Database.temp.returnItem(2, i));
            }

            CurrentDefense[currentBuildingIndex].Sort();

            b.buildingID = currentBuildingIndex;
        }

        else if (b.ID == 1)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Buildings.Add(2);
                Available_Player_Defense.Add(1);

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(1, 2));
                        l.Sort();
                    }
                }

                foreach (List<DB_Item> l in CurrentDefense)
                {
                    if (FriendlyDefense[CurrentDefense.IndexOf(l)])
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
                Available_Player_Infantry.Add(1);
                Available_Player_Infantry.Add(2);
                Available_Player_Defense.Add(2);

                if (myBuildingIDs.Contains(3))
                {
                    Available_Player_Buildings.Add(4);
                }

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)] && myBuildingIDs.Contains(3))
                    {
                        l.Add(DB_Database.temp.returnItem(1, 4));
                        l.Sort();
                    }
                }

                foreach (List<DB_Item> l in CurrentDefense)
                {
                    if (FriendlyDefense[CurrentDefense.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(2, 2));
                    }
                }
            }

            CurrentInfantry.Add(new List<DB_Item>());
            FriendlyInfantry.Add(true);

            foreach (int i in Available_Player_Infantry)
            {
                DB_Item iTemp = DB_Database.temp.returnItem(3, i);
                iTemp.Parent = b.gameObject;
                CurrentInfantry[numberInfantry].Add(iTemp);
            }

            b.buildingID = numberInfantry++;
        }


        else if (b.ID == 3)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Buildings.Add(5);
                Available_Player_Buildings.Add(6);

                if (myBuildingIDs.Contains(2))
                {
                    Available_Player_Buildings.Add(4);
                }

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)] && myBuildingIDs.Contains(2))
                    {
                        l.Add(DB_Database.temp.returnItem(1, 4));
                    }

                    l.Add(DB_Database.temp.returnItem(1, 5));
                    l.Add(DB_Database.temp.returnItem(1, 6));
                    l.Sort();
                }
            }
        }

        else if (b.ID == 4)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Vehicles.Add(1);
                Available_Player_Vehicles.Add(2);

                if (myBuildingIDs.Contains(6))
                {
                    Available_Player_Buildings.Add(7);
                }

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)] && myBuildingIDs.Contains(6))
                    {
                        l.Add(DB_Database.temp.returnItem(1, 7));
                        l.Sort();
                    }
                }
            }

            CurrentVehicles.Add(new List<DB_Item>());
            VehicleFactories.Add(b.gameObject);
            FriendlyVehicles.Add(true);

            foreach (int i in Available_Player_Vehicles)
            {
                DB_Item iTemp = DB_Database.temp.returnItem(4, i);
                iTemp.Parent = b.gameObject;
                CurrentVehicles[numberVehicles].Add(iTemp);
            }

            b.buildingID = numberVehicles++;
        }

        else if (b.ID == 5)
        {

        }

        else if (b.ID == 6)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Vehicles.Add(4);

                if (myBuildingIDs.Contains(4))
                {
                    Available_Player_Buildings.Add(7);
                }

                int currentBuildingIndex = numberConstruction++;

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(1, 8));

                        if (myBuildingIDs.Contains(4))
                        {
                            l.Add(DB_Database.temp.returnItem(1, 7));
                        }

                        l.Sort();
                    }
                }

                int buildingCounter = 0;
                foreach (List<DB_Item> l in CurrentVehicles)
                {
                    if (FriendlyVehicles[buildingCounter])
                    {
                        DB_Item iTemp = DB_Database.temp.returnItem(4, 4);
                        iTemp.Parent = VehicleFactories[buildingCounter];
                        l.Add(iTemp);
                        l.Sort();
                    }
                    buildingCounter++;
                }
            }
        }

        else if (b.ID == 7)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Vehicles.Add(3);
            }

            int buildingCounter = 0;
            foreach (List<DB_Item> l in CurrentVehicles)
            {
                if (FriendlyVehicles[buildingCounter])
                {
                    DB_Item iTemp = DB_Database.temp.returnItem(4, 3);
                    iTemp.Parent = VehicleFactories[buildingCounter];
                    l.Add(iTemp);
                    l.Sort();
                }
                buildingCounter++;
            }
        }

        else if (b.ID == 8)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_Player_Aircraft.Add(1);
            }

            CurrentAircraft.Add(new List<DB_Item>());
            FriendlyAircraft.Add(true);

            foreach (int i in Available_Player_Aircraft)
            {
                DB_Item iTemp = DB_Database.temp.returnItem(5, i);
                iTemp.Parent = b.gameObject;
                CurrentAircraft[numberAir++].Add(iTemp);
            }

            b.buildingID = numberAir;
        }

        else if (b.ID == 100)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_AI_Buildings.Add(101);
            }

            CurrentBuildings.Add(new List<DB_Item>());
            CurrentDefense.Add(new List<DB_Item>());

            FriendlyBuilding.Add(false);
            FriendlyDefense.Add(false);

            foreach (int i in Available_AI_Buildings)
            {
                CurrentBuildings[numberConstruction].Add(DB_Database.temp.returnItem(1, i));
            }

            CurrentBuildings[numberConstruction].Sort();

            foreach (int i in Available_AI_Defense)
            {
                CurrentDefense[numberConstruction].Add(DB_Database.temp.returnItem(2, i));
            }

            CurrentDefense[numberConstruction].Sort();

            numberConstruction++;
            b.buildingID = numberConstruction;
        }

        else if (b.ID == 101)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                Available_AI_Buildings.Add(102);

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (!FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        l.Add(DB_Database.temp.returnItem(1, 102));
                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 102)
        {

        }

        TotalConsumption += b.powerConsumption;
        myBuildings.Add(b);
        myBuildingIDs.Add(b.ID);
    }

    public int numberOfConstruction()
    {
        return numberConstruction;
    }

    public int numberOfInfantry()
    {
        return numberInfantry;
    }

    public int numberOfVehicles()
    {
        return numberVehicles;
    }

    public int numberOfAir()
    {
        return numberAir;
    }

    public List<int> getBuildings(int t)
    {
        switch (t)
        {
            case 1:
                return Available_Player_Buildings;
            case 2:
                return Available_Player_Defense;
            case 3:
                return Available_Player_Infantry;
            case 4:
                return Available_Player_Vehicles;
            case 5:
                return Available_Player_Aircraft;
            default:
                return null;
        }
    }

    public List<DB_Item> getItems(int type, int constructor)
    {
        List<List<DB_Item>> currentLists = null;

        switch (type)
        {
            case 1:
                currentLists = CurrentBuildings;
                break;
            case 2:
                currentLists = CurrentDefense;
                break;
            case 3:
                currentLists = CurrentInfantry;
                break;
            case 4:
                currentLists = CurrentVehicles;
                break;
            case 5:
                currentLists = CurrentAircraft;
                break;
            default:
                return null;
        }

        if (currentLists == null || currentLists.Count == 0 || constructor <= 0 || constructor > currentLists.Count)
        {
            return new List<DB_Item>();
        }

        return currentLists[constructor - 1];
    }

    public void removeBuilding(Building_Controller b, bool sold)
    {
        if (b.ID == 0 || b.ID == 100)
        {
            int indexToRemove = b.buildingID - 1;

            /*  foreach (DB_Item i in CurrentBuildings[indexToRemove])
              {
                  if (i.isBuilding || i.isFinished)
                  {
                      i.cancel(sold);
                  }
              }

              CurrentBuildings.RemoveAt(indexToRemove);
              CurrentDefense.RemoveAt(indexToRemove);
              FriendlyBuilding.RemoveAt(indexToRemove);
              FriendlyDefense.RemoveAt(indexToRemove);*/

            numberConstruction--;

            foreach (Building_Controller build in myBuildings)
            {
                if (build.ID == 0 || build.ID == 100)
                {
                    if (build.buildingID > b.buildingID) build.buildingID -= 1;
                }
            }

            myBuildingIDs.Remove(b.ID);
            myBuildings.Remove(b);

            if ((b.ID == 0 && !myBuildingIDs.Contains(0)) || (b.ID == 100 && !myBuildingIDs.Contains(100)))
            {
                if (b.ID == 0)
                {
                    Available_Player_Buildings.Remove(1);
                    Available_Player_Buildings.Remove(3);
                    Available_Player_Buildings.Remove(8);
                }
                else if (b.ID == 100)
                {
                    Available_AI_Buildings.Remove(101);
                }
            }
        }

        else if (b.ID == 1 || b.ID == 101)
        {
            myBuildingIDs.Remove(b.ID);
            myBuildings.Remove(b);

            if (b.ID == 1 && !myBuildingIDs.Contains(1))
            {
                Available_Player_Buildings.Remove(2);
                Available_Player_Defense.Remove(1);

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        l.RemoveAll(item => item.ID == 2);
                        l.Sort();
                    }
                }

                foreach (List<DB_Item> l in CurrentDefense)
                {
                    if (FriendlyDefense[CurrentDefense.IndexOf(l)])
                    {
                        l.RemoveAll(item => item.ID == 1);
                        l.Sort();
                    }
                }
            }

            if (b.ID == 101 && !myBuildingIDs.Contains(101))
            {
                Available_AI_Buildings.Remove(102);

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (!FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        l.RemoveAll(item => item.ID == 102);
                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 2 || b.ID == 102)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                if (myBuildingIDs.Contains(3))
                {
                }

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(3))
                        {
                        }
                        l.Sort();
                    }
                }

                foreach (List<DB_Item> l in CurrentDefense)
                {
                    if (FriendlyDefense[CurrentDefense.IndexOf(l)])
                    {

                    }
                }
            }

            foreach (int i in Available_Player_Infantry)
            {

            }
        }

        else if (b.ID == 3 || b.ID == 103)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                if (myBuildingIDs.Contains(2))
                {
                }

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(2))
                        {
                        }
                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 4)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                if (myBuildingIDs.Contains(6))
                {
                }

                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(6))
                        {
                        }

                        l.Sort();
                    }
                }
            }

            foreach (int i in Available_Player_Vehicles)
            {

            }
        }

        else if (b.ID == 5)
        {

        }

        else if (b.ID == 6)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                if (myBuildingIDs.Contains(4))
                {

                }
                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {
                        if (myBuildingIDs.Contains(4))
                        {
                        }

                        l.Sort();
                    }
                }
            }
        }

        else if (b.ID == 7)
        {

        }
        else if (b.ID == 8)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
            }

            foreach (int i in Available_Player_Aircraft)
            {

            }
        }

        else if (b.ID == 100)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {

            }

            CurrentBuildings.Add(new List<DB_Item>());
            CurrentDefense.Add(new List<DB_Item>());

            FriendlyBuilding.Add(false);
            FriendlyDefense.Add(false);

            foreach (int i in Available_AI_Buildings)
            {

            }

            CurrentBuildings[numberConstruction].Sort();

            foreach (int i in Available_AI_Defense)
            {

            }

            CurrentDefense[numberConstruction].Sort();

        }

        else if (b.ID == 101)
        {
            if (!myBuildingIDs.Contains(b.ID))
            {
                foreach (List<DB_Item> l in CurrentBuildings)
                {
                    if (!FriendlyBuilding[CurrentBuildings.IndexOf(l)])
                    {

                    }
                }
            }
        }

        else if (b.ID == 102)
        {

        }
    }

    public void removeUpdateItem(DB_Item i)
    {
        RemoveItems = true;
        ItemsToRemove.Add(i);
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

    public void addOreTile(tile t)
    {
        OreTiles.Add(t);
    }

    public void removeOreTile(tile t)
    {
        OreTiles.Remove(t);
    }

    public List<tile> getOreTiles()
    {
        return OreTiles;
    }

    public bool hasRadar()
    {
        if (myBuildingIDs.Contains(6) || myBuildingIDs.Contains(106))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
