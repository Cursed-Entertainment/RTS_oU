using UnityEngine;

[DisallowMultipleComponent]
public class Defense_Controller : MonoBehaviour
{
    public float Range = 10.0f;
    public float RateOfFire = 2.0f;
    public float Damage = 10.0f;
    public float BarrelSpinSpeed = 5.0f;
    public float BarrelSpinAcceleration = 0.3f;
    public float RotationSpeed = 150.0f;

    Collider[] Targets;

    public GameObject turretMount;
    public GameObject turret;

    bool LockedOn = false;
    float CurrentSpinSpeed = 0;

    MeshRenderer _MuzzleRenderer;
    public GameObject bullet;
    float FireCounter;
    float DamageCounter;
    public float MuzzleFireRate = 0.1f;

    void Start()
    {
        MuzzleFlash muzzleFlash = GetComponentInChildren<MuzzleFlash>();

        if (muzzleFlash != null)
        {
            _MuzzleRenderer = muzzleFlash.gameObject.GetComponent<MeshRenderer>();
        }

        _MuzzleRenderer.enabled = false;
    }

    void Update()
    {
        Targets = Physics.OverlapSphere(transform.position, Range, 1 << 9);

        if (Targets.Length == 0)
        {
            Targets = Physics.OverlapSphere(transform.position, Range, 1 << 13);
        }

        LockedOn = false;

        if (Targets.Length > 0)
        {
            Vector3 targetDir = Targets[0].transform.position - transform.position;
            Vector3 forwardDir = turretMount.transform.forward;

            forwardDir.y = 0;
            targetDir.y = 0;

            float angle = Vector3.Angle(forwardDir, targetDir);
            Vector3 cVec = Vector3.Cross(forwardDir, targetDir);
            if (cVec.y < 0) angle *= -1;

            if (angle < -2)
            {
                turretMount.transform.Rotate(0, -RotationSpeed * Time.deltaTime, 0);
            }
            else if (angle > 2)
            {
                turretMount.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                turretMount.transform.LookAt(new Vector3(Targets[0].transform.position.x, turretMount.transform.position.y, Targets[0].transform.position.z));
                LockedOn = true;
            }
        }

        if (LockedOn)
        {
            if (CurrentSpinSpeed < BarrelSpinSpeed) CurrentSpinSpeed += BarrelSpinAcceleration;

            turret.transform.Rotate(0, 0, CurrentSpinSpeed, Space.Self);

            _MuzzleRenderer.enabled = true;

            DamageCounter += Time.deltaTime;
            FireCounter += Time.deltaTime;

            if (FireCounter >= MuzzleFireRate)
            {
                GameObject bTemp = (GameObject)Instantiate(bullet, turret.transform.position + (turret.transform.forward * 2), transform.rotation);
                bTemp.GetComponentInChildren<BulletHit>().FiredFrom = gameObject;
                bTemp.GetComponent<BulletTrajectory>().Target = Targets[0].transform;
                FireCounter = 0;
            }

            if (DamageCounter >= 1 / RateOfFire)
            {
                if (Targets[0].gameObject.layer == 9)
                {
                    Targets[0].GetComponent<Unit_Core>().Stats.takeDamage(10);
                }
                else if (Targets[0].gameObject.layer == 13)
                {
                    Targets[0].GetComponent<Building_Controller>().takeDamage(10);
                }

                DamageCounter = 0;
            }
        }
        else
        {
            if (CurrentSpinSpeed > 0)
            {
                CurrentSpinSpeed -= BarrelSpinAcceleration / 2;
                turret.transform.Rotate(0, 0, CurrentSpinSpeed, Space.Self);
            }
            else
            {
                CurrentSpinSpeed = 0;
            }

            FireCounter = MuzzleFireRate;

            _MuzzleRenderer.enabled = false;
        }
    }
}
