using UnityEngine;

[DisallowMultipleComponent]
public class OreUnit : MonoBehaviour
{
    private tile targetTile;

    public int maxOre = 700;
    private int currentOre = 0;
    public int depositRate = 5;

    private bool atRefinery = false;

    private float timer = 0;

    private bool depositingOre = false;

    public float rotateSpeed = 100.0f;
    public float reverseSpeed = 4.0f;
    public float oreCollectionRate = 0.05f;

    private enum DepositState
    {
        faceForward,
        deposit,
    };

    private enum State
    {
        travel,
        collect,
        travelToRefinery,
        depositOre,
        nothing,
    };

    private Refinery_Controller targetRefinery;

    private DepositState depositState = DepositState.faceForward;
    private State state = State.nothing;

    private bool collectOre = true;
    private bool targetSet = false;

    private bool checkTarget = false;

    private ParticleSystem dustEmitter;

    private Vector3 checkTargetPosition;

    public bool alreadyCreated = false;

    public GameObject oreLevel;
    private float oreYval;
    private float oreXval;
    private float oreZval;

    void Start()
    {
        dustEmitter = GetComponentInChildren<ParticleSystem>();
        if (dustEmitter)
        {
            dustEmitter.Stop();
            dustEmitter.Clear();
        }

        GetComponent<Selected>().emptyAmmo();

        if (oreLevel)
        {
            oreYval = oreLevel.transform.localPosition.y;
            oreXval = oreLevel.transform.localPosition.x;
            oreZval = oreLevel.transform.localPosition.z;
        }

    }

    void Update()
    {
        if (oreLevel)
        {
            float oreRatio = (float)currentOre / (float)maxOre;
            if (oreRatio > 0.01f)
            {
                oreLevel.transform.localPosition = new Vector3(oreXval, oreYval + (oreRatio * 0.3f), oreZval);
                oreLevel.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                oreLevel.GetComponent<Renderer>().enabled = false;
            }
        }

        if (state == State.travel)
        {
            if (!targetSet && collectOre)
            {
                if (!findClosestOreTile())
                {
                    collectOre = false;
                }
                else
                {
                    GetComponent<Unit_Movement>().SetTarget(targetTile.center);
                }

                targetSet = true;
            }
            else if (!targetSet && !collectOre)
            {
                if (targetTile != null)
                {
                    GetComponent<Unit_Movement>().SetTarget(targetTile.center);
                }
                targetSet = true;
            }

            if (checkTarget)
            {
                setTarget(checkTargetPosition);
                checkTarget = false;
            }

            if (Vector3.Distance(transform.position, targetTile.center) < 0.5f)
            {
                if (targetTile != null && targetTile.hasOre && collectOre)
                {
                    state = State.collect;
                }
            }
        }
        else if (state == State.collect)
        {
            int boxes = (int)(((float)currentOre / (float)maxOre) * 5);
            boxes -= 1;
            GetComponent<Selected>().addAmmo(boxes);

            if (currentOre >= maxOre)
            {
                state = State.travelToRefinery;
                targetSet = false;
                return;
            }

            if (targetTile != null && targetTile.hasOre && collectOre)
            {
                timer += Time.deltaTime;
                if (timer > oreCollectionRate)
                {
                    timer = 0;
                    targetTile.tOre.OreLeft -= 1;
                    currentOre += 5;

                    if (targetTile.tOre.OreLeft == 0)
                    {
                        targetSet = false;
                        targetTile = null;

                        if (findClosestOreTile())
                        {
                            state = State.travel;
                            targetSet = false;
                        }
                        else
                        {
                            state = State.travelToRefinery;
                            targetSet = false;
                        }
                    }
                }
            }
        }
        else if (state == State.travelToRefinery)
        {
            if (!targetSet)
            {
                if (findClosestRefinery())
                {
                    targetSet = true;
                    GetComponent<Unit_Movement>().SetTarget(targetTile.center);
                }
                else
                {
                    state = State.nothing;
                }
            }

            if (findClosestRefinery() && Vector3.Distance(transform.position, targetTile.center) < 0.3f)
            {
                state = State.depositOre;
                targetSet = false;
            }
        }
        else if (state == State.depositOre)
        {
            targetRefinery.Occupied = true;
            targetRefinery.OccupiedBy = gameObject;

            if (depositState == DepositState.faceForward)
            {
                if (transform.rotation.eulerAngles.y >= 0 && transform.rotation.eulerAngles.y < 172)
                {
                    transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
                }
                else if (transform.rotation.eulerAngles.y > 182)
                {
                    transform.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0));
                }
                else if (true)
                {
                    transform.LookAt(transform.position + new Vector3(0, 0, -1));
                    depositState = DepositState.deposit;
                }
            }
            else if (depositState == DepositState.deposit)
            {
                depositOre();
            }
        }
        else if (state == State.nothing)
        {
        }
    }

    public void depositOre()
    {
        GetComponent<Selected>().emptyAmmo();

        if (!depositingOre)
        {
            depositingOre = true;
            targetRefinery.addOre(currentOre);
            currentOre = 0;
        }

        else
        {
            depositingOre = false;
            depositState = DepositState.faceForward;
            targetRefinery.Occupied = false;
            state = State.travel;
        }
    }

    public bool AtRefinery()
    {
        return atRefinery;
    }

    public void falseTarget()
    {
        targetSet = false;
    }

    private bool findClosestOreTile()
    {
        if (RTS_Player.temp.getOreTiles().Count == 0) return false;

        float cDist = 10000;
        float tempDist;
        foreach (tile t in RTS_Player.temp.getOreTiles())
        {
            tempDist = Vector3.Distance(transform.position, t.center);
            if (tempDist < cDist)
            {
                if (!t.objectArrival)
                {
                    cDist = tempDist;
                    targetTile = t;
                    targetSet = true;
                }
            }
        }

        return true;
    }

    public bool findClosestFreeOreTile(ref Vector3 pos)
    {
        if (RTS_Player.temp.getOreTiles().Count == 0) return false;

        float cDist = 10000;
        float tempDist;
        foreach (tile t in RTS_Player.temp.getOreTiles())
        {
            tempDist = Vector3.Distance(transform.position, t.center);
            if (tempDist < cDist)
            {
                if (!t.objectArrival && t.occupied == null)
                {
                    cDist = tempDist;
                    targetTile = t;
                    targetSet = true;
                    pos = targetTile.center;
                }
            }
        }

        return true;
    }

    private bool findClosestRefinery()
    {
        bool friendly = GetComponent<Faction>().isFriend();

        if (friendly)
        {
            if (RTS_Player.temp.Refineries.Count == 0) return false;
        }
        else
        {
            if (RTS_AI.temp.Refineries.Count == 0) return false;
        }


        float cDist = 10000;
        float tempDist;
        if (friendly)
        {
            foreach (Refinery_Controller r in RTS_Player.temp.Refineries)
            {
                tempDist = Vector3.Distance(transform.position, r.transform.position);
                if (tempDist < cDist && r.ForwardTile.status != 4)
                {
                    cDist = tempDist;
                    targetTile = r.ForwardTile;
                    targetRefinery = r;
                }
            }
        }
        else
        {
            foreach (Refinery_Controller r in RTS_AI.temp.Refineries)
            {
                tempDist = Vector3.Distance(transform.position, r.transform.position);
                if (tempDist < cDist && r.ForwardTile.status != 4)
                {
                    cDist = tempDist;
                    targetTile = r.ForwardTile;
                    targetRefinery = r;
                }
            }
        }


        return true;
    }

    public void setState(int s)
    {
        if (s == 1)
        {
            state = State.travel;
        }
        else if (s == 2)
        {
            state = State.collect;
        }
        else if (s == 3)
        {
            state = State.travelToRefinery;
        }
        else if (s == 4)
        {
            state = State.depositOre;
        }
        else
        {
            state = State.nothing;
        }
    }

    public int getState()
    {
        if (state == State.travel)
        {
            return 1;
        }
        else if (state == State.collect)
        {
            return 2;
        }
        else if (state == State.travelToRefinery)
        {
            return 3;
        }
        else if (state == State.depositOre)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    public void setTarget(Vector3 pos)
    {
        targetSet = true;

        if (state == State.depositOre && depositState == DepositState.deposit)
        {
            checkTarget = true;
            checkTargetPosition = pos;
            return;
        }

        GetComponent<Unit_Movement>().SetTarget(pos);
        state = State.travel;
        targetTile = GetComponent<Unit_Movement>().getArrivalTile();

        if (targetTile.hasOre)
        {
            collectOre = true;
        }
        else
        {
            collectOre = false;
        }
    }

    public bool collectingOre()
    {
        return collectOre;
    }

    public void updateTarget(Vector3 pos)
    {
        targetSet = true;
        targetTile = GetComponent<Unit_Movement>().getArrivalTile();

        if (targetTile.hasOre)
        {
            collectOre = true;
        }
        else
        {
            collectOre = false;
        }
    }

    public void goToRefinery(Refinery_Controller r)
    {
        state = State.travelToRefinery;

        if (r)
        {
            targetRefinery = r;
            targetTile = r.ForwardTile;
            GetComponent<Unit_Movement>().SetTarget(targetTile.center);
            targetSet = true;
        }
        else
        {
            targetSet = false;
        }
    }
}