using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public MapManagerBehaviour mapManager; // 地图管理器
    public VirtualClockUI virtualClock; // 虚拟时钟
    public OrderDB orderDB; // 订单数据库
    public float interval = 8f; // 计时器持续时间，单位秒
    public int quality = 1; // 订单数量
    private float timer; // 计时器

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
        (float _interval, int _quality) = virtualClock.GetOrderRefreshRate();
        this.interval = _interval;
        this.quality = _quality;

        // 初始化计时器
        timer = interval;
        NextOrderID = 0;
        WPcount = mapManager.GetWayPoints().Count;
    }

    void Update()
    {
        // 更新计时器
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GeneratePairs(); // 生成预制件
            (float _interval, int _quality) = virtualClock.GetOrderRefreshRate();
            this.interval = _interval;
            this.quality = _quality;
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
        for (int i = 0; i < quality; i++)
        {
            GeneratePair();
        }
    }

    void GeneratePair(){
        //TODO: 生成一对订单
        GameObject orderPair = Instantiate(orderPairPrefab);
        orderPair.GetComponent<PairOrder>().SetOrderID(NextOrderID);
        NextOrderID++;
        orderDB.AddOrder(orderPair.GetComponent<PairOrder>());
    }
}
