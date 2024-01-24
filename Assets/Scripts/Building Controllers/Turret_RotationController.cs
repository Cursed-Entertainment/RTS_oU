using UnityEngine;

[DisallowMultipleComponent]
public class Turret_RotationController : MonoBehaviour
{
    public float range = 10.0f;
    public float rateOfFire = 2.0f;
    public float damage = 10.0f;
    public float rotationSpeed = 150.0f;

    Collider[] targets;

    public GameObject turretMount;
    public GameObject turret;
    public Transform turretLeftFire;
    public Transform turretRightFire;

    bool lockedOn = false;

    public GameObject bullet;
    float fireCounter;

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
    }
}