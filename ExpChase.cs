using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpChase : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float distance;
    [SerializeField] private PlayerStats playerStats;

    private void Update()
    {   
        // Move towards player if in range
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 2f*playerStats.expRange) {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, 24 * Time.deltaTime);
        }
        
    }
}
