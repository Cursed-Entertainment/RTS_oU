using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Helipad_Controller : MonoBehaviour
{
    public int MaxAircraft = 3;

    int CurrentAircraft = 0;
    List<GameObject> Aircraft = new List<GameObject>();
    Transform[] LandingPads = new Transform[3];

    PadInfo[] PadStatus = new PadInfo[3];

    void Start()
    {
        RTS_Player.temp.Helipads.Add(this);
        InitPads();
    }

    void InitPads()
    {
        LandingPads[0] = transform.Find("pad1");
        LandingPads[1] = transform.Find("pad2");
        LandingPads[2] = transform.Find("pad3");
        PadStatus[0] = new PadInfo(0);
        PadStatus[1] = new PadInfo(1);
        PadStatus[2] = new PadInfo(2);
    }

    void OnDestroy()
    {
        RTS_Player.temp.Helipads.Remove(this);
    }

    public bool CanBuild()
    {
        if (CurrentAircraft < MaxAircraft)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddAircraft(GameObject g)
    {
        if (CurrentAircraft < MaxAircraft)
        {
            for (int i = 0; i < MaxAircraft; i++)
            {
                if (PadStatus[i].isFree())
                {
                    PadStatus[i].setInfo(g);
                    g.GetComponent<Aircraft_Movement>().setHome(gameObject, LandingPads[i].position);
                    break;
                }
            }
            CurrentAircraft++;
        }
    }

    public void RemoveAircraft(GameObject g)
    {
        for (int i = 0; i < MaxAircraft; i++)
        {
            if (PadStatus[i].getAircraft() == g)
            {
                PadStatus[i].removeInfo();
                break;
            }
        }

        CurrentAircraft--;
    }
}
