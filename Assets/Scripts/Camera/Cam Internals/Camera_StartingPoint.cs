using UnityEngine;

[DisallowMultipleComponent]
public class Camera_StartingPoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}
