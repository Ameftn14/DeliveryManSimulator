using System;
using System.Dynamic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemModel : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler {
    Guid guid; // this can be replaced with actual data class/struct
    [SerializeField] protected int index;
    [SerializeField] protected Vector2 originalPosition;
    protected RectTransform rectTransform;
    [SerializeField] protected ListModel listModel;


    // Start is called before the first frame update
    protected void init() {
        guid = Guid.NewGuid();
        index = transform.GetSiblingIndex();
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }
    void Start() {
        init();
    }
    public void bindTo(ListModel listModel) {
        gameObject.transform.SetParent(listModel.gameObject.transform);
        this.listModel = listModel;
        listModel.postDeleteActions += onDeleteAction;
        listModel.preInsertActions += onInsertAction;
    }
    public static ItemModel spawnNewItem(string prefabFilePath = "Prefabs/UI/item") {
        GameObject item = Instantiate(Resources.Load(prefabFilePath, typeof(GameObject))) as GameObject;
        Debug.Assert(item != null, "Loading prefab failed: path=" + prefabFilePath);
        ItemModel itemModel = item.GetComponent<ItemModel>();
        return itemModel;
    }
    void onDeleteAction(int deletedIndex) {
        Debug.Log("post delete: " + index);
        if (index < deletedIndex)
            return;
        if (index == deletedIndex) {
            suicide();
            return;
        }
        index -= 1;
    }
    void onInsertAction(int instertedIndex) {
        if (index < instertedIndex) return;
        index += 1;
    }
    void suicide() {
        listModel.postDeleteActions -= onDeleteAction;
        listModel.preInsertActions -= onInsertAction;
        Destroy(gameObject);
    }
    public void setSiblingIndex(int index) {
        transform.SetSiblingIndex(index);
        this.index = index;
    }
    public int getIndex() {
        return index;
    }
    virtual public void OnBeginDrag(PointerEventData eventData) {
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
    virtual public void OnDrag(PointerEventData eventData) {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }
    virtual public void OnEndDrag(PointerEventData eventData) {
        // canvas.sortingOrder--;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        // rectTransform.anchoredPosition = originalPosition;
        LayoutRebuilder.ForceRebuildLayoutImmediate(listModel.GetComponent<RectTransform>());
    }
    virtual public void OnDrop(PointerEventData eventData) {
        GameObject droppedGameObject = eventData.pointerDrag;
        ItemModel droppedItemModel = droppedGameObject.GetComponent<ItemModel>();
        // TODO more robust

        int droppedIndex = droppedItemModel.getIndex();
        Debug.Log("item " + droppedIndex + "->" + index);

        listModel.drop(index, droppedIndex);
    }
}