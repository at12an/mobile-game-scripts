using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDamage : MonoBehaviour
{
    private List<EnemyStats> enemiesStats = new List<EnemyStats>();
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject damagePopup;
    public bool nerfed;

    private void Update() {
        // Deal damage to each enemy in pool
        float damage = playerStats.atk.GetValue() * playerStats.damageModifier*(((playerStats.atkHpScale*100*(playerStats.maxHealth-playerStats.currentHealth)/playerStats.maxHealth) )+ 1);
        damage = damage/5;
        if (nerfed) {
            damage *= 0.5f;
        }
        int i = 0;
        foreach (EnemyStats enemyStat in enemiesStats) {
            if (enemyStat.isDamagable()) { 
                GameObject dP = Instantiate(damagePopup, enemies[i].transform.position, Quaternion.identity);
                dP.SetActive(true);
                DamagePopup dp = dP.GetComponent<DamagePopup>();
                if (Random.value <= playerStats.critChance) {
                    damage *= playerStats.critDmg;
                    dp.Setup(damage, true);
                } else {
                    dp.Setup(damage, false);
                }
                enemyStat.TakePoisonDamage(damage);
            }
            i += 1;
        }
    }


    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) { 
            EnemyStats enemyStat = col.gameObject.GetComponent<EnemyStats>();
            enemyStat.speed *= playerStats.poisonSlow;
            enemiesStats.Add(enemyStat);
            enemies.Add(col.gameObject);
            
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) { 
            EnemyStats enemyStat = col.gameObject.GetComponent<EnemyStats>();
            enemyStat.speed /= playerStats.poisonSlow;
            enemiesStats.Remove(enemyStat);
            enemies.Remove(col.gameObject);
        }
    }
}
