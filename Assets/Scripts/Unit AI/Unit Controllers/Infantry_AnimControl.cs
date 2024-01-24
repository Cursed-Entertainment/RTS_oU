using UnityEngine;

public class Infantry_AnimControl : MonoBehaviour
{
    public bool canShoot = true;

    public Transform shootBone;
    private Unit_Core unit;

    private bool shooting;
    private bool moving;
    private bool facingTarget;
    private bool oldMove;
    private bool oldShoot;

    private MeshRenderer muzzleRenderer;

    public float rateOfFire = 0.1f;
    private float fireCounter = 0;
    public GameObject bullet;
    public Transform gun;

    void Start()
    {
        if (canShoot)
        {
            GetComponent<Animation>()["shoot"].AddMixingTransform(shootBone);
            GetComponent<Animation>()["shoot"].layer = 5;
            muzzleRenderer = GetComponentInChildren<MuzzleFlash>().gameObject.GetComponent<MeshRenderer>();
        }

        unit = GetComponent<Unit_Core>();
    }

    void Update()
    {
        unit.getInfantryStats(ref moving, ref shooting, ref facingTarget);

        if (canShoot)
        {
            if (moving && shooting)
            {
                GetComponent<Animation>().CrossFade("running", 0.3f);
                GetComponent<Animation>().CrossFade("shoot", 0.1f);

            }
            else if (moving)
            {
                GetComponent<Animation>().CrossFade("running", 0.3f);
                GetComponent<Animation>().Stop("shoot");
            }
            else if (shooting)
            {
                GetComponent<Animation>().CrossFade("shootIdle", 0.1f);
            }
            else
            {
                GetComponent<Animation>().CrossFade("idle", 0.1f);
                GetComponent<Animation>().Stop("shoot");
            }
        }
        else
        {
            if (moving)
            {
                GetComponent<Animation>().CrossFade("running", 0.3f);
            }
            else
            {
                GetComponent<Animation>().CrossFade("idle", 0.1f);
            }
        }

        if (shooting && facingTarget && canShoot)
        {
            muzzleRenderer.enabled = true;
            fireCounter += Time.deltaTime;
            if (fireCounter >= rateOfFire && GetComponent<Unit_Core>().getUnitToAttack() != null)
            {
                GameObject bTemp = (GameObject)Instantiate(bullet, gun.position + (transform.forward * 2) + (Vector3.up * 0.3f) + (transform.right * -0.1f), transform.rotation);
                bTemp.GetComponentInChildren<BulletHit>().FiredFrom = gameObject;
                bTemp.GetComponent<BulletTrajectory>().Target = GetComponent<Unit_Core>().getUnitToAttack().transform;
                fireCounter = 0;
            }
        }
        else if (canShoot)
        {
            muzzleRenderer.enabled = false;
            fireCounter = rateOfFire;
        }
    }
}
