using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StatScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerAttack;
    [SerializeField] private TextMeshProUGUI playerGold;
    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private TextMeshProUGUI atkCost;
    [SerializeField] private TextMeshProUGUI healthCost;
    [SerializeField] private TextMeshProUGUI traitCost;
    [SerializeField] private TextMeshProUGUI traitEffect;

    void Awake() {
        if (!PlayerPrefs.HasKey("attack"))
        {
            PlayerPrefs.SetInt("attack", 100);
        }
        if (!PlayerPrefs.HasKey("health"))
        {
            PlayerPrefs.SetInt("health", 1000);
        }
        if (!PlayerPrefs.HasKey("gold"))
        {
            PlayerPrefs.SetInt("gold", 0);
        }
        if (!PlayerPrefs.HasKey("numAtkUpgrades"))
        {
            PlayerPrefs.SetInt("numAtkUpgrades", 0);
        }
        if (!PlayerPrefs.HasKey("numHpUpgrades"))
        {
            PlayerPrefs.SetInt("numHpUpgrades", 0);
        }

        if (!PlayerPrefs.HasKey("traitLvl"))
        {
            PlayerPrefs.SetInt("traitLvl", 0);
        }

        if (!PlayerPrefs.HasKey("startSpecial"))
        {
            PlayerPrefs.SetInt("startSpecial", 0);
        }

        if (!PlayerPrefs.HasKey("numSuck"))
        {
            PlayerPrefs.SetInt("numSuck", 1);
        }
    }

    void Update() {
        if (PlayerPrefs.GetInt("traitLvl") >= (int)((PlayerPrefs.GetInt("numAtkUpgrades") + PlayerPrefs.GetInt("numHpUpgrades"))/10)) {
            traitEffect.text = "Next unlock in " + (10 - (PlayerPrefs.GetInt("numAtkUpgrades") + PlayerPrefs.GetInt("numHpUpgrades")) % 10);
            traitCost.text = "LOCKED";
        } else if (PlayerPrefs.GetInt("numAtkUpgrades") + PlayerPrefs.GetInt("numHpUpgrades") >= 20 && PlayerPrefs.GetInt("traitLvl") == 1) {
            traitEffect.text = "Start game with random special";
            traitCost.text = (10 *Math.Pow(4, PlayerPrefs.GetInt("traitLvl"))).ToString() + " GOLD";
        } else {
            traitEffect.text = "Increase enemies storable by 1";
            traitCost.text = (10 *Math.Pow(4, PlayerPrefs.GetInt("traitLvl"))).ToString() + " GOLD";
        }
        playerAttack.text = PlayerPrefs.GetInt("attack").ToString() + " ATK";
        playerGold.text = PlayerPrefs.GetInt("gold").ToString() + " GOLD";
        playerHealth.text = PlayerPrefs.GetInt("health").ToString() + " HP";
        atkCost.text = ((int)(20 * Math.Pow(1.2,PlayerPrefs.GetInt("numAtkUpgrades")))).ToString() + " GOLD";
        healthCost.text = ((int)(20  * Math.Pow(1.2,PlayerPrefs.GetInt("numHpUpgrades")))).ToString() + " GOLD";
    }
    public void IncreaseAttack() {
        if (PlayerPrefs.GetInt("gold") >=(int)( 20 * Math.Pow(1.2,PlayerPrefs.GetInt("numAtkUpgrades")))) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold")-(int)(20 * Math.Pow(1.2,PlayerPrefs.GetInt("numAtkUpgrades"))));
            PlayerPrefs.SetInt("attack", PlayerPrefs.GetInt("attack") + 1);
            PlayerPrefs.SetInt("numAtkUpgrades", PlayerPrefs.GetInt("numAtkUpgrades") + 1);
        }
    }

    public void IncreaseHealth() {
        if (PlayerPrefs.GetInt("gold") >= (int)( 20 * Math.Pow(1.2,PlayerPrefs.GetInt("numAtkUpgrades")))) {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold")-(int)(20 * Math.Pow(1.2,PlayerPrefs.GetInt("numHpUpgrades"))));
            PlayerPrefs.SetInt("health", PlayerPrefs.GetInt("health") + 10);
            PlayerPrefs.SetInt("numHpUpgrades", PlayerPrefs.GetInt("numHpUpgrades") + 1);
        }
    }

    public void GetNextTrait() {
        if (PlayerPrefs.GetInt("gold") >= 10 *Math.Pow(4, PlayerPrefs.GetInt("traitLvl"))) {
            if (PlayerPrefs.GetInt("traitLvl") < (int)((PlayerPrefs.GetInt("numAtkUpgrades") + PlayerPrefs.GetInt("numHpUpgrades"))/10)) {
                PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold")-(int)(10 *Math.Pow(4, PlayerPrefs.GetInt("traitLvl"))));
                if (PlayerPrefs.GetInt("traitLvl") == 1) {
                    PlayerPrefs.SetInt("startSpecial", 1);
                    PlayerPrefs.SetInt("traitLvl", PlayerPrefs.GetInt("traitLvl") + 1);
                } else {
                    PlayerPrefs.SetInt("numSuck", PlayerPrefs.GetInt("numSuck") + 1);
                    PlayerPrefs.SetInt("traitLvl", PlayerPrefs.GetInt("traitLvl") + 1);
                }
            }
        }
    }
}
