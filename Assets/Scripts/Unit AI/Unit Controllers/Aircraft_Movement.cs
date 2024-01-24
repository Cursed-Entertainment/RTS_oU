using UnityEngine;

public class Aircraft_Movement : MonoBehaviour
{
    public float cruisingAltitude = 4.0f;
    public float firingRange = 10.0f;
    public int maxAmmo = 2;
    private int currentAmmo;
    public float reloadSpeed = 5.0f;
    public float speed = 10.0f;
    public float takeOffSpeed = 10.0f;
    public float acceleration = 4.0f;
    public float bankAngle = 5;
    public float bankAcceleration = 1.0f;
    public float bankForce = 100.0f;
    public float landingAngle = 15.0f;
    public float altitudeRateOfChange = 4.0f;

    public float captureRadius = 0.5f;
    public float homeCaptureRadius = 8.0f;

    private float currentSpeed = 0;
    private float currentBankAngle = 0;
    private float currentLandingAngle = 0;

    private bool attacking = false;
    private bool hasAmmo = true;
    private bool hasTarget = false;
    private bool atHome = true;
    private bool takeOff = false;
    private bool landing = false;
    private bool flying = false;

    private Vector3 target;
    private Vector3 targetDistanceCheck;
    private GameObject home;
    private Vector3 targetHome;

    private GameObject[] vapourTrails = new GameObject[2];
    private Transform[] fans = new Transform[2];
    private float fanSpeed = 0;

    public bool hasFans = true;

    public float maxFanSpeed = 20;
    public float fanAcceleration = 10;

    private GameObject targetToAttack;

    private float currentAltitude;
    private float targetAltitude;
    private float timer = 0;
    private float timeToRotate;

    private float approachSpeed;

    private float angleToTarget;
    private Vector3 targetDir;

    private TrailRenderer[] trails;
    private ParticleRenderer engine;
    private float[] emitSpeed = new float[2];

    private float bankRadius;
    private float stuckTimer = 0;
    private bool checkStuck = false;

    public float bombPosX = 1, bombPosZ = 1;

    private GameObject[] bombs = new GameObject[2];

    private float reloadTimer = 0;

    private bool returnAttack = false;
    private float returnTimer = 0;
    public float returnAttackDelay = 2;


    void Start()
    {
        currentAmmo = maxAmmo;
        approachSpeed = speed * 0.15f;
        trails = GetComponentsInChildren<TrailRenderer>();
        engine = GetComponentInChildren<ParticleRenderer>();
        if (hasFans)
        {
            fans[0] = transform.Find("FanL");
            fans[1] = transform.Find("FanR");
        }

        bankRadius = 360 / bankForce;

        emitSpeed[0] = engine.GetComponent<ParticleEmitter>().minEmission;
        emitSpeed[1] = engine.GetComponent<ParticleEmitter>().maxEmission;

        engine.GetComponent<ParticleEmitter>().minEmission = 0;
        engine.GetComponent<ParticleEmitter>().maxEmission = 0;

        loadMissile(0);
        loadMissile(1);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (checkStuck) stuckTimer += Time.deltaTime;

        if (GetComponent<Selected>().isSelected)
        {
            if (Input.GetKeyDown("s"))
            {
                stop();
            }
        }

        if (flying)
        {
            foreach (TrailRenderer t in trails)
            {
                t.enabled = true;
            }

            if (engine.GetComponent<ParticleEmitter>().minEmission < emitSpeed[0])
            {
                engine.GetComponent<ParticleEmitter>().minEmission += 4;
                engine.GetComponent<ParticleEmitter>().maxEmission += 4;
            }
            else
            {
                engine.GetComponent<ParticleEmitter>().minEmission = emitSpeed[0];
                engine.GetComponent<ParticleEmitter>().maxEmission = emitSpeed[1];
            }

        }
        else
        {
            foreach (TrailRenderer t in trails)
            {
                t.enabled = false;
            }

            engine.GetComponent<ParticleEmitter>().minEmission -= 1;
            engine.GetComponent<ParticleEmitter>().maxEmission -= 1;

            if (engine.GetComponent<ParticleEmitter>().minEmission < 0.5f)
            {
                engine.GetComponent<ParticleEmitter>().minEmission = 0;
                engine.GetComponent<ParticleEmitter>().maxEmission = 0;
            }
        }

        if (hasFans && atHome)
        {
            fanSpeed -= fanAcceleration * Time.deltaTime;
            if (fanSpeed < 0)
            {
                fanSpeed = 0;
            }

            fans[0].Rotate(new Vector3(0, fanSpeed, 0));
            fans[1].Rotate(new Vector3(0, -fanSpeed, 0));
        }
        else if (hasFans)
        {
            fanSpeed += fanAcceleration * Time.deltaTime;
            if (fanSpeed > maxFanSpeed)
            {
                fanSpeed = maxFanSpeed;
            }

            fans[0].Rotate(new Vector3(0, fanSpeed, 0));
            fans[1].Rotate(new Vector3(0, -fanSpeed, 0));
        }

        currentAltitude = transform.position.y;
        targetAltitude = Mathf.Max(Terrain.activeTerrain.SampleHeight(transform.position) + cruisingAltitude, grid.mGrid.waterLevel + cruisingAltitude);

        if (atHome)
        {
            if (currentAmmo < maxAmmo)
            {
                reloadTimer += Time.deltaTime;

                if (reloadTimer >= reloadSpeed)
                {
                    reloadTimer = 0;

                    loadMissile(currentAmmo);
                    GetComponent<Selected>().addAmmo(currentAmmo);
                    currentAmmo++;

                }
            }

            if (hasTarget)
            {
                takeOff = true;
                atHome = false;
                gameObject.layer = 14;
            }

            currentSpeed = 0;
        }
        else if (takeOff && fanSpeed > maxFanSpeed * 0.9f)
        {
            if (currentAltitude < targetAltitude)
            {
                transform.Translate(new Vector3(0, takeOffSpeed * Time.deltaTime, 0), Space.World);
            }
            else
            {
                takeOff = false;
                flying = true;
            }
        }
        else if (landing)
        {
            if (hasTarget)
            {
                landing = false;
                takeOff = true;
            }

            targetDir = targetHome - transform.position;
            targetDir.y = 0;

            Vector3 forwardDir = transform.forward;
            forwardDir.y = 0;

            angleToTarget = Vector3.Angle(forwardDir, targetDir);
            if (Vector3.Cross(forwardDir, targetDir).y < 0) angleToTarget *= -1;

            if (angleToTarget > -2 && angleToTarget < 2)
            {

                if (currentSpeed > approachSpeed)
                {
                    currentSpeed -= acceleration * Time.deltaTime * 1.5f;
                }

                transform.LookAt(new Vector3(targetHome.x, transform.position.y, targetHome.z));

                if (Vector3.Distance(transform.position, new Vector3(targetHome.x, transform.position.y, targetHome.z)) < captureRadius)
                {
                    transform.Translate(new Vector3(0, -takeOffSpeed * Time.deltaTime, 0), Space.Self);
                    transform.position = Vector3.Lerp(transform.position, new Vector3(targetHome.x, transform.position.y, targetHome.z), Time.deltaTime * 4);

                    if (Vector3.Distance(transform.position, targetHome) < captureRadius / 2)
                    {
                        landing = false;
                        atHome = true;
                        gameObject.layer = 8;
                    }
                }
                else
                {
                    transform.Translate(new Vector3(0, 0, currentSpeed * Time.deltaTime), Space.Self);
                }
            }
            else
            {
                setTarget(transform.position + (transform.forward * 15), false);
                landing = false;
                flying = true;
            }
        }
        else if (flying)
        {
            if (returnAttack && targetToAttack != null)
            {
                returnTimer += Time.deltaTime;

                if (returnTimer > returnAttackDelay)
                {
                    setTarget(targetToAttack, true);
                    returnAttack = false;
                }
            }
            else if (returnAttack)
            {
                target = targetHome;
                attacking = false;
                hasTarget = false;
                returnAttack = false;
            }


            if (hasTarget && !attacking)
            {

                Vector3 tTemp = new Vector3(target.x, transform.position.y, target.z);
                if (Vector3.Distance(transform.position, tTemp) <= captureRadius)
                {
                    hasTarget = false;
                }
            }
            else if (hasTarget && attacking)
            {
                if (targetToAttack == null)
                {
                    target = targetHome;
                    attacking = false;
                    hasTarget = false;
                }
                else
                {
                    target = targetToAttack.transform.position;
                    Vector3 tTemp = new Vector3(target.x, transform.position.y, target.z);
                    if (Vector3.Distance(transform.position, tTemp) <= firingRange)
                    {
                        dropBombs();
                        if (currentAmmo > 0)
                        {
                            returnAttack = true;
                            returnTimer = 0;
                            setTarget(transform.position + (transform.forward * 150), false, true);
                        }
                        else
                        {
                            attacking = false;
                            hasTarget = false;
                            returnAttack = false;
                        }
                    }
                }
            }
            else
            {
                target = targetHome;
                Vector3 tTemp = new Vector3(targetHome.x, transform.position.y, targetHome.z);
                if (Vector3.Distance(transform.position, tTemp) <= homeCaptureRadius)
                {
                    landing = true;
                    flying = false;
                }
            }

            if (currentSpeed < speed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }

            if (hasTarget)
            {
                targetDir = target - transform.position;
                targetDir.y = 0;
                angleToTarget = Vector3.Angle(transform.forward, targetDir);
                if (Vector3.Cross(transform.forward, targetDir).y < 0) angleToTarget *= -1;
            }
            else
            {
                targetDir = targetHome - transform.position;
                targetDir.y = 0;
                angleToTarget = Vector3.Angle(transform.forward, targetDir);
                if (Vector3.Cross(transform.forward, targetDir).y < 0) angleToTarget *= -1;
            }

            checkStuck = false;
            if (angleToTarget < -2)
            {
                if (currentBankAngle < bankAngle) currentBankAngle += bankAcceleration * Time.deltaTime;
                transform.Rotate(new Vector3(0, -bankForce * Time.deltaTime, 0), Space.World);
                checkStuck = true;
            }
            else if (angleToTarget > 2)
            {
                if (currentBankAngle > -bankAngle) currentBankAngle -= bankAcceleration * Time.deltaTime;
                transform.Rotate(new Vector3(0, bankForce * Time.deltaTime, 0), Space.World);
                checkStuck = true;
            }
            else
            {
                if (currentBankAngle < 0)
                {
                    currentBankAngle += bankAcceleration * Time.deltaTime;
                }
                else if (currentBankAngle > 0)
                {
                    currentBankAngle -= bankAcceleration * Time.deltaTime;
                }

                transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
            }

            if (checkStuck)
            {
                if (stuckTimer > 3)
                {
                    setTarget(transform.position + (transform.forward * 15), false);
                    stuckTimer = 0;

                }
            }
            else
            {
                stuckTimer = 0;
            }

            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, targetAltitude, transform.position.z), Time.deltaTime * altitudeRateOfChange);


            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, currentBankAngle);

            transform.Translate(new Vector3(0, 0, currentSpeed * Time.deltaTime), Space.Self);
        }

    }

    public void setTarget(Vector3 t, bool a)
    {
        target = t;
        attacking = a;
        hasTarget = true;
        returnAttack = false;
    }

    public void setTarget(Vector3 t, bool a, bool r)
    {
        target = t;
        attacking = a;
        hasTarget = true;
    }

    public void setTarget(GameObject t, bool a)
    {
        if (currentAmmo > 0)
        {
            target = t.transform.position;
            targetToAttack = t;
            attacking = a;
            hasTarget = true;
            returnAttack = false;
        }
    }

    public void stop()
    {
        hasTarget = false;
        attacking = false;
        target = home.transform.position;
    }

    public void setHome(GameObject g, Vector3 tH)
    {
        home = g;
        targetHome = tH;
        transform.position = tH;
    }

    public GameObject getHome()
    {
        return home;
    }

    private void calculateTimeLeft(bool takeOff)
    {
        timer = 0;
        float dist;
        if (takeOff)
        {
            dist = targetAltitude - currentAltitude;
        }
        else
        {
            dist = transform.position.y - home.transform.position.y;
        }

        timeToRotate = (dist / takeOffSpeed) * 0.9f;
    }

    private void dropBombs()
    {
        bombs[currentAmmo - 1].GetComponent<MissileTrajectory>().fire(targetToAttack.transform.position);
        currentAmmo--;
        GetComponent<Selected>().removeAmmo(currentAmmo);
    }

    void OnDestroy()
    {

    }

    private void loadMissile(int n)
    {
        GameObject bTemp = (GameObject)Instantiate(Resources.Load("Projectiles/Aircraft Bomb", typeof(GameObject)), transform.position, transform.rotation);
        bTemp.transform.parent = transform;
        bombs[n] = bTemp;

        if (n == 0)
        {
            bTemp.transform.Translate(new Vector3(bombPosX, 0, -bombPosZ), Space.Self);
        }
        else
        {
            bTemp.transform.Translate(new Vector3(-bombPosX, 0, -bombPosZ), Space.Self);
        }
    }

    public bool isFlying()
    {
        return !atHome;
    }

    public bool isAttacking()
    {
        return attacking;
    }

    public Vector3 getTarget()
    {
        return target;
    }

    public GameObject getTargetUnit()
    {
        return targetToAttack;
    }
}
