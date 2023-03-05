using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specials : MonoBehaviour
{
    [SerializeField] PauseFunction pauseFunction;
    private List<string> titles = new List<string> {
        "Sporuption",
        "Diabetic Delirium",
        "Flatulent Fast",
        "Steel Spit",
        "Toxin Tummy",
        "Vortex",
        "Auto Aim",
        "Sumo Slam",
        "Spore Parasite"
    };

    private List<string> descriptions = new List<string> {
        "Enemies eaten are infected with sporupters that spread and explode on death",
        "Players falls into hallucination, manifesting a second self",
        "Player is able to dash on cooldown",
        "Enemies eaten are forged into a metal blade",
        "Develop a toxinous stomach biome and regurgitate poison pools",
        "Player able to create a vortex like suction",
        "Automatic aiming on suction and regurgitation",
        "Player heal and gain macho of enemies, replacing regurgitation with a body slam",
        "Spore Parasite"
    };

    private List<string> subsequents = new List<string> {
        "Subsequent upgrades increase explosion radius and lifespan of spores",
        "Subsequent upgrades increase stomach storage by 1",
        "Subsequent upgrades increase dash speed and decrease couldown",
        "Subsequent upgrades increase the speed and lifespan of blade",
        "Subsequent upgrades increase slow and size of poison pool",
        "Subsequent upgrades increase suction size and decrease cooldown",
        "Subsequent upgrades increase suction size and projectile range",
        "Spore Parasite"
    };

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject dashButton;
    [SerializeField] private GameObject dilirium;
    [SerializeField] private GameObject player;
    [SerializeField] private CanvasGroup canvasGroup;

    public string GetDescription(int index) {
        return descriptions[index];
    }

    public string GetSubsequents(int index) {
        return subsequents[index];
    }

    public string GetTitles(int index) {
        return titles[index];
    }
    
    public void ApplyUpgrade(int id) {
        player.transform.Find("MushroomHat").gameObject.SetActive(true);
        if (id == 0) {
            Sporuption();
        } else if (id == 1) {
            DiabeticDelirium();
        } else if (id == 2) {
            FungalFeet();
        } else if (id == 3) {
            CrystalCoom();
        } else if (id == 4) {
            PoisonPool();
        } else if (id == 5) {
            Vortex();
        } else if (id == 6) {
            AutoAim();
        }
        gameObject.SetActive(false);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        pauseFunction.paused = false;
    }

    private void Sporuption() {
        if (playerStats.sporuptionLvl == 0) {
            playerStats.sporuptionLvl = 2;
            playerStats.explosionSizeMod += 0.5f;
        } else {
            playerStats.sporuptionLvl += 1;
            playerStats.explosionSizeMod += 0.25f;
        }
    }   

    private void FungalFeet() {
        if (!dashButton.activeInHierarchy) {
            dashButton.SetActive(true);
        } else {
            playerStats.dashCouldown = playerStats.dashCouldown * 0.9f;
            playerStats.moveSpeed.IncreaseModifier(0.2f);
        }
    }

    private void DiabeticDelirium() {
        if (!playerStats.delirium) {
            dilirium.SetActive(true);
            playerStats.delirium = true;
        } else {
            playerStats.maxSuck += 1;
        }
    }

    private void CrystalCoom() {
        if (!playerStats.crystalProj) {
            playerStats.crystalProj = true;
            playerStats.suckCouldown.SetBaseValue(4f);
        } else {
            // Increase projectile speed, size and lifetime
            playerStats.spitSpeed.IncreaseModifier(0.25f);
            playerStats.maxSuck += 1;
            playerStats.crystalLife += 1f;
        }
    }
    
    private void PoisonPool(){
        if (!playerStats.poisonAttack) {
            playerStats.poisonAttack = true;
            playerStats.suckCouldown.SetBaseValue(4f);
        } else {
            playerStats.explosionSizeMod += 0.25f;
            playerStats.poisonSlow -= 0.05f;
            // Increase projectile speed, size and lifetime
        }
    }

    private void Vortex() {
        if (!playerStats.vortex) {
            playerStats.vortex = true;
            playerStats.maxSuck += 2;
        } else {
            playerStats.suckSizeModifier += 0.5f;
            playerStats.maxSuck += 1;
            playerStats.suckCouldown.DecreaseModifier(0.05f);
        }
    }

    private void AutoAim() {
        if (!playerStats.autoAim) {
            playerStats.autoAim = true;
        } else {
            playerStats.suckSizeModifier += 0.25f;
            playerStats.spitSpeed.IncreaseModifier(0.25f);
        }
    }

}
