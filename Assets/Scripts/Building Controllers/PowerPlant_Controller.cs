using UnityEngine;

[DisallowMultipleComponent]
public class PowerPlantController : MonoBehaviour
{
    public int Electricty = 100;

    void Start()
    {
        RTS_Player.temp.TotalPower += Electricty;
    }

    void OnDestroy()
    {
        RTS_Player.temp.TotalPower -= Electricty;
    }
}
