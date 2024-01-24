using UnityEngine;

[DisallowMultipleComponent]
public class DB_Database : MonoBehaviour
{
    public static DB_Database temp;

    DB_UnitPaths ResourcePath;
    RTS_Explosions Explosions;

    DB_Item Player_CommandCentre = new DB_Item();
    public string Player_CommandCentre_Name = "Command Centre";

    DB_Item Player_PowerPlant = new DB_Item();
    public string Player_PowerPlant_Name = "Power Plant";
    public float Player_PowerPlant_Cost = 100;
    public float Player_PowerPlant_BuildTime = 2;

    DB_Item Player_Barracks = new DB_Item();
    public string Player_Barracks_Name = "Barracks";
    public float Player_Barracks_Cost = 200;
    public float Player_Barracks_BuildTime = 2;
    public int Player_Barracks_Consumption = 10;

    DB_Item Player_Refinery = new DB_Item();
    public string Player_Refinery_Name = "Refinery";
    public float Player_Refinery_Cost = 200;
    public float Player_Refinery_BuildTime = 2;
    public int Player_RefineryConsumption = 10;

    DB_Item Player_WarFactory = new DB_Item();
    public string Player_WarFactory_Name = "War Factory";
    public float Player_WarFactory_Cost = 200;
    public float Player_WarFactory_BuildTime = 2;
    public int Player_WarFactory_Consumption = 10;

    DB_Item Player_Silo = new DB_Item();
    public string Player_Silo_Name = "Silo";
    public float Player_SiloCost = 200;
    public float Player_BuildTime = 2;
    public int Player_Consumption = 10;

    DB_Item Player_Radar = new DB_Item();
    public string Player_Radar_Name = "Radar";
    public float Player_Radar_Cost = 200;
    public float Player_Radar_BuildTime = 2;
    public int Player_Radar_Consumption = 10;

    DB_Item Player_TechCentre = new DB_Item();
    public string Player_TechCentre_Name = "Tech Centre";
    public float Player_TechCentre_Cost = 200;
    public float Player_TechCentre_BuildTime = 2;
    public int Player_TechCentre_Consumption = 10;

    DB_Item Player_Helipad = new DB_Item();
    public string Player_Helipad_Name = "Helipad";
    public float Player_Helipad_Cost = 200;
    public float Player_Helipad_BuildTime = 2;
    public int Player_Helipad_Consumption = 10;

    DB_Item Player_LightDefense = new DB_Item();
    public string Player_LightDefense_Name = "Light Defense";
    public float Player_LightDefense_Cost = 200;
    public float Player_LightDefense_BuildTime = 2;
    public int Player_LightDefense_Consumption = 10;

    DB_Item Player_HeavyDefense = new DB_Item();
    public string Player_HeavyDefense_Name = "Heavy Defense";
    public float Player_HeavyDefense_Cost = 200;
    public float Player_HeavyDefense_BuildTime = 2;
    public int Player_HeavyDefense_Consumption = 10;

    DB_Item Player_Infantry00 = new DB_Item();
    public string Player_Infantry00_Name = "Soldier";
    public float Player_Infantry00_Cost = 200;
    public float Player_Infantry00_BuildTime = 2;

    DB_Item Player_Infantry01 = new DB_Item();
    public float Player_Infantry01_Cost = 200;
    public float Player_Infantry01_BuildTime = 2;

    DB_Item Player_Infantry02 = new DB_Item();
    public float Player_Infantry02_Cost = 200;
    public float Player_Infantry02_BuildTime = 2;

    DB_Item Player_Infantry03 = new DB_Item();
    public float Player_Infantry03_Cost = 200;
    public float Player_Infantry03_BuildTime = 2;

   DB_Item Player_Vehicle00 = new DB_Item();
    public float Player_Vehicle00_Cost = 200;
    public float Player_Vehicle00_BuildTime = 2;

     DB_Item Player_Vehicle01 = new DB_Item();
    public float Player_Vehicle01_Cost = 200;
    public float Player_Vehicle01_BuildTime = 2;

    DB_Item Player_Vehicle02 = new DB_Item();
    public float Player_Vehicle02_Cost = 200;
    public float Player_Vehicle02_BuildTime = 2;

     DB_Item Player_Vehicle03 = new DB_Item();
    public float Player_Vehicle03_Cost = 200;
    public float Player_Vehicle03_BuildTime = 2;

    DB_Item Player_Aircraft = new DB_Item();
    public float Player_Aircraft_Cost = 200;
    public float Player_Aircraft_BuildTime = 2;

    DB_Item AI_CommandCentre = new DB_Item();

    DB_Item AI_PowerPlant = new DB_Item();
    public float AI_PowerCost = 200;
    public float AI_PowerBuildTime = 2;

    DB_Item AI_Barracks = new DB_Item();
    public float AI_BarracksCost = 200;
    public float AI_BarracksBuildTime = 2;
    public int AI_BarracksConsumption = 10;

    DB_Item AI_Refinary = new DB_Item();
    public float AI_Refinary_Cost = 200;
    public float AI_Refinary_BuildTime = 2;
    public int AI_Refinary_Consumption = 10;

    DB_Item AI_Warfactory = new DB_Item();
    public float AI_Warfactory_Cost = 200;
    public float AI_Warfactory_BuildTime = 2;
    public int AI_Warfactory_Consumption = 10;

    DB_Item AI_Silo = new DB_Item();
    public float AI_Silo_Cost = 200;
    public float AI_Silo_BuildTime = 2;
    public int AI_Silo_Consumption = 10;

     DB_Item AI_Radar = new DB_Item();
    public float AI_Radar_Cost = 200;
    public float AI_Radar_BuildTime = 2;
    public int AI_Radar_Consumption = 10;

    DB_Item AI_TechCentre = new DB_Item();
    public float AI_TechCentre_Cost = 200;
    public float AI_TechCentre_BuildTime = 2;
    public int AI_TechCentre_Consumption = 10;

    DB_Item AI_Helipad = new DB_Item();
    public float AI_Helipad_Cost = 200;
    public float AI_Helipad_BuildTime = 2;
    public int AI_Helipad_Consumption = 10;

    DB_Item AI_Laser = new DB_Item();
    public float AI_Laser_Cost = 200;
    public float AI_LaserBuildTime = 2;
    public int AI_LaserConsumption = 10;

    DB_Item AI_TeslaCoil = new DB_Item();
    public float AI_TeslaCoil_Cost = 200;
    public float AI_TeslaCoil_BuildTime = 2;
    public int AI_TeslaCoil_Consumption = 10;

    DB_Item AI_SAM = new DB_Item();
    public float AI_SAM_Cost = 200;
    public float AI_SAM_BuildTime = 2;
    public int AI_SAM_Consumption = 10;

    DB_Item AI_Infantry00 = new DB_Item();
    DB_Item AI_Infantry01 = new DB_Item();

    DB_Item AI_Vehicle00 = new DB_Item();
    DB_Item AI_Vehicle01 = new DB_Item();
    DB_Item AI_Vehicle02 = new DB_Item();

    DB_Item AI_Aircraft = new DB_Item();

    void Start()
    {
        temp = this;

        ResourcePath = GetComponent<DB_UnitPaths>();
        Explosions = GetComponent<RTS_Explosions>();

        Init_Player();
        InitAI();
    }

    void Init_Player()
    {
        Init_Player_Buildings();
        Init_Player_Defense();
        Init_Player_Infantry();
        Init_Player_Vehicles();
        Init_Player_Aircraft();
    }

    void Init_Player_Buildings()
    {
        Init_Player_ConstructionYard();
        Init_Player_PowerPlant();
        Init_Player_Barracks();
        Init_Player_Refinery();
        Init_Player_WarFactory();
        Init_Player_Silo();
        Init_Player_Radar();
        Init_Player_TechCentre();
        Init_Player_Helipad();
    }

    void Init_Player_ConstructionYard()
    {
        Player_CommandCentre.Name = Player_CommandCentre_Name;
        Player_CommandCentre.Cost = 0;
        Player_CommandCentre.BuildTime = 0;
        Player_CommandCentre.ID = 0;
        Player_CommandCentre.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_PowerPlant()
    {
        Player_PowerPlant.Name = Player_PowerPlant_Name;
        Player_PowerPlant.Cost = Player_PowerPlant_Cost;
        Player_PowerPlant.BuildTime = Player_PowerPlant_BuildTime;
        Player_PowerPlant.ID = 1;
        Player_PowerPlant.Sprite = (Texture2D)Resources.Load("Sprites/Item Buttons/PowerPlant", typeof(Texture2D));
        Player_PowerPlant.SpriteHover = (Texture2D)Resources.Load("Sprites/Item Buttons/PowerPlantHover", typeof(Texture2D));
        Player_PowerPlant.Unit = (GameObject)Resources.Load(ResourcePath.Player_PowerPlant_Path, typeof(GameObject));
        Player_PowerPlant.isUnit = false;
        Player_PowerPlant.SortOrder = 1;
        Player_PowerPlant.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_Barracks()
    {
        Player_Barracks.Name = Player_Barracks_Name;
        Player_Barracks.Cost = Player_Barracks_Cost;
        Player_Barracks.BuildTime = Player_Barracks_BuildTime;
        Player_Barracks.ID = 2;
        Player_Barracks.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Barracks.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Barracks.Unit = (GameObject)Resources.Load(ResourcePath.Player_Barracks_Path, typeof(GameObject));
        Player_Barracks.isUnit = false;
        Player_Barracks.PowerConsumption = Player_Barracks_Consumption;
        Player_Barracks.SortOrder = 2;
        Player_Barracks.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_Refinery()
    {
        Player_Refinery.Name = Player_Refinery_Name;
        Player_Refinery.Cost = Player_Refinery_Cost;
        Player_Refinery.BuildTime = Player_Refinery_BuildTime;
        Player_Refinery.ID = 3;
        Player_Refinery.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Refinery.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Refinery.Unit = (GameObject)Resources.Load(ResourcePath.Player_Refinery_Path, typeof(GameObject));
        Player_Refinery.isUnit = false;
        Player_Refinery.PowerConsumption = Player_RefineryConsumption;
        Player_Refinery.SortOrder = 3;
        Player_Refinery.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_WarFactory()
    {
        Player_WarFactory.Name = Player_WarFactory_Name;
        Player_WarFactory.Cost = Player_WarFactory_Cost;
        Player_WarFactory.BuildTime = Player_WarFactory_BuildTime;
        Player_WarFactory.ID = 4;
        Player_WarFactory.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_WarFactory.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_WarFactory.Unit = (GameObject)Resources.Load(ResourcePath.Player_WarFactory_Path, typeof(GameObject));
        Player_WarFactory.isUnit = false;
        Player_WarFactory.PowerConsumption = Player_WarFactory_Consumption;
        Player_WarFactory.SortOrder = 5;
        Player_WarFactory.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_Silo()
    {
        Player_Silo.Name = Player_Silo_Name;
        Player_Silo.Cost = Player_SiloCost;
        Player_Silo.BuildTime = Player_BuildTime;
        Player_Silo.ID = 5;
        Player_Silo.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Silo.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Silo.Unit = (GameObject)Resources.Load(ResourcePath.Player_Silo_Path, typeof(GameObject));
        Player_Silo.isUnit = false;
        Player_Silo.PowerConsumption = Player_Consumption;
        Player_Silo.SortOrder = 4;
        Player_Silo.Explosion = Explosions.SmallExplosion;
    }

    void Init_Player_Radar()
    {
        Player_Radar.Name = Player_Radar_Name;
        Player_Radar.Cost = Player_Radar_Cost;
        Player_Radar.BuildTime = Player_Radar_BuildTime;
        Player_Radar.ID = 6;
        Player_Radar.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Radar.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Radar.Unit = (GameObject)Resources.Load(ResourcePath.Player_Radar_Path, typeof(GameObject));
        Player_Radar.PowerConsumption = Player_Radar_Consumption;
        Player_Radar.isUnit = false;
        Player_Radar.SortOrder = 6;
        Player_Radar.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_TechCentre()
    {
        Player_TechCentre.Name = Player_TechCentre_Name;
        Player_TechCentre.Cost = Player_TechCentre_Cost;
        Player_TechCentre.BuildTime = Player_TechCentre_BuildTime;
        Player_TechCentre.ID = 7;
        Player_TechCentre.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_TechCentre.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_TechCentre.Unit = (GameObject)Resources.Load(ResourcePath.Player_TechCentre_Path, typeof(GameObject)); ;
        Player_TechCentre.isUnit = false;
        Player_TechCentre.PowerConsumption = Player_TechCentre_Consumption;
        Player_TechCentre.SortOrder = 8;
        Player_TechCentre.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_Helipad()
    {
        Player_Helipad.Name = Player_Helipad_Name;
        Player_Helipad.Cost = Player_Helipad_Cost;
        Player_Helipad.BuildTime = Player_Helipad_BuildTime;
        Player_Helipad.ID = 8;
        Player_Helipad.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Helipad.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Helipad.Unit = (GameObject)Resources.Load(ResourcePath.Player_Helipad_Path, typeof(GameObject)); ;
        Player_Helipad.isUnit = false;
        Player_Helipad.PowerConsumption = Player_Helipad_Consumption;
        Player_Helipad.SortOrder = 7;
        Player_Helipad.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_Defense()
    {
        Init_Player_LightDefense();
        Init_Player_HeavyDefense();
    }

    void Init_Player_LightDefense()
    {
        Player_LightDefense.Name = Player_LightDefense_Name;
        Player_LightDefense.Cost = Player_LightDefense_Cost;
        Player_LightDefense.BuildTime = Player_LightDefense_BuildTime;
        Player_LightDefense.ID = 21;
        Player_LightDefense.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_LightDefense.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_LightDefense.Unit = (GameObject)Resources.Load(ResourcePath.Player_LightDefense_Path, typeof(GameObject));
        Player_LightDefense.isUnit = false;
        Player_LightDefense.PowerConsumption = Player_LightDefense_Consumption;
        Player_LightDefense.SortOrder = 1;
        Player_LightDefense.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_HeavyDefense()
    {
        Player_HeavyDefense.Name = Player_HeavyDefense_Name;
        Player_HeavyDefense.Cost = 200;
        Player_HeavyDefense.BuildTime = 10;
        Player_HeavyDefense.ID = 22;
        Player_HeavyDefense.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_HeavyDefense.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_HeavyDefense.Unit = (GameObject)Resources.Load(ResourcePath.Player_HeavyDefense_Path, typeof(GameObject));
        Player_HeavyDefense.isUnit = false;
        Player_HeavyDefense.PowerConsumption = 20;
        Player_HeavyDefense.SortOrder = 2;
        Player_HeavyDefense.Explosion = Explosions.MediumExplosion;
    }

    void Init_Player_Infantry()
    {
        Init_Player_Infantry00();
        Init_Player_Infantry01();
        Init_Player_Infantry02();
        Init_Player_Infantry03();
    }

    void Init_Player_Infantry00()
    {
        Player_Infantry00.Name = "Light Infantry";
        Player_Infantry00.Cost = 100;
        Player_Infantry00.BuildTime = 5;
        Player_Infantry00.ID = 31;
        Player_Infantry00.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry00.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry00.Unit = (GameObject)Resources.Load(ResourcePath.Player_Infantry00_Path, typeof(GameObject));
        Player_Infantry00.SortOrder = 1;
    }

    void Init_Player_Infantry01()
    {
        Player_Infantry01.Name = "Engineer";
        Player_Infantry01.Cost = 300;
        Player_Infantry01.BuildTime = 15;
        Player_Infantry01.ID = 32;
        Player_Infantry01.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry01.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry01.Unit = (GameObject)Resources.Load(ResourcePath.Player_Infantry01_Path, typeof(GameObject));
        Player_Infantry01.SortOrder = 2;
    }

    void Init_Player_Infantry02()
    {
        Player_Infantry02.Name = "Infantry 02";
        Player_Infantry02.Cost = 500;
        Player_Infantry02.BuildTime = 20;
        Player_Infantry02.ID = 33;
        Player_Infantry02.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry02.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry02.Unit = (GameObject)Resources.Load(ResourcePath.Player_Infantry02_Path, typeof(GameObject));
        Player_Infantry02.SortOrder = 3;
    }

    void Init_Player_Infantry03()
    {
        Player_Infantry03.Name = "Infantry 03";
        Player_Infantry03.Cost = 500;
        Player_Infantry03.BuildTime = 25;
        Player_Infantry03.ID = 34;
        Player_Infantry03.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry03.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Infantry03.Unit = (GameObject)Resources.Load(ResourcePath.Player_Infantry03_Path, typeof(GameObject));
        Player_Infantry03.SortOrder = 4;
    }

    void Init_Player_Vehicles()
    {
        Init_Player_Vehicle00();
        Init_Player_Vehicle01();
        Init_Player_Vehicle02();
        Init_Player_Vehicle03();
    }

    void Init_Player_Vehicle00()
    {
        Player_Vehicle00.Name = "Light Tank";
        Player_Vehicle00.Cost = 300;
        Player_Vehicle00.BuildTime = 15;
        Player_Vehicle00.ID = 41;
        Player_Vehicle00.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle00.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle00.Unit = (GameObject)Resources.Load(ResourcePath.Player_Vehicle00_Path, typeof(GameObject));
        Player_Vehicle00.SortOrder = 1;
    }

    void Init_Player_Vehicle01()
    {
        Player_Vehicle01.Name = "Harvester";
        Player_Vehicle01.Cost = 100;
        Player_Vehicle01.BuildTime = 2;
        Player_Vehicle01.ID = 42;
        Player_Vehicle01.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle01.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle01.Unit = (GameObject)Resources.Load(ResourcePath.Harvestor_Path, typeof(GameObject));
        Player_Vehicle01.SortOrder = 2;
    }

    void Init_Player_Vehicle02()
    {
        Player_Vehicle02.Name = "Heavy Tank";
        Player_Vehicle02.Cost = 800;
        Player_Vehicle02.BuildTime = 30;
        Player_Vehicle02.ID = 43;
        Player_Vehicle02.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle02.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle02.Unit = (GameObject)Resources.Load(ResourcePath.Player_Vehicle02_Path, typeof(GameObject));
        Player_Vehicle02.SortOrder = 4;
    }

    void Init_Player_Vehicle03()
    {
        Player_Vehicle03.Name = "Super Tank";
        Player_Vehicle03.Cost = 1200;
        Player_Vehicle03.BuildTime = 35;
        Player_Vehicle03.ID = 44;
        Player_Vehicle03.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle03.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Vehicle03.Unit = (GameObject)Resources.Load(ResourcePath.Player_Vehicle03_Path, typeof(GameObject));
        Player_Vehicle03.SortOrder = 3;
    }

    void Init_Player_Aircraft()
    {
        Player_Aircraft.Name = "Aircraft";
        Player_Aircraft.Cost = 5;
        Player_Aircraft.BuildTime = 2;
        Player_Aircraft.ID = 61;
        Player_Aircraft.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Aircraft.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        Player_Aircraft.Unit = (GameObject)Resources.Load(ResourcePath.Player_Aircraft00_Path, typeof(GameObject));
        Player_Aircraft.SortOrder = 1;
        Player_Aircraft.isAircraft = true;
    }

    void InitAI()
    {
        Init_AI_Buildings();
        Init_AI_Defense();
        Init_AI_Infantry();
        Init_AI_Vehicles();
    }

    void Init_AI_Buildings()
    {
        Init_AI_CommandCentre();
        Init_AI_PowerPlant();
        Init_AI_Barracks();
        Init_AI_Refinary();
        Init_AI_Silo();
        Init_AI_Warfactory();
        Init_AI_Radar();
    }

    void Init_AI_CommandCentre()
    {
        AI_CommandCentre.Name = "Command Centre";
        AI_CommandCentre.Cost = 0;
        AI_CommandCentre.BuildTime = 0;
        AI_CommandCentre.ID = 100;
        AI_CommandCentre.Explosion = Explosions.MediumExplosion;
    }

    void Init_AI_PowerPlant()
    {
        AI_PowerPlant.Name = "Power Plant";
        AI_PowerPlant.Cost = AI_PowerCost;
        AI_PowerPlant.BuildTime = AI_PowerBuildTime;
        AI_PowerPlant.ID = 101;
        AI_PowerPlant.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_PowerPlant.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_PowerPlant.Unit = (GameObject)Resources.Load(ResourcePath.AI_PowerPlant_Path, typeof(GameObject));
        AI_PowerPlant.isUnit = false;
        AI_PowerPlant.SortOrder = 1;
        AI_PowerPlant.Explosion = Explosions.MediumExplosion;
    }

    void Init_AI_Barracks()
    {
        AI_Barracks.Name = "Barracks";
        AI_Barracks.Cost = AI_BarracksCost;
        AI_Barracks.BuildTime = AI_BarracksBuildTime;
        AI_Barracks.ID = 102;
        AI_Barracks.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Barracks.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Barracks.Unit = (GameObject)Resources.Load(ResourcePath.AI_Barracks_Path, typeof(GameObject));
        AI_Barracks.isUnit = false;
        AI_Barracks.PowerConsumption = AI_BarracksConsumption;
        AI_Barracks.SortOrder = 2;
        AI_Barracks.Explosion = Explosions.MediumExplosion;
    }

    void Init_AI_Refinary()
    {
        AI_Refinary.Name = "Refinary";
        AI_Refinary.Cost = AI_Refinary_Cost;
        AI_Refinary.BuildTime = AI_Refinary_BuildTime;
        AI_Refinary.ID = 103;
        AI_Refinary.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Refinary.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Refinary.Unit = (GameObject)Resources.Load(ResourcePath.AI_Refinery_Path, typeof(GameObject));
        AI_Refinary.isUnit = false;
        AI_Refinary.PowerConsumption = AI_Refinary_Consumption;
        AI_Refinary.SortOrder = 3;
        AI_Refinary.Explosion = Explosions.LargeExplosion;
    }

    void Init_AI_Silo()
    {
        AI_Silo.Name = "Silo";
        AI_Silo.Cost = AI_Silo_Cost;
        AI_Silo.BuildTime = AI_Silo_BuildTime;
        AI_Silo.ID = 104;
        AI_Silo.isUnit = false;
        AI_Silo.PowerConsumption = AI_Silo_Consumption;
        AI_Silo.SortOrder = 4;
        AI_Silo.Explosion = Explosions.SmallExplosion;
    }

    void Init_AI_Warfactory()
    {
        AI_Warfactory.Name = "War Factory";
        AI_Warfactory.Cost = AI_Warfactory_Cost;
        AI_Warfactory.BuildTime = AI_Warfactory_BuildTime;
        AI_Warfactory.ID = 105;
        AI_Warfactory.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Warfactory.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Warfactory.Unit = (GameObject)Resources.Load(ResourcePath.AI_WarFactory_Path, typeof(GameObject));
        AI_Warfactory.isUnit = false;
        AI_Warfactory.PowerConsumption = AI_Warfactory_Consumption;
        AI_Warfactory.SortOrder = 5;
        AI_Warfactory.Explosion = Explosions.LargeExplosion;
    }

    void Init_AI_Radar()
    {
        AI_Radar.Name = "Radar";
        AI_Radar.Cost = AI_Radar_Cost;
        AI_Radar.BuildTime = AI_Radar_BuildTime;
        AI_Radar.ID = 106;
        AI_Radar.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Radar.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Radar.Unit = (GameObject)Resources.Load(ResourcePath.AI_Radar_Path, typeof(GameObject));
        AI_Radar.isUnit = false;
        AI_Radar.PowerConsumption = AI_Radar_Consumption;
        AI_Radar.SortOrder = 6;
        AI_Radar.Explosion = Explosions.MediumExplosion;
    }

    void Init_AI_Defense()
    {
        Init_AI_Laser();
        Init_AI_SAM();
        Init_AI_TeslaCoil();
    }

    void Init_AI_Laser()
    {
        AI_Laser.Name = "Laser";
        AI_Laser.Cost = 300;
        AI_Laser.BuildTime = 5;
        AI_Laser.ID = 120;
        AI_Laser.isUnit = false;
        AI_Laser.PowerConsumption = 15;
        AI_Laser.SortOrder = 1;
        AI_Laser.Explosion = Explosions.SmallExplosion;
    }

    void Init_AI_SAM()
    {
        AI_SAM.Name = "SAM";
        AI_SAM.Cost = 500;
        AI_SAM.BuildTime = 5;
        AI_SAM.ID = 121;
        AI_SAM.isUnit = false;
        AI_SAM.PowerConsumption = 10;
        AI_SAM.SortOrder = 2;
        AI_SAM.Explosion = Explosions.SmallExplosion;
    }

    void Init_AI_TeslaCoil()
    {
        AI_TeslaCoil.Name = "Tesla Coil";
        AI_TeslaCoil.Cost = 800;
        AI_TeslaCoil.BuildTime = 15;
        AI_TeslaCoil.ID = 122;
        AI_TeslaCoil.isUnit = false;
        AI_TeslaCoil.PowerConsumption = 20;
        AI_TeslaCoil.SortOrder = 3;
        AI_TeslaCoil.Explosion = Explosions.MediumExplosion;
    }

    void Init_AI_Infantry()
    {
        Init_AI_Infantry00();
        Init_AI_Infantry01();
    }    

    void Init_AI_Infantry00()
    {
        AI_Infantry00.Name = "Light Infantry";
        AI_Infantry00.Cost = 100;
        AI_Infantry00.BuildTime = 5;
        AI_Infantry00.ID = 131;
        AI_Infantry00.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Infantry00.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Infantry00.Unit = (GameObject)Resources.Load(ResourcePath.Player_Infantry00_Path, typeof(GameObject));
        AI_Infantry00.SortOrder = 1;
    }

    void Init_AI_Infantry01()
    {
        AI_Infantry01.Name = "Engineer";
        AI_Infantry01.Cost = 300;
        AI_Infantry01.BuildTime = 15;
        AI_Infantry01.ID = 132;
        AI_Infantry01.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Infantry01.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Infantry01.Unit = (GameObject)Resources.Load(ResourcePath.Player_Infantry01_Path, typeof(GameObject));
        AI_Infantry01.SortOrder = 2;
    }

    void Init_AI_Vehicles()
    {
        Init_AI_Vehicle00();
        Init_AI_Vehicle01();
        Init_AI_Vehicle02();
    }

    void Init_AI_Vehicle00()
    {
        AI_Vehicle00.Name = "Light Tank";
        AI_Vehicle00.Cost = 300;
        AI_Vehicle00.BuildTime = 2;
        AI_Vehicle00.ID = 141;
        AI_Vehicle00.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Vehicle00.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Vehicle00.Unit = (GameObject)Resources.Load(ResourcePath.AI_Vehicle00_Path, typeof(GameObject));
        AI_Vehicle00.SortOrder = 1;
    }

    void Init_AI_Vehicle01()
    {
        AI_Vehicle01.Name = "Harvestor";
        AI_Vehicle01.Cost = 100;
        AI_Vehicle01.BuildTime = 2;
        AI_Vehicle01.ID = 142;
        AI_Vehicle01.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Vehicle01.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Vehicle01.Unit = (GameObject)Resources.Load(ResourcePath.Harvestor_Path, typeof(GameObject));
        AI_Vehicle01.SortOrder = 2;
    }

    void Init_AI_Vehicle02()
    {
        AI_Vehicle02.Name = "Heavy Tank";
        AI_Vehicle02.Cost = 800;
        AI_Vehicle02.BuildTime = 30;
        AI_Vehicle02.ID = 143;
        AI_Vehicle02.Sprite = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Vehicle02.SpriteHover = (Texture2D)Resources.Load("", typeof(Texture2D));
        AI_Vehicle02.Unit = (GameObject)Resources.Load(ResourcePath.AI_Vehicle02_Path, typeof(GameObject));
        AI_Vehicle02.SortOrder = 4;
    }

    public DB_Item returnItem(int type, int building)
    {
        switch (type)
        {
            case 1:

                switch (building)
                {
                    case 1:
                        return new DB_Item(Player_PowerPlant);

                    case 2:
                        return new DB_Item(Player_Barracks);

                    case 3:
                        return new DB_Item(Player_Refinery);

                    case 4:
                        return new DB_Item(Player_WarFactory);

                    case 5:
                        return new DB_Item(Player_Silo);

                    case 6:
                        return new DB_Item(Player_Radar);

                    case 7:
                        return new DB_Item(Player_TechCentre);

                    case 8:
                        return new DB_Item(Player_Helipad);

                    case 101:
                        return new DB_Item(AI_PowerPlant);

                    case 102:
                        return new DB_Item(AI_Barracks);

                    case 103:
                        return new DB_Item(AI_Refinary);

                    case 104:
                        return new DB_Item(AI_Silo);

                    case 105:
                        return new DB_Item(AI_Warfactory);

                    case 106:
                        return new DB_Item(AI_Radar);

                    default:
                        return null;
                }
            case 2:

                switch (building)
                {
                    case 1:
                        return new DB_Item(Player_LightDefense);

                    case 2:
                        return new DB_Item(Player_HeavyDefense);

                    default:
                        return null;
                }

            case 3:

                switch (building)
                {
                    case 1:
                        return new DB_Item(Player_Infantry00);

                    case 2:
                        return new DB_Item(Player_Infantry01);

                    case 3:
                        return new DB_Item(Player_Infantry02);

                    case 4:
                        return new DB_Item(Player_Infantry03);

                    default:
                        return null;
                }

            case 4:

                switch (building)
                {
                    case 1:
                        return new DB_Item(Player_Vehicle00);

                    case 2:
                        return new DB_Item(Player_Vehicle01);

                    case 3:
                        return new DB_Item(Player_Vehicle02);

                    case 4:
                        return new DB_Item(Player_Vehicle03);

                    case 141:
                        return new DB_Item(AI_Vehicle00);

                    case 142:
                        return new DB_Item(AI_Vehicle01);

                    case 143:
                        return new DB_Item(AI_Vehicle02);

                    default:
                        return null;
                }

            case 5:

                switch (building)
                {
                    case 1:
                        return new DB_Item(Player_Aircraft);

                    default:
                        return null;
                }

            default:
                return null;
        }
    }
}
