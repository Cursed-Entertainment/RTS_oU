using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    private Vector3 eulerAngles;
    public float changeDelay = 0.02f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > changeDelay)
        {
            eulerAngles = transform.localEulerAngles;
            eulerAngles.z = Random.Range(0, 90.0f);
            transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            transform.localEulerAngles = eulerAngles;
            timer = 0;
        }
    }
}
