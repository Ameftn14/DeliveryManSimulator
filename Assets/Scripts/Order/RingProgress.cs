using UnityEngine;
using System;

public class RingProgress : MonoBehaviour
{
    public TimeSpan ddl;
    public VirtualClockUI virtualClockUI;
    public SpriteRenderer sp_render; // 环形进度条的 SpriteRenderer 组件
    public float lifetime = 5f; // 存在时间，单位秒
    public TimeSpan acceptTime;
    public PairOrder.State state;
    public bool isFrom;
    private float timer = 5f; // 计时器
    public bool shouldDestroy = false;

    void Start()
    {
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
    }

    void Update()
    {
        if(isFrom){
            UpdateFrom();
        }
        else{
            UpdateTo();
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
        else if (state >= PairOrder.State.Accept && state <= PairOrder.State.Lated){
            //计算（截止时间-当前时间）/（截止时间-接收时间）
            TimeSpan currentTime = virtualClockUI.GetTime();
            TimeSpan lefttime = ddl - currentTime;
            TimeSpan totaltime = ddl - acceptTime;
            float Timeratio = (float)lefttime.TotalSeconds / (float)totaltime.TotalSeconds;
            sp_render.material.SetFloat("_Fill", Timeratio);
            // 如果时间到了，销毁自身 GameObject
            if (currentTime >= ddl)
            {
                shouldDestroy = true;
            }
        }
        else if (state == PairOrder.State.Finished){
            sp_render.material.SetFloat("_Fill", 0);
            shouldDestroy = true;
        }
    }
}
