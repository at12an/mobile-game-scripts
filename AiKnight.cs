using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiKnight : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject slash;
    private float distance;  
    [SerializeField] private EnemyStats enemyStats;
    private bool lookingRight = true;
    private bool slashingRight;
    public Animator animator;
    public bool shield;
    public bool blocking;

    private void Update()
    {   
        // Deal with shield
        if (!shield) {
            animator.SetBool("Shield", false);
        } else if (shield) {
            animator.SetBool("Shield", true);
        }

        // Remove shield if damaged
        if (enemyStats.damaged) {
            shield = false;
        }

        // Chase player
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemyStats.speed * Time.deltaTime);

        // Slash / stop slashing
        if (distance <= 3 && !blocking) {
            animator.SetBool("Attacking", true);
            slash.SetActive(true);
            Vector3 targ = player.transform.position;
            targ.z = 0f;
            Vector3 objectPos = transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;
            float anglee = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            slash.transform.rotation = Quaternion.Euler(new Vector3(0, 0, anglee));
        } else {
            animator.SetBool("Attacking", false);
            slash.SetActive(false);
        }

        // Deal with slash and move direction
        if (transform.position.x - player.transform.position.x < 0 && !lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            scale = slash.transform.localScale;
            scale.x *= -1;
            scale.y *= -1;
            slash.transform.localScale = scale;
            lookingRight = !lookingRight;
            slashingRight = !slashingRight;
        }
        if (transform.position.x - player.transform.position.x > 0 && lookingRight) {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            scale = slash.transform.localScale;
            scale.x *= -1;
            scale.y *= -1;
            slash.transform.localScale = scale;
            lookingRight = !lookingRight;
            slashingRight = !slashingRight;
        }
        
    }

}