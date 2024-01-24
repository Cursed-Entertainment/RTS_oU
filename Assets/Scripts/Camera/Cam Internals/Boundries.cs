using UnityEngine;

[DisallowMultipleComponent]
public class Boundries : MonoBehaviour
{
    public int length = 10;
    public int width = 10;

    public static Boundries temp;

    void Start()
    {
        temp = this;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((GameObject.Find("level").GetComponent<Terrain>().terrainData.size / 2), new Vector3(length, 30, width));
    }
}
