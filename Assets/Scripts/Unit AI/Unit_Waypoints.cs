using UnityEngine;

[DisallowMultipleComponent]
public class Unit_Waypoints : MonoBehaviour
{
    bool Move = false;
    Vector3 pos, wTarget;
    public float Speed = 10.0f;
    public float CaptureRadius = 0.5f;
    aStar aPath = new aStar();
    int ci, cj;
    public int[,] Path;
    int[] _Start = new int[2];
    int[] _End = new int[2];
    const int ONROUTE = 6, OPEN = 1, CLOSED = 2, UNVISITED = 3, BLOCKED = 4;
    public bool UseWaypoints = true;
    GameObject[] Waypoints;
    bool Selected = false;
    GameObject Point;

    void Start()
    {
        if (UseWaypoints)
        {
            Waypoints = GameObject.FindGameObjectsWithTag("wp");
        }
    }

    void Update()
    {
        if (UseWaypoints)
        {
            if (!Selected)
            {
                int i = Waypoints.GetLength(0);
                int j = Random.Range(0, i);
                Point = Waypoints[j];
                target(Point.transform.position);
                Selected = true;
            }
        }

        if (Move)
        {
            for (int i = 1; i < Path.GetLength(0); i++)
            {

                Vector3 pFrom = grid.mGrid.mainGrid[Path[i - 1, 0], Path[i - 1, 1]].center;
                Vector3 pTo = grid.mGrid.mainGrid[Path[i, 0], Path[i, 1]].center;
                Debug.DrawLine(pFrom, pTo, Color.green);
            }

            int iPos = Path[ci, 0];
            int jPos = Path[cj, 1];
            Vector3 targetPos = grid.mGrid.mainGrid[iPos, jPos].center;
            transform.LookAt(targetPos);
            transform.Translate(new Vector3(0, 0, Speed * Time.deltaTime), Space.Self);
            if (Vector3.Distance(transform.position, grid.mGrid.mainGrid[iPos, jPos].center) < CaptureRadius)
            {
                if (ci + 1 < Path.GetLength(0))
                {
                    ci++;
                    cj++;
                }
                else
                {
                    Move = false;
                    if (UseWaypoints)
                    {
                        Selected = false;
                    }
                }
            }
        }
    }

    public void movement(bool m)
    {
        Move = m;
    }

    public void target(Vector3 tar)
    {
        _Start = findClosestTile(transform.position);
        _End = findClosestTile(tar);
        if (grid.mGrid.mainGrid[_End[0], _End[1]].status != BLOCKED)
        {
            Path = aPath.findPath(_Start[0], _Start[1], _End[0], _End[1]);
            ci = 1;
            cj = 1;
            Move = true;
        }
    }

    int[] findClosestTile(Vector3 pos)
    {
        int gSize = grid.mGrid.mainGrid.GetLength(0);
        float dist = 10000.0f;
        int[] ans = new int[2];
        for (int i = 0; i < gSize; i++)
        {
            for (int j = 0; j < gSize; j++)
            {
                if (Vector3.Distance(grid.mGrid.mainGrid[i, j].center, pos) < dist)
                {
                    dist = Vector3.Distance(grid.mGrid.mainGrid[i, j].center, pos);
                    ans[0] = i;
                    ans[1] = j;
                }
            }
        }

        return ans;
    }
}
