using UnityEngine;

[DisallowMultipleComponent]
public class Turret_TowerController : MonoBehaviour
{
    public float range = 10.0f;
    public float rateOfFire = 2.0f;
    public int damage = 10;
    public float rotationSpeed = 150.0f;

    private Collider[] targets;

    public GameObject turretMount;
    public GameObject turret;
    public Transform turretLeftFire;
    public Transform turretRightFire;

    private bool fireFromLeft = true;

    private bool lockedOn = false;

    public GameObject bullet;
    private float fireCounter;
    private float firedTime;

    void Start()
    {
        firedTime = Time.time - rateOfFire;
    }

    void Update()
    {
        targets = Physics.OverlapSphere(transform.position, range, 1 << 9);

        if (targets.Length == 0)
        {
            targets = Physics.OverlapSphere(transform.position, range, 1 << 13);
        }
        lockedOn = false;

        if (targets.Length > 0)
        {
            Vector3 targetDir = targets[0].transform.position - transform.position;
            Vector3 forwardDir = turretMount.transform.forward;

            forwardDir.y = 0;
            targetDir.y = 0;

            float angle = Vector3.Angle(forwardDir, targetDir);
            Vector3 cVec = Vector3.Cross(forwardDir, targetDir);
            if (cVec.y < 0) angle *= -1;

            if (angle < -2)
            {
                turretMount.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
            }
            else if (angle > 2)
            {
                turretMount.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                turretMount.transform.LookAt(new Vector3(targets[0].transform.position.x, turretMount.transform.position.y, targets[0].transform.position.z));
                lockedOn = true;
            }
        }

        if (lockedOn)
        {
            if (Time.time >= firedTime + rateOfFire)
            {
                GameObject bTemp;
                if (fireFromLeft)
                {
                    bTemp = (GameObject)Instantiate(bullet, turretLeftFire.position, bullet.transform.rotation);
                    fireFromLeft = false;
                }
                else
                {
                    bTemp = (GameObject)Instantiate(bullet, turretRightFire.position, bullet.transform.rotation);
                    fireFromLeft = true;
                }
                bTemp.GetComponent<ShellTrajectory>().TargetPos = targets[0].transform.position;
                bTemp.GetComponent<ShellTrajectory>().TargetTransform = targets[0].transform;
                bTemp.GetComponent<ShellTrajectory>().Damage = damage;
                firedTime = Time.time;
            }
        }
    }
}
