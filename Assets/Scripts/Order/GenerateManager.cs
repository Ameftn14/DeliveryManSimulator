using System;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public MapManagerBehaviour mapManager; // 地图管理器
    public VirtualClockUI virtualClock; // 虚拟时钟
    public OrderDB orderDB; // 订单数据库
    public float interval = 6.0f; // 计时器持续时间，单位秒
    public int quantity = 1; // 订单数量
    private float timer; // 计时器
    public TimeSpan cutoffTime; // 结算时间

    public static int NextOrderID; // 订单编号

    public GameObject orderPairPrefab; // 订单预制件
    public int WPcount = 0;

    void Start()
    {
        // 获取虚拟时钟 GameObject 的 VirtualClock 组件
        virtualClock = GameObject.Find("Time").GetComponent<VirtualClockUI>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        if (virtualClock == null)
        {
            Debug.LogError("VirtualClock is not assigned!");
            return;
        }
        if (mapManager == null)
        {
            Debug.LogError("MapManager is not assigned!");
            return;
        }
        if (orderDB == null)
        {
            Debug.LogError("OrderDB is not assigned!");
            return;
        }
        (float _interval, int _quantity) = virtualClock.GetOrderRefreshRate();
        interval = _interval;
        quantity = _quantity;

        // 初始化计时器
        timer = 3f;
        NextOrderID = 0;
        WPcount = mapManager.GetWayPoints().Count;
        cutoffTime = new TimeSpan(22, 0, 0);
    }

    void Update()
    {
        // 更新计时器
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if( virtualClock.GetTime() < (cutoffTime - new TimeSpan(3,0,0)))
            {
                GeneratePairs(); // 生成预制件
            }
            (float _interval, int _quantity) = virtualClock.GetOrderRefreshRate();
            interval = _interval;
            quantity = _quantity;
            timer = interval;
        }

        // if( Input.GetKeyDown(KeyCode.Space) )
        // {
        //     GeneratePrefab();
        // }

        // if( Input.GetKeyDown(KeyCode.Q) )
        // {
        //     //退出游戏
        //     Application.Quit();
        // }
    }

    void GeneratePairs()
    {
        for (int i = 0; i < quantity; i++)
        {
            GeneratePair();
        }
        if (TutorialManagerBehaviour.addlist == false) {
            TutorialManagerBehaviour.AddList();
        }
    }

    void GeneratePair(){
        GameObject orderPair = Instantiate(orderPairPrefab);
        orderPair.GetComponent<PairOrder>().SetOrderID(NextOrderID);
        NextOrderID++;
        orderDB.AddOrder(orderPair.GetComponent<PairOrder>());
    }

    public void SetCutoffTime(TimeSpan time)
    {
        cutoffTime = time;
    }
}
