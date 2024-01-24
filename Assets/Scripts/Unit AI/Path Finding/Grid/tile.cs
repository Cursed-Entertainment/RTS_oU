using UnityEngine;
using System.Collections.Generic;

public class tile
{
    public int status;
    public float cost;
    public int iparent = 0;
    public int jparent = 0;
    public bool start;
    public bool end;
    public bool selected;
    public Vector3 center;
    public float hCost;
    public float fCost;
    public float costinc;
    public int baseCost;
    public int[,] twTile = new int[8, 2];
    public int twTileNum = 0;
    public int i, j;
    public bool objectArrival = false;
    public int navMesh = 0;
    public List<tile> connectedTiles = new List<tile>();
    public GameObject occupied = null;
    public bool occupiedStatic = false;
    public bool hasOre = false;
    public Ore tOre;
    public bool buildCheck = false;
    public bool shrouded = false;

    public tile()
    {
    }

    public tile(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public tile(tile other)
    {
        status = other.status;
        cost = other.cost;
        iparent = other.iparent;
        jparent = other.jparent;
        start = other.start;
        end = other.end;
        selected = other.selected;
        center = other.center;
        hCost = other.hCost;
        fCost = other.fCost;
        costinc = other.costinc;
        baseCost = other.baseCost;
        twTile = other.twTile;
        twTileNum = other.twTileNum;
        i = other.i;
        j = other.j;
    }
}
