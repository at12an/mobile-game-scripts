using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    private List<string> titles = new List<string> {
        "Protein Power",
        "Protein Power S",
        "Calcium Como",
        "Calcium Como S",
        "Buffet Stretch",
        "Rabid Regurgation",
        "Digestion Demon",
        "Sugar Rush",
        "Absorption",
        "Bloodlust", 
        "Metabolic Aid",
        "Cuisine Expertise",
        "Cuisine Expertise S",
        "Lethal Chode",
        "Replenishment",
        "_",
        "Ravenous Hunger",
        "Hospital Comatose",
        "Monkey Eating",
        "Gigantification",
        "Coke Can",
    };
    private List<string> descriptions = new List<string> {
        "Increase attack by 7.5%", 
        "Increase attack by 15%", 
        "Increase health by 10%",
        "Increase health by 20%",
        "Increase number of enemies storable by 1\nDamage is decreased by 20%",
        "Increase suck and spit explosion size by 15%",
        "Decrease all cooldowns by 10%",
        "Increase movement and projectile speed by 10%",
        "Increase exp gain by 10% and exp magnet by 0.5",
        "Increase healing on kill by 2.5%",
        "Increase health regen by 1%",
        "Increase crit chance by 5% and crit damage by 5%",
        "Increase crit chance by 10% and crit damage by 10%",
        "Increase attack by 100%, projectile speed and size is halfed",
        "Restore 40% hp",
        "Increase projectile rebound by 1\nDamage is decreased by 15%",
        "Increase attack scaling to missing health by 0.4% per 1% hp missing",
        "Get a free revive\nHealth is decreased by 50% on revive",
        "Increase cooldown by 20%\nIncrease attack by 40%",
        "Player size by 100%\nIncrease player damage and health by 40%",
        "Minor increase to most stats",
    };
    private List<Sprite> images = new List<Sprite> ();

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private CanvasGroup canvasGroup;

    public string GetDescription(int index) {
        return descriptions[index];
    }

    public string GetTitles(int index) {
        return titles[index];
    }
    
    public void ApplyUpgrade(int id) {
        if (id == 0) {
            Upgrade0();
        } else if (id == 1) {
            Upgrade1();
        } else if (id == 2) {
            Upgrade2();
        } else if (id == 3) {
            Upgrade3();
        } else if (id == 4) {
            Upgrade4();
        } else if (id == 5) {
            Upgrade5();
        } else if (id == 6) {
            Upgrade6();
        } else if (id == 7) {
            Upgrade7();
        } else if (id == 8) {
            Upgrade8();
        } else if (id == 9) {
            Upgrade9();
        } else if (id == 10) {
            Upgrade10();
        } else if (id == 11) {
            Upgrade11();
        } else if (id == 12) {
            Upgrade12();
        } else if (id == 13) {
            Upgrade13();
        } else if (id == 14) {
            Upgrade14();
        } else if (id == 15) {
            Upgrade15();
        } else if (id == 16) {
            Upgrade16();
        } else if (id == 17) {
            Upgrade17();
        } else if (id == 18) {
            Upgrade18();
        } else if (id == 19) {
            Upgrade19();
        } else if (id == 20) {
            Upgrade20();
        }
        gameObject.SetActive(false);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        Time.timeScale = 1;
    }

    public void Upgrade0() {
        playerStats.atk.IncreaseModifier(0.075f);
    }

    public void Upgrade1() {
        playerStats.atk.IncreaseModifier(0.15f);
    }

    public void Upgrade2() {
        playerStats.ApplyHpModifier(0.1f);
    }

    public void Upgrade3() {
        playerStats.ApplyHpModifier(0.2f);
    }

    public void Upgrade4() {
        playerStats.maxSuck += 1;
        playerStats.damageModifier *= 0.8f;
    }

    public void Upgrade5() {
        playerStats.suckSizeModifier += 0.15f;
        playerStats.explosionSizeMod += 0.15f;
    }

    public void Upgrade6() {
        playerStats.suckCouldown.DecreaseModifier(0.1f);
        playerStats.dashCouldown /= 0.9f;
    }

    public void Upgrade7() {
        playerStats.moveSpeed.IncreaseModifier(0.1f);
        playerStats.spitSpeed.IncreaseModifier(0.1f);
    }

    public void Upgrade8() {
        playerStats.expModifier += 0.1f;
        playerStats.expRange += 0.5f;
    }

    public void Upgrade9() {
        playerStats.lifeSteal += 0.025f;
    }

    public void Upgrade10() {
        playerStats.lifeRegen += 0.02f;
    }

    public void Upgrade11() {
        playerStats.critChance += 0.05f;
        playerStats.critDmg += 0.05f;
    }

    public void Upgrade12() {
        playerStats.critChance += 0.1f;
        playerStats.critDmg += 0.1f;
    }

    public void Upgrade13() {
        playerStats.atk.IncreaseModifier(1f);
        playerStats.spitSpeed.SetBaseValue(playerStats.spitSpeed.GetValue()/2f);
        playerStats.explosionSizeMod /= 2f;
    }

    public void Upgrade14() {
        playerStats.currentHealth += playerStats.maxHealth * 0.4f;
        if (playerStats.currentHealth > playerStats.maxHealth) {
            playerStats.currentHealth = playerStats.maxHealth;
        }
    }

    public void Upgrade15() {
        // Ricochet
        playerStats.damageModifier += 0.15f;
    }

    public void Upgrade16() {
        playerStats.atkHpScale += 0.005f;
    }

    public void Upgrade17() {
        playerStats.revives += 1;
    }

    public void Upgrade18() {
        playerStats.suckCouldown.IncreaseModifier(0.2f);
        playerStats.atk.IncreaseModifier(0.4f);
    }

    public void Upgrade19() {
        playerStats.ApplySizeIncrease(1f);
        playerStats.ApplyHpModifier(0.4f);
        playerStats.atk.IncreaseModifier(0.4f);
    }

    public void Upgrade20() {
        playerStats.critChance += 0.025f;
        playerStats.critDmg += 0.025f;
        playerStats.moveSpeed.IncreaseModifier(0.025f);
        playerStats.suckSizeModifier += 0.025f;
        playerStats.explosionSizeMod += 0.025f;
        playerStats.ApplyHpModifier(0.05f);
        playerStats.atk.IncreaseModifier(0.025f);
        playerStats.suckCouldown.DecreaseModifier(0.025f);
        playerStats.dashCouldown /= 0.975f;
        playerStats.expModifier += 0.025f;
        playerStats.expRange += 0.2f;
    }
}


