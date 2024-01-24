using UnityEngine;

[DisallowMultipleComponent]
public class Unit_Stats : MonoBehaviour
{
    Unit_Core Core;

    [HideInInspector]
    public bool isAI;

    public float Health = 100;
    public float maxHealth = 100;
    public int Armour = 10;
    public float Damage = 10;
    public float Range = 10.0f;

    public bool isVehicle = true;
    public bool hasTurret = false;
    public bool isOreTruck = false;
    public bool isInfantry = false;
    public bool isEngineer = false;
    public bool isMCV = false;
    public bool isSpecial = false;
    public bool isAir = false;
    public bool Deployable = false;

    public GameObject Explosion;

    public int CurrentFaction;

    bool NavMeshControl;

    void Awake()
    {
        Core = GetComponent<Unit_Core>();
    }

    void Start()
    {
        Health = maxHealth;

        NavMeshControl = Core.GameCore.UseNavMesh;

        if (!Explosion)
        {
            Explosion = Core.GameCore.ExplosionController.MediumExplosion;
        }

        if (NavMeshControl)
        {

        }
    }

 public   void CheckHealth()
    {
        if (Health > 0)
        {
            return;
        }
        else if (Health <= 0)
        {
            UnitDestroyed();
        }
    }

    void UnitDestroyed()
    {
        Core.Deselect();

        RTS_Player.temp.Units.Remove(gameObject);

        if (!Core.Stats.isAir)//
        {
            Core.UnitMovement.resetTiles();
        }
        else
        {
            Aircraft_Movement AircraftMovement = GetComponent<Aircraft_Movement>();
            AircraftMovement.getHome().GetComponent<Helipad_Controller>().RemoveAircraft(gameObject);
        }

        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
        }

        if (!Core.Faction.isFriend() && Core.Stats.isOreTruck)//
        {
            RTS_AI.temp.removeUnit(gameObject);
        }

        if (GetComponent<ThreatInfo>())
        {
            RTS_AI.temp.removeThreatInfo(GetComponent<ThreatInfo>());
        }

        Destroy(gameObject);
    }

    public void takeDamage(int d)
    {
        Health -= (d / Armour);
        GetComponent<Selected>().updateOverlay((Health / maxHealth));
        CheckHealth();
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    void UpgradeArmour()
    {

    }
}
