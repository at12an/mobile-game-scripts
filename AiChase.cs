using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChase : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distance;  
    [SerializeField] private EnemyStats enemyStats;
    private bool lookingRight;

    private void Awake() {
        lookingRight = true;
    }

    private void Update()
    {
        // Chase player
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemyStats.speed * Time.deltaTime);
        
        // Fix face direction
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
