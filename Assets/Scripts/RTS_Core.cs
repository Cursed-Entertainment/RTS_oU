using UnityEngine;

[DisallowMultipleComponent]
//Add To Ou-RTS Menu
[RequireComponent(typeof(RTS_Player))]
[RequireComponent(typeof(RTS_AI))]
[RequireComponent(typeof(RTS_Explosions))]
[RequireComponent(typeof(RTS_Projectiles))]
[RequireComponent(typeof(RTS_Materials))]
[RequireComponent(typeof(RTS_TeamColors))]
[RequireComponent(typeof(Input_Core))]
public class RTS_Core : MonoBehaviour
{
    [HideInInspector]
    public RTS_Explosions ExplosionController;
    [HideInInspector]
    public RTS_Player Player;
    [HideInInspector]
    public RTS_AI AI;
    [HideInInspector]
    public RTS_Projectiles Projectiles;
    [HideInInspector]
    public RTS_Materials Materials;
    [HideInInspector]
    public RTS_TeamColors Colors;
    [HideInInspector]
    public Input_Core Input;

    public int StartingMoney = 10000;
    public int StartingTechLevel = 10;

    [HideInInspector]
    public bool UseNavMesh = false;

    void Awake()
    {
        GetComponents();
    }

    void GetComponents()
    {
        Player = GetComponent<RTS_Player>();
        AI = GetComponent<RTS_AI>();
        ExplosionController = GetComponent<RTS_Explosions>();
        Projectiles = GetComponent<RTS_Projectiles>();
        Materials = GetComponent<RTS_Materials>();
        Colors = GetComponent<RTS_TeamColors>();
    }

    void LoadData()
    {

    }
}
