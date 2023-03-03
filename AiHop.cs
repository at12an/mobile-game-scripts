using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHop : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float distance;  
    [SerializeField] private EnemyStats enemyStats;
    private float startHop;
    private int jumpStage;
    private bool lookingRight;
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpHeight;
    private Vector3 direction;
    private bool directionPicked;
    
    private void Start() {
        startHop = Time.time;
        jumpStage = 0;
        lookingRight = true;
    }

    private void Update()
    {   
        // Jumping and moving
        // Jump up
        if (Time.time - startHop < jumpHeight && jumpStage == 0) {
            if (!directionPicked) {
                direction = GetDirection();
                directionPicked = true;
            }
            animator.SetBool("Jumping", true);
            transform.position =  new Vector3(transform.position.x, transform.position.y + 2f * Time.deltaTime, transform.position.z);
            AiChase();
        } else if (Time.time - startHop >= 0.25f && jumpStage == 0) {
            jumpStage = 1;
            startHop = Time.time;
        }  
        // Jump down
        if (Time.time - startHop < jumpHeight && jumpStage == 1) {
            transform.position =  new Vector3(transform.position.x, transform.position.y - 2f * Time.deltaTime, transform.position.z);
            AiChase();
        } else if (Time.time - startHop >= 0.25f && jumpStage == 1) {
            jumpStage = 2;
            startHop = Time.time;
        }
        // Cooldown
        if (Time.time - startHop < 0.25f && jumpStage == 2) {
            animator.SetBool("Jumping", false);
        } else if (Time.time - startHop >= 1f && jumpStage == 2) {
            jumpStage = 0;
            startHop = Time.time;
            directionPicked = false;
        }

        // Fix facing direction
        if (jumpStage == 2) {
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
    }

    private Vector3 GetDirection() {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 dir = player.transform.position - transform.position;
        dir.Normalize();
        return dir;
    }

    private void AiChase() {
        transform.Translate(direction * Time.deltaTime * enemyStats.speed);
    }
}
