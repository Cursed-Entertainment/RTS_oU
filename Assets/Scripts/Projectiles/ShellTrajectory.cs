using UnityEngine;

[DisallowMultipleComponent]
public class ShellTrajectory : MonoBehaviour
{
    public GameObject Explosion;
    public Vector3 TargetPos;
    public float Speed = 150f;
    public Transform TargetTransform;
    public float Damage = 10;

    void Start()
    {
        //PlayAudio Here
    }

    void Update()
    {
        MoveTowardsTarget();

        if (HasReachedTarget())
        {
            HandleTargetReached();
        }
    }

    void MoveTowardsTarget()
    {
        if (!TargetTransform)
        {
            return;
        }

        transform.LookAt(TargetTransform.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, TargetTransform.position) < 0.3f;
    }

    void HandleTargetReached()
    {
        if (!TargetTransform)
        {
            Destroy(gameObject);
            return;
        }

        Unit_Core unitCore = TargetTransform.gameObject.GetComponent<Unit_Core>();
        Building_Controller buildingController = TargetTransform.gameObject.GetComponent<Building_Controller>();

        if (unitCore != null)
        {
            Explode();
            unitCore.Stats.takeDamage((int)Damage);
        }
        else if (buildingController)
        {
            Explode();
            buildingController.takeDamage((int)Damage);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        if (Explosion != null)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
        }
    }
}
