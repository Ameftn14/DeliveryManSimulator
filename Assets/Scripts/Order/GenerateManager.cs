using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    public GameObject VirtualClockUI; // 虚拟时钟 GameObject
    public float interval = 8f; // 计时器持续时间，单位秒
    public int quality = 1; // 订单数量
    private float timer; // 计时器

    public static int orderid; // 订单编号

    public GameObject orderFromPrefab; // OrderFrom 预制件
    public GameObject orderToPrefab; // OrderTo 预制件

    void Start()
    {
        // 获取虚拟时钟 GameObject 的 VirtualClock 组件
        VirtualClockUI virtualClock = VirtualClockUI.GetComponent<VirtualClockUI>();
        if (virtualClock == null)
        {
            Debug.LogError("VirtualClock is not assigned!");
            return;
        }
        (float _interval, int _quality) = virtualClock.GetOrderRefreshRate();
        this.interval = _interval;
        this.quality = _quality;

        // 初始化计时器
        timer = interval;
        orderid = 0;
    }

    void Update()
    {
        // 更新计时器
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            VirtualClockUI virtualClock = VirtualClockUI.GetComponent<VirtualClockUI>();
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
    }
}
