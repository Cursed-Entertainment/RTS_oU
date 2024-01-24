using UnityEngine;

public class ThreatInfo : MonoBehaviour
{
    public MediumLevel handlerAI;
    public Collider threat;
    public bool dealtWith = false;

    void Start()
    {
        threat = gameObject.GetComponent<Collider>();
        RTS_AI.temp.addThreatInfo(this);
    }

    void Update()
    {
        if (!RTS_AI.temp.isThreat(GetComponent<Collider>()))
        {
            Destroy(this);
        }
    }

    public void setProperties(Collider threat)
    {
        dealtWith = false;
    }

    public void addHandler(MediumLevel AI)
    {
        handlerAI = AI;
        dealtWith = true;
    }

    public void removeHandler()
    {
        dealtWith = false;
        handlerAI = null;
    }

    void OnDestroy()
    {
        RTS_AI.temp.removeThreatInfo(this);
    }
}
