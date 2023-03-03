using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiShooter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject proj;
    [SerializeField] private Transform projStart;
    private float distance;  
    [SerializeField] private EnemyStats enemyStats;
    private bool lookingRight = true;
    [SerializeField] private Animator animator;
    private float detectonDistance;

    private void Update()
    {   
        detectonDistance = 4 + enemyStats.atk / 5;
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Move or shoot
        if (distance > detectonDistance) {
            animator.SetBool("Shooting", false);
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemyStats.speed * Time.deltaTime);
        } else {
            animator.SetBool("Shooting", true);
        }

        // Fix rotation
        if (transform.position.x - player.transform.position.x < 0 && !lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            lookingRight = !lookingRight;
        }
        if (transform.position.x - player.transform.position.x > 0 && lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            lookingRight = !lookingRight;
        }
        
    }

    private void Shoot() {
        animator.SetBool("Shooting", true);
        GameObject newProj = Instantiate(proj, projStart.position, Quaternion.identity);
        newProj.SetActive(true);
        newProj.transform.localScale = new Vector3(0.15f, 0.15f, 0);
    }
}
