using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectManager : MonoBehaviour
{
    [SerializeField] Color[] m_Color = new Color[0];
    public ScrollRect m_ScorllRect;
    internal GridLayoutGroup m_GridLayoutGroup;
    internal RectTransform m_RectTransformGrid;
    Queue<ScrollRectItem> m_Item = new Queue<ScrollRectItem>();

    internal float minY = 0;
    internal float maxY = 0;

    async void Start()
    {
        this.m_GridLayoutGroup = this.m_ScorllRect.content.GetComponent<GridLayoutGroup>();
        this.m_RectTransformGrid = this.m_GridLayoutGroup.GetComponent<RectTransform>();
        foreach (var color in m_Color)
        {
            ScrollRectItem item = new GameObject().AddComponent<Image>().AddComponent<ScrollRectItem>();
            m_Item.Enqueue(item);
            item.Show(color, this);
        }
        await Task.Delay(TimeSpan.FromSeconds(0.1f));

        minY = -this.m_RectTransformGrid.rect.height + this.m_GridLayoutGroup.cellSize.y / 2;
        maxY = -this.m_GridLayoutGroup.cellSize.y / 2;
    }

    private void OnDestroy()
    {
        while (m_Item.Count > 0) { Destroy(m_Item.Dequeue()); }
        this.m_Item.Clear();
    }
}