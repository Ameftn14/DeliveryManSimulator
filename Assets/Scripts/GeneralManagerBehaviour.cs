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
        Debug.Assert(theProperty != null);
        Debug.Assert(theSearchRoad != null);
        Debug.Assert(theMapManager != null);
        Debug.Assert(theOrderDB != null);
        Debug.Assert(displayManager != null);
        Debug.Assert(virtualClock != null);
    }

    void DBConfirmOrder(int OrderID) // 需要挂监听
    {
        PairOrder theOrder = theOrderDB.orderDict[OrderID];
        if (theProperty.nowCapacity + 1 <= theProperty.allCapacity) {
            SingleOrder theFrom = theOrder.fromScript;
            SingleOrder theTo = theOrder.toScript;
            TimeSpan dueTime = theOrder.GetDeadline();
            Color color = ColorDictionary.GetColor(theOrder.OrderID);
            displayManager.appendNewOrder(new OrderInfo(dueTime, color, LocationType.Restaurant, theFrom.Getpid(), theOrder.OrderID));
            displayManager.appendNewOrder(new OrderInfo(dueTime, color, LocationType.Customer, theTo.Getpid(), theOrder.OrderID));
            theProperty.nowCapacity += 1;
        } else
            theOrder.state = PairOrder.State.NotAccept;
    }


    // Update is called once per frame
    void Update() {
        if (theSearchRoad.orderFinished) // 可能会有卡轴的bug 但是好修
        {
            SingleOrder theOrder = null;
            PairOrder thePairOrder = theOrderDB.orderDict[theSearchRoad.targetOrderID];
            if (theSearchRoad.targetIsFrom)
                theOrder = thePairOrder.fromScript;
            else
                theOrder = thePairOrder.toScript;
            if (theOrder.GetIsFrom())
                thePairOrder.state = PairOrder.State.PickUp;
            else {
                thePairOrder.state = PairOrder.State.Delivered;
                theProperty.nowCapacity -= 1;
                if (virtualClock.GetTime() < thePairOrder.GetDeadline())
                    theProperty.money += thePairOrder.GetPrice();
            }
        } else {
            OrderInfo theFirstOrder = displayManager.getFirstOrder();
            if (theFirstOrder.pid != theSearchRoad.targetwaypoint) {
                theSearchRoad.targetOrderID = theFirstOrder.orderID;
                theSearchRoad.targetIsFrom = theFirstOrder.locationType == LocationType.Restaurant;
                theSearchRoad.targetwaypoint = theFirstOrder.pid;
                theSearchRoad.isMoving = false;
                theSearchRoad.orderFinished = false;
            }
        }
    }

}