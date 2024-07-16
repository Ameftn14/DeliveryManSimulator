using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PurchaseMenuBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] public UpgradeOption[] upgradeOptions;
    [SerializeField] public PurchaseButtonBehaviour[] purchaseButtons;

    public Shopping shopping;
    public void OnButtonClicked(UpgradeOption upgradeOption) {
        UpgradeOption option = upgradeOption;
        switch (option.type) {
            case UpgradeType.TempararySpeedBoost:
                Debug.Log("TempararySpeedBoost");
                break;
            case UpgradeType.PermanentSpeedBoost:
                Debug.Log("PermanentSpeedBoost");
                break;
            case UpgradeType.TempararyTimeSlow:
                Debug.Log("TempararyTimeSlow");
                break;
            case UpgradeType.BiggerStorage:
                Debug.Log("BiggerStorage");
                break;
            default:
                Debug.Log("Unknown UpgradeType");
                break;
        }

        shopping.doPurchace(option);
        for (int i = 0; i < purchaseButtons.Length; i++) {
            if (upgradeOptions[i] == option) {
                purchaseButtons[i].setAvailability(false);
            }
        }

    }
    // void example() {
    //     purchaseButtons[0].setAvailability(false);
    //     // purchaseButtons[0].GetType();
    // }

    void Start() {
        shopping = GameObject.Find("Shopping").GetComponent<Shopping>();
        purchaseButtons = GetComponentsInChildren<PurchaseButtonBehaviour>();
        upgradeOptions = new UpgradeOption[purchaseButtons.Length];
        // bool isAvailable = true;
        //TODO 填入真正的isAvailable值
        Debug.Log("purchaseButtons.Length"+purchaseButtons.Length);
        Debug.Log("shopping.options.Length"+shopping.options.Count);
        for (int i = 0; i < purchaseButtons.Length; i++) {
            upgradeOptions[i] = new UpgradeOption((UpgradeType)i, shopping.options[i].isAvailable);
            purchaseButtons[i].init(upgradeOptions[i]);
        }
        for (int i = 0; i < purchaseButtons.Length; i++) {
            if (!upgradeOptions[i].isAvailable) {
                purchaseButtons[i].setAvailability(false);
            }
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.P)){
            Debug.Log("now shopping.shoppingCount in PMB: " + shopping.shoppingCount);
        }
        if (shopping.shoppingCount <= 0) {
            for (int i = 0; i < purchaseButtons.Length; i++) {
                purchaseButtons[i].setAvailability(false);
            }
        }
    }
}
