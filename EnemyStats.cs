using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private HpBarBehaviour hpbar;
    [SerializeField] private CurrentEnemyStats currentEnemyStats;

    [SerializeField] private GameObject explosion;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private GameObject exp;
    [SerializeField] private GameObject blood;
    public float maxHealth;  
    public float currentHealth;
    public float atk;
    public float speed;
    public bool special;
    public bool sporuption;
    public int sporuptionGen;
    [SerializeField] private PlayerStats playerStats;
    private float damageTaken;
    private List<int> damages = new List<int>();
    private float immunityTime;
    [SerializeField] private GameObject gold;
    public bool damaged;
    public bool shielded;
    [SerializeField] private GameObject deathScreen;

    public bool isDamagable(int sporuptionExp) {
        // Check for different generation of damages / in immunity time / or shielded
        if ((!damages.Contains(sporuptionExp) || Time.time - damageTaken >= immunityTime ) && !shielded) {
            return true;
        }
        return false;
    }

    public bool isDamagable() {
        // Check for dimmunity time / or shielded
        if ((Time.time - damageTaken >= 0.3f) && !shielded) {
            return true;
        }
        return false;
    }

    private void Update() {
        if (sporuptionGen > 0) {
            transform.Find("Sporupter").gameObject.SetActive(true);
        }
        if (currentHealth < maxHealth) {
            damaged = true;
        }
        if (currentHealth <= 0) {
            Die();
        }
        hpbar.SetHealth(currentHealth, maxHealth);
    }

    private void Awake() {
        immunityTime = 0.1f;
        currentHealth = maxHealth;
        atk = currentEnemyStats.atk;
        maxHealth = currentEnemyStats.maxHealth;
        currentHealth = maxHealth;
        speed = currentEnemyStats.speed;
    }

    public void TakeDamage (float damage, int sporuptionExp) {
        if (sporuptionExp == 0) {
            damageTaken = Time.time;
            damages = new List<int>();
            damages.Add(sporuptionExp);
        }
        currentHealth -= damage; 
        GameObject bloodCopy = Instantiate(blood, transform.position, Quaternion.identity);
        bloodCopy.SetActive(true);
        damaged = true;
    }

    public void TakePoisonDamage(float damage) {
        damageTaken = Time.time;
        currentHealth -= damage;
        GameObject bloodCopy = Instantiate(blood, transform.position, Quaternion.identity);
        bloodCopy.SetActive(true);
        damaged = true;
    }

    public virtual void Die () {
        UnityEngine.Object.Destroy(gameObject);
        if (gameObject.name.Contains("Boss")) {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 100);
            PlayerPrefs.SetInt("passedS" + ((int)((PlayerPrefs.GetFloat("scaleFactor")-0.9f)/0.1f)).ToString(),1);
            deathScreen.SetActive(true);
        }
        if (!gameObject.name.Contains("S")) {
            GameObject new_exp = exp;
            new_exp = Instantiate(new_exp, transform.position, transform.rotation);
            new_exp.SetActive(true);
            // GameObject new_exp = ObjectPool.instance.GetPooledExp();
            // if (new_exp != null) {
            //     new_exp.transform.position = transform.position;
            //     new_exp.SetActive(true);
            // }
        } else if (!gameObject.name.Contains("Broccoli")) {
            GameObject new_exp = exp;
            new_exp = Instantiate(new_exp, transform.position, transform.rotation);
            new_exp.SetActive(true);
        }
        if (Random.value <= 0.1f) {
            Vector3 goldPos = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
            GameObject newGold = Instantiate(gold, goldPos, transform.rotation);
            newGold.SetActive(true);
        }
        gameInfo.enemiesKilled += 1;
        if (sporuptionGen > 0) {
            GameObject explosion_clone = ObjectPool.instance.GetPooledSporuption();
            if (explosion_clone != null) {
                explosion_clone.transform.position = transform.position;
                explosion_clone.SetActive(true);
            }
            ExplosionBehaviour explosionStats = explosion_clone.GetComponent<ExplosionBehaviour>();
            explosionStats.sporuptionGen = sporuptionGen - 1;
        }
        playerStats.currentHealth += playerStats.lifeSteal * playerStats.maxHealth;
    }
}
