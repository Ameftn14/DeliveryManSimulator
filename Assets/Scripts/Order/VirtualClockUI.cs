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

    // if I am 'The' instance of this class, set instance to null
    void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }

    // call this in the Start() method to check if there is already an instance of this class
    bool AlreadyInitialised() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return true;
        } else {
            instance = this;
            return false;
        }
    }
    /* -------------------------------------------------------------------------- */
    /*                        rest of the functional codes                        */
    /* -------------------------------------------------------------------------- */

    public TMP_Text timeText;
    //public Button backwardButton;
    //public Button forwardButton;

    // 初始时间（24小时制）
    public int startHour;
    public int startMinute;
    // 虚拟时间的时间间隔（每过真实的一秒钟，虚拟时间增加的分钟数）
    //public int timeStepMinutes = 5;
    //虚拟时间走一分钟所需要的真实时间
    public float VMinInSeconds;

    // 虚拟时间
    private int currentHour;
    private int currentMinute;

    private float timer = 0f;

    void Start() {
        if (AlreadyInitialised()) return;

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
        if (timer >= VMinInSeconds) {
            timer = 0f;
            UpdateVirtualTime();
        }
    }


    void UpdateVirtualTime() {
        // 增加虚拟时间
        currentMinute += 1;

        if (currentMinute >= 60) {
            currentMinute -= 60;
            currentHour++;
        }

        if (currentHour >= 24) {
            currentHour -= 24;
        }

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
    public TimeSpan GetVirtualTime(float RealSeconds) {
        float virtualMinuts = RealSeconds * VMinInSeconds;
        int hour = (int)virtualMinuts / 60;
        int minute = (int)virtualMinuts % 60;
        int second = (int)((virtualMinuts - hour * 60 - minute) * 60);

        return new TimeSpan(hour, minute, second);
    }

    public float GetRealTime(TimeSpan VirtualTime) {
        return (VirtualTime.Hours * 60 + VirtualTime.Minutes + VirtualTime.Seconds / 60) * VMinInSeconds;
    }

    public float GetRealTime(int virtualMinuts) {
        return virtualMinuts * VMinInSeconds;
    }
    public TimeSpan GetDayStart() {
        return new TimeSpan(startHour, startMinute, 0);
    }
    public TimeSpan GetDayEnd() {
        // TODO 获取一天强制计算时间
        return new TimeSpan(22, 0, 0);
    }
    public TimeSpan GetLunchStart() {
        return new TimeSpan(11, 30, 0);
    }

    public TimeSpan GetLunchEnd() {
        return new TimeSpan(14, 00, 0);
    }

    public TimeSpan GetDinnerStart() {
        return new TimeSpan(17, 00, 0);
    }

    public TimeSpan GetDinnerEnd() {
        return new TimeSpan(19, 00, 0);
    }
}

public static class OrderRefreshRate {
    private static readonly System.Random random = new();

    public static (float TimeInterval, int Quantity) GetOrderRefreshRate(int hour, int minute) {
        float baseInterval = 5f;

        int quantity; // 基准订单数量为 1

        TimeSpan lunchStart = new(11, 30, 0);
        TimeSpan lunchEnd = new(14, 00, 0);

        TimeSpan dinnerStart = new(17, 00, 0);
        TimeSpan dinnerEnd = new(19, 00, 0);

        // 当前时间
        TimeSpan currentTime = new(hour, minute, 0);
        int probabilityBound = 30;
        // 判断是否在高峰期
        if ((currentTime >= lunchStart && currentTime <= lunchEnd) ||
            (currentTime >= dinnerStart && currentTime <= dinnerEnd)) {
            switch (DeliverymanManager.Instance.round) {
                case 0:
                    baseInterval = 3.5f;
                    probabilityBound = 30;
                    break;
                case 1:
                case 2:
                    baseInterval = 3.0f;
                    probabilityBound = 35;
                    break;
                case 3:
                case 4:
                    baseInterval = 2.5f;
                    probabilityBound = 40;
                    break;
                default:
                    baseInterval = 2.0f;
                    probabilityBound = 45;
                    break;
            }
            // 根据概率决定 quality 的值
            int probability = random.Next(100);

            if (probability < probabilityBound) {
                quantity = 2; // 40% 的概率 quality 为 2
            }
            // else if (probability < 30) {
            //     quantity = 3; // 15% 的概率 quality 为 3
            // } 
            else {
                quantity = 1; // 60% 的概率 quality 为 1
            }
        } else {
            switch (DeliverymanManager.Instance.round) {
                case 0:
                    baseInterval = 5.0f;
                    break;
                case 1:
                case 2:
                    baseInterval = 4.5f;
                    break;
                case 3:
                case 4:
                    baseInterval = 4.0f;
                    break;
                default:
                    baseInterval = 3.5f;
                    break;
            }
            // 非高峰期，根据概率决定 quality 的值
            int probability = random.Next(100);

            if (probability < 20) {
                quantity = 2; // 15% 的概率 quality 为 2
            }
            // else if (probability < 15) {
            //     quantity = 3; // 5% 的概率 quality 为 3
            // } 
            else {
                quantity = 1; // 80% 的概率 quality 为 1
            }
        }

        // 添加随机浮动性
        float timeInterval = baseInterval + UnityEngine.Random.Range(-0.7f, 0.7f); // 浮动范围在±1秒

        return (timeInterval, quantity);
    }
}