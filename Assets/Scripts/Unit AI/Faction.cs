using UnityEngine;

[DisallowMultipleComponent]
public class Faction : MonoBehaviour
{
    public bool Friend;
    Material[] Materials;

    void Start()
    {
        if (GetComponent<Renderer>())
        {
            Materials = GetComponent<Renderer>().materials;
        }
        else
        {
            Materials = GetComponentInChildren<Renderer>().materials;
        }

        if (Friend)
        {
            gameObject.layer = 8;
            foreach (Material m in Materials)
            {
                if (m.name == "teamColor (Instance)")
                {
                    m.color = RTS_Player.temp.Player_Material.color;
                }
            }
        }
        else
        {
            gameObject.layer = 9;
            gameObject.tag = "npc";
            foreach (Material m in Materials)
            {
                if (m.name == "teamColor (Instance)")
                {
                    m.color = RTS_Player.temp.AI_Material.color;
                }
            }
        }
    }

    public void setFriend(bool b)
    {
        Friend = b;
        if (b)
        {
            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 9;
        }
    }

    public bool isFriend()
    {
        return Friend;
    }

    void OnDestroy()
    {
        foreach (Material m in Materials)
        {
            Destroy(m);
        }
    }
}
