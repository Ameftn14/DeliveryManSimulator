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

    public void DBConfirmOrder(int OrderID)
    {
        //Debug.Log("DBConfirmOrder");
        PairOrder theOrder = theOrderDB.orderDict[OrderID];
        if (theProperty.nowCapacity - 1 >= 0) {
            SingleOrder theFrom = theOrder.fromScript;
            SingleOrder theTo = theOrder.toScript;
            TimeSpan dueTime = theOrder.GetDeadline();
            //Debug.Log("dueTime: " + dueTime);
            Color color = ColorDictionary.PeekColor(theOrder.ColorIndex);
            displayManager.appendNewOrder(new OrderInfo(dueTime, color, LocationType.Restaurant, theFrom.Getpid(), theOrder.OrderID));
            displayManager.appendNewOrder(new OrderInfo(dueTime, color, LocationType.Customer, theTo.Getpid(), theOrder.OrderID));
            theProperty.nowCapacity -= 1;
            theOrder.playMusic("AcceptVoice");
        } else
            theOrder.OrderNotAccept();
    }

    public void LateOrder(int OrderID)
    {
        AudioSource audioSource = GameObject.Find("LateVoice").GetComponent<AudioSource>();
        audioSource.Play();
        PairOrder theOrder = theOrderDB.orderDict[OrderID];
        if (theOrder.state == PairOrder.State.Accept)
            theProperty.money -= theOrder.GetPrice() / 2;
        else if (theOrder.state == PairOrder.State.PickUp) {
            // displayManager.removeOrder(OrderID, LocationType.Customer);
            theProperty.money -= theOrder.GetPrice() / 2;
            // theProperty.nowCapacity += 1;
        }
    }
    public void DistroyOrder(int OrderID) {
        PairOrder theOrder = theOrderDB.orderDict[OrderID];
        theProperty.nowCapacity += 1;
        theProperty.money -= theOrder.GetPrice();
        if (theOrder.state == PairOrder.State.Accept) {
            SingleOrder theFrom = theOrder.fromScript;
            displayManager.removeOrder(OrderID, LocationType.Restaurant);
        }
        SingleOrder theTo = theOrder.toScript;
        displayManager.removeOrder(OrderID, LocationType.Customer);
        return;
    }

    // Update is called once per frame
    void Update() {
        if (theSearchRoad.orderFinished && theSearchRoad.isMoving) {
            SingleOrder theOrder = null;
            PairOrder thePairOrder = theOrderDB.orderDict[theSearchRoad.targetOrderID];
            if (theSearchRoad.targetIsFrom)
                theOrder = thePairOrder.fromScript;
            else
                theOrder = thePairOrder.toScript;
            // change the order's state
            if (theOrder.GetIsFrom()){              
                RandomEventManager.Instance.WhenPickUp(thePairOrder.OrderID);
                thePairOrder.OrderPickUp();
            }
            else {
                if (thePairOrder.state != PairOrder.State.PickUp) {
                    Debug.Assert(thePairOrder.state == PairOrder.State.Accept);
                    return;
                }
                thePairOrder.playMusic("FinishVoice");              
                RandomEventManager.Instance.WhenArrive(thePairOrder.OrderID);
                thePairOrder.OrderFinished();
                theProperty.nowCapacity += 1;
                theProperty.money += thePairOrder.GetPrice();
            }
            // update the display
            displayManager.removeOrder(theSearchRoad.targetOrderID, theSearchRoad.targetIsFrom ? LocationType.Restaurant : LocationType.Customer);
            // change the player's state
            theSearchRoad.isMoving = false;
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