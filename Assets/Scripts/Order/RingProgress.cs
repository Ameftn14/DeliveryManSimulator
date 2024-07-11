using UnityEngine;

public class RingProgress : MonoBehaviour
{
    public SpriteRenderer sp_render; // 环形进度条的 SpriteRenderer 组件
    public float transitionDuration = 5f; // 过渡时间，单位秒

    private float timer = 5f; // 计时器

    void Start()
    {
        sp_render = GetComponent<SpriteRenderer>();
        SingleOrder singleOrder = transform.parent.GetComponent<SingleOrder>();
        if (singleOrder != null)
        {
            transitionDuration = singleOrder.LifeTime;
            timer = transitionDuration;
        }
        if (sp_render == null)
        {
            Debug.LogError("SpriteRenderer is not assigned!");
            return;
        }

    }

    void Update()
    {
        timer -= Time.deltaTime;

        // 计算当前进度比例
        float progress = Mathf.Clamp01(timer / transitionDuration);

        // 设置填充比例
        sp_render.material.SetFloat("_Fill", progress);

        // 如果时间到了，销毁自身 GameObject
        if (timer >= transitionDuration)
        {
            Destroy(gameObject);
        }
    }
}
