using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dash : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float firstPressed;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject fart;
    [SerializeField] private Transform fartStart;
    private float speedIncreaseTimer;
    private bool dashed;


    private void Update() {
        if (firstPressed == 0) {
            firstPressed = Time.time;
        }

        // Dash cooldown indicater
        if (Time.time - firstPressed < playerStats.dashCouldown) {
            text.text = "COOLDOWN";
        } else {
            text.text = "";
        }

        // Reset speed increase
        if (Time.time - speedIncreaseTimer >= 0.25f && dashed) {
            dashed = false;
            playerStats.moveSpeed.DecreaseModifier(4f);
        }
    }

    private void dash() {
        // Cooldown check
        if (Time.time - firstPressed >= playerStats.dashCouldown) {
            // Fart animation + temporary movespeed increase
            GameObject newFart = Instantiate(fart, fartStart.position, Quaternion.identity);
            newFart.SetActive(true);
            if (!playerStats.lookingRight) {
                Vector3 scale = newFart.transform.localScale;
                scale.x *= -1;
                newFart.transform.localScale = scale;
            }
            playerStats.moveSpeed.IncreaseModifier(4f);
            speedIncreaseTimer = Time.time;
            dashed = true;
            firstPressed = Time.time;
        }
    }
}
