using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Unit_Movement : MonoBehaviour
{
    Unit_Core Core;

    aStar temp;
    List<Vector3> Path = new List<Vector3>();
    public bool Move = false;
    public float Speed = 5.0f;
    public float CaptureRadius = 0.5f;
    Vector3 Destination;
    int CurrentNavMesh = 1;

    tile ArrivalTile;
    tile AurrentTile;
    tile NextTile;

    bool AtartTimer = false;
    public float WaitTime = 2.0f;
    float WaitTimeCheck = 0;
    bool ChangePath = true;

    float CurrentSpeed = 0;
    public float RotationSpeed = 100.0f;
    bool MoveForward = true;
    bool DynamicObstacle = false;

    float DistanceBetweenAnglePoints;

    public bool JustBeenCreated = false;

    public bool MoveToAttack = false;
    Transform Target;

    bool stuck = false;

    void Awake()
    {
        Core = GetComponent<Unit_Core>();
    }

    void Start()
    {
        WaitTime += Random.value;
        temp = new aStar(gameObject);
        transform.position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z);
        NextTile = grid.mGrid.returnClosestTile(transform.position);
        CurrentNavMesh = grid.mGrid.returnClosestTile(transform.position).navMesh;
        SetTarget(transform.position);
        AurrentTile = grid.mGrid.returnClosestTile(transform.position);
        AurrentTile.occupied = gameObject;
    }

    void Update()
    {
        if (GetComponent<Selected>().isSelected)
        {
            for (int i = 1; i < Path.Count; i++)
            {
                Debug.DrawLine(Path[i], Path[i - 1]);
            }
        }

        if (!JustBeenCreated)
        {
            if (AurrentTile != null) AurrentTile.occupied = null;
            AurrentTile = grid.mGrid.returnClosestTile(transform.position);
            AurrentTile.occupied = gameObject;
            if (ArrivalTile != null && AurrentTile.Equals(ArrivalTile) && !MoveToAttack)
            {
                AurrentTile.occupiedStatic = true;
            }
            else
            {
                AurrentTile.occupiedStatic = false;
            }

            if (MoveToAttack && Target != null)
            {
                Destination = Target.position;
            }

            if (Move && (NextTile.occupied == null || NextTile.occupied.Equals(gameObject)))
            {
                if (!MoveToAttack)
                {
                    if (ArrivalTile != null)
                    {
                        if (ArrivalTile.status == 4)
                        {
                            ArrivalTile.objectArrival = false;
                            ArrivalTile = grid.mGrid.findClosestAvailableTile(ArrivalTile, CurrentNavMesh);
                            ArrivalTile.objectArrival = true;
                            Destination = ArrivalTile.center;
                        }
                    }
                }

                if (Core.Stats.isInfantry)
                {
                    Core.isMoving(true);
                }

                if (MoveToAttack && Target != null)
                {
                    Destination = Target.position;
                }

                if (ChangePath && AurrentTile != null && AurrentTile.status != 4 && !DynamicObstacle && Vector3.Distance(transform.position, Path[0]) < CaptureRadius)
                {
                    Path = temp.findVectorPath(Destination, transform.position, 0);

                    if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                    {
                        Move = false;
                        ArrivalTile.objectArrival = false;
                        ArrivalTile = AurrentTile;
                        if (ArrivalTile.objectArrival)
                        {
                            ArrivalTile.occupied.GetComponent<Unit_Movement>().SetTarget(grid.mGrid.findClosestAvailableTile(ArrivalTile, CurrentNavMesh).center);
                        }
                        ArrivalTile.objectArrival = true;
                    }
                }
                else if (!ChangePath && AurrentTile != null && AurrentTile.status != 4 && !DynamicObstacle && Vector3.Distance(transform.position, Path[0]) < CaptureRadius)
                {
                    Path = temp.findVectorPath(Destination, transform.position, 1);

                    if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                    {
                        Move = false;
                        ArrivalTile.objectArrival = false;
                        ArrivalTile = AurrentTile;
                        if (ArrivalTile.objectArrival)
                        {
                            ArrivalTile.occupied.GetComponent<Unit_Movement>().SetTarget(grid.mGrid.findClosestAvailableTile(ArrivalTile, CurrentNavMesh).center);
                        }
                        ArrivalTile.objectArrival = true;
                    }
                }
                else if (DynamicObstacle)
                {
                    if (Vector3.Distance(transform.position, Destination) < CaptureRadius)
                    {
                        DynamicObstacle = false;
                        ChangePath = true;
                    }
                    else if (ChangePath && Vector3.Distance(transform.position, Path[0]) < CaptureRadius)
                    {
                        DynamicObstacle = false;
                        AtartTimer = false;
                    }
                    else if (Vector3.Distance(transform.position, Path[0]) < CaptureRadius)
                    {
                        Path = temp.findVectorPath(Destination, transform.position, 2);
                        if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                        {
                        }
                    }
                }

                Vector3 targetVector = Path[0] - transform.position;
                targetVector.y = 0;
                Vector3 forwardVector = transform.forward;
                forwardVector.y = 0;
                Vector3 cVec = Vector3.Cross(forwardVector, targetVector);
                float tAngle = Vector3.Angle(targetVector, forwardVector);

                if (cVec.y < 0) tAngle *= -1;

                if (tAngle < -8)
                {
                    transform.Rotate(new Vector3(0, 1, 0), -RotationSpeed * Time.deltaTime);
                    MoveForward = false;
                }
                else if (tAngle > 8)
                {
                    transform.Rotate(new Vector3(0, 1, 0), RotationSpeed * Time.deltaTime);
                    MoveForward = false;
                }
                else
                {
                    transform.LookAt(new Vector3(Path[0].x, transform.position.y, Path[0].z));
                    MoveForward = true;
                }

                if (NextTile.i != AurrentTile.i && NextTile.j != AurrentTile.j)
                {
                    if (NextTile.i - AurrentTile.i > 0)
                    {
                        if (NextTile.j - AurrentTile.j > 0)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (NextTile.j - AurrentTile.j > 0)
                        {

                        }
                        else
                        {

                        }
                    }
                }

                if (MoveForward)
                {
                    CurrentSpeed = Mathf.Lerp(CurrentSpeed, Speed, Time.deltaTime);
                    transform.Translate(new Vector3(0, 0, CurrentSpeed * Time.deltaTime), Space.Self);
                }
                else
                {
                    if (CurrentSpeed > 0.1f)
                    {
                        CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0, Time.deltaTime * 3);
                        transform.Translate(new Vector3(0, 0, CurrentSpeed * Time.deltaTime), Space.Self);
                    }
                    else
                    {
                        CurrentSpeed = 0;
                    }
                }

                transform.position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z);

                float unitLeft = Terrain.activeTerrain.SampleHeight(transform.position - transform.right);
                float unitRight = Terrain.activeTerrain.SampleHeight(transform.position + transform.right);
                float unitForward = Terrain.activeTerrain.SampleHeight(transform.position + transform.forward);
                float unitBackward = Terrain.activeTerrain.SampleHeight(transform.position - transform.forward);

                float unitAngleZ = Mathf.Atan((unitLeft - unitRight) / 2) * Mathf.Rad2Deg;

                float unitAngleX = Mathf.Atan((unitForward - unitBackward) / 2) * Mathf.Rad2Deg;

                transform.localRotation = Quaternion.Euler(new Vector3(-unitAngleX, transform.localRotation.eulerAngles.y, -unitAngleZ));

                if (Vector3.Distance(transform.position, Destination) < CaptureRadius)
                {
                    Move = false;
                    ChangePath = true;
                    if (!ChangePath) ChangePath = true;
                }

                if (!MoveToAttack && !NextTile.Equals(ArrivalTile))
                {
                    NextTile.occupied = null;
                    NextTile = grid.mGrid.returnClosestTile(Path[0]);
                    if (NextTile.occupied == null)
                    {
                        NextTile.occupied = gameObject;
                    }
                }
                else if (MoveToAttack)
                {
                    NextTile.occupied = null;
                    NextTile = grid.mGrid.returnClosestTile(Path[0]);
                    if (NextTile.occupied == null)
                    {
                        NextTile.occupied = gameObject;
                    }
                }
            }
            else if (Move && NextTile.occupied != null && !NextTile.occupied.Equals(gameObject) && !NextTile.occupiedStatic)
            {
                if (Core.Stats.isInfantry)
                {
                    Core.isMoving(false);
                }
                if (!AtartTimer)
                {
                    WaitTimeCheck = 0;
                    AtartTimer = true;
                }

                WaitTimeCheck += Time.deltaTime;

                Vector3 targetVector = Path[0] - transform.position;
                targetVector.y = 0;
                Vector3 forwardVector = transform.forward;
                forwardVector.y = 0;
                Vector3 cVec = Vector3.Cross(forwardVector, targetVector);
                float tAngle = Vector3.Angle(targetVector, forwardVector);

                if (cVec.y < 0) tAngle *= -1;

                if (tAngle < -8)
                {
                    transform.Rotate(new Vector3(0, 1, 0), -RotationSpeed * Time.deltaTime);
                    MoveForward = false;
                }
                else if (tAngle > 8)
                {
                    transform.Rotate(new Vector3(0, 1, 0), RotationSpeed * Time.deltaTime);
                    MoveForward = false;
                }
                else
                {
                    transform.LookAt(new Vector3(Path[0].x, transform.position.y, Path[0].z), transform.up);
                    MoveForward = true;
                }

                transform.position = new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position), transform.position.z);

                float unitLeft = Terrain.activeTerrain.SampleHeight(transform.position - transform.right);
                float unitRight = Terrain.activeTerrain.SampleHeight(transform.position + transform.right);
                float unitForward = Terrain.activeTerrain.SampleHeight(transform.position + transform.forward);
                float unitBackward = Terrain.activeTerrain.SampleHeight(transform.position - transform.forward);

                float unitAngleZ = Mathf.Atan((unitLeft - unitRight) / 2) * Mathf.Rad2Deg;

                float unitAngleX = Mathf.Atan((unitForward - unitBackward) / 2) * Mathf.Rad2Deg;

                transform.localRotation = Quaternion.Euler(new Vector3(-unitAngleX, transform.localRotation.eulerAngles.y, -unitAngleZ));

                if (NextTile.occupied.GetComponent<Unit_Movement>().NextTile.occupied == gameObject)
                {
                    if (!Core.Stats.isOreTruck || (Core.Stats.isOreTruck && GetComponent<OreUnit>().getState() != 3))
                    {
                        Path = temp.findVectorPath(Destination, transform.position, 2);
                        if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                        {
                            if (Core.Stats.isOreTruck && GetComponent<OreUnit>().collectingOre())
                            {
                                Destination = grid.mGrid.findClosestAvailableTileWithOre(transform.position, CurrentNavMesh).center;
                                GetComponent<OreUnit>().updateTarget(Destination);
                            }
                            else
                            {
                                Destination = grid.mGrid.findClosestAvailableTile(transform.position, CurrentNavMesh).center;
                            }

                            Path = temp.findVectorPath(Destination, transform.position, 2);
                        }

                        DynamicObstacle = true;
                        ChangePath = false;
                        updateNextTile();
                        updateArrivalTile();
                    }
                }
                else if (Core.Stats.isOreTruck)
                {
                    if (NextTile.occupied.GetComponent<Unit_Core>().Stats.isOreTruck && NextTile.occupied.GetComponent<OreUnit>().getState() == 3 && GetComponent<OreUnit>().getState() != 3)
                    {
                        if (GetComponent<OreUnit>().findClosestFreeOreTile(ref Destination))
                        {
                            SetTarget(Destination);
                        }
                    }
                }

                if (WaitTimeCheck >= WaitTime && false)
                {

                    Path = temp.findVectorPath(Destination, transform.position, 2);

                    if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                    {
                    }
                    else
                    {
                        NextTile = grid.mGrid.returnClosestTile(Path[0]);
                        DynamicObstacle = true;
                    }

                    WaitTimeCheck = 0;
                }
            }
            else if (Move && NextTile.occupied != null && !NextTile.occupied.Equals(gameObject) && NextTile.occupiedStatic)
            {
                if (Core.Stats.isInfantry)
                {
                    Core.isMoving(false);
                }

                if (Core.Stats.isOreTruck && GetComponent<OreUnit>().getState() == 3)
                {
                    if (NextTile == ArrivalTile)
                    {

                    }
                    else
                    {
                        Path = temp.findVectorPath(Destination, transform.position, 1);

                        if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                        {
                            Move = false;
                            ArrivalTile.objectArrival = false;
                            ArrivalTile = AurrentTile;
                        }
                        else
                        {
                            ChangePath = false;
                            updateNextTile();
                        }
                    }
                }
                else
                {
                    if (NextTile.Equals(ArrivalTile))
                    {
                        Move = false;
                        ArrivalTile.objectArrival = false;
                        ArrivalTile = AurrentTile;
                        if (ArrivalTile.objectArrival)
                        {
                            ArrivalTile.occupied.GetComponent<Unit_Movement>().SetTarget(grid.mGrid.findClosestAvailableTile(ArrivalTile, CurrentNavMesh).center);
                        }
                        ArrivalTile.objectArrival = true;
                    }
                    else
                    {
                        Path = temp.findVectorPath(Destination, transform.position, 1);
                        if (Path.Count == 1 && Path[0] == grid.mGrid.returnClosestTile(transform.position).center)
                        {
                            Move = false;
                            ArrivalTile.objectArrival = false;
                            ArrivalTile = AurrentTile;
                        }
                        else
                        {
                            ChangePath = false;
                            updateNextTile();
                        }
                    }
                }
            }
            else
            {
                if (CurrentSpeed > 0.1f)
                {
                    CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0, Time.deltaTime * 10);
                    transform.Translate(new Vector3(0, 0, CurrentSpeed * Time.deltaTime), Space.Self);
                    if (Core.Stats.isInfantry)
                    {
                       Core.isMoving(true);
                    }
                }
                else
                {
                    CurrentSpeed = 0;
                    if (Core.Stats.isInfantry)
                    {
                        Core.isMoving(false);
                    }
                }
            }
        }
        else
        {
            if (grid.mGrid.returnClosestTile(transform.position).status != 4)
            {
                JustBeenCreated = false;

                if (AurrentTile != null) AurrentTile.occupied = null;
                AurrentTile = grid.mGrid.returnClosestTile(transform.position);
                AurrentTile.occupied = gameObject;

                if (Core.Stats.isInfantry)
                {
                    Core.isMoving(false);
                }

                if (Core.Stats.isOreTruck)
                {
                    GetComponent<OreUnit>().setState(1);
                }
            }
            else
            {
                transform.Translate(new Vector3(0, 0, Speed * Time.deltaTime), Space.Self);
                if (Core.Stats.isInfantry)
                {
                    Core.isMoving(true);
                }
            }
        }
    }

    public void SetTarget(Vector3 p)
    {

        Target = null;
        MoveToAttack = false;

        if (p == transform.position)
        {
            Stop();
            return;
        }

        if (ArrivalTile != null) ArrivalTile.objectArrival = false;

        if (Core.Stats.isOreTruck && GetComponent<OreUnit>().getState() == 3)
        {
            ArrivalTile = grid.mGrid.returnClosestTile(p);
        }
        else
        {
            ArrivalTile = grid.mGrid.findClosestAvailableTile(p, CurrentNavMesh);
        }


        ArrivalTile.objectArrival = true;

        Destination = ArrivalTile.center;

        if (AurrentTile.status != 4)
        {
            Path = temp.findVectorPath(Destination, transform.position, 0);
        }
        else
        {
            Path = temp.findVectorPath(Destination, grid.mGrid.findClosestAvailableTile(AurrentTile, CurrentNavMesh).center, 0);
        }

        if (NextTile != null) NextTile.occupied = null;
        NextTile = grid.mGrid.returnClosestTile(Path[0]);
        if (NextTile.occupied == null)
        {
            NextTile.occupied = gameObject;
        }

        Move = true;
        ChangePath = true;
        DynamicObstacle = false;
    }

    public void setAttackTarget(Transform t)
    {
        Target = t;
        MoveToAttack = true;

        Destination = t.position;

        if (ArrivalTile != null) ArrivalTile.objectArrival = false;

        if (AurrentTile.status != 4)
        {
            Path = temp.findVectorPath(Destination, transform.position, 0);
        }
        else
        {
            Path = temp.findVectorPath(Destination, grid.mGrid.findClosestAvailableTile(AurrentTile, CurrentNavMesh).center, 0);
        }

        if (NextTile != null) NextTile.occupied = null;
        NextTile = grid.mGrid.returnClosestTile(Path[0]);
        if (NextTile.occupied == null)
        {
            NextTile.occupied = gameObject;
        }

        Move = true;
        ChangePath = true;
        DynamicObstacle = false;
    }

    public void stopAttack()
    {
        MoveToAttack = false;
        Target = null;
    }

    public void canMove(bool m)
    {
        Move = m;
    }

    public int getNavMesh()
    {
        return CurrentNavMesh;
    }

    public tile getCurrentTile()
    {
        return AurrentTile;
    }

    public tile getNextTile()
    {
        return NextTile;
    }

    public tile getArrivalTile()
    {
        return ArrivalTile;
    }

    public void resetTiles()
    {
        AurrentTile.occupied = null;
        AurrentTile.occupiedStatic = false;
        if (ArrivalTile != null)
        {
            ArrivalTile.objectArrival = false;
        }
        if (NextTile != null && NextTile.occupied != null && NextTile.occupied.Equals(gameObject))
        {
            NextTile.occupied = null;
        }
    }

    public float getCurrentSpeed()
    {
        return CurrentSpeed;
    }

    public void Stop()
    {
        if (NextTile != null)
        {
            if (ArrivalTile != null) ArrivalTile.objectArrival = false;
            ArrivalTile = NextTile;
            Destination = NextTile.center;
            Path.Clear();
            Path.Add(NextTile.center);
            MoveToAttack = false;
        }
    }

    public bool IsMoving()
    {
        if (Path.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isMoving2()
    {
        if (ArrivalTile == AurrentTile)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void setStuck(bool s)
    {
        stuck = s;
    }

    void updateNextTile()
    {
        if (NextTile != null) NextTile.occupied = null;
        NextTile = grid.mGrid.returnClosestTile(Path[0]);
        if (NextTile.occupied == null)
        {
            NextTile.occupied = gameObject;
        }
    }

    void updateArrivalTile()
    {
        if (ArrivalTile != null) ArrivalTile.objectArrival = false;

        ArrivalTile = grid.mGrid.returnClosestTile(Destination);

        ArrivalTile.objectArrival = true;

        Destination = ArrivalTile.center;
    }

    public Transform getTarget()
    {
        return Target;
    }
}
