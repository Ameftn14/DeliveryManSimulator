using UnityEngine;
using UnityEngine.UI;
using System;

public class PairOrder : MonoBehaviour {
    public OrderDB orderDB;
    public MapManagerBehaviour mapManager;
    public VirtualClockUI virtualClock;
    public GeneralManagerBehaviour generalManager;
    public int OrderID;
    public int ColorIndex;
    public SingleOrder fromScript;//单个订单:起点
    public SingleOrder toScript;//单个订单:终点
    public int from_pid;
    public TimeSpan NotPreparedTime;
    public int level;
    private int price;
    public int to_pid;
    private float distance;
    public bool isLate;
    public TimeSpan Deadline;
    public TimeSpan AcceptTime;
    public bool isStop;
    //private TimeSpan PickUpTime;
    public TimeSpan TimeToDeadline;
    private float LifeTime;
    private float timer;

    public enum State {
        NotAccept,//未接单
        Accept,//已接单
        PickUp,//已取货
        Finished,//已送达      
    }

    public State state;

    public void Start() {
        generalManager = GameObject.Find("GeneralManager").GetComponent<GeneralManagerBehaviour>();
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        virtualClock = GameObject.Find("Time").GetComponent<VirtualClockUI>();

        state = State.NotAccept;
        //TimeToDeadline = new TimeSpan(1, UnityEngine.Random.Range(45, 61), 0);
        AcceptTime = new(0, 0, 0);
        NotPreparedTime = new(0, 0, 0);
        isStop = false;

        //Deadline = virtualClock.GetTime().Add(TimeToDeadline);

        WayPointBehaviour from_wp = null;
        WayPointBehaviour to_wp = null;
        bool isSameEdge = false;
        float mindistance = 15f;
        float maxdistance = 65f;
        float tempDistance = 0f; 
        //随机获取两个pid
        do {
            from_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
            from_wp = mapManager.GetWayPoints()[from_pid].GetComponent<WayPointBehaviour>();
        } while (from_wp.isBusy || from_wp.isResturant == 0);

        do {
            to_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
            to_wp = mapManager.GetWayPoints()[to_pid].GetComponent<WayPointBehaviour>();
            isSameEdge = from_wp.startVid == to_wp.startVid && from_wp.endVid == to_wp.endVid;
            tempDistance = Vector2.Distance(from_wp.transform.position, to_wp.transform.position);
        } while (to_wp.isBusy || to_wp.isResturant == 1 || to_pid == from_pid || isSameEdge || tempDistance < mindistance || tempDistance > maxdistance);

        mapManager.GetWayPoints()[from_pid].GetComponent<WayPointBehaviour>().BecomeBusy();
        mapManager.GetWayPoints()[to_pid].GetComponent<WayPointBehaviour>().BecomeBusy();
        //获取两个位置
        Vector2 from_position = mapManager.GetWayPoints()[from_pid].transform.position;
        Vector2 to_position = mapManager.GetWayPoints()[to_pid].transform.position;

        Vector3 from_position3 = new Vector3(from_position.x, from_position.y, -5);
        Vector3 to_position3 = new Vector3(to_position.x, to_position.y, -5);

        //修改两个子对象
        Transform childfrom = transform.Find("OrderFrom");
        childfrom.position = from_position3;

        fromScript = childfrom.gameObject.GetComponent<SingleOrder>();
        fromScript.SetIsFrom(true);
        fromScript.SetPid(from_pid);
        fromScript.SetOrderID(OrderID);
        fromScript.parentPairOrder = this;
        fromScript.SetTimeToDeadline(TimeToDeadline);

        Transform childto = transform.Find("OrderTo");
        childto.position = to_position3;

        toScript = childto.gameObject.GetComponent<SingleOrder>();
        toScript.SetIsFrom(false);
        toScript.SetPid(to_pid);
        toScript.SetOrderID(OrderID);
        toScript.parentPairOrder = this;
        toScript.SetTimeToDeadline(TimeToDeadline);

        toScript.brotherSingleOrder = fromScript;
        fromScript.brotherSingleOrder = toScript;
        //设置等级
        SetLevel();
        //设置价格
        SetPrice();
        //设置颜色
        SetColorIndex();  
        SetLiftime();

        SetDeadline();
        //获取距离
        distance = Vector2.Distance(fromScript.GetPosition(), toScript.GetPosition());

        if(level == 4 && !TutorialManagerBehaviour.assign){
            TutorialManagerBehaviour.AssignedOrder();
        }
        else if(level == 5 && !TutorialManagerBehaviour.hot){
            TutorialManagerBehaviour.HotOrder();
        }

        timer = LifeTime;
        state = State.NotAccept;
        isLate = false;
        OrderMenuListBehaviour.Instance.OnMouseHoverOrderChanged += HighLight;
        OrderMenuListBehaviour.Instance.OnMouseOutOrder += Unhighlight;
    }

    public void Update() {

        TimeSpan currentTime = virtualClock.GetTime();
        TimeSpan AllowExceedTime = TimeToDeadline / 2;
        if (currentTime > Deadline + AllowExceedTime) {
            if (!isLate) {
                Debug.LogError("Order " + OrderID + " exceeds the time but not marked as late!");
            } else {
                if(level == 4){
                    //扣钱
                    Property property = GameObject.Find("Deliveryman").GetComponent<Property>();
                    property.money -= price;
                }
                generalManager.DistroyOrder(OrderID);
                OrderFinished();
            }
        }
        //状态传达
        if (state == State.NotAccept) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {//超时未接单   
                if(level == 4){
                    //扣钱
                    Property property = GameObject.Find("Deliveryman").GetComponent<Property>();
                    property.money -= price;
                }
                OrderFinished();
                DistroyEverything();
            }
        } else {
            if (state == State.Finished)//已送达或者过量超时
            {
                OrderFinished();
                orderDB.RemoveOrder(OrderID);
                DistroyEverything();
            }
            if (currentTime > Deadline && isLate == false) {//超时
                generalManager.LateOrder(OrderID);
                OrderLated();
                isLate = true;
            }
        }

        if(isStop){ 
            RingForNotPrepared();
        }
    }

    public void DistroyEverything() {
        orderDB.RemoveOrder(OrderID);
        ColorDictionary.ReleaseColor(ColorIndex);
        Destroy(transform.Find("OrderFrom").gameObject);
        Destroy(transform.Find("OrderTo").gameObject);
        Destroy(gameObject);
        //两个pid变成free
        mapManager.GetWayPoints()[from_pid].GetComponent<WayPointBehaviour>().BecomeFree();
        mapManager.GetWayPoints()[to_pid].GetComponent<WayPointBehaviour>().BecomeFree();
    }

    //下面是接口

    public int GetOrderID() {
        return OrderID;
    }
    public void SetOrderID(int id) {
        this.OrderID = id;
    }

    public int GetPrice() {
        return price;
    }
    //获取订单利润
    public void SetPrice(int price) {
        this.price = price;
    }
    //获取订单距离
    public TimeSpan GetDeadline() {
        return Deadline;
    }
    //获取订单截止时间
    public float GetDistance() {
        return distance;
    }
    public void SetDistance(float distance) {
        this.distance = distance;
    }

    public void SetDeadline() {
        TimeToDeadline = level switch {
            1 => new TimeSpan(2, 0, 0),
            2 => new TimeSpan(1, UnityEngine.Random.Range(45,60), 0),
            3 => new TimeSpan(1, UnityEngine.Random.Range(30,45), 0),
            4 => new TimeSpan(1, 45, 0),
            5 => new TimeSpan(1, UnityEngine.Random.Range(40,55), 0),
            _ => new TimeSpan(2, 0, 0),
        };
        Deadline = virtualClock.GetTime().Add(TimeToDeadline);

        fromScript.SetTimeToDeadline(TimeToDeadline);
        toScript.SetTimeToDeadline(TimeToDeadline);

        fromScript.Deadline = Deadline;
        toScript.Deadline = Deadline;
    }

    // public TimeSpan GetAcceptTime()
    // {
    //     return AcceptTime;
    // }
    // public void SetAcceptTime(TimeSpan acceptTime)
    // {
    //     this.AcceptTime = acceptTime;
    // }

    public State GetState() {
        return state;
    }
    //StateChange
    public void OrderNotAccept() {
        state = State.NotAccept;
        //更新子对象状态
        fromScript.OrderNotAccept();
        toScript.OrderNotAccept();
        orderDB.UpdateOrder(this);
    }
    public void OrderAccept() {
        state = State.Accept;
        //更新子对象状态
        fromScript.OrderAccept();
        toScript.OrderAccept();
        orderDB.UpdateOrder(this);
    }
    public void OrderPickUp() {
        state = State.PickUp;
        //更新子对象状态
        fromScript.OrderPickUp();
        toScript.OrderPickUp();
        orderDB.UpdateOrder(this);
    }
    public void OrderFinished() {
        state = State.Finished;
        //更新子对象状态
        fromScript.OrderFinished();
        toScript.OrderFinished();
        orderDB.UpdateOrder(this);
    }

    public void OrderLated() {
        isLate = true;
        // fromScript.OrderLated();
        // toScript.OrderLated();
    }

    //提供一个接口，调用这个接口时，两个singleoreder对象的大小逐渐变大成原来的两倍
    public void OrderSizeUp(bool isfrom){
        if (isfrom) {
            StartCoroutine(fromScript.SizeUp());
        } else {
            StartCoroutine(toScript.SizeUp());
        }
    }

    // public void OrderSizeDown() {
    //     StartCoroutine(fromScript.SizeDown());
    //     StartCoroutine(toScript.SizeDown());
    // }

    // public void OrderSizeUpAndDown() {
    //     StartCoroutine(fromScript.SizeUpAndDown());
    //     StartCoroutine(toScript.SizeUpAndDown());
    // }

    public void BackToOriginalSize(bool isfrom) {
        if (isfrom) {
            StartCoroutine(fromScript.BackToOriginalSize());
        } else {
            StartCoroutine(toScript.BackToOriginalSize());
        }
    }
    public bool GetIsLate() {
        return isLate;
    }

    public void SetLevel() {
        int probability = UnityEngine.Random.Range(0, 100);
        int day = DeliverymanManager.Instance.round;
        int threshold1, threshold2, threshold3, threshold4;
        switch(day)
        {
            case 0:
                threshold1 = 70;
                threshold2 = 99;
                threshold3 = 100;
                threshold4 = 100;
                // threshold1 = 1;
                // threshold2 = 2;
                // threshold3 = 3;
                // threshold4 = 50;
                break;
            case 1:
                threshold1 = 65;
                threshold2 = 92;//27
                threshold3 = 98;//6
                threshold4 = 100;
                break;
            case 2:
                threshold1 = 55;
                threshold2 = 81;//26
                threshold3 = 93;//12
                threshold4 = 96;
                break;
            case 3:
                threshold1 = 50;
                threshold2 = 75;//25
                threshold3 = 88;//13
                threshold4 = 93;
                break;
            case 4:
                threshold1 = 45;
                threshold2 = 68;//23
                threshold3 = 85;//17
                threshold4 = 90;
                break;
            default:
                threshold1 = 55;
                threshold2 = 85;
                threshold3 = 100;
                threshold4 = 100;
                break;
        }
        if (probability < threshold1) {
            level = 1;
        } else if (probability < threshold2) {
            level = 2;
        } else if(probability < threshold3){
            level = 3;
        }else if(probability < threshold4){
            level = 4;
        }else{
            level = 5;
        }
        fromScript.level = level;
        toScript.level = level;
    }

    public void SetLevel(int level) {
        this.level = level;
        fromScript.level = level;
        toScript.level = level;
    }

    public void SetColorIndex() {
        ColorIndex = ColorDictionary.GetColorIndex(OrderID);
        fromScript.ColorIndex = ColorIndex;
        toScript.ColorIndex = ColorIndex;
    }

    public void SetPrice() {
        WeatherManager.Weather weather = WeatherManager.Instance.GetWeather();
        int priceOnlevel = level switch {
            1 => UnityEngine.Random.Range(25, 50),
            2 => UnityEngine.Random.Range(45, 70),
            3 => UnityEngine.Random.Range(65, 95),
            4 => UnityEngine.Random.Range(55, 80),
            5 => UnityEngine.Random.Range(130, 165),
            _ => 30,
        };
        if(weather == WeatherManager.Weather.Rainy){
            priceOnlevel += 20;
        }
        price = priceOnlevel + (int)(distance * 0.2);
    }

    public void HighLight(OrderInfo orderinfo) {
        if (orderinfo == null) {
            return;
        } else if (orderinfo.orderID == OrderID) {
            OrderSizeUp(orderinfo.pid == from_pid);
        }
    }

    public void Unhighlight(OrderInfo orderinfo) {
        if (orderinfo == null) {
            return;
        } else if (orderinfo.orderID == OrderID) {
            BackToOriginalSize(orderinfo.pid == from_pid);
        }
    }
    public void playMusic(string musicName) {
        Debug.Log("playMusic: " + musicName);
        GameObject music = GameObject.Find(musicName);
        Debug.Assert(music != null);
        music.GetComponent<AudioSource>().PlayOneShot(music.GetComponent<AudioSource>().clip);
    }

    public void RingForNotPrepared() {
        fromScript.ringProgress.recoveryTime = NotPreparedTime;        
    }

    public void SetIsStop(bool isStop){
        this.isStop = isStop;
        fromScript.ringProgress.isStop = isStop;
        if(!isStop){
            fromScript.SetUnvisible();
        }
    } 


    public void SetLiftime(){
        WeatherManager.Weather weather = WeatherManager.Instance.GetWeather();
        LifeTime = weather switch {
            WeatherManager.Weather.Cloudy => 2.3f,
            WeatherManager.Weather.Foggy => 4.2f,
            _ => 3f,
        };
        if(level == 4){
            LifeTime += 2f;
        }
        if(level == 5){
            LifeTime -= 1.85f;
            if(weather == WeatherManager.Weather.Cloudy){
                LifeTime = 0.75f;
            }
        }

        fromScript.LifeTime = LifeTime;
        toScript.LifeTime = LifeTime;
    }

    public void MouseEnter() {
        //TODO:接口
    }

    public void MouseExit() {
        //TODO:接口
    }

}