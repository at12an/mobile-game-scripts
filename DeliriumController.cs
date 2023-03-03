using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliriumController : MonoBehaviour
{
    [SerializeField] private Transform behindPlayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStats playerStats;
    private bool lookingRight;
    private List<GameObject> eatenEnemies = new List<GameObject>();
    [SerializeField] private GameObject spit;
    [SerializeField] private GameObject crystalObj;
    private int numEnemiesEaten;
    
    private void Start() {
        lookingRight = true;
        transform.position = behindPlayer.position;
    }

    private void Update() {
        ///////// Count enemies eaten currently
        numEnemiesEaten = playerStats.eatenEnemies.Count;

        // Move towards players back
        Vector3 direction = behindPlayer.transform.position - transform.position;
        direction.Normalize();
        transform.Translate(direction * Time.deltaTime * 3f);

        // Fix rotation
        if (playerController.lookingRight && lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            lookingRight = false;
        } else if (!playerController.lookingRight && !lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            lookingRight = true;
        }
    }
    
    public void Suck() {
        animator.SetBool("Suction_On", true);
    }

    public void UnSuck() {
        animator.SetBool("Suction_On", false);
    }

    public void Spit(Quaternion rotation) {
        numEnemiesEaten = playerStats.eatenEnemies.Count;
        animator.SetBool("Spitting", true);
        if (!playerStats.crystalProj && !playerStats.poisonAttack) {
            if (numEnemiesEaten % 2 == 0) {
                GameObject new_spit = spit;
                new_spit = Instantiate(new_spit, transform.position, rotation);
                new_spit.SetActive(true);
                float angle = 120f/(numEnemiesEaten+1);
                for (int i = 0; i < (numEnemiesEaten)/2; i++) {
                    new_spit = spit;
                    new_spit = Instantiate(new_spit, transform.position, rotation);
                    new_spit.SetActive(true);
                    new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
                    angle += 120f/(numEnemiesEaten);
                }
                angle = -120f/(numEnemiesEaten);
                for (int i = 0; i < (numEnemiesEaten/2)-1; i++) {
                    new_spit = spit;
                    new_spit = Instantiate(new_spit, transform.position, rotation);
                    new_spit.SetActive(true);
                    new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
                    angle -= 120f/(numEnemiesEaten);
                }
            } else {
                GameObject new_spit = spit;
                new_spit = Instantiate(new_spit, transform.position, rotation);
                new_spit.SetActive(true);
                float angle = 120f/(numEnemiesEaten);
                for (int i = 0; i < (numEnemiesEaten-1)/2; i++) {
                    new_spit = spit;
                    new_spit = Instantiate(new_spit, transform.position, rotation);
                    new_spit.SetActive(true);
                    new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
                    angle += 120f/(numEnemiesEaten);
                }
                angle = -120f/(numEnemiesEaten);
                for (int i = 0; i < numEnemiesEaten/2; i++) {
                    new_spit = spit;
                    new_spit = Instantiate(new_spit, transform.position, rotation);
                    new_spit.SetActive(true);
                    new_spit.transform.Rotate(0.0f,0.0f, angle,Space.Self);
                    angle -= 120f/(numEnemiesEaten);
                }
            }
        } else if (playerStats.crystalProj) {
            GameObject new_crystal = crystalObj;
            new_crystal = Instantiate(new_crystal, transform.position, rotation);
            new_crystal.SetActive(true);
            new_crystal.GetComponent<SlicingAttack>().nerfed = true;
        } else if (playerStats.poisonAttack) {
            GameObject new_spit = spit;
            new_spit = Instantiate(new_spit, transform.position, rotation);
            new_spit.SetActive(true);
        }
    }

    public void Full() {
        animator.SetBool("Full", true);
    }

    public void Reset() {
        animator.SetBool("Full", false);
        animator.SetBool("Spitting", false);
    }
}
