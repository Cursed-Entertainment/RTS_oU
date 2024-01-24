using UnityEngine;

[DisallowMultipleComponent]
public class Vehicle_Controller : MonoBehaviour
{
    Unit_Core Core;

    public GameObject missile;
    public float shootDelay = 20.0f;
    public float damage = 300.0f;

    float shootDelayCounter = 0;
    bool hasTarget = false;
    bool hasMissile = false;
    bool facingTarget = false;
    Transform target;

    float range;

    Vector3 missilePos;
    Vector3 missileRot;

    GameObject currentMissile;

    void Awake()
    {
        Core = GetComponent<Unit_Core>();
    }

    void Start()
    {
        range = Core.Stats.Range;

        missilePos = new Vector3(0, 1.085591f, 0.5704345f);
        missileRot = new Vector3(-18.69568f, 0, 0);

        if (!missile)
        {
            if (Core.Stats.isSpecial)
            {

            }
            else if (Core.Stats.isAir)
            {

            }
            else if (Core.Stats.isInfantry)
            {

            }
            else
            {

            }
        }

        currentMissile = (GameObject)Instantiate(missile);
        currentMissile.transform.parent = transform;
        currentMissile.transform.localPosition = missilePos;
        currentMissile.transform.localRotation = Quaternion.Euler(missileRot);
        hasMissile = true;
    }

    void Update()
    {
        if (shootDelayCounter < shootDelay)
        {
            shootDelayCounter += Time.deltaTime;
        }
        else if (!hasMissile)
        {
            hasMissile = true;
            currentMissile = (GameObject)Instantiate(missile);
            currentMissile.transform.parent = transform;
            currentMissile.transform.localPosition = missilePos;
            currentMissile.transform.localRotation = Quaternion.Euler(missileRot);
        }

        facingTarget = false;
        if (target != null && hasTarget && Vector3.Distance(transform.position, target.position) <= range && !GetComponent<Unit_Movement>().Move)
        {
            float angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
            Vector3 cVec = Vector3.Cross(transform.forward, target.transform.position - transform.position);
            if (cVec.y < 0) angle *= -1;

            if (angle < -2)
            {
                transform.Rotate(new Vector3(0, -Core.UnitMovement.RotationSpeed * Time.deltaTime, 0), Space.Self);
            }
            else if (angle > 2)
            {
                transform.Rotate(new Vector3(0, Core.UnitMovement.RotationSpeed * Time.deltaTime, 0), Space.Self);
            }
            else
            {
                facingTarget = true;
            }
        }

        if (hasMissile && hasTarget && facingTarget && shootDelayCounter >= shootDelay)
        {
            shootDelayCounter = 0;
            currentMissile.GetComponent<SpecialTrajectory>().setTarget(target.position, 100);
            hasMissile = false;
        }
    }

    public void setTarget(Transform t)
    {
        hasTarget = true;
        target = t;
    }

    public void targetLost()
    {
        hasTarget = false;
        target = null;
        facingTarget = false;
    }
}
