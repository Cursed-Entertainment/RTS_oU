using UnityEngine;

[DisallowMultipleComponent]
//Add Mini Map Menu
[RequireComponent(typeof(MiniMap_Controller))]
[RequireComponent(typeof(MiniMap_Materials))]
[RequireComponent(typeof(Camera))]
public class MiniMap_Core : MonoBehaviour
{
    public GameObject GameController;
    public RTS_Core RTSCore;
    public Camera _Camera;

    public MiniMap_Materials Materials;
    public MiniMap_Controller Conroller;

    void Awake()
    {
        GameController = GameObject.FindGameObjectWithTag("manager");       

        _Camera = GetComponent<Camera>();
        Materials = GetComponent<MiniMap_Materials>();
        Conroller = GetComponent<MiniMap_Controller>();
    }

    void Start()
    {
        RTSCore = GameController.GetComponent<RTS_Core>();
    }
}
