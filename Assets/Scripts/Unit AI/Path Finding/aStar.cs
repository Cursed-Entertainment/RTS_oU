using UnityEngine;
using System.Collections.Generic;

public class aStar
{
    float costinc = 10.0f;
    float costDinc = 14.0f;
    bool endsearch;
    int icurrent, jcurrent, IGOAL, JGOAL, GSIZE, count;
    const int ONROUTE = 6, OPEN = 1, CLOSED = 2, UNVISITED = 3, BLOCKED = 4;
    tile[,] tgrid;
    bool fromDiag;
    List<int> iList = new List<int>();
    List<int> jList = new List<int>();
    List<tile> openTiles = new List<tile>();
    List<tile> closedTiles = new List<tile>();
    private bool blockOccupied = false;
    public int blockingLevel = 0;
    public GameObject parentGameObject;

    public aStar(GameObject g)
    {
        parentGameObject = g;
    }

    public aStar()
    {

    }

    public int[,] findPath(int si, int sj, int gi, int gj)
    {
        icurrent = si;
        jcurrent = sj;
        IGOAL = gi;
        JGOAL = gj;
        tgrid = grid.mGrid.copyGrid();
        GSIZE = tgrid.GetLength(0);
        endsearch = false;


        tgrid[si, sj].status = OPEN;
        tgrid[si, sj].cost = 0;
        tgrid[si, sj].start = true;
        tgrid[gi, gj].end = true;

        do
        {
            afindcheapest();
            awalk();
        } while (endsearch == false);

        int[,] g = new int[count, 2];
        int k = 0;
        foreach (int n in iList)
        {
            g[k, 0] = n;
            k++;
        }
        k = 0;
        foreach (int mn in jList)
        {
            g[k, 1] = mn;
            k++;
        }
        return g;
    }

    public List<Vector3> findVectorPath(Vector3 target, Vector3 cPos, int blockOccupied)
    {
        blockingLevel = blockOccupied;
        icurrent = grid.mGrid.returnClosestTile(cPos).i;
        jcurrent = grid.mGrid.returnClosestTile(cPos).j;
        IGOAL = grid.mGrid.returnClosestTile(target).i;
        JGOAL = grid.mGrid.returnClosestTile(target).j;
        tgrid = grid.mGrid.getGrid();
        GSIZE = tgrid.GetLength(0);
        endsearch = false;


        tgrid[icurrent, jcurrent].status = OPEN;
        tgrid[icurrent, jcurrent].cost = 0;
        tgrid[icurrent, jcurrent].start = true;
        tgrid[IGOAL, JGOAL].end = true;
        openTiles.Clear();
        closedTiles.Clear();
        openTiles.Add(tgrid[icurrent, jcurrent]);
        do
        {
            afindcheapest();
            awalk();

            if (openTiles.Count == 0 && !endsearch)
            {
                foreach (tile t in openTiles)
                {
                    t.status = 3;
                    t.start = false;
                    t.end = false;
                    t.cost = 0;
                    t.hCost = 0;
                    t.fCost = 0;
                    t.iparent = 0;
                    t.jparent = 0;
                }

                foreach (tile t in closedTiles)
                {
                    t.status = 3;
                    t.start = false;
                    t.end = false;
                    t.cost = 0;
                    t.hCost = 0;
                    t.fCost = 0;
                    t.iparent = 0;
                    t.jparent = 0;
                }

                List<Vector3> tempList = new List<Vector3>();
                tempList.Add(grid.mGrid.returnClosestTile(cPos).center);
                return tempList;
            }
        } while (endsearch == false);

        List<Vector3> g = new List<Vector3>();
        int k = 0;
        foreach (int n in iList)
        {
            g.Add(tgrid[iList[k], jList[k]].center);
            k++;
        }
        g.RemoveAt(0);

        foreach (tile t in openTiles)
        {
            t.status = 3;
            t.start = false;
            t.end = false;
            t.cost = 0;
            t.hCost = 0;
            t.fCost = 0;
            t.iparent = 0;
            t.jparent = 0;
        }

        foreach (tile t in closedTiles)
        {
            t.status = 3;
            t.start = false;
            t.end = false;
            t.cost = 0;
            t.hCost = 0;
            t.fCost = 0;
            t.iparent = 0;
            t.jparent = 0;
        }

        return g;
    }

    public List<Vector3> findVectorPathOccupied(Vector3 target, Vector3 cPos)
    {
        blockingLevel = 2;
        icurrent = grid.mGrid.returnClosestTile(cPos).i;
        jcurrent = grid.mGrid.returnClosestTile(cPos).j;
        IGOAL = grid.mGrid.returnClosestTile(target).i;
        JGOAL = grid.mGrid.returnClosestTile(target).j;
        tgrid = grid.mGrid.getGrid();
        GSIZE = tgrid.GetLength(0);
        endsearch = false;


        tgrid[icurrent, jcurrent].status = OPEN;
        tgrid[icurrent, jcurrent].cost = 0;
        tgrid[icurrent, jcurrent].start = true;
        tgrid[IGOAL, JGOAL].end = true;
        openTiles.Clear();
        closedTiles.Clear();
        openTiles.Add(tgrid[icurrent, jcurrent]);

        do
        {
            afindcheapest();
            awalk();

            if (openTiles.Count == 0 && !endsearch)
            {
                foreach (tile t in openTiles)
                {
                    t.status = 3;
                    t.start = false;
                    t.end = false;
                    t.cost = 0;
                    t.hCost = 0;
                    t.fCost = 0;
                    t.iparent = 0;
                    t.jparent = 0;
                }

                foreach (tile t in closedTiles)
                {
                    t.status = 3;
                    t.start = false;
                    t.end = false;
                    t.cost = 0;
                    t.hCost = 0;
                    t.fCost = 0;
                    t.iparent = 0;
                    t.jparent = 0;
                }

                List<Vector3> tempList = new List<Vector3>();
                tempList.Add(grid.mGrid.returnClosestTile(cPos).center);
                return tempList;
            }
        } while (endsearch == false);

        List<Vector3> g = new List<Vector3>();
        int k = 0;
        foreach (int n in iList)
        {
            g.Add(tgrid[iList[k], jList[k]].center);
            k++;
        }
        g.RemoveAt(0);

        foreach (tile t in openTiles)
        {
            t.status = 3;
            t.start = false;
            t.end = false;
            t.cost = 0;
            t.hCost = 0;
            t.fCost = 0;
            t.iparent = 0;
            t.jparent = 0;
        }

        foreach (tile t in closedTiles)
        {
            t.status = 3;
            t.start = false;
            t.end = false;
            t.cost = 0;
            t.hCost = 0;
            t.fCost = 0;
            t.iparent = 0;
            t.jparent = 0;
        }

        return g;
    }

    float diagonal(int x, int y)
    {
        int dx = Mathf.Abs(IGOAL - x);
        int dy = Mathf.Abs(JGOAL - y);

        int min_d = min(dx, dy);

        int h_travel = dx + dy;

        return ((costDinc * min_d) + (costinc * (h_travel - (2 * min_d))));
    }

    float manhatten(int x, int y)
    {
        int dx = Mathf.Abs(IGOAL - x);
        int dy = Mathf.Abs(JGOAL - y);

        return (costinc * (dx + dy));
    }



    int min(int a, int b)
    {
        if (a <= b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    void agetroute()
    {
        int i = icurrent;
        int j = jcurrent;
        int itemp, jtemp;
        count = 0;
        iList = new List<int>();
        jList = new List<int>();
        iList.Insert(0, IGOAL);
        jList.Insert(0, JGOAL);
        count++;
        do
        {
            iList.Insert(0, i);
            jList.Insert(0, j);
            itemp = tgrid[i, j].iparent;
            jtemp = tgrid[i, j].jparent;
            i = itemp;
            j = jtemp;
            count++;
        }
        while (i != 0);
        endsearch = true;
    }

    void afindcheapest()
    {
        float lowcost = 10000.0f;
        float lowcost2 = 10000.0f;
        int i, j, i2 = 0, j2 = 0;

        foreach (tile t in openTiles)
        {
            if (t.fCost < lowcost)
            {
                lowcost = t.fCost;
                icurrent = t.i;
                jcurrent = t.j;
            }
        }

        if ((icurrent == IGOAL) && (jcurrent == JGOAL))
        {
            agetroute();
        }
    }

    void aupdate(int i, int j)
    {
        tgrid[i, j].status = OPEN;
        openTiles.Add(tgrid[i, j]);
        tgrid[i, j].cost = tgrid[icurrent, jcurrent].cost + costinc;
        tgrid[i, j].hCost = diagonal(i, j);
        tgrid[i, j].fCost = tgrid[i, j].cost + tgrid[i, j].hCost;
        tgrid[i, j].iparent = icurrent;
        tgrid[i, j].jparent = jcurrent;

    }

    void agetconnection(int i, int j, int oldi, int oldj)
    {
        if (fromDiag)
        {
            costinc = tgrid[i, j].costinc * 1.4f;
        }
        else
        {
            costinc = tgrid[i, j].costinc;
        }
        float newcost;

        if (tgrid[i, j].status != BLOCKED)
        {
            switch (tgrid[i, j].status)
            {
                case UNVISITED:
                    aupdate(i, j);
                    break;

                case OPEN:
                    newcost = tgrid[icurrent, jcurrent].cost + costinc;
                    if (newcost < tgrid[i, j].cost)
                    {
                        aupdate(i, j);
                    }
                    break;
            }
        }
    }

    private void getConnectionOccupiedStatic(int i, int j, int oldi, int oldj)
    {
        if (fromDiag)
        {
            costinc = tgrid[i, j].costinc * 1.4f;
        }
        else
        {
            costinc = tgrid[i, j].costinc;
        }
        float newcost;

        if (tgrid[i, j].status != BLOCKED && (!tgrid[i, j].occupiedStatic || (tgrid[i, j].i == IGOAL && tgrid[i, j].j == JGOAL)))
        {
            switch (tgrid[i, j].status)
            {
                case UNVISITED:
                    aupdate(i, j);
                    break;

                case OPEN:
                    newcost = tgrid[icurrent, jcurrent].cost + costinc;
                    if (newcost < tgrid[i, j].cost)
                    {
                        aupdate(i, j);
                    }
                    break;
            }
        }
    }

    private void getConnectionOccupied(int i, int j, int oldi, int oldj)
    {
        if (fromDiag)
        {
            costinc = tgrid[i, j].costinc * 1.4f;
        }
        else
        {
            costinc = tgrid[i, j].costinc;
        }
        float newcost;

        if (tgrid[i, j].status != BLOCKED && (tgrid[i, j].occupied == null || tgrid[i, j].occupied.Equals(parentGameObject)))
        {
            switch (tgrid[i, j].status)
            {
                case UNVISITED:
                    aupdate(i, j);
                    break;

                case OPEN:
                    newcost = tgrid[icurrent, jcurrent].cost + costinc;
                    if (newcost < tgrid[i, j].cost)
                    {
                        aupdate(i, j);
                    }
                    break;
            }
        }
    }

    void awalk()
    {
        int i = icurrent;
        int j = jcurrent;

        if (blockingLevel == 2)
        {
            fromDiag = false;
            getConnectionOccupied(i + 1, j, i, j);
            getConnectionOccupied(i - 1, j, i, j);
            getConnectionOccupied(i, j + 1, i, j);
            getConnectionOccupied(i, j - 1, i, j);

            fromDiag = true;
            getConnectionOccupied(i + 1, j + 1, i, j);
            getConnectionOccupied(i + 1, j - 1, i, j);
            getConnectionOccupied(i - 1, j + 1, i, j);
            getConnectionOccupied(i - 1, j - 1, i, j);
        }
        else if (blockingLevel == 1)
        {
            fromDiag = false;
            getConnectionOccupiedStatic(i + 1, j, i, j);
            getConnectionOccupiedStatic(i - 1, j, i, j);
            getConnectionOccupiedStatic(i, j + 1, i, j);
            getConnectionOccupiedStatic(i, j - 1, i, j);

            fromDiag = true;
            getConnectionOccupiedStatic(i + 1, j + 1, i, j);
            getConnectionOccupiedStatic(i + 1, j - 1, i, j);
            getConnectionOccupiedStatic(i - 1, j + 1, i, j);
            getConnectionOccupiedStatic(i - 1, j - 1, i, j);
        }
        else if (blockingLevel == 0)
        {
            fromDiag = false;
            agetconnection(i + 1, j, i, j);
            agetconnection(i - 1, j, i, j);
            agetconnection(i, j + 1, i, j);
            agetconnection(i, j - 1, i, j);

            fromDiag = true;
            agetconnection(i + 1, j + 1, i, j);
            agetconnection(i + 1, j - 1, i, j);
            agetconnection(i - 1, j + 1, i, j);
            agetconnection(i - 1, j - 1, i, j);
        }

        tgrid[i, j].status = CLOSED;
        openTiles.Remove(tgrid[i, j]);
        closedTiles.Add(tgrid[i, j]);
    }
}
