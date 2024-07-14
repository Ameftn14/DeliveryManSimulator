using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseMenuBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private UpgradeOption[] upgradeOptions;
    [SerializeField] private PurchaseButtonBehaviour[] purchaseButtons;
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
    }


    void Start() {
        purchaseButtons = GetComponentsInChildren<PurchaseButtonBehaviour>();
        upgradeOptions = new UpgradeOption[purchaseButtons.Length];
        bool isAvailable = true;
        for (int i = 0; i < purchaseButtons.Length; i++) {
            upgradeOptions[i] = new UpgradeOption((UpgradeType)i, isAvailable);
            purchaseButtons[i].init(upgradeOptions[i]);
        }
    }
}
