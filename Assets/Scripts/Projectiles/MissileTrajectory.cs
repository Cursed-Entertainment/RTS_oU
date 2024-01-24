using UnityEngine;

[DisallowMultipleComponent]
public class MissileTrajectory : MonoBehaviour
{
    public float speed = 30;
    public float captureRadius = 0.5f;
    bool fired = false;
    Vector3 target;

    public float damage = 100;
    public float impactRadius = 10;

    public GameObject explosion;

    bool isQuitting = false;

    void Start()
    {
        isQuitting = false;
    }

    void Update()
    {
        if (fired)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }

        if (Vector3.Distance(transform.position, target) < captureRadius)
        {
            Destroy(gameObject);
        }
    }

    public void fire(Vector3 v)
    {
        transform.parent = null;
        target = v;
        transform.LookAt(v);
        fired = true;
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting)
        {
            if (explosion)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Collider[] unitsToDamage = Physics.OverlapSphere(transform.position, impactRadius, 1 << 9 | 1 << 13);
            foreach (Collider c in unitsToDamage)
            {
                float dist = Vector3.Distance(transform.position, c.transform.position);
                if (c.gameObject.layer == 9)
                {
                    c.gameObject.GetComponent<Unit_Core>().Stats.takeDamage((int)(damage / (dist + 1)));
                }
                else
                {
                    c.gameObject.GetComponent<Building_Controller>().takeDamage((int)(damage / (dist + 1)));
                }
            }
        }
    }
}
