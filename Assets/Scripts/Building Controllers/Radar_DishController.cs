using UnityEngine;

[DisallowMultipleComponent]
public class Radar_DishController : MonoBehaviour
{
    public float RotationSpeed = 100.0f;

    void Update()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }
}
