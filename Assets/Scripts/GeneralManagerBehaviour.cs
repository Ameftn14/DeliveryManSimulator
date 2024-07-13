using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManagerBehaviour : MonoBehaviour {
    public Property theProperty = null;
    public SearchRoad theSearchRoad = null;
    public MapManagerBehaviour theMapManager = null;
    public OrderDB theOrderDB = null;
    public AcceptedUnfinishedOrderDisplayManager displayManager = null;

    public VirtualClockUI virtualClock = null;

    // Start is called before the first frame update
    void Start() {
        //Debug.Assert(theProperty != null);
        Debug.Assert(theSearchRoad != null);
        Debug.Assert(theMapManager != null);
        Debug.Assert(theOrderDB != null);
        Debug.Assert(displayManager != null);
        Debug.Assert(virtualClock != null);
        theProperty = GameObject.Find("Deliveryman").GetComponent<Property>();
    }

    public void DBConfirmOrder(int OrderID)
    {
        Debug.Log("DBConfirmOrder");
        PairOrder theOrder = theOrderDB.orderDict[OrderID];
        if (theProperty.nowCapacity - 1 >= 0) {
            SingleOrder theFrom = theOrder.fromScript;
            SingleOrder theTo = theOrder.toScript;
            TimeSpan dueTime = theOrder.GetDeadline();
            Debug.Log("dueTime: " + dueTime);
            Color color = ColorDictionary.GetColor(theOrder.OrderID);
            displayManager.appendNewOrder(new OrderInfo(dueTime, color, LocationType.Restaurant, theFrom.Getpid(), theOrder.OrderID));
            displayManager.appendNewOrder(new OrderInfo(dueTime, color, LocationType.Customer, theTo.Getpid(), theOrder.OrderID));
            theProperty.nowCapacity -= 1;
        } else
            theOrder.state = PairOrder.State.NotAccept;
    }

    public void LateOrder(int OrderID)
    {
        PairOrder theOrder = theOrderDB.orderDict[OrderID];
        if (theOrder.state == PairOrder.State.Accept)
        {
            displayManager.removeOrder(OrderID, LocationType.Restaurant);
            displayManager.removeOrder(OrderID, LocationType.Customer);
            theProperty.money -= theOrder.GetPrice();
            theProperty.nowCapacity += 1;
        }
        else
        if (theOrder.state == PairOrder.State.PickUp)
        {
            displayManager.removeOrder(OrderID, LocationType.Customer);
            theProperty.money -= theOrder.GetPrice();
            theProperty.nowCapacity += 1;
        }
    }


    // Update is called once per frame
    void Update() {
        if (theSearchRoad.orderFinished && theSearchRoad.isMoving) // 可能会有卡轴的bug 但是好修
        {
            theSearchRoad.isMoving = false;
            SingleOrder theOrder = null;
            PairOrder thePairOrder = theOrderDB.orderDict[theSearchRoad.targetOrderID];
            if (theSearchRoad.targetIsFrom)
                theOrder = thePairOrder.fromScript;
            else
                theOrder = thePairOrder.toScript;
            displayManager.removeOrder(theSearchRoad.targetOrderID, theSearchRoad.targetIsFrom ? LocationType.Restaurant : LocationType.Customer);
            if (theOrder.GetIsFrom()){
                thePairOrder.state = PairOrder.State.PickUp;
            }
            else {
                thePairOrder.state = PairOrder.State.Finished;
                theProperty.nowCapacity += 1;
                if (virtualClock.GetTime() < thePairOrder.GetDeadline())
                    theProperty.money += thePairOrder.GetPrice();
            }
        }
        else {
            OrderInfo theFirstOrder = displayManager.getFirstOrder();
            if(theFirstOrder == null){
                theSearchRoad.targetOrderID = -1;
                theSearchRoad.targetIsFrom = false;
                theSearchRoad.targetwaypoint = -1;
            }
            else if (theFirstOrder.pid != theSearchRoad.targetwaypoint)
            {
                theSearchRoad.targetOrderID = theFirstOrder.orderID;
                theSearchRoad.targetIsFrom = theFirstOrder.locationType == LocationType.Restaurant;
                theSearchRoad.targetwaypoint = theFirstOrder.pid;
                theSearchRoad.isMoving = false;
                theSearchRoad.orderFinished = false;
            }
        }
    }

}