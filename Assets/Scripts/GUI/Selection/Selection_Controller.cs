using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Selection_Controller : MonoBehaviour
{
    public static Selection_Controller temp;

    public List<GameObject> Objects;    
    public List<GameObject> BuildingObjects;

    List<GameObject> OverlayObjects;

    List<GameObject> List0 = new List<GameObject>();
    List<GameObject> List1 = new List<GameObject>();
    List<GameObject> List2 = new List<GameObject>();
    List<GameObject> List3 = new List<GameObject>();
    List<GameObject> List4 = new List<GameObject>();
    List<GameObject> List5 = new List<GameObject>();
    List<GameObject> List6 = new List<GameObject>();
    List<GameObject> List7 = new List<GameObject>();
    List<GameObject> List8 = new List<GameObject>();
    List<GameObject> List9 = new List<GameObject>();

    List<List<GameObject>> GroupLists = new List<List<GameObject>>();

    void Start()
    {
        temp = this;

        for (int i = 0; i < 10; i++)
        {
            GroupLists.Add(new List<GameObject>());
        }
    }

    void Update()
    {
        if (Input.GetKey("g"))
        {
            if (Input.GetKeyDown("1"))
            {
                foreach (GameObject g in GroupLists[1])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[1].Clear();

                foreach (GameObject g in Objects)
                {
                    if (g.GetComponent<Faction>().isFriend())
                    {
                        g.GetComponent<Selected>().assignNumber(1);
                        GroupLists[1].Add(g);
                    }
                }
            }
            else if (Input.GetKeyDown("2"))
            {
                foreach (GameObject g in GroupLists[2])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[2].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(2);
                    GroupLists[2].Add(g);
                }
            }
            else if (Input.GetKeyDown("3"))
            {
                foreach (GameObject g in GroupLists[3])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[3].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(3);
                    GroupLists[3].Add(g);
                }
            }
            else if (Input.GetKeyDown("4"))
            {
                foreach (GameObject g in GroupLists[4])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[4].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(4);
                    GroupLists[4].Add(g);
                }
            }
            else if (Input.GetKeyDown("5"))
            {
                foreach (GameObject g in GroupLists[5])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[5].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(5);
                    GroupLists[5].Add(g);
                }
            }
            else if (Input.GetKeyDown("6"))
            {
                foreach (GameObject g in GroupLists[6])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[6].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(6);
                    GroupLists[6].Add(g);
                }
            }
            else if (Input.GetKeyDown("7"))
            {
                foreach (GameObject g in GroupLists[7])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[7].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(7);
                    GroupLists[7].Add(g);
                }
            }
            else if (Input.GetKeyDown("8"))
            {
                foreach (GameObject g in GroupLists[8])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[8].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(8);
                    GroupLists[8].Add(g);
                }
            }
            else if (Input.GetKeyDown("9"))
            {
                foreach (GameObject g in GroupLists[9])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[9].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(9);
                    GroupLists[9].Add(g);
                }

                Debug.Log("added");
            }
            else if (Input.GetKeyDown("0"))
            {
                foreach (GameObject g in GroupLists[0])
                {
                    g.GetComponent<Selected>().assignNumber(-1);
                }
                GroupLists[0].Clear();

                foreach (GameObject g in Objects)
                {
                    g.GetComponent<Selected>().assignNumber(0);
                    GroupLists[0].Add(g);
                }
            }
        }

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown("1"))
            {
                deselectAll();
                foreach (GameObject g in GroupLists[1])
                {
                    addObject(g);
                }

                if (GroupLists[1].Count > 0) Cam_CameraCore.temp.GroupObjects(true);
            }
            else if (Input.GetKeyDown("2"))
            {

            }
            else if (Input.GetKeyDown("3"))
            {

            }
            else if (Input.GetKeyDown("4"))
            {

            }
            else if (Input.GetKeyDown("5"))
            {

            }
            else if (Input.GetKeyDown("6"))
            {

            }
            else if (Input.GetKeyDown("7"))
            {

            }
            else if (Input.GetKeyDown("8"))
            {

            }
            else if (Input.GetKeyDown("9"))
            {

            }
            else if (Input.GetKeyDown("0"))
            {

            }
        }
    }

    public void Deselect()
    {
        foreach (GameObject g in Objects)
        {
            g.GetComponent<Selected>().isSelected = false;
            Destroy(g.GetComponent<GUITexture>());
        }

        Objects.Clear();
    }

    public void deselect(GameObject g)
    {
        g.GetComponent<Selected>().isSelected = false;
        Destroy(g.GetComponent<GUITexture>());
        Objects.Remove(g);
    }

    public void addObject(GameObject g)
    {
        g.GetComponent<Selected>().isSelected = true;
        g.GetComponent<Selected>().JustBeenSelected = true;
        Objects.Add(g);
    }

    public void addBuilding(GameObject g)
    {
        BuildingObjects.Add(g);
    }

    public void DeselectBuildings()
    {
        BuildingObjects.Clear();
    }

    public void deselectAll()
    {
        Deselect();
        DeselectBuildings();
    }

    public bool isSelected()
    {
        if (Objects.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddToGroup(GameObject g, int number)
    {
        GroupLists[number].Add(g);
    }

    public void RemoveFromGroup(GameObject g, int number)
    {
        if (number != -1)
        {
            GroupLists[number].Remove(g);
        }
    }
}
