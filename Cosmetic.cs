using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Cosmetic : MonoBehaviour
{
    [SerializeField] private Button purchase;
    [SerializeField] private TextMeshProUGUI purchaseText;
    [SerializeField] private TextMeshProUGUI playerGold;

    private void Update() {
        purchase.onClick.RemoveAllListeners();
        if (PlayerPrefs.HasKey("skin1")) {
            purchaseText.text = "Equip";
            purchase.onClick.AddListener(() => ApplySkin1());
        }
        playerGold.text = PlayerPrefs.GetInt("gold").ToString() + " GOLD";
    }
 
    public void AttemptPurchase() {
        if (!PlayerPrefs.HasKey("skin1")) {
            if (PlayerPrefs.HasKey("gold")) {
                if (PlayerPrefs.GetInt("gold") >= 10000) {
                    PlayerPrefs.SetInt("skin1", 1);
                    PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - 10000);
                }
            } else {
                PlayerPrefs.SetInt("gold", 20000);
            }
        }
    }

    public void ApplySkin1() {
        EquipSkin(1);
    }

    public void EquipSkin(int skinId) {
        PlayerPrefs.SetInt("skinEquipped", skinId);
    }
}
