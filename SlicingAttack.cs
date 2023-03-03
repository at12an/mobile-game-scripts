using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicingAttack : MonoBehaviour
{
    private GameObject target;
    private Vector3 direction;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private float speed;
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private Animator animator;
    private float cooldown;
    private int slices;
    private bool reattack;
    private float lifetime;
    private float life;
    private bool firstThrow;
    public bool nerfed;

    private void Awake()
    {
        animator.SetFloat("Speed", playerStats.spitSpeed.GetValue()/9f);
        if (nerfed) {
            transform.localScale *= 0.5f;
        }
        firstThrow = true;
        lifetime = (float)playerStats.maxSuck;
        life = Time.time;
        transform.localScale *= playerStats.explosionSizeMod;
        speed = playerStats.spitSpeed.GetValue() * 1.5f;
        slices = playerStats.maxSuck * 2;
    }

    private void Update()
    {
        // Check if first throw
        if (!firstThrow) {
            // Check lifespan
            if (Time.time - life >= lifetime) {
                float damage = playerStats.atk.GetValue() * playerStats.damageModifier*(((playerStats.atkHpScale*100*(playerStats.maxHealth-playerStats.currentHealth)/playerStats.maxHealth) )+ 1);
                if (nerfed) {
                    damage *= 0.5f;
                }
                playerStats.DamageFirstEnemy(damage);
                playerStats.InstantiateEatenEnemy(transform);
                Destroy(gameObject);
            }
            // Find closest enemy
            if (target == null) {
                target = FindClosestEnemy();
                if (target != null) {
                    direction = target.transform.position - transform.position;
                    direction.Normalize();
                }
            // Attack closest enemy on cooldown
            } else if (Time.time - cooldown >= attackDelay) {
                if (reattack) {
                    direction = target.transform.position - transform.position;
                    direction.Normalize();
                    reattack = false;
                }
                transform.Translate(direction * Time.deltaTime * speed);
            }
        } else {
            // Move in straight line till hit for first throw
            transform.Translate(Vector2.right*speed *Time.deltaTime);
        }

    }

    private GameObject FindClosestEnemy() {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in GameObject.FindGameObjectsWithTag("Enemy")) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) {
            EnemyStats enemyStats = col.gameObject.GetComponent<EnemyStats>();
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
            enemyStats.TakeDamage(damage,0);
            if (playerStats.currentHealth > playerStats.maxHealth) {
                playerStats.currentHealth = playerStats.maxHealth;
            }
        } else if (col.gameObject.name.Contains("Border")) {
            float damage = playerStats.atk.GetValue() * playerStats.damageModifier*(((playerStats.atkHpScale*100*(playerStats.maxHealth-playerStats.currentHealth)/playerStats.maxHealth) )+ 1);
            if (nerfed) {
                damage *= 0.5f;
            }
            playerStats.DamageFirstEnemy(damage);
            playerStats.InstantiateEatenEnemy(transform);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) {
            if (firstThrow) {
                firstThrow = false;
                transform.rotation = Quaternion.identity;
            } else {
                slices += -1;
                if (slices == 0) {
                    float damage = playerStats.atk.GetValue() * playerStats.damageModifier*(((playerStats.atkHpScale*100*(playerStats.maxHealth-playerStats.currentHealth)/playerStats.maxHealth) )+ 1);
                    if (nerfed) {
                        damage *= 0.5f;
                    }
                    playerStats.DamageFirstEnemy(damage);
                    playerStats.InstantiateEatenEnemy(transform);
                    Destroy(gameObject);
                }
                
                cooldown = Time.time;
            }
            if (target != null) {
                reattack = true;
            }
        }
    }
}
