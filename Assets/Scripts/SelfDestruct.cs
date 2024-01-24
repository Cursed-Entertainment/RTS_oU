using UnityEngine;

[DisallowMultipleComponent]
public class SelfDestruct : MonoBehaviour
{
    public float timeLeft = 0.8f;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
