using UnityEngine;

[DisallowMultipleComponent]
public class BulletTrajectory : MonoBehaviour
{
    public float Speed = 10.0f;
    public Transform Target;
    public float maxAngle = 1.0f;

    void Start()
    {
        if (Target != null) transform.LookAt(Target.position + (Vector3.up * 0.2f));
        transform.Rotate(Random.Range(-maxAngle, maxAngle), Random.Range(-maxAngle, maxAngle), Random.Range(-45.0f, 45.0f));
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(0, 0, Speed * Time.deltaTime, Space.Self);
    }
}
