using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charm : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private float speed;
    private bool right;

    private void Awake()
    {
        // Get direction and rotation
        direction = new Vector3(Random.Range(player.transform.position.x-2,player.transform.position.x+2),Random.Range(player.transform.position.y-2,player.transform.position.y+2),0) - transform.position;
        direction.Normalize();
        if (direction.x < 0) {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
        Vector3 targ = player.transform.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
        float anglee = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, anglee));
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + direction.x * Time.deltaTime * speed ,transform.position.y + direction.y * Time.deltaTime * speed , 0);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            playerStats.charmed = true;
            Destroy(gameObject);
        } else if (col.gameObject.name.Contains("Border")) {
            Destroy(gameObject);
        }
    }
}
