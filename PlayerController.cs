using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform LaunchOffset;
    [SerializeField] private GameObject suction;
    [SerializeField] private Image button;
    [SerializeField] private Sprite suckButton;
    [SerializeField] private Sprite cdButton;
    [SerializeField] private Sprite spitButton;
    [SerializeField] private GameObject spit;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private HpBarBehaviour hpbar;
    [SerializeField] private ExpBarBehaviour expbar;
    [SerializeField] private GameObject cursor;
    [SerializeField] private EnemyStats enemyStats;
    private GameObject newSuction;
    private Quaternion rotation;
    private float takingDmg;
    public PlayerStats playerStats;
    public bool lookingRight = true;
    public float moveSpeed;
    private float lastSuction;
    [SerializeField] private float suctionCouldown;
    [SerializeField] private int numSuckable;
    [SerializeField] private int enemiesSucked;
    public SpecialScreen specialScreen;
    [SerializeField] private GameObject crystalObj;
    public bool circularProj;
    [SerializeField] private GameObject boost;
    [SerializeField] private DeliriumController deliriumController; 
    private int numEnemiesEaten;
    [SerializeField] private GameObject vortex;
    private float expScale;
    private float timer;
    [SerializeField] private GameObject pillow;
    [SerializeField] private FloatingJoystick aimStick;
    private bool sucking;
    private Vector3 prevMoveVector;
    private bool moved;
    
    private void Awake() {
        if (PlayerPrefs.HasKey("skinEquipped")) {
            if (PlayerPrefs.GetInt("skinEquipped") == 1) {
                animator.SetBool("Skin2", true);
            }
        }
        timer = Time.time;
        expScale = 1;
    }

    private void FixedUpdate()
    { 
        Time.timeScale = 2;
        //////// Increase exp scaling
        if (Time.time - timer >= 90f) {
            expScale += 0.25f;
            timer = Time.time;
        }

        /////// Count enemies eaten currently
        numEnemiesEaten = playerStats.eatenEnemies.Count;
    
        /////// Update simple stats
        playerStats.lookingRight = lookingRight;
        moveSpeed = playerStats.moveSpeed.GetValue();
        suctionCouldown = playerStats.suckCouldown.GetValue();

        /////// Health Regen
        if (playerStats.currentHealth <= playerStats.maxHealth) {
            playerStats.currentHealth += playerStats.currentHealth * playerStats.lifeRegen * Time.deltaTime;
        }


        //////// Move player
        if (!playerStats.rooted) {
            rigidbody.velocity = new Vector3(joystick.Horizontal * moveSpeed, joystick.Vertical * moveSpeed, 0);
        } else {
            rigidbody.velocity = new Vector3(0, 0 ,0);
        }

        /////// Deal with cursor rotation
        GameObject enemy = FindClosestEnemy();
        Vector3 moveVector = (Vector3.up * aimStick.Horizontal + Vector3.left * aimStick.Vertical);
        if ((aimStick.Horizontal != 0 || aimStick.Vertical != 0) && !playerStats.autoAim)
        {
            rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
            if (!sucking && numEnemiesEaten > 0) {
                moved = true;
            } else {
                useVacuum();
            }
        } else if (playerStats.autoAim) {
            if (enemy != null) {
                rotation = Quaternion.LookRotation(Vector3.forward, (enemy.transform.position - transform.position));
                rotation *= Quaternion.Euler(0, 0, 90);
            }
            
        } else if (aimStick.Horizontal == 0 && aimStick.Vertical == 0 && moved) {
            if (!sucking && numEnemiesEaten > 0) {
                useVacuum();
                moveVector = prevMoveVector;
            }
        }

        if (aimStick.Horizontal == 0 && aimStick.Vertical == 0 && sucking) {
            stopVacuum();
            sucking = false;
            moved = false;
        }

        //// Deal with suction size
        if (newSuction != null) {
            newSuction.transform.localScale = new Vector3(0.15f*playerStats.suckSizeModifier, 0.15f*playerStats.suckSizeModifier, 0);
        }

        ///// Deal with player rotation
        if (joystick.Horizontal > 0 && !lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            lookingRight = !lookingRight;
        }
        if (joystick.Horizontal < 0 && lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            lookingRight = !lookingRight;
        }

        /////// Deal with player damage & hp bar
        playerStats.TakeDamage(takingDmg * Time.deltaTime * 16);
        hpbar.SetHealth(playerStats.currentHealth , playerStats.maxHealth);

        /////// Deal with cursor
        cursor.transform.position = CalculateCursorPos(1.2f * playerStats.modelModifier);
        cursor.transform.rotation = rotation;

        ///// Deal with suction position
        if (numEnemiesEaten < playerStats.maxSuck && newSuction != null && !playerStats.vortex && !playerStats.autoAim) {
            newSuction.transform.position = CalculateCursorPos(2*playerStats.suckSizeModifier);
            newSuction.transform.rotation = rotation;
        }

        if (numEnemiesEaten < playerStats.maxSuck && playerStats.autoAim && !playerStats.crystalProj) {
            SuckClosestEnemy();
        }

        if (numEnemiesEaten < playerStats.maxSuck && playerStats.autoAim && playerStats.crystalProj && numEnemiesEaten < 1) {
            SuckClosestEnemy();
        }

        if (enemy != null && !playerStats.crystalProj) {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= 6f * playerStats.spitSpeed.GetValue()/9f && numEnemiesEaten >= playerStats.maxSuck && playerStats.autoAim && Time.time - lastSuction >= playerStats.suckCouldown.GetValue()) {
                AimClosestEnemy();
            }
        } else if (enemy != null && playerStats.crystalProj) {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= 10f && numEnemiesEaten >= 1 && playerStats.autoAim && Time.time - lastSuction >= playerStats.suckCouldown.GetValue()) {
                AimClosestEnemy();
            }
        }


        if (numEnemiesEaten < playerStats.maxSuck && newSuction != null && playerStats.vortex ) {
            newSuction.transform.position = LaunchOffset.position;
        }

        //////// Deal with vacuum limits
        if (playerStats.crystalProj && numEnemiesEaten >= 1) {
            stopVacuum();
        } else if (numEnemiesEaten == playerStats.maxSuck) {
            stopVacuum();
        }

        ////////// Deal with button sprite
        if (numEnemiesEaten > 0) {
            button.sprite = spitButton;
            button.color = new Color(1f,1f,1f,1f);
        } else if (Time.time - lastSuction < suctionCouldown) {
            button.sprite = suckButton;
            button.color = new Color(1f,1f,1f,.5f);
        } else if (playerStats.spitOut) {
            button.sprite = suckButton;
            button.color = new Color(1f,1f,1f,1f);
        }

        //////// Deal with level
        playerStats.LevelUp();
        expbar.SetExp(playerStats.currentExp, playerStats.prevExpMax*(float)1.1);

        /////// Deal with player animation
        if (numEnemiesEaten > 0 && newSuction == null) {
            animator.SetBool("Full", true);
            if (playerStats.delirium) {
                deliriumController.Full();
            }
        } else if (playerStats.spitOut) {
            animator.SetBool("Full", false);
            animator.SetBool("Spit", false);
            if (playerStats.delirium) {
                deliriumController.Reset();
            }
        }
    }

    private IEnumerator WaitThenShoot(){
        yield return new WaitForSeconds(0.1f);
        useVacuum();
    }

    // private Vector3 AutoAimDirection() {
    //     GameObject enemy = FindClosestEnemy();
    //     if (enemy != null) {
    //         rotation = Quaternion.LookRotation(Vector3.forward, (enemy.transform.position - transform.position));
    //         rotation *= Quaternion.Euler(0, 0, 90);
    //         float angle = rotation.eulerAngles.z * Mathf.PI/180;
    //         float x = Mathf.Cos(angle);
    //         float y = Mathf.Sin(angle);
    //         float z = 0;
    //         Vector3 suction_adjust = new Vector3(x,y,z);
    //         suction_adjust *= size;
    //         return LaunchOffset.position + suction_adjust;
    //     }
    // }

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

    public void SuckClosestEnemy() {
        Vector3 pos = CalculateCursorPos(2*playerStats.suckSizeModifier);
        if (Time.time - lastSuction < suctionCouldown) {
            return;
        }
        if (newSuction == null) {
            animator.SetBool("Suction_On", true);
            if (playerStats.delirium) {
                deliriumController.Suck();
            }
            if (!playerStats.vortex) {
                newSuction = Instantiate(suction, pos, rotation);
                newSuction.SetActive(true);
            } else {
                newSuction = Instantiate(vortex, LaunchOffset.position, Quaternion.identity);
                newSuction.SetActive(true);
            }
        } else {
            if (!playerStats.vortex) {
                newSuction.transform.position = pos;
                newSuction.transform.rotation = rotation;
            } else {
                newSuction.transform.position = LaunchOffset.position;
            }
        }
    }

    public void AimClosestEnemy() {
        numEnemiesEaten = playerStats.eatenEnemies.Count;
        Vector3 pos = CalculateCursorPos(2*playerStats.suckSizeModifier);
        lastSuction = Time.time;
        animator.SetBool("Spit", true);
        Quaternion rot = rotation;
        rot *= Quaternion.Euler(0, 0, 180);
        if (playerStats.delirium) {
            deliriumController.Spit(rot);
        }
        animator.SetBool("Suction_On", false);
        if (playerStats.delirium) {
            deliriumController.UnSuck();
        }
        if (!playerStats.crystalProj && !playerStats.poisonAttack) {
            if (numEnemiesEaten % 2 == 0) {
                EvenSpit();
            } else {
                OddSpit();
            }
        } else if (playerStats.crystalProj) {
            GameObject new_crystal = crystalObj;
            new_crystal = Instantiate(new_crystal, LaunchOffset.position, rotation);
            new_crystal.SetActive(true);
            // playerStats.eatenEnemies.Clear();
        } else if (playerStats.poisonAttack) {
            GameObject newBoost = Instantiate(boost, LaunchOffset.position, rotation);
            newBoost.SetActive(true);
            GameObject new_spit = spit;
            new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
            new_spit.SetActive(true);
        }
        playerStats.spitOut = true;
    }



    // Calculate position on circle around the mouth
    private Vector3 CalculateCursorPos(float size) {
        float angle = rotation.eulerAngles.z * Mathf.PI/180;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        float z = 0;
        Vector3 suction_adjust = new Vector3(x,y,z);
        suction_adjust *= size;
        return LaunchOffset.position + suction_adjust;
    }


    // Deal with enemy and exp collision
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) {
            takingDmg += col.gameObject.GetComponent<EnemyStats>().atk; 
        } else if (col.gameObject.name == "Exp(Clone)") {
            col.gameObject.SetActive(false);
            playerStats.GainExp(1*expScale);
        } else if (col.gameObject.name.Contains("ExpS")) {
            UnityEngine.Object.Destroy(col.gameObject);
            specialScreen.StartSpecial();
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy")) { 
            takingDmg -= col.gameObject.GetComponent<EnemyStats>().atk;
        }
    }

    private IEnumerator InstantiateVacuum() {
        Vector3 pos = CalculateCursorPos(2*playerStats.suckSizeModifier);
        yield return new WaitForSeconds(0.5f);
        newSuction = suction;
        newSuction.SetActive(true);
        newSuction = Instantiate(newSuction, pos, rotation);
        suction.SetActive(false);
    }
    
    public void useVacuum() {
        // Get number of enemies eaten
        numEnemiesEaten = playerStats.eatenEnemies.Count;

        // Get shoot direction
        Vector3 pos = CalculateCursorPos(2*playerStats.suckSizeModifier);

        // Check if anyone has been eaten
        if (numEnemiesEaten == 0) {     
            // Check cooldown then start suck
            if (Time.time - lastSuction < suctionCouldown) {
                return;
            }
            animator.SetBool("Suction_On", true);
            if (playerStats.delirium) {
                deliriumController.Suck();
            }
            if (newSuction == null) {
                if (!playerStats.vortex && !playerStats.pillow) {
                    newSuction = Instantiate(suction, pos, rotation);
                    newSuction.SetActive(true);
                } else if (playerStats.vortex) {
                    newSuction = Instantiate(vortex, LaunchOffset.position, Quaternion.identity);
                    newSuction.SetActive(true);
                } else if (playerStats.pillow) {
                    newSuction = Instantiate(pillow, pos, rotation);
                    newSuction.SetActive(true);
                }
                sucking = true;
            }
        // Check if spit out but enemies not spawned
        } else if (!playerStats.spitOut && !sucking) {
            // Deal with attack variations and animation
            lastSuction = Time.time;
            animator.SetBool("Spit", true);
            Quaternion rot = rotation;
            rot *= Quaternion.Euler(0, 0, 180);
            if (playerStats.delirium) {
                deliriumController.Spit(rot);
            }
            animator.SetBool("Suction_On", false);
            if (playerStats.delirium) {
                deliriumController.UnSuck();
            }
            if (!playerStats.crystalProj && !playerStats.poisonAttack) {
                if (numEnemiesEaten % 2 == 0) {
                    EvenSpit();
                } else {
                    OddSpit();
                }
            } else if (playerStats.crystalProj) {
                GameObject new_crystal = crystalObj;
                new_crystal = Instantiate(new_crystal, LaunchOffset.position, rotation);
                new_crystal.SetActive(true);
                // playerStats.eatenEnemies.Clear();
            } else if (playerStats.poisonAttack) {
                GameObject newBoost = Instantiate(boost, LaunchOffset.position, rotation);
                newBoost.SetActive(true);
                GameObject new_spit = spit;
                new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
                new_spit.SetActive(true);
            }
            playerStats.spitOut = true;
        }
    }

    // Spit out with even enemies in stomach
    private void EvenSpit() {
        GameObject new_spit = spit;
        new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
        new_spit.SetActive(true);
        float angle = 120f/(numEnemiesEaten+1);
        for (int i = 0; i < (numEnemiesEaten)/2; i++) {
            new_spit = spit;
            new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
            new_spit.SetActive(true);
            new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
            angle += 120f/(numEnemiesEaten);
        }
        angle = -120f/(numEnemiesEaten);
        for (int i = 0; i < (numEnemiesEaten/2)-1; i++) {
            new_spit = spit;
            new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
            new_spit.SetActive(true);
            new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
            angle -= 120f/(numEnemiesEaten);
        }
    }

    // Spit out with odd enemies in stomach
    private void OddSpit() {
        GameObject new_spit = spit; 
        new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
        new_spit.SetActive(true);
        float angle = 120f/(numEnemiesEaten);
        for (int i = 0; i < (numEnemiesEaten-1)/2; i++) {
            new_spit = spit;
            new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
            new_spit.SetActive(true);
            new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
            angle += 120f/(numEnemiesEaten);
        }
        angle = -120f/(numEnemiesEaten);
        for (int i = 0; i < numEnemiesEaten/2; i++) {
            new_spit = spit;
            new_spit = Instantiate(new_spit, LaunchOffset.position, rotation);
            new_spit.SetActive(true);
            new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
            angle -= 120f/(numEnemiesEaten);
        }
    }


    // Stop vacuum
    private void stopVacuum() {
        numEnemiesEaten = playerStats.eatenEnemies.Count;
        if (numEnemiesEaten == 0) {
            animator.SetBool("Suction_On", false);
            if (playerStats.delirium) {
                deliriumController.UnSuck();
            }
        }
        if (suction != null) {
            UnityEngine.Object.Destroy(newSuction);
        }
    }
}
