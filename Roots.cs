using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour
{
    [SerializeField] EnemyStats enemyStats;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            PlayerStats playerStats = col.gameObject.GetComponent<PlayerStats>();
            playerStats.TakeDamage(enemyStats.atk * 5);
            playerStats.rooted = true;
            Destroy(gameObject);
        }
    }
}
