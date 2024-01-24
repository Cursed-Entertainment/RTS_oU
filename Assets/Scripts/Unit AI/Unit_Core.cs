using UnityEngine;

[DisallowMultipleComponent]
public class Unit_Core : MonoBehaviour
{
    public RTS_Core GameCore;

    Turret_Controller Turret;
    [HideInInspector]
    public Unit_Movement UnitMovement;
    [HideInInspector]
    public Vehicle_Controller VehicleController;
    [HideInInspector]
    public Unit_Logic Logic;
    [HideInInspector]
    public Faction Faction;
    [HideInInspector]
    public Unit_Stats Stats;

    bool MoveToAttack = false;
    private GameObject UnitToAttack;

    RaycastHit Hit;
    Collider[] ObjectsInRange;
    Vector3 TargetDestination;

    public bool canAttack = true;

    bool deploy = false;
    bool canDeploy = false;
    bool moving = false;
    bool attacking = false;
    bool facingTarget = false;

    public GameObject explosion;

    float infantryCounter = 0;

    public int setStartID;

    DB_Item Item;
    bool attackTargetSet = false;

    void Awake()
    {
        if (!GameCore)
        {
            GameCore = GameObject.FindGameObjectWithTag("manager").GetComponent<RTS_Core>();
        }

        UnitMovement = GetComponent<Unit_Movement>();
        if (!UnitMovement)
        {
            UnitMovement = gameObject.AddComponent<Unit_Movement>();
        }

        Stats = GetComponent<Unit_Stats>();
        if (!Stats)
        {
            Stats = gameObject.AddComponent<Unit_Stats>();
        }

        VehicleController = GetComponent<Vehicle_Controller>();
        Faction = GetComponent<Faction>();

        GetTurret();
    }

    void Start()
    {
        RTS_Player.temp.Units.Add(gameObject);

        if (!Faction.isFriend())
        {
            if (GetComponent<Unit_Logic>() == null)
            {
                Logic = gameObject.AddComponent<Unit_Logic>();
            }
        }

        if (Item == null)
        {
            if (Stats.isInfantry)
            {
                Item = DB_Database.temp.returnItem(3, setStartID);
            }
            else if (Stats.isVehicle)
            {
                Item = DB_Database.temp.returnItem(4, setStartID);
            }
            else if (Stats.isAir)
            {
                Item = DB_Database.temp.returnItem(5, setStartID);
            }
        }
    }

    void Update()
    {
        if (GUI_Core.temp.Drag.mouseDrag)
        {
            Vector3[] dragPos = GUI_Core.temp.Drag.dragLocations();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            if (screenPos.x > dragPos[0].x && screenPos.x < dragPos[1].x && screenPos.y > dragPos[0].y && screenPos.y < dragPos[1].y)
            {
                if (!GetComponent<Selected>().isSelected)
                {
                    Cam_CameraCore.temp.UnitSelected(gameObject);
                }
            }
            else
            {
                if (GetComponent<Selected>().isSelected)
                {
                    Cam_CameraCore.temp.UnitDeselected(gameObject);
                }
            }
        }

        if (deploy)
        {
            if (Stats.isMCV)
            {
                if (!canDeploy)
                {
                    bool directionValid = false;
                    if (UnitMovement.getCurrentSpeed() == 0)
                    {
                        Vector3 targetVector = new Vector3(0, 0, -1);
                        Vector3 forwardVector = transform.forward;
                        forwardVector.y = 0;
                        Vector3 cVec = Vector3.Cross(forwardVector, targetVector);
                        float tAngle = Vector3.Angle(targetVector, forwardVector);

                        if (cVec.y < 0) tAngle *= -1;

                        if (tAngle < -2)
                        {
                            transform.Rotate(new Vector3(0, 1, 0), -UnitMovement.RotationSpeed * Time.deltaTime);
                        }
                        else if (tAngle > 2)
                        {
                            transform.Rotate(new Vector3(0, 1, 0), UnitMovement.RotationSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.LookAt(transform.position + new Vector3(0, 0, -1));
                            directionValid = true;
                        }
                    }
                    CheckDeploymentValidity(directionValid);
                }
                else
                {
                    DeployMCV();
                }
            }
        }

        if (canAttack)
        {
            CheckRange();

            if (!MoveToAttack)
            {
                if (ObjectsInRange.Length > 0)
                {
                    attacking = true;
                    if (Stats.hasTurret && !Turret.hasTarget())
                    {
                        Turret.targetFound(ObjectsInRange[0].transform);
                    }

                    if (Stats.isSpecial && !moving)
                    {
                        VehicleController.setTarget(ObjectsInRange[0].transform);
                    }
                    else if (Stats.isSpecial)
                    {
                        VehicleController.targetLost();
                    }

                    if (Stats.isInfantry)
                    {
                        if (!moving)
                        {
                            infantryCounter += Time.deltaTime;
                            transform.LookAt(ObjectsInRange[0].transform.position);
                            facingTarget = true;
                            UnitToAttack = ObjectsInRange[0].gameObject;

                            if (infantryCounter > 0.1f)
                            {
                                DealDamage();
                                infantryCounter = 0;
                            }
                        }
                        else
                        {
                            facingTarget = false;
                            infantryCounter = 0;
                        }
                    }
                }
                else
                {
                    attacking = false;
                    facingTarget = false;

                    if (Stats.hasTurret && Turret.hasTarget())
                    {
                        Turret.targetLost();
                    }

                    if (Stats.isSpecial)
                    {
                        VehicleController.targetLost();
                    }
                }
            }
            else if (UnitToAttack != null && Vector3.Distance(transform.position, UnitToAttack.transform.position) <= Stats.Range)
            {
                UnitMovement.SetTarget(transform.position);
                attackTargetSet = false;
                attacking = true;

                if (Stats.hasTurret && !Turret.hasTarget())
                {
                    Turret.targetFound(UnitToAttack.transform);
                }

                if (Stats.isSpecial)
                {
                    VehicleController.setTarget(UnitToAttack.transform);
                }

                if (Stats.isInfantry && !moving)
                {
                    transform.LookAt(UnitToAttack.transform.position);
                    facingTarget = true;
                    infantryCounter += Time.deltaTime;
                    if (infantryCounter > 0.1f)
                    {
                        DealDamage();
                        infantryCounter = 0;
                    }
                }

                if (UnitToAttack == null)
                {
                    MoveToAttack = false;
                }
            }
            else if (UnitToAttack != null)
            {
                attacking = false;
                facingTarget = false;
                if (Stats.hasTurret)
                {
                    Turret.targetLost();
                }

                if (Stats.isSpecial)
                {
                    VehicleController.targetLost();
                }

                if (!attackTargetSet)
                {
                    UnitMovement.setAttackTarget(UnitToAttack.transform);
                    attackTargetSet = true;
                }
            }
            else
            {
                attacking = false;
                facingTarget = false;

                UnitMovement.SetTarget(transform.position);
                UnitMovement.stopAttack();

                if (Stats.hasTurret)
                {
                    Turret.targetLost();
                }
                if (Stats.isSpecial)
                {
                    VehicleController.targetLost();
                }
                MoveToAttack = false;
            }
        }

        if (Stats.isEngineer && MoveToAttack && UnitToAttack != null)
        {
            if (Vector3.Distance(transform.position, TargetDestination) < 0.5f)
            {
                bool checkFriend;
                if (gameObject.layer == 8)
                {
                    checkFriend = true;
                }
                else
                {
                    checkFriend = false;
                }
                UnitToAttack.GetComponent<Building_Controller>().takeOver(checkFriend);
                MoveToAttack = false;
            }
        }
    }

    void DealDamage()
    {
        if (UnitToAttack.layer == 9)
        {
            UnitToAttack.GetComponent<Unit_Core>().Stats.takeDamage(10);
        }
        else
        {
            UnitToAttack.GetComponent<Building_Controller>().takeDamage(10);
        }
    }

    void CheckRange()
    {
        if (Faction.isFriend())
        {
            ObjectsInRange = Physics.OverlapSphere(transform.position, Stats.Range, 1 << 9 | 1 << 13);
        }
        else
        {
            ObjectsInRange = Physics.OverlapSphere(transform.position, Stats.Range, 1 << 8 | 1 << 12);
        }
    }

    void CheckDeploymentValidity(bool directionValid)
    {
        if (directionValid)
        {
            if (deployUnit())
            {
                canDeploy = true;
            }
            else
            {
                deploy = false;
            }
        }
    }

    public void Deselect()
    {
        if (GetComponent<Selected>().isSelected)
        {
            Selection_Controller.temp.Objects.Remove(gameObject);
        }
    }

    void GetTurret()
    {
        if (Stats.hasTurret)
        {
            Turret = GetComponentInChildren<Turret_Controller>();
            if (Turret)
            {
                Turret.setProperties(Stats.Damage, Stats.Range);
            }
            else
            {

            }
        }
    }

    void DeployMCV()
    {
        DB_UnitPaths ResourcePath = RTS_Player.temp.gameObject.GetComponent<DB_UnitPaths>();
        GameObject objToPlace = (GameObject)Instantiate(Resources.Load(ResourcePath.Player_CommandCentre_Path, typeof(GameObject)), grid.mGrid.returnClosestTile(transform.position).center, Quaternion.Euler(new Vector3(0, 180, 0)));
        Building_Controller _BuildingController = objToPlace.AddComponent<Building_Controller>();
        _BuildingController.ID = 0;

        _BuildingController.materials = objToPlace.GetComponent<Renderer>().materials;
        foreach (Material m in _BuildingController.materials)
        {
            m.shader = Shader.Find("Diffuse");
        }

        _BuildingController.explosion = RTS_Player.temp.gameObject.GetComponent<RTS_Explosions>().LargeExplosion;
        objToPlace.layer = 12;
        Selection_Controller.temp.addBuilding(objToPlace);

        Selection_Controller.temp.Objects.Remove(gameObject);
        UnitMovement.resetTiles();
        RTS_Player.temp.Units.Remove(gameObject);
        Destroy(gameObject);
    }

    public void attack(GameObject g, bool attackBuilding)
    {
        MoveToAttack = true;
        UnitToAttack = g;

        if (Vector3.Distance(transform.position, g.transform.position) > Stats.Range)
        {
            if (attackBuilding)
            {
                TargetDestination = grid.mGrid.findClosestAvailableTile(g.transform.position, UnitMovement.getNavMesh()).center;
                UnitMovement.SetTarget(TargetDestination);
            }
            else
            {
                if (Stats.isEngineer)
                {
                    UnitMovement.SetTarget(UnitToAttack.transform.position);
                }
                else
                {
                    UnitMovement.setAttackTarget(UnitToAttack.transform);
                }
            }
        }
        attackTargetSet = true;
    }

    public void stopAttack()
    {
        MoveToAttack = false;
        UnitMovement.stopAttack();
    }

    public void setDestination(Vector3 t)
    {
        TargetDestination = t;
    }

    public bool canUnitDeploy()
    {
        tile currentTile = UnitMovement.getCurrentTile();

        if (Stats.isMCV)
        {
            int i = currentTile.i;
            int j = currentTile.j;

            for (int k = i - 1; k <= i + 1; k++)
            {
                for (int l = j - 1; l <= j + 1; l++)
                {
                    if (grid.mGrid.mainGrid[k, l].status == 4 || (grid.mGrid.mainGrid[k, l].occupied != null && grid.mGrid.mainGrid[k, l].occupied != gameObject) || grid.mGrid.mainGrid[k, l].hasOre)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool deployUnit()
    {
        UnitMovement.Stop();

        tile currentTile = UnitMovement.getCurrentTile();

        if (Stats.isMCV)
        {
            int i = currentTile.i;
            int j = currentTile.j;

            for (int k = i - 1; k <= i + 1; k++)
            {
                for (int l = j - 1; l <= j + 1; l++)
                {
                    if (grid.mGrid.mainGrid[k, l].status == 4 || (grid.mGrid.mainGrid[k, l].occupied != null && grid.mGrid.mainGrid[k, l].occupied != gameObject) || grid.mGrid.mainGrid[k, l].hasOre)
                    {
                        return false;
                    }
                }
            }

            deploy = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void stopDeploy()
    {
        deploy = false;
    }

    public void isMoving(bool m)
    {
        moving = m;
    }

    public bool isMoving()
    {
        if (Stats.isAir)
        {
            return GetComponent<Aircraft_Movement>().isFlying();
        }
        else
        {
            return UnitMovement.isMoving2();
        }
    }

    public void getInfantryStats(ref bool moving, ref bool shooting, ref bool facingTarget)
    {
        moving = this.moving;
        shooting = attacking;
        facingTarget = this.facingTarget;
    }

    public GameObject getUnitToAttack()
    {
        return UnitToAttack;
    }

    public bool isAttacking()
    {
        return MoveToAttack;
    }

    public void setItem(DB_Item i)
    {
        Item = i;
    }

    public DB_Item getItem()
    {
        return Item;
    }

    public Collider[] getObjectsInRange()
    {
        return ObjectsInRange;
    }
}
