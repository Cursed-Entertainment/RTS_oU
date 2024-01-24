using UnityEngine;

public class grid : MonoBehaviour
{
    public int width = 100, length = 100, gridSize = 10;
    public tile[,] mainGrid;
    private float lsi, wsi, yValue;
    public float waterLevel = 10;
    public float terrainSlopeAngle = 2.0f;
    public float xOffset = 0, zOffset = 0;
    public static grid mGrid;

    public int blockIndent = 1;

    void Start()
    {
        mGrid = this;
        wsi = (float)width / (float)gridSize;
        lsi = (float)length / (float)gridSize;
        mainGrid = new tile[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 posCheck = new Vector3((i * wsi) + (wsi / 2) + xOffset, 0, (j * lsi) + (lsi / 2) + zOffset);
                yValue = Terrain.activeTerrain.SampleHeight(posCheck);
                posCheck.y = yValue;

                mainGrid[i, j] = new tile(i, j);

                if (i < blockIndent || j < blockIndent || i > gridSize - blockIndent || j > gridSize - blockIndent)
                {
                    mainGrid[i, j].status = 4;
                }
                else if (posCheck.y < waterLevel)
                {
                    mainGrid[i, j].status = 4;
                }
                else
                {
                    Vector3 origin = posCheck;
                    origin.y += 100;
                    float maskDistance = wsi / 2;
                    Vector3 otl = origin + new Vector3(maskDistance, 0, maskDistance);
                    Vector3 otr = origin + new Vector3(maskDistance, 0, -maskDistance);
                    Vector3 obl = origin + new Vector3(-maskDistance, 0, -maskDistance);
                    Vector3 obr = origin + new Vector3(-maskDistance, 0, maskDistance);

                    float ytl = Terrain.activeTerrain.SampleHeight(otl);
                    float ytr = Terrain.activeTerrain.SampleHeight(otr);
                    float ybl = Terrain.activeTerrain.SampleHeight(obl);
                    float ybr = Terrain.activeTerrain.SampleHeight(obr);

                    if (Physics.Raycast(origin, Vector3.down, Mathf.Infinity, 1 << 10) || Physics.Raycast(otr, Vector3.down, Mathf.Infinity, 1 << 10) || Physics.Raycast(otl, Vector3.down, Mathf.Infinity, 1 << 10) || Physics.Raycast(obr, Vector3.down, Mathf.Infinity, 1 << 10) || Physics.Raycast(obl, Vector3.down, Mathf.Infinity, 1 << 10))
                    {
                        mainGrid[i, j].status = 4;
                    }
                    else if (Mathf.Abs(ytl - ybr) > terrainSlopeAngle || Mathf.Abs(ytr - ybl) > terrainSlopeAngle || Mathf.Abs(ytl - ybl) > terrainSlopeAngle || Mathf.Abs(ytr - ybr) > terrainSlopeAngle)
                    {
                        mainGrid[i, j].status = 4;
                    }
                    else
                    {
                        mainGrid[i, j].status = 3;
                    }
                }

                mainGrid[i, j].center = posCheck;
                mainGrid[i, j].start = false;
                mainGrid[i, j].end = false;
                mainGrid[i, j].selected = false;
                mainGrid[i, j].cost = 0;
                mainGrid[i, j].hCost = 0;
                mainGrid[i, j].fCost = 0;
                mainGrid[i, j].costinc = 10;
                mainGrid[i, j].baseCost = 10;
                mainGrid[i, j].twTileNum = 0;
            }
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (mainGrid[i, j].status != 4)
                {
                    testHeight(i, j, i - 1, j - 1);
                    testHeight(i, j, i - 1, j);
                    testHeight(i, j, i - 1, j + 1);
                    testHeight(i, j, i, j + 1);
                    testHeight(i, j, i + 1, j + 1);
                    testHeight(i, j, i + 1, j);
                    testHeight(i, j, i + 1, j - 1);
                    testHeight(i, j, i, j - 1);
                }
            }
        }

        int navMeshCounter = 1;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (mainGrid[i, j].status == 3 && mainGrid[i, j].navMesh == 0)
                {
                    applyNavmesh(mainGrid[i, j], navMeshCounter);
                    navMeshCounter++;
                }
            }
        }
    }

    void applyNavmesh(tile t, int counter)
    {
        t.navMesh = counter;
        foreach (tile connectedTile in t.connectedTiles)
        {
            if (connectedTile.navMesh == 0)
            {
                applyNavmesh(connectedTile, counter);
            }
        }
    }

    void testHeight(int i, int j, int i2, int j2)
    {
        try
        {
            if (mainGrid[i2, j2].status != 4)
            {
                if (mainGrid[i2, j2].status == 3)
                {
                    mainGrid[i, j].twTile[mainGrid[i, j].twTileNum, 0] = i2;
                    mainGrid[i, j].twTile[mainGrid[i, j].twTileNum, 1] = j2;
                    mainGrid[i, j].twTileNum++;
                    mainGrid[i, j].connectedTiles.Add(mainGrid[i2, j2]);
                }
            }
        }
        catch
        {
        }
    }

    public tile[,] copyGrid()
    {
        tile[,] tempGrid = new tile[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                tempGrid[i, j] = new tile(mainGrid[i, j]);
            }
        }
        return tempGrid;
    }

    public tile[,] getGrid()
    {
        return mainGrid;
    }

    public tile returnClosestTile(Vector3 pos)
    {
        int[] ans = new int[2];

        ans[0] = (int)((pos.x - xOffset) / wsi);
        ans[1] = (int)((pos.z - zOffset) / wsi);

        if (ans[0] < 0) ans[0] = 0;
        else if (ans[0] >= gridSize) ans[0] = gridSize - 1;
        else if (ans[1] < 0) ans[1] = 0;
        else if (ans[1] >= gridSize) ans[1] = gridSize - 1;

        try
        {
            return mainGrid[ans[0], ans[1]];
        }
        catch
        {
            return null;
        }
    }

    public tile findClosestAvailableTileWithOre(Vector3 pos, int navMesh)
    {
        return findClosestAvailableTileWithOre(returnClosestTile(pos), navMesh);
    }

    public tile findClosestAvailableTileWithOre(tile t, int navMesh)
    {
        int counter = 0;
        int limit = 1;
        int dir = 1;
        if (!t.objectArrival && t.status != 4 && t.navMesh == navMesh && t.hasOre)
        {
            return t;
        }
        else
        {
            return findClosestAvailableTileWithOre(mainGrid[t.i, t.j + 1], counter, limit, dir, navMesh);
        }
    }

    private tile findClosestAvailableTileWithOre(tile t, int counter, int limit, int direction, int navMesh)
    {
        if (!t.objectArrival && t.status != 4 && t.navMesh == navMesh && t.hasOre)
        {
            return t;
        }

        counter++;

        if (direction == 0)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
            }
            return findClosestAvailableTileWithOre(mainGrid[t.i, t.j + 1], counter, limit, direction, navMesh);
        }
        else if (direction == 1)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
                limit++;
            }
            return findClosestAvailableTileWithOre(mainGrid[t.i + 1, t.j], counter, limit, direction, navMesh);
        }
        else if (direction == 2)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
            }
            return findClosestAvailableTileWithOre(mainGrid[t.i, t.j - 1], counter, limit, direction, navMesh);
        }
        else
        {
            if (counter == limit)
            {
                direction = 0;
                counter = 0;
                limit++;
            }
            return findClosestAvailableTileWithOre(mainGrid[t.i - 1, t.j], counter, limit, direction, navMesh);
        }
    }

    public tile findClosestAvailableTile(Vector3 pos, int navMesh)
    {
        return findClosestAvailableTile(returnClosestTile(pos), navMesh);
    }

    public tile findClosestAvailableTile(tile t, int navMesh)
    {
        int counter = 0;
        int limit = 1;
        int dir = 1;
        if (!t.objectArrival && t.status != 4 && t.navMesh == navMesh)
        {
            return t;
        }
        else
        {
            if (t.j + 1 >= gridSize) return findClosestAvailableTile(mainGrid[t.i, t.j], counter, limit, dir, navMesh);
            else return findClosestAvailableTile(mainGrid[t.i, t.j + 1], counter, limit, dir, navMesh);
        }
    }

    private tile findClosestAvailableTile(tile t, int counter, int limit, int direction, int navMesh)
    {
        if (!t.objectArrival && t.status != 4 && t.navMesh == navMesh)
        {
            return t;
        }

        counter++;

        if (direction == 0)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
            }

            if (t.j + 1 >= gridSize) return findClosestAvailableTile(mainGrid[t.i, t.j], counter, limit, direction, navMesh);
            else return findClosestAvailableTile(mainGrid[t.i, t.j + 1], counter, limit, direction, navMesh);
        }
        else if (direction == 1)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
                limit++;
            }

            if (t.i + 1 >= gridSize) return findClosestAvailableTile(mainGrid[t.i, t.j], counter, limit, direction, navMesh);
            else return findClosestAvailableTile(mainGrid[t.i + 1, t.j], counter, limit, direction, navMesh);
        }
        else if (direction == 2)
        {
            if (counter == limit)
            {
                direction++;
                counter = 0;
            }

            if (t.j - 1 < 0) return findClosestAvailableTile(mainGrid[t.i, t.j], counter, limit, direction, navMesh);
            else return findClosestAvailableTile(mainGrid[t.i, t.j - 1], counter, limit, direction, navMesh);
        }
        else
        {
            if (counter == limit)
            {
                direction = 0;
                counter = 0;
                limit++;
            }

            if (t.i - 1 < 0) return findClosestAvailableTile(mainGrid[t.i, t.j], counter, limit, direction, navMesh);
            else return findClosestAvailableTile(mainGrid[t.i - 1, t.j], counter, limit, direction, navMesh);
        }
    }

    public float getGridSize()
    {
        return lsi;
    }
}

