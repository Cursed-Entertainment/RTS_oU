using UnityEngine;

[ExecuteInEditMode]
public class GridEditor : MonoBehaviour
{
    public int width = 100, length = 100, gridSize = 10;
    public tile[,] mainGrid;
    float lsi, wsi, yValue;
    public float waterLevel = 10;
    public float terrainSlopeAngle = 5.0f;
    public float xOffset = 0, zOffset = 0;

    public int blockIndent = 1;

    void Update()
    {
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
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (mainGrid[i, j].status == 4)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawWireCube(mainGrid[i, j].center, new Vector3((float)width / (float)gridSize, 0.1f, (float)length / (float)gridSize));
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
                }
            }
        }
        catch
        {
        }
    }
}
