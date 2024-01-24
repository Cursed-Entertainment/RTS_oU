using UnityEngine;

[DisallowMultipleComponent]
public class TeslaCoil_Modern : MonoBehaviour
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

    ParticleSystem laserParticleSystem;
    ParticleSystemRenderer laserRenderer;

    void Start()
    {
        Counter = RateOfFire;
        laserParticleSystem = GetComponentInChildren<ParticleSystem>();
        laserRenderer = laserParticleSystem.GetComponent<ParticleSystemRenderer>();
        laserParticleSystem.Stop();
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

        if (Fire && Targets.Length == 0)
        {
            StopFire();
            Targets = new Collider[0];
        }

        Counter += Time.deltaTime;

        if (Targets.Length > 0 && Counter >= RateOfFire)
        {
            DealDamage(Targets[0]);
            Fire = true;
            Counter = 0;
        }

        if (Counter > 100000) Counter = RateOfFire;

        if (Fire)
        {
            float yOffset = Targets[0].GetComponent<Collider>().bounds.extents.y;
            Vector3 laserTargetPos = Targets[0].GetComponent<Collider>().transform.position + new Vector3(0, yOffset, 0);
            UpdateLaser(laserTargetPos);
            DurationCounter += Time.deltaTime;

            if (DurationCounter >= LaserDuration)
            {
                StopFire();
            }
        }
    }

    void DealDamage(Collider target)
    {
        if (target.gameObject.layer == 8 || target.gameObject.layer == 9)
        {
            target.gameObject.GetComponent<Unit_Core>().Stats.takeDamage(Damage);
        }
        else
        {
            target.gameObject.GetComponent<Building_Controller>().takeDamage(Damage);
        }
    }

    void UpdateLaser(Vector3 targetPosition)
    {
        laserParticleSystem.Play();
        laserRenderer.enabled = true;
        laserParticleSystem.transform.LookAt(targetPosition);
    }

    void StopFire()
    {
        Fire = false;
        DurationCounter = 0;
        laserParticleSystem.Stop();
        laserRenderer.enabled = false;
    }
}
