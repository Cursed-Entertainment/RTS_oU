using UnityEngine;

public class Immovable_Controller : MonoBehaviour
{
    private Vector3 StartPos;

    void Start()
    {
        StartPos = transform.position;
    }

    void Update()
    {
        transform.position = StartPos;
    }
}
