using UnityEngine;

[DisallowMultipleComponent]
public class PadInfo
{
    public GameObject Aircraft;
    public int PadNumber;
    public bool Free = true;

    public PadInfo()
    {

    }

    public PadInfo(int pNumber)
    {
        PadNumber = pNumber;
    }

    public PadInfo(GameObject craft, int pNumber)
    {
        Aircraft = craft;
        PadNumber = pNumber;
    }

    public GameObject getAircraft()
    {
        return Aircraft;
    }

    public int getPadNumber()
    {
        return PadNumber;
    }

    public void setInfo(GameObject craft)
    {
        Aircraft = craft;
        Free = false;
    }

    public void removeInfo()
    {
        Aircraft = null;
        Free = true;
    }

    public bool isFree()
    {
        return Free;
    }
}
