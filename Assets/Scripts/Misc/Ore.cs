using UnityEngine;

[DisallowMultipleComponent]
public class Ore : MonoBehaviour
{
    public int OreLeft = 50;

    public Mesh Middle;
    public Mesh Low;

    void Start()
    {
        grid.mGrid.returnClosestTile(transform.position).tOre = this;
        grid.mGrid.returnClosestTile(transform.position).hasOre = true;
        RTS_Player.temp.addOreTile(grid.mGrid.returnClosestTile(transform.position));
        transform.position = grid.mGrid.returnClosestTile(transform.position).center;
    }

    void Update()
    {
        if (OreLeft <= 0)
        {
            grid.mGrid.returnClosestTile(transform.position).hasOre = false;
            grid.mGrid.returnClosestTile(transform.position).tOre = null;
            RTS_Player.temp.removeOreTile(grid.mGrid.returnClosestTile(transform.position));
            Destroy(gameObject);
        }
        else if (OreLeft < 20)
        {
            if (Low != null)
            {
                GetComponent<MeshFilter>().mesh = Low;
            }
        }
        else if (OreLeft < 40)
        {
            if (Middle != null)
            {
                GetComponent<MeshFilter>().mesh = Middle;
            }
        }
    }
}
