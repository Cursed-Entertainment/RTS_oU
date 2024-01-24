using UnityEngine;

[DisallowMultipleComponent]
public class SAM_Controller : MonoBehaviour
{
    public bool friendly = false;
    private Collider[] targets;

    public float range = 10.0f;
    public float rateOfFire = 2.0f;
    public float rotationSpeed = 4.0f;
    private float fireCounter = 0;
    public GameObject missile;
    public GameObject turret;
    private bool lockedOn = false;
    public float damage = 100.0f;
    
    void Update()
    {
        if (friendly)
        {
            targets = Physics.OverlapSphere(transform.position, range, 1 << 15);
        }
        else
        {
            targets = Physics.OverlapSphere(transform.position, range, 1 << 14);
        }

        lockedOn = false;

        if (targets.Length > 0)
        {
            Vector3 targetDir = targets[0].transform.position - transform.position;
            Vector3 forwardDir = turret.transform.forward;

            forwardDir.y = 0;
            targetDir.y = 0;

            float angle = Vector3.Angle(forwardDir, targetDir);
            Vector3 cVec = Vector3.Cross(forwardDir, targetDir);
            if (cVec.y < 0) angle *= -1;

            if (angle < -2)
            {
                turret.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0, Space.World);
            }
            else if (angle > 2)
            {
                turret.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
            }
            else
            {
                turret.transform.LookAt(new Vector3(targets[0].transform.position.x, targets[0].transform.position.y, targets[0].transform.position.z));
                lockedOn = true;
            }
        }
        fireCounter += Time.deltaTime;
        if (lockedOn)
        {
            if (fireCounter >= rateOfFire)
            {
                fireCounter = 0;

                GameObject mTemp = (GameObject)Instantiate(missile, turret.transform.position, turret.transform.rotation);
                mTemp.GetComponent<SAMTrajectory>().target = targets[0].gameObject;
                mTemp.GetComponent<SAMTrajectory>().damage = damage;
            }
        }
    }

    public void swapTeams()
    {
        friendly = !friendly;
    }
}
