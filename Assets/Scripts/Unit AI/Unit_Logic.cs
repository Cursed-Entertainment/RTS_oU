using UnityEngine;

[DisallowMultipleComponent]
public class Unit_Logic : MonoBehaviour
{
    Unit_Core Core;

    bool underOrders = false;

    public int timeDelay = 1;
    float timeDelayCounter;

    Collider[] enemiesInRange;

    float range;

    int orders = 0;

    Transform target;

    MediumLevel controller;

    Vector3 orderLocation;
    bool needToGiveOrders = false;
    int delayedOrder;
    Vector3 delayedLocation;
    GameObject delayedTarget;

    bool breakToAttack = false;

    bool checking = false;

    void Awake()
    {
        Core = GetComponent<Unit_Core>();
    }

    void Start()
    {
        range = Core.Stats.Range;
        timeDelayCounter = timeDelay;

        if (!Core.Stats.isOreTruck) RTS_AI.temp.requestAI(gameObject);
    }

    void Update()
    {
        timeDelayCounter += Time.deltaTime;

        enemiesInRange = Physics.OverlapSphere(transform.position, range * 1.1f, 1 << 8);
        if (orders == 1)
        {
            if (target == null)
            {
                underOrders = false;

            }
        }
        else if (orders == 2)
        {
            if (enemiesInRange.Length > 0)
            {
                breakToAttack = true;
                giveCommand(2, enemiesInRange[0].gameObject);
            }

            if (breakToAttack && enemiesInRange.Length == 0)
            {
                breakToAttack = false;
                controller.requestOrder(this);
            }
        }
        else if (orders == 3)
        {

        }
        else if (orders == 4)
        {
        }

        if (!Core.UnitMovement.JustBeenCreated && needToGiveOrders)
        {
            if (delayedLocation != Vector3.zero)
            {
                underOrders = true;
                Core.UnitMovement.SetTarget(delayedLocation);
                orders = delayedOrder;
                orderLocation = delayedLocation;
            }
            else
            {
                underOrders = true;
                orders = delayedOrder;
                Core.attack(delayedTarget, false);
                target = delayedTarget.transform;
            }
            needToGiveOrders = false;
        }

        if (GetComponent<Selected>().isSelected)
        {

        }
    }

    public void giveCommand(int command, Vector3 position)
    {
        Core.stopAttack();

        if (Core.UnitMovement.JustBeenCreated)
        {
            needToGiveOrders = true;
            delayedOrder = command;
            delayedLocation = position;
            delayedTarget = null;
        }
        else
        {
            underOrders = true;
            Core.UnitMovement.SetTarget(position);
            orders = command;
            orderLocation = position;
        }

    }

    public void giveCommand(int command, GameObject target)
    {
        if (Core.UnitMovement.JustBeenCreated)
        {
            needToGiveOrders = true;
            delayedOrder = command;
            delayedTarget = target;
            delayedLocation = Vector3.zero;
        }
        else
        {
            underOrders = true;
            orders = command;
            Core.attack(target, false);
            this.target = target.transform;
        }
    }

    void stop()
    {

    }

    void setTarget(Vector3 pos)
    {

    }

    void setTarget(Transform target)
    {

    }

    public bool hasController()
    {
        if (controller == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void setController(MediumLevel m)
    {
        controller = m;
    }

    public void removeController()
    {
        controller = null;
    }

    void OnDestroy()
    {
        if (controller)
        {
            controller.removeUnit(gameObject);
            RTS_AI.temp.removeUnit(gameObject);
        }
    }

    public bool getOrders()
    {
        return underOrders;
    }
}
