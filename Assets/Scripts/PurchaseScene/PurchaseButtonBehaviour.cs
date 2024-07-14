using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButtonBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private UpgradeOption upgradeOption;
    private CanvasGroup canvasGroup;
    public PurchaseMenuBehaviour purchaseMenuBehaviour;
    [SerializeField] private Button button;
    [SerializeField] private bool stillAvailable = false;


    public void OnButtonClicked() {
        if (!stillAvailable) {
            return;
        }
        stillAvailable = !stillAvailable;
        purchaseMenuBehaviour.OnButtonClicked(upgradeOption);
        syncDisplay();
    }
    public void setAvailability(bool isAvailable) {
        stillAvailable = isAvailable;
        syncDisplay();
    }
    void Start() {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        Debug.Assert(purchaseMenuBehaviour != null);
    }
    public void init(UpgradeOption upgradeOption) {
        this.upgradeOption = upgradeOption;
        stillAvailable = upgradeOption.isAvailable;
        syncDisplay();
    }
    void syncDisplay() {
        if (stillAvailable) {
            button.interactable = true;
            canvasGroup.alpha = button.colors.normalColor.a;
        } else {
            button.interactable = false;
            canvasGroup.alpha = button.colors.disabledColor.a;
        }
    }
    // Update is called once per frame
    // void Update() {
    //     syncDisplay();
    // }
}
