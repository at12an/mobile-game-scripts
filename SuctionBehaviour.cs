using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuctionBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

     void Start()
    {
        transform.localScale *= playerStats.suckSizeModifier;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        int numEnemiesEaten = playerStats.eatenEnemies.Count;
        if (col.gameObject.name.Contains("Enemy") && numEnemiesEaten != playerStats.maxSuck && !col.gameObject.name.Contains("S")) { 
            if (!(playerStats.crystalProj && numEnemiesEaten > 1) && !col.gameObject.name.Contains("Boss") && !col.gameObject.name.Contains("3")) {
                if (!col.gameObject.name.Contains("Enemy4")) {
                    EnemyStats enemyStats = col.gameObject.GetComponent<EnemyStats>();
                    
                    enemyStats.sporuptionGen = playerStats.sporuptionLvl;
                    playerStats.AddEatenEnemy(col.gameObject);
                    col.gameObject.SetActive(false);
                    playerStats.spitOut = false;
                } else {
                    AiKnight aiKnight = col.gameObject.GetComponent<AiKnight>();
                    if (aiKnight.shield) {
                        aiKnight.animator.SetBool("Block", true);
                        aiKnight.blocking = true;
                    } else {
                        EnemyStats enemyStats = col.gameObject.GetComponent<EnemyStats>();
                        enemyStats.sporuptionGen = playerStats.sporuptionLvl;
                        playerStats.AddEatenEnemy(col.gameObject);
                        col.gameObject.SetActive(false);
                        playerStats.spitOut = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy4")) {
            col.gameObject.GetComponent<AiKnight>().animator.SetBool("Block", false);
            col.gameObject.GetComponent<AiKnight>().blocking = false;
        }        
    }
}
