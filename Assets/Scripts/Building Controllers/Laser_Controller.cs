using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Laser_Controller : MonoBehaviour
{
    public float range = 10.0f;
    public float rateOfFire = 2.0f;
    public float damage = 10.0f;
    public float rotationSpeed = 150.0f;
    public float laserDuration = 0.3f;

    private Collider[] targets;

    public GameObject turret;

    private bool lockedOn = false;
    private bool fire = false;
    private float durationCounter = 0;
    private float counter;

    private bool friendly = false;

    private ParticleRenderer laserBeam;
    private LightningBolt laserScript;

    private List<GameObject> laserLights = new List<GameObject>();

    new public GameObject light;
    public int lightIntensity = 1;
    private RaycastHit hit;

    void Start()
    {
        counter = rateOfFire;
        laserBeam = GetComponentInChildren<ParticleRenderer>();
        laserScript = GetComponentInChildren<LightningBolt>();
        laserBeam.enabled = false;
    }

    void Update()
    {
        if (friendly && !fire)
        {
            targets = Physics.OverlapSphere(transform.position, range, 1 << 9);

            if (targets.Length == 0)
            {
                targets = Physics.OverlapSphere(transform.position, range, 1 << 13);
            }
        }
        else if (!fire)
        {
            targets = Physics.OverlapSphere(transform.position, range, 1 << 8);

            if (targets.Length == 0)
            {
                targets = Physics.OverlapSphere(transform.position, range, 1 << 12);
            }
        }

        if (fire && targets[0] == null)
        {
            stopFire();
            targets = new Collider[0];
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
                turret.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
            }
            else if (angle > 2)
            {
                turret.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                turret.transform.LookAt(new Vector3(targets[0].transform.position.x, turret.transform.position.y, targets[0].transform.position.z));
                lockedOn = true;
            }
        }

        counter += Time.deltaTime;

        if (counter > 100000) counter = rateOfFire;
        Vector3 rayFirePos = Vector3.zero;
        if (lockedOn)
        {
            int rayCastLayer;
            if (friendly)
            {
                rayCastLayer = 1 << 9 | 1 << 13;
            }
            else
            {
                rayCastLayer = 1 << 8 | 1 << 12;
            }
            rayFirePos = turret.transform.position + (turret.transform.forward);
            Vector3 targetPos = targets[0].transform.position;
            targetPos.y += targets[0].GetComponent<BoxCollider>().bounds.extents.y;
            Physics.Raycast(rayFirePos, targetPos - rayFirePos, out hit, Mathf.Infinity, rayCastLayer);
            GameObject unitToTakeDamage = hit.collider.gameObject;

            float yOffset = hit.collider.bounds.extents.y;
            laserScript.targetPos = hit.transform.position + new Vector3(0, yOffset, 0);
            laserScript.Update();
            if (counter >= rateOfFire)
            {
                if (!fire)
                {
                    if (targets[0].gameObject.layer == 8 || targets[0].gameObject.layer == 9)
                    {
                        unitToTakeDamage.GetComponent<Unit_Core>().Stats.takeDamage((int)damage);
                    }
                    else if (targets[0].gameObject.layer == 12 || targets[0].gameObject.layer == 13)
                    {
                        unitToTakeDamage.GetComponent<Building_Controller>().takeDamage((int)damage);
                    }
                }
                fire = true;
                counter = 0;
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            laserScript.target = null;
        }

        if (fire && lockedOn)
        {
            float distToTarget = Vector3.Distance(rayFirePos, hit.point);
            int numberOfLights = (int)(distToTarget / lightIntensity);

            numberOfLights++;
            numberOfLights++;

            if (laserLights.Count == 0)
            {
                for (int i = 0; i < numberOfLights; i++)
                {
                    laserLights.Add((GameObject)Instantiate(light));
                }
            }

            float distBetweenLights = distToTarget / (numberOfLights - 1);

            for (int i = 0; i < laserLights.Count; i++)
            {
                if (i == 0)
                {
                    laserLights[i].transform.position = rayFirePos;
                }
                else if (i == laserLights.Count - 1)
                {
                    laserLights[i].transform.position = hit.point;
                }
                else
                {
                    Vector3 lightDir = Vector3.Normalize(hit.point - rayFirePos);
                    laserLights[i].transform.position = rayFirePos + (lightDir * distBetweenLights * i);
                }
            }

            laserBeam.enabled = true;
            durationCounter += Time.deltaTime;

            if (durationCounter >= laserDuration)
            {
                stopFire();
            }
        }
    }

    private void stopFire()
    {
        fire = false;
        durationCounter = 0;
        laserBeam.enabled = false;
        stopLights();
        laserScript.targetPos = Vector3.zero;
    }

    private void stopLights()
    {
        foreach (GameObject g in laserLights)
        {
            Destroy(g);
        }
        laserLights.Clear();
    }

    void OnDestroy()
    {
        foreach (GameObject g in laserLights)
        {
            Destroy(g);
        }
    }
}
