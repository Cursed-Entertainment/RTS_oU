using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class Unit_NavMesh : MonoBehaviour
{
    Unit_Core Core;
    NavMeshAgent navAgent;

    GameObject navTarget;

    void Awake()
    {
        Core = GetComponent<Unit_Core>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
