using UnityEngine;
using System;
using System.Xml.Schema;

public class RingProgress : MonoBehaviour
{
    public TimeSpan ddl;
    public TimeSpan eventTime;
    public TimeSpan TimeToDeadline;
    public VirtualClockUI virtualClockUI;
    public SpriteRenderer sp_render; // 环形进度条的 SpriteRenderer 组件
    public float lifetime = 5f; // 存在时间，单位秒
    public TimeSpan acceptTime;
    public PairOrder.State state;
    public bool isFrom;
    private float timer = 5f; // 计时器
    public bool shouldDestroy = false;
    public bool isStop = false;
    public TimeSpan recoveryTime ;

    void Start()
    {
        isStop = false;
        TimeSpan eventTime = new(0, 0, 0);
        ddl = transform.parent.GetComponent<SingleOrder>().Deadline;
        virtualClockUI = GameObject.Find("Time").GetComponent<VirtualClockUI>();
        sp_render = GetComponent<SpriteRenderer>();
        SingleOrder singleOrder = transform.parent.GetComponent<SingleOrder>();
        if (singleOrder != null)
        {
            lifetime = singleOrder.LifeTime;
            timer = lifetime;
        }
        if (sp_render == null)
        {
            Debug.LogError("SpriteRenderer is not assigned!");
            return;
        }
        shouldDestroy = false;

        state = PairOrder.State.NotAccept;
        recoveryTime = new TimeSpan(0, 0, 0);
    }

    void Update()
    {

        SingleOrder parent = transform.parent.GetComponent<SingleOrder>();
        if(isFrom){
            UpdateFrom();
        }
        else{
            UpdateTo();
        }
        if(isStop && isFrom){
            recoveryTime = RandomEventManager.Instance.searchRoad.recoveryTime;
            RingForNotPrepared();
        }
    }

    
    private void UpdateFrom(){
        timer -= Time.deltaTime;

        // 计算当前进度比例
        if(state == PairOrder.State.NotAccept){
            float progress = Mathf.Clamp01(timer / lifetime);

            // 设置填充比例
            sp_render.material.SetFloat("_Fill", progress);

            // 如果时间到了，销毁自身 GameObject
            if (timer <= 0)
            {
                shouldDestroy = true;
            }
        }
        else{
            sp_render.material.SetFloat("_Fill", 0);        
        }
    }

    private void UpdateTo(){
        timer -= Time.deltaTime;
        TimeSpan currentTime = virtualClockUI.GetTime();
        if(currentTime > ddl){
            //把自己的颜色变成红色
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 0f, 0f);
        }

        // 计算当前进度比例
        if(state == PairOrder.State.NotAccept){
            //float progress = Mathf.Clamp01(timer / lifetime);

            // 设置填充比例
            sp_render.material.SetFloat("_Fill", 0);

            // 如果时间到了，销毁自身 GameObject
            if (timer >= lifetime)
            {
                shouldDestroy = true;
            }
        }
        else if (state >= PairOrder.State.Accept && state <= PairOrder.State.PickUp){
            //计算（截止时间-当前时间）/（截止时间-接收时间）
            if (currentTime < ddl)
            {
                TimeSpan lefttime = ddl - currentTime;
                TimeSpan totaltime = ddl - acceptTime;
                float Timeratio = (float)lefttime.TotalSeconds / (float)totaltime.TotalSeconds;
                sp_render.material.SetFloat("_Fill", Timeratio);
            }
            else if(currentTime >= ddl)
            {
                TimeSpan overtime = ddl - currentTime;
                float Timeratio = (float)overtime.TotalSeconds / ((float)TimeToDeadline.TotalSeconds/2);
                if(Timeratio < -1){
                    shouldDestroy = true;
                }
                sp_render.material.SetFloat("_Fill", Timeratio);
            }
            
            // if (currentTime >= ddl)
            // {
            //     shouldDestroy = true;
            // }
        }
        else if (state == PairOrder.State.Finished){
            sp_render.material.SetFloat("_Fill", 0);
            shouldDestroy = true;
        }
    }

    public void RingForNotPrepared(){
        if(isStop){
            //变红色
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 0f, 0f);
            //计算当前进度比例
            float total = (float)(recoveryTime - RandomEventManager.Instance.NPeventTime).TotalSeconds;
            float part = (float)(recoveryTime - virtualClockUI.GetTime()).TotalSeconds;
            float progress = Mathf.Clamp01(part / total);
            //设置填充比例
            sp_render.material.SetFloat("_Fill", progress);
        }
    }   
}
