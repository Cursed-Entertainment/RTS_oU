using UnityEngine;

[DisallowMultipleComponent]
public class TeslaCoil_Controller : MonoBehaviour
{
    public float Range = 10.0f;
    public float RateOfFire = 2.0f;
    public int Damage = 100;
    public float LaserDuration = 0.3f;

    Collider[] Targets;

    bool Fire = false;
    float DurationCounter = 0;
    float Counter;

    bool Friendly = false;

    ParticleRenderer Arc;
    LightningBolt Lightning;

    void Start()
    {
        Counter = RateOfFire;
        Arc = GetComponentInChildren<ParticleRenderer>();
        Lightning = GetComponentInChildren<LightningBolt>();
        Arc.enabled = false;
    }

    void Update()
    {
        if (Friendly && !Fire)
        {
            Targets = Physics.OverlapSphere(transform.position, Range, 1 << 9);

            if (Targets.Length == 0)
            {
                Targets = Physics.OverlapSphere(transform.position, Range, 1 << 13);
            }
        }
        else if (!Fire)
        {
            Targets = Physics.OverlapSphere(transform.position, Range, 1 << 8);

            if (Targets.Length == 0)
            {
                Targets = Physics.OverlapSphere(transform.position, Range, 1 << 12);
            }
        }

        if (Fire && Targets[0] == null)
        {
            stopFire();
            Targets = new Collider[0];
        }

        Counter += Time.deltaTime;

        if (Targets.Length > 0 && Counter >= RateOfFire)
        {
            if (Targets[0].gameObject.layer == 8 || Targets[0].gameObject.layer == 9)
            {
                Targets[0].gameObject.GetComponent<Unit_Core>().Stats.takeDamage(Damage);
            }
            else
            {
                Targets[0].gameObject.GetComponent<Building_Controller>().takeDamage(Damage);
            }

            Fire = true;
            Counter = 0;
        }

        if (Counter > 100000) Counter = RateOfFire;

        if (Fire)
        {
            float yOffset = Targets[0].GetComponent<Collider>().bounds.extents.y;
            Lightning.targetPos = Targets[0].GetComponent<Collider>().transform.position + new Vector3(0, yOffset, 0);
            Lightning.Update();
            Arc.enabled = true;
            DurationCounter += Time.deltaTime;

            if (DurationCounter >= LaserDuration)
            {
                stopFire();
            }
        }
    }

    void stopFire()
    {
        Fire = false;
        DurationCounter = 0;
        Arc.enabled = false;
        Lightning.targetPos = Vector3.zero;
    }
}
