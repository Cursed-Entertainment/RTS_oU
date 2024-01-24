using UnityEngine;

[DisallowMultipleComponent]
public class Refinery_Controller : MonoBehaviour
{
    public tile ForwardTile;
    public bool Occupied = false;
    public GameObject OccupiedBy;
    int OreToAdd = 0;
    public int DepositRate = 9;
    public float DepositTime = 0.05f;
    float DepositCounter = 0;
    bool Friendly;

    void Start()
    {
        if (gameObject.layer == 12)
        {
            Friendly = true;
            RTS_Player.temp.Refineries.Add(this);
        }
        else
        {
            RTS_AI.temp.Refineries.Add(this);
            Friendly = false;
        }

        ForwardTile = grid.mGrid.returnClosestTile(transform.position + (transform.forward * 7.5f));

        CreateHarvester();
    }

    void CreateHarvester()
    {
        DB_UnitPaths ResourcePath = RTS_Player.temp.gameObject.GetComponent<DB_UnitPaths>();
        GameObject Harvester = (GameObject)Instantiate((GameObject)Resources.Load(ResourcePath.Harvestor_Path, typeof(GameObject)), transform.position, transform.rotation);
        Harvester.GetComponent<Unit_Movement>().JustBeenCreated = true;
        if (!Friendly)
        {
            Harvester.GetComponent<Faction>().Friend = false;
            Harvester.GetComponent<Unit_Core>().setItem(DB_Database.temp.returnItem(4, 142));
        }
    }

    void Update()
    {
        if (Occupied)
        {
            ForwardTile.occupied = OccupiedBy;
        }

        if (OreToAdd > 0)
        {
            DepositCounter += Time.deltaTime;
            if (DepositCounter > DepositTime)
            {
                if (OreToAdd - DepositRate >= 0)
                {
                    if (Friendly)
                    {
                        RTS_Player.temp.AddMoney(DepositRate);
                    }
                    else
                    {
                        RTS_AI.temp.AddMoney(DepositRate);
                    }

                    OreToAdd -= DepositRate;
                }
                else
                {
                    if (Friendly)
                    {
                        RTS_Player.temp.AddMoney(OreToAdd);
                    }
                    else
                    {
                        RTS_AI.temp.AddMoney(OreToAdd);
                    }

                    OreToAdd = 0;
                }

                DepositCounter = 0;
            }
        }
        else
        {
            DepositCounter = 0;
        }
    }

    public void addOre(int o)
    {
        OreToAdd += o;
    }

    void OnDestroy()
    {
        if (gameObject.layer == 12)
        {
            RTS_Player.temp.Refineries.Remove(this);
        }
        else
        {
            RTS_AI.temp.Refineries.Remove(this);
        }
    }
}
