using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AiTurret aiTurret;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;
    private bool hittingPlayer;

    private void Update()
    {

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - Time.deltaTime, 0);
        if (transform.localScale.y <= 0) {
            gameObject.SetActive(false);
            transform.localScale = new Vector3(1.4f, 1, 0); 
            animator.SetBool("Shoot", false);
            animator.SetBool("Charging", false);
            aiTurret.shooting = false;
            aiTurret.chargeUp = Time.time;
            aiTurret.charging = false;
            aiTurret.aim = false;
        }
        if (hittingPlayer) {
            playerStats.TakeDamage(enemyStats.atk * Time.deltaTime * 25);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) {
            hittingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) {
            hittingPlayer = false;
        }
    }
}
