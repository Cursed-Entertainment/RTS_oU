using UnityEngine;

[DisallowMultipleComponent]
public class BulletHit : MonoBehaviour
{
    public GameObject FiredFrom;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != FiredFrom) Destroy(transform.parent.gameObject);
    }
}
