using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitProjBehaviour : MonoBehaviour
{
    private float speed;
    [SerializeField] private GameObject explosion;
    [SerializeField] private PlayerStats playerStats;
    private bool exploded;
    [SerializeField] private GameObject player;
    private float distance;
    [SerializeField] private GameObject poisonPool;

    void Start()
    {
        exploded = false;
        speed = playerStats.spitSpeed.GetValue();
    }

    void Update()
    {
        transform.Translate(Vector2.right*speed *Time.deltaTime);
        if (speed > 2) {
            speed -= 10 * Time.deltaTime;
        } else {
            InstantiateEatenEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy") && !exploded) {
            UnityEngine.Object.Destroy(gameObject);
            if (!playerStats.poisonAttack) {    
                InstantiateEatenEnemy();
                GameObject explosion_clone = ObjectPool.instance.GetPooledExplosion();
                if (explosion_clone != null) {
                    explosion_clone.transform.position = transform.position;
                    explosion_clone.SetActive(true);
                }
            } else {
                GameObject ppClone = Instantiate(poisonPool, transform.position, Quaternion.identity);
                ppClone.SetActive(true);
                foreach (GameObject obj in playerStats.eatenEnemies) {
                    obj.SetActive(true);
                    obj.transform.position = new Vector3(Random.Range(transform.position.x-3f, transform.position.x+3f), Random.Range(transform.position.y-3f, transform.position.y+3f), 0 );
                }
                playerStats.eatenEnemies.Clear();
            }
            exploded = true;
        } else if (col.gameObject.name.Contains("Border") && !exploded){
            UnityEngine.Object.Destroy(gameObject);
            if (!playerStats.poisonAttack) {    
                InstantiateEatenEnemy();
                GameObject explosion_clone = ObjectPool.instance.GetPooledExplosion();
                if (explosion_clone != null) {
                    explosion_clone.transform.position = transform.position;
                    explosion_clone.SetActive(true);
                }
            } else {
                GameObject ppClone = Instantiate(poisonPool, transform.position, Quaternion.identity);
                ppClone.SetActive(true);
                foreach (GameObject obj in playerStats.eatenEnemies) {
                    obj.SetActive(true);
                    obj.transform.position = new Vector3(Random.Range(transform.position.x-3f, transform.position.x+3f), Random.Range(transform.position.y-3f, transform.position.y+3f), 0 );
                }
                playerStats.eatenEnemies.Clear();
            }
            exploded = true;
        }
    }

    private void InstantiateEatenEnemy() {
        playerStats.InstantiateEatenEnemy(transform);
    }
}
