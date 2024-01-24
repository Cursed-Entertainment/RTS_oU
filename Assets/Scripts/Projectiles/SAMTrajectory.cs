using UnityEngine;

[DisallowMultipleComponent]
public class SAMTrajectory : MonoBehaviour
{
    public GameObject target;
    public float speed = 30.0f;
    public float launchDelay = 1.0f;
    float delayCounter = 0;
    bool justBeenLaunched = true;
    public GameObject explosion;

    public float lifeTime = 10.0f;
    private float lifeTimer = 0;

    public float damage;

    void Update()
    {
        if (target == null)
        {
            Explode();
            Destroy(gameObject);
            return;
        }

        if (justBeenLaunched)
        {
            delayCounter += Time.deltaTime;
            if (delayCounter >= launchDelay)
            {
                justBeenLaunched = false;
            }
            else
            {
                transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime * 8);
            transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            other.gameObject.GetComponent<Unit_Core>().Stats.takeDamage((int)damage);
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (explosion)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
