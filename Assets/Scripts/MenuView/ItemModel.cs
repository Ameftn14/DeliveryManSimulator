using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemModel : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
{
    Guid guid; // this can be replaced with actual data class/struct
    [SerializeField] int index;
    [SerializeField] Vector2 originalPosition;
    RectTransform rectTransform;
    [SerializeField] ListModel listModel;


    // Start is called before the first frame update
    void Start()
    {
        guid = Guid.NewGuid();
        index = transform.GetSiblingIndex();
        rectTransform = GetComponent<RectTransform>();

        Debug.Assert(rectTransform != null);
    }
    public void bindTo(ListModel listModel)
    {
        gameObject.transform.SetParent(listModel.gameObject.transform);
        this.listModel = listModel;
        listModel.postDeleteActions += onDeleteAction;
        listModel.preInsertActions += onInsertAction;
    }
    public static ItemModel spawnNewItem(string prefabFilePath = "Prefabs/UI/item")
    {
        //load from Resources/Prefabs/item
        GameObject item = Instantiate(Resources.Load(prefabFilePath, typeof(GameObject))) as GameObject;
        ItemModel itemModel = item.GetComponent<ItemModel>();
        return itemModel;
    }
    void onDeleteAction(int deletedIndex)
    {
        Debug.Log("post delete: " + index);
        if (index < deletedIndex)
            return;
        if (index == deletedIndex)
        {
            suicide();
            return;
        }
        index -= 1;
    }
    void onInsertAction(int instertedIndex)
    {
        if (index < instertedIndex) return;
        index += 1;
    }
    void suicide()
    {
        listModel.postDeleteActions -= onDeleteAction;
        listModel.preInsertActions -= onInsertAction;
        Destroy(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedGameObject = eventData.pointerDrag;
        ItemModel droppedItemModel = droppedGameObject.GetComponent<ItemModel>();
        // TODO more robust

        int droppedIndex = droppedItemModel.getIndex();
        Debug.Log("item " + droppedIndex + "->" + index);

        listModel.swap(index, droppedIndex);
    }
    public void setSiblingIndex(int index)
    {
        transform.SetSiblingIndex(index);
        this.index = index;
    }
    public int getIndex()
    {
        return index;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
        Debug.Log("sibling index: " + transform.GetSiblingIndex());
        index = transform.GetSiblingIndex();
        originalPosition = rectTransform.anchoredPosition;

        // render order thing
        // canvas.sortingOrder++;
        // transform.SetAsLastSibling();
        // rectTransform.anchoredPosition = originalPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // canvas.sortingOrder--;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPosition;
    }
}