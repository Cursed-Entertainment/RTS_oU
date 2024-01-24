using UnityEngine;

[DisallowMultipleComponent]
public class SpecialTrajectory : MonoBehaviour
{
    private Vector3 targetPos;
    public float delayAfterLaunch = 0.1f;
    public float speed = 10.0f;
    public float turnRate = 4.0f;
    public float damage;

    float delayCounter = 0;

   bool go = false;

    public GameObject explosion;

    void Update()
    {
        if (go)
        {
            if (delayCounter >= delayAfterLaunch)
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), Time.deltaTime * turnRate);
            }
            else
            {
                delayCounter += Time.deltaTime;
                transform.Translate(0, 0, speed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, targetPos) < 1.0f)
            {
                if (explosion)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                }

                Collider[] objInRange = Physics.OverlapSphere(transform.position, 3.0f, 1 << 9 | 1 << 13);

                foreach (Collider c in objInRange)
                {
                    float dist = Vector3.Distance(transform.position, c.transform.position);
                    if (dist < 1) dist = 1;
                    if (c.gameObject.GetComponent<Unit_Core>())
                    {
                        c.gameObject.GetComponent<Unit_Core>().Stats.takeDamage((int)(damage / dist));
                    }
                    else
                    {
                        c.gameObject.GetComponent<Building_Controller>().takeDamage((int)(damage / dist));
                    }
                }
                Destroy(gameObject);
            }
        }
    }

    public void setTarget(Vector3 v, float damage)
    {
        targetPos = v;
        transform.parent = null;
        this.damage = damage;
        go = true;
        Destroy(gameObject, 4.0f);
    }
}
