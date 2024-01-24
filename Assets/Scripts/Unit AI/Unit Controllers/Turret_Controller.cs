using UnityEngine;

public class Turret_Controller : MonoBehaviour
{
    private bool targetAquired = false;
    private Transform targetTransform;
    private float timeCheck = 0;
    private float shootTimeCheck = 0;
    public float shootDelay = 1.0f;
    private Vector3 targetVector;

    public GameObject bullet;

    private bool locked = false;

    public float turretSpeed = 100;

    private float range;
    private float damage;

    public bool doubleBarrell = false;
    private bool firedFromLeft = true;

    public AudioClip fireSound;

    void Update()
    {
        timeCheck += Time.deltaTime;
        shootTimeCheck += Time.deltaTime;

        if (targetAquired && (targetTransform == null || Vector3.Distance(transform.parent.position, targetTransform.position) > range))
        {
            targetLost();
        }
        else if (targetAquired)
        {
            targetVector = targetTransform.position;
            targetVector.y = transform.position.y;

            Vector3 cVec = Vector3.Cross(transform.forward, targetTransform.position - transform.position);
            float tAngle = Vector3.Angle(transform.forward, targetTransform.position - transform.position);
            if (cVec.y < 0) tAngle *= -1;

            if (tAngle < -8)
            {
                locked = false;
                transform.Rotate(new Vector3(0, 1, 0), -turretSpeed * Time.deltaTime);
            }
            else if (tAngle > 8)
            {
                locked = false;
                transform.Rotate(new Vector3(0, 1, 0), turretSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(-transform.position + targetVector);
                locked = true;
            }
        }
        else
        {
            Vector3 cVec = Vector3.Cross(transform.forward, transform.parent.forward);
            float tAngle = Vector3.Angle(transform.forward, transform.parent.forward);
            if (cVec.y < 0) tAngle *= -1;

            if (tAngle < -2)
            {
                locked = false;
                transform.Rotate(new Vector3(0, 1, 0), -turretSpeed * Time.deltaTime);
            }
            else if (tAngle > 2)
            {
                locked = false;
                transform.Rotate(new Vector3(0, 1, 0), turretSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(transform.parent.forward, transform.parent.up);
            }
        }

        if (targetAquired && targetTransform != null)
        {
            if (locked)
            {
                if (shootTimeCheck >= shootDelay)
                {
                    fire();
                }
            }
        }
    }

    public void targetFound(Transform targetPos)
    {
        targetTransform = targetPos;
        targetAquired = true;
        timeCheck = 0;
    }

    public void targetLost()
    {
        targetAquired = false;
        timeCheck = 0;
    }

    public bool hasTarget()
    {
        return targetAquired;
    }

    private void fire()
    {
        if (targetTransform != null)
        {
            shootTimeCheck = 0;
            GameObject b;
            if (doubleBarrell)
            {
                if (firedFromLeft)
                {
                    b = (GameObject)Instantiate(bullet, transform.position + (transform.forward * 2) - (transform.right * 0.2f), transform.rotation);
                    Invoke("fire", 0.1f);
                }
                else
                {
                    b = (GameObject)Instantiate(bullet, transform.position + (transform.forward * 2) + (transform.right * 0.2f), transform.rotation);
                }
                firedFromLeft = !firedFromLeft;
            }
            else
            {
                b = (GameObject)Instantiate(bullet, transform.position + (transform.forward * 2), transform.rotation);
            }

            if (fireSound)
            {
                GetComponent<AudioSource>().clip = fireSound;
                GetComponent<AudioSource>().Play();
            }

            b.GetComponent<ShellTrajectory>().TargetPos = targetTransform.position;
            b.GetComponent<ShellTrajectory>().TargetTransform = targetTransform;
            b.GetComponent<ShellTrajectory>().Damage = damage;
        }
    }

    public void setProperties(float Damage, float Range)
    {
        range = Range;
        damage = Damage;
    }
}
