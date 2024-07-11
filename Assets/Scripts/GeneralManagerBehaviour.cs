// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GeneralManagerBehaviour : MonoBehaviour
// {
//     public Property theProperty = null;
//     public SearchRoad theSearchRoad = null;
//     public MapManagerBehaviour theMapManager = null;
//     public orderDB theOrderDB = null;
//     public MenuView theMenuView = null;

//     // Start is called before the first frame update
//     void Start()
//     {
//         Debug.Assert(theProperty != null);
//         Debug.Assert(theSearchRoad != null);
//         Debug.Assert(theMapManager != null);
//         Debug.Assert(theOrderDB != null);
//         Debug.Assert(theMenuView != null);
//     }

//     void DBConfirmOrder(PairOrder theOrder) // 需要挂监听
//     {
//         if (theProperty.nowCapacity + 1 <= theProperty.allCapacity)
//         {
//             theMenuView.新增项目(theOrder.OrderFrom);
//             theMenuView.新增项目(theOrder.OrderTo);
//             theProperty.nowCapacity += 1;
//         }
//         else
//             theOrder.state = NotAccept;
//     }


//     // Update is called once per frame
//     void Update()
//     {
//         if (theSearchRoad.orderFinished) // 可能会有卡轴的bug 但是好修
//         {
//             SingleOrder FinishedOrder = theMenuView.去掉该项目(theSearchRoad.targetwaypoint);
//             PairOrder thePairOrder = FinishedOrder.parentPairOrder;
//             if (FinishedOrder.isFrom)
//                 thePairOrder.state = PickUp;
//             else
//             {
//                 thePairOrder.state = Delivered;
//                 theProperty.nowCapacity -= 1;
//                 if (当前时间 < thePairOrder.Deadline)
//                     theProperty.money += thePairOrder.price;
//             }
//         }
//         else if (theMenuView.第一项的pid() != theSearchRoad.targetwaypoint)
//         {
//             theSearchRoad.targetwaypoint = theMenuView.第一项的pid();
//             theSearchRoad.isMoving = false;
//         }
//     }

// }