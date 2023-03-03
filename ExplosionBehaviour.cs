using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{   
    public PlayerStats playerStats;
    public int sporuptionGen;
    public GameObject damagePopup;
    public bool nerfed;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) { 
            EnemyStats enemyStats = col.gameObject.GetComponent<EnemyStats>();
            if (enemyStats.isDamagable(sporuptionGen)) {
                GameObject dP = Instantiate(damagePopup, col.gameObject.transform.position, Quaternion.identity);
                dP.SetActive(true);
                DamagePopup dp = dP.GetComponent<DamagePopup>();
                float damage = playerStats.atk.GetValue() * playerStats.damageModifier*(((playerStats.atkHpScale*100*(playerStats.maxHealth-playerStats.currentHealth)/playerStats.maxHealth) )+ 1);
                if (nerfed) {
                    damage *= 0.5f;
                }
                if (Random.value <= playerStats.critChance) {
                    damage *= playerStats.critDmg;
                    dp.Setup(damage, true);
                } else {
                    dp.Setup(damage, false);
                }
                enemyStats.TakeDamage(damage,sporuptionGen);
                if (playerStats.currentHealth > playerStats.maxHealth) {
                    playerStats.currentHealth = playerStats.maxHealth;
                }
                if (sporuptionGen > 0) {
                    enemyStats.sporuptionGen = sporuptionGen;
                }
            }
        }
    }
    
    private void EndExplosion() {
        gameObject.SetActive(false);
        transform.localScale /= playerStats.explosionSizeMod;
        sporuptionGen = 0;
        if (nerfed) {
            transform.localScale *= 2;
        }
        nerfed = false;
    }   

    private void ApplySizeModifier() {
        if (nerfed) {
            transform.localScale *= 0.5f;
        }
        transform.localScale *= playerStats.explosionSizeMod;
    }
}
