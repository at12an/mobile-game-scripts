using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform pos;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;
    private bool damagingPlayer;

    // Script to follow / deal with enemy slashes
    void Update()
    {
        transform.position = pos.position;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            damagingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            damagingPlayer = false;
        }
    }

    private void Damage() {
        if (damagingPlayer) {
            playerStats.TakeDamage(enemyStats.atk*8);
        }
    }
}
