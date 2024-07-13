using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VirtualClockUI : MonoBehaviour {
    /* -------------------------------------------------------------------------- */
    /*                            for singleton partern                           */
    /* -------------------------------------------------------------------------- */
    private static VirtualClockUI instance;
    public static VirtualClockUI Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<VirtualClockUI>();
            }
            return instance;
        }
    }
    void OnDestroy() {
        // if I am 'The' instance of this class, set instance to null
        if (instance == this) {
            instance = null;
        }
    }
    void singletonCheck() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    /* -------------------------------------------------------------------------- */
    /*                        rest of the functional codes                        */
    /* -------------------------------------------------------------------------- */

    public TMP_Text timeText;
    //public Button backwardButton;
    //public Button forwardButton;

    // 初始时间（24小时制）
    public int startHour = 8;
    public int startMinute = 30;

    // 虚拟时间的时间间隔（每过真实的一秒钟，虚拟时间增加的分钟数）
    public int timeStepMinutes = 5;

    // 虚拟时间
    private int currentHour;
    private int currentMinute;

    private float timer = 0f;

    void Start() {
        singletonCheck();

        // 设置初始时间
        currentHour = startHour;
        currentMinute = startMinute;

        // 初始化按钮点击事件
        // forwardButton.onClick.AddListener(AdvanceOneHour);
        // backwardButton.onClick.AddListener(RetreatOneHour);

        // 设置初始时间显示
        UpdateTimeDisplay();
    }

    void Update() {
        // 计时
        timer += Time.deltaTime;

        // 每过1秒钟，虚拟时间增加timeStepMinutes分钟
        if (timer >= 1f) {
            timer = 0f;
            UpdateVirtualTime();
        }
    }

    void UpdateVirtualTime() {
        // 增加虚拟时间
        currentMinute += timeStepMinutes;

        if (currentMinute >= 60) {
            currentMinute -= 60;
            currentHour++;
        }

        if (currentHour >= 24) {
            currentHour -= 24;
        }

        // 获取订单刷新频率和数量
        var result = OrderRefreshRate.GetOrderRefreshRate(currentHour, currentMinute);

        // 输出结果
        // Debug.Log($"Virtual Time: {currentHour:D2}:{currentMinute:D2} - TimeInterval: {result.TimeInterval} seconds, Quality: {result.Quality}");

        // 更新时间显示
        UpdateTimeDisplay();
    }

    void UpdateTimeDisplay() {
        timeText.text = $"{currentHour:D2}:{currentMinute:D2}";
    }

    void AdvanceOneHour() {
        currentHour++;
        if (currentHour >= 24) {
            currentHour = 0;
        }
        UpdateTimeDisplay();
    }

    void RetreatOneHour() {
        currentHour--;
        if (currentHour < 0) {
            currentHour = 23;
        }
        UpdateTimeDisplay();
    }

    public (float TimeInterval, int Quality) GetOrderRefreshRate() {
        return OrderRefreshRate.GetOrderRefreshRate(currentHour, currentMinute);
    }
    public TimeSpan GetTime() {
        return new TimeSpan(currentHour, currentMinute, 0);
    }
    //换算真实时间间隔到虚拟时间间隔
    public TimeSpan GetVirtualTime(float realseconds) {
        int virtualminutes = (int)(realseconds / timeStepMinutes);
        return new TimeSpan(0, virtualminutes, 0);//超出60分钟会自动进位
    }
}

public static class OrderRefreshRate {
    private static readonly System.Random random = new System.Random();

    public static (float TimeInterval, int Quantity) GetOrderRefreshRate(int hour, int minute) {
        float baseInterval = 10f; // 基准刷新间隔为 10 秒
        //int quantity; // 基准订单数量为 1

        // 定义高峰期时间段
        TimeSpan breakfastStart = new(7, 20, 0);
        TimeSpan breakfastEnd = new(9, 20, 0);

        TimeSpan lunchStart = new(11, 30, 0);
        TimeSpan lunchEnd = new(13, 20, 0);

        TimeSpan dinnerStart = new(17, 30, 0);
        TimeSpan dinnerEnd = new(19, 20, 0);

        // 当前时间
        TimeSpan currentTime = new TimeSpan(hour, minute, 0);

        // 判断是否在高峰期
        if ((currentTime >= breakfastStart && currentTime <= breakfastEnd) ||
            (currentTime >= lunchStart && currentTime <= lunchEnd) ||
            (currentTime >= dinnerStart && currentTime <= dinnerEnd)) {
            baseInterval = 7f; // 高峰期刷新间隔缩短到 7秒

            // 根据概率决定 quality 的值
            int probability = random.Next(100);

            // if (probability < 40) {
            //     quantity = 2; // 40% 的概率 quality 为 2
            // } else if (probability < 55) {
            //     quantity = 3; // 15% 的概率 quality 为 3
            // } else {
            //     quantity = 1; // 60% 的概率 quality 为 1
            // }
        } else {
            // 非高峰期，根据概率决定 quality 的值
            int probability = random.Next(100);

            // if (probability < 15) {
            //     quantity = 2; // 15% 的概率 quality 为 2
            // } else if (probability < 20) {
            //     quantity = 3; // 5% 的概率 quality 为 3
            // } else {
            //     quantity = 1; // 80% 的概率 quality 为 1
            // }
        }

        // 添加随机浮动性
        float timeInterval = baseInterval + UnityEngine.Random.Range(-1f, 1f); // 浮动范围在±1秒

        return (timeInterval, 1);
    }
}