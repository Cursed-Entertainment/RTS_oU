using UnityEngine;

[DisallowMultipleComponent]
public class WayPointGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2.0f);
    }
}
