using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform m_RectTransform;
    ScrollRectManager m_ScrollRectManager;
    Canvas m_CanvasThis;
    GraphicRaycaster m_GraphicRaycaster;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localCursor;
        //计算鼠标或触摸事件的局部位置
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_ScrollRectManager.m_ScorllRect.content, eventData.position, eventData.pressEventCamera, out localCursor);
        //重置位置
        this.transform.localPosition = new Vector3(0, Mathf.Clamp(localCursor.y, this.m_ScrollRectManager.minY, this.m_ScrollRectManager.maxY), 0);

        //重定义顺序 - 使用当前Item的位置，除以Item的高度+行间距，得到的值就是当前Item的Order
        int order = (int)(this.transform.localPosition.y / -(this.m_ScrollRectManager.m_GridLayoutGroup.cellSize.y + this.m_ScrollRectManager.m_GridLayoutGroup.spacing.y));
        if (order != this.transform.GetSiblingIndex())
        {
            this.transform.SetSiblingIndex(order);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.m_CanvasThis.sortingOrder = 2;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.m_CanvasThis.sortingOrder = 1;
        ///松开后，刷新列表
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_ScrollRectManager.m_GridLayoutGroup.transform as RectTransform);
    }

    public async void Show(Color m_Color, ScrollRectManager m_ScrollRectManager)
    {
        this.m_ScrollRectManager = m_ScrollRectManager;
        this.transform.SetParent(this.m_ScrollRectManager.m_ScorllRect.content);
        this.GetComponent<Image>().color = m_Color;
        this.transform.name = UnityEngine.ColorUtility.ToHtmlStringRGB(m_Color);
        this.m_RectTransform = this.GetComponent<RectTransform>();
        this.m_RectTransform.pivot = new Vector2(0.5f, 0.5f);

        this.m_CanvasThis = this.AddComponent<Canvas>();
        this.m_GraphicRaycaster = this.AddComponent<GraphicRaycaster>();
        await Task.Yield();
        this.m_CanvasThis.overrideSorting = true;
        this.m_CanvasThis.sortingOrder = 1;
    }
}
