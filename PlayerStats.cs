using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public List<GameObject> eatenEnemies = new List<GameObject>();
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject UI;
    [SerializeField] private LevelUpScreen levelUpScreen;
    [SerializeField] private SpecialScreen specialScreen;
    [SerializeField] private GameObject damagePopup;
    public float currentExp;
    public float prevExpMax;
    public float expModifier;
    public int level;
    public float maxHealth;
    public float currentHealth;
    public float hpModifier;
    public Stat atk;
    public int maxSuck;
    public float damageModifier;
    public Stat moveSpeed;
    public Stat suckCouldown;
    public Stat spitSpeed;
    public bool spitOut = false;
    public float startTime;
    public float explosionSizeMod;
    public float suckSizeModifier;
    public float lifeSteal;
    public float expRange;
    public float lifeRegen;
    public float critChance;
    public float critDmg;
    public int sporuptionLvl;
    public float dashCouldown;
    public float atkHpScale;
    public int revives;
    public float modelModifier;
    public bool crystalProj;
    public float crystalLife;
    public bool poisonAttack;
    public float poisonSlow;
    public bool delirium;
    public bool lookingRight;
    public bool poisoned;
    public bool rooted;
    public bool vortex;
    public bool autoAim;
    public bool charmed;
    [SerializeField] private Transform boss;

    public bool pillow;

    public IEnumerator CharmCD() {
        yield return new WaitForSeconds(2f);
        charmed = false;
    }

    private void Awake() {
        if (!PlayerPrefs.HasKey("attack"))
        {
            PlayerPrefs.SetInt("attack", 100);
        }
        if (!PlayerPrefs.HasKey("health"))
        {
            PlayerPrefs.SetInt("health", 1000);
        }
        if (!PlayerPrefs.HasKey("startSpecial"))
        {
            PlayerPrefs.SetInt("startSpecial", 0);
        }

        if (!PlayerPrefs.HasKey("numSuck"))
        {
            PlayerPrefs.SetInt("numSuck", 1);
        }
        maxHealth = PlayerPrefs.GetInt("health");
        maxSuck = PlayerPrefs.GetInt("numSuck");
        atk.SetBaseValue(PlayerPrefs.GetInt("attack"));
        if (PlayerPrefs.GetInt("startSpecial") == 1) {
            specialScreen.StartSpecial();
        }
        currentHealth = maxHealth;
        level = 0;
        startTime = Time.time;
        dashCouldown = 2.5f;
        
    }

    private void Update() {
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        if (charmed) {
            StartCoroutine(CharmCD());
            Vector3 direction = boss.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * 4f);
        }
        if (poisoned) {
            currentHealth -= maxHealth * Time.deltaTime * 0.5f;
        }
        if (rooted) {
            StartCoroutine(UnRoot());
        }
        if (currentHealth <= 0) {
            Die();
            // Debug.Log("Died");
        }
    }
    
    public IEnumerator UnRoot() {
        yield return new WaitForSeconds(1f);
        rooted = false;
    }

    public void TakeDamage (float damage) {
        currentHealth -= damage / hpModifier; 
        // Debug.Log("Player took " + damage + " dmg");
        if (currentHealth <= 0) {
            Die();
            // Debug.Log("Died");
        }
    }

    public void ApplyHpModifier(float modifierChange) {
        currentHealth /= hpModifier;
        maxHealth /= hpModifier;
        hpModifier += modifierChange;
        currentHealth *= hpModifier;
        maxHealth *= hpModifier;
    }

    public void ApplySizeIncrease(float modifierChange) {
        transform.localScale /= modelModifier;
        modelModifier += modifierChange;
        transform.localScale *= modelModifier;
    }

    public virtual void Die () {
        // gameObject.SetActive(false);
        // Debug.Log("Died");
        if (revives > 0) {
            revives -= 1;
            maxHealth = maxHealth /2;
            currentHealth = maxHealth;
        } else {
            // Time.timeScale = 0;
            gameObject.SetActive(false);
            deathScreen.SetActive(true);
            UI.SetActive(false);
        }
    }

   public void LevelUp() {
        if (currentExp > prevExpMax * (float)1.1) {
            prevExpMax = prevExpMax * (float)1.1;
            currentExp -= prevExpMax * (float)1.1;
            level += 1;
            levelUpScreen.StartLevelUp();
        }
    }

    public void GainExp(float amount) {
        currentExp += amount * expModifier;
    }
    
    public void AddEatenEnemy(GameObject eatenEnemy) {
        eatenEnemies.Add(eatenEnemy);
    }

    public void InstantiateEatenEnemy(Transform pos) {
        if (eatenEnemies.Count != 0) {
            GameObject enemy = eatenEnemies[0];
            eatenEnemies.Remove(enemy);
            enemy.SetActive(true);
            enemy.transform.position = pos.transform.position;
            enemy.transform.rotation = Quaternion.identity;
        }
    }

    public void DamageFirstEnemy(float damage) {
        if (eatenEnemies.Count != 0) {
            GameObject enemy = eatenEnemies[eatenEnemies.Count - 1];
            enemy.GetComponent<EnemyStats>().TakeDamage(damage, 0);
            GameObject dP = Instantiate(damagePopup, enemy.transform.position, Quaternion.identity);
            dP.SetActive(true);
            DamagePopup dp = dP.GetComponent<DamagePopup>();
            if (Random.value <= critChance) {
                float dmg = damage * critDmg;
                dp.Setup(damage, true);
            } else {
                dp.Setup(damage, false);
            }
        }
    }
}
