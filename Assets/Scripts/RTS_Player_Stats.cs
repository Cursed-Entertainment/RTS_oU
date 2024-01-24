using UnityEngine;

[DisallowMultipleComponent]
public class RTS_Player_Stats : MonoBehaviour
{
    int Money;

    public int TechLevel = 10;
    public int TotalPower = 0;
    public int TotalConsumption = 0;

    public float FixRate = 10.0f;
    public float FixCostRate = 10.0f;

    void Start()
    {

    }
}
