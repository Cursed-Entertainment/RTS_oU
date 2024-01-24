using UnityEngine;
using System;

[DisallowMultipleComponent]
public class DB_Item : IComparable<DB_Item>
{
    public bool isBuilding = false;
    public bool isFinished = false;
    public float RemainingTime;
    public string Name;
    public float BuildTime;
    public float Cost;
    public int ID;
    public Texture2D Sprite;
    public Texture2D SpriteHover;
    public GameObject Unit;
    public bool isUnit = true;
    public GameObject Parent;
    public int TechLevel;
    public int PowerConsumption = 0;
    public int Health = 100;
    public int SortOrder = 0;
    public bool isAircraft = false;
    public GameObject Explosion;
    public bool BuiltByAI = false;

    float CostSoFar = 0;
    float CostPerSecond;
    bool Ready = false;
    bool Paused = false;

    public DB_Item()
    {

    }

    public DB_Item(DB_Item i)
    {
        Name = i.Name;
        BuildTime = i.BuildTime;
        Cost = i.Cost;
        ID = i.ID;
        Sprite = i.Sprite;
        SpriteHover = i.SpriteHover;
        Unit = i.Unit;
        isUnit = i.isUnit;
        PowerConsumption = i.PowerConsumption;
        TechLevel = i.TechLevel;
        CostPerSecond = Cost / BuildTime;
        SortOrder = i.SortOrder;
        isAircraft = i.isAircraft;
        Explosion = i.Explosion;
    }

    public void startBuilding()
    {
        RemainingTime = BuildTime;
        isBuilding = true;
        if (BuiltByAI)
        {
            RTS_AI.temp.ItemsToUpdate.Add(this);
        }
        else
        {
            RTS_Player.temp.ItemsToUpdate.Add(this);
        }
    }

    public void finishBuild()
    {
        isBuilding = false;
        isFinished = false;
        Ready = false;
        CostSoFar = 0;
    }

    public void updateItem()
    {
        int tMoney;
        if (BuiltByAI)
        {
            tMoney = RTS_AI.temp.getMoney();
        }
        else
        {
            tMoney = RTS_Player.temp.getMoney();
        }
        if (CostSoFar < Cost && tMoney - (CostPerSecond * Time.deltaTime) > 0 && !Paused)
        {
            RemainingTime -= Time.deltaTime;

            if (BuiltByAI)
            {
                RTS_AI.temp.AddMoney((int)CostSoFar);
            }
            else
            {
                RTS_Player.temp.AddMoney((int)CostSoFar);
            }

            CostSoFar += CostPerSecond * Time.deltaTime;

            if (CostSoFar > Cost)
            {
                CostSoFar = Cost;
                Ready = true;
            }

            if (BuiltByAI)
            {
                RTS_AI.temp.costMoney((int)CostSoFar);
            }
            else
            {
                RTS_Player.temp.costMoney((int)CostSoFar);
            }
        }
        else if (Ready)
        {
            isFinished = true;
            if (BuiltByAI)
            {
                RTS_AI.temp.removeUpdateItem(this);
            }
            else
            {
                RTS_Player.temp.removeUpdateItem(this);
            }
            if (isUnit)
            {
                finishBuild();
                GameObject gTemp = (GameObject)UnityEngine.Object.Instantiate(Unit, Parent.transform.position, Parent.transform.rotation);

                if (BuiltByAI)
                {
                    gTemp.GetComponent<Faction>().Friend = false;
                }
                else
                {
                    gTemp.GetComponent<Faction>().Friend = true;
                }

                if (!isAircraft) gTemp.GetComponent<Unit_Movement>().JustBeenCreated = true;
                if (isAircraft)
                {
                    Parent.GetComponent<Helipad_Controller>().AddAircraft(gTemp);
                }
                if (!BuiltByAI) RTS_VoiceController.temp.playVoice("unitReady");

                gTemp.GetComponent<Unit_Core>().setItem(this);
            }
            else
            {
                if (!BuiltByAI) RTS_VoiceController.temp.playVoice("constructionComplete");
            }
        }
    }

    public void cancel(bool recieveMoney)
    {
        if (BuiltByAI)
        {
            RTS_AI.temp.ItemsToUpdate.Remove(this);
        }
        else
        {
            RTS_Player.temp.ItemsToUpdate.Remove(this);
        }

        if (recieveMoney)
        {
            if (BuiltByAI)
            {
                RTS_AI.temp.AddMoney((int)CostSoFar);
            }
            else
            {
                RTS_Player.temp.AddMoney((int)CostSoFar);
            }
        }

        finishBuild();
        CostSoFar = 0;
        Paused = false;
    }

    public void pause()
    {
        Paused = true;
    }

    public void carryOn()
    {
        Paused = false;
    }

    public int CompareTo(DB_Item b)
    {
        return SortOrder.CompareTo(b.SortOrder);
    }

    public float getRatio()
    {
        return CostSoFar / Cost;
    }

    public bool isPaused()
    {
        return Paused;
    }
}
