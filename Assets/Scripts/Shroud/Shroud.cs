using UnityEngine;

[DisallowMultipleComponent]
public class Shroud : MonoBehaviour
{
    Collider[] Object;
    public bool DrawGizmo = false;
    public float Range = 25;
    public tile[] cTiles;

    void Update()
    {
        if (Physics.CheckSphere(transform.position, Range, 1 << 8 | 1 << 12))
        {
            foreach (tile t in cTiles)
            {
                if (t != null) t.shrouded = false;
            }
            Destroy(gameObject);
        }
    }
    public void passTiles(tile[] tiles)
    {
        cTiles = tiles;
        foreach (tile t in cTiles)
        {
            if (t != null) t.shrouded = true;
        }
    }
}
