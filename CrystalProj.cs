using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalProj : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private float speed;

    private void Awake()
    {
        direction = player.transform.position - transform.position;
        direction.Normalize();
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            playerStats.TakeDamage(enemyStats.atk * 5f);
            Destroy(gameObject);
        }
    }
}
