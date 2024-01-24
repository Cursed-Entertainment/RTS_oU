using UnityEngine;

[DisallowMultipleComponent]
public class ShroudGenerator : MonoBehaviour
{
    public bool GenerateShroud = true;
    public GameObject Shroud;
    public float ShroudHeight = 20;
    public float ShroudRange = 21;

    tile[,] tempGrid;

    void Start()
    {
        if (GenerateShroud)
        {
            tempGrid = grid.mGrid.mainGrid;
            for (int i = 0; i < grid.mGrid.gridSize; i++)
            {
                for (int j = 0; j < grid.mGrid.gridSize; j++)
                {
                    if (i % 3 == 0 && j % 3 == 0)
                    {
                        GameObject gTemp = (GameObject)Instantiate(Shroud, new Vector3(tempGrid[i, j].center.x, ShroudHeight, tempGrid[i, j].center.z), Shroud.transform.rotation);
                        tempGrid[i, j].shrouded = true;

                        tile[] tiles = new tile[9];

                        if (i - 1 >= 0 && j - 1 >= 0)
                        {
                            tiles[0] = tempGrid[i - 1, j - 1];
                        }
                        else
                        {
                            tiles[0] = null;
                        }

                        if (j - 1 >= 0)
                        {
                            tiles[1] = tempGrid[i, j - 1];
                        }
                        else
                        {
                            tiles[1] = null;
                        }

                        if (i + 1 < grid.mGrid.gridSize && j - 1 >= 0)
                        {
                            tiles[2] = tempGrid[i + 1, j - 1];
                        }
                        else
                        {
                            tiles[2] = null;
                        }

                        if (i - 1 >= 0)
                        {
                            tiles[3] = tempGrid[i - 1, j];
                        }
                        else
                        {
                            tiles[3] = null;
                        }

                        tiles[4] = tempGrid[i, j];

                        if (i + 1 < grid.mGrid.gridSize)
                        {
                            tiles[5] = tempGrid[i + 1, j];
                        }
                        else
                        {
                            tiles[5] = null;
                        }

                        if (i - 1 >= 0 && j + 1 < grid.mGrid.gridSize)
                        {
                            tiles[6] = tempGrid[i - 1, j + 1];
                        }
                        else
                        {
                            tiles[6] = null;
                        }

                        if (j + 1 < grid.mGrid.gridSize)
                        {
                            tiles[7] = tempGrid[i, j + 1];
                        }
                        else
                        {
                            tiles[7] = null;
                        }

                        if (i + 1 < grid.mGrid.gridSize && j + 1 < grid.mGrid.gridSize)
                        {
                            tiles[8] = tempGrid[i + 1, j + 1];
                        }
                        else
                        {
                            tiles[8] = null;
                        }

                        gTemp.GetComponent<Shroud>().passTiles(tiles);
                    }
                }
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
