using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSpit : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;
    [SerializeField] private PlayerStats playerStats;
    private bool exploded;
    [SerializeField] private GameObject poisonPool;

    private void Start()
    {
        exploded = false;
        speed = playerStats.spitSpeed.GetValue();
    }

    private void Update()
    {
        // Move towards direction
        transform.Translate(Vector2.right*speed *Time.deltaTime);

        // Slow then collide 
        if (speed > 2) {
            speed -= 10 * Time.deltaTime;
        } else {
            Collision();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Enemy") && !exploded) { 
            Collision();
            exploded = true;
        } else if (col.gameObject.name.Contains("Border") && !exploded){
            Collision();
            exploded = true;
        }
    }

    private void Collision() {
        UnityEngine.Object.Destroy(gameObject);

        // Poison attack or normal
        if (!playerStats.poisonAttack) {    
            GameObject explosion_clone = ObjectPool.instance.GetPooledExplosion();
            if (explosion_clone != null) {
                explosion_clone.transform.position = transform.position;
                explosion_clone.SetActive(true);
                explosion_clone.GetComponent<ExplosionBehaviour>().nerfed = true;
            }
        } else {
            GameObject ppClone = Instantiate(poisonPool, transform.position, Quaternion.identity);
            ppClone.SetActive(true);
            ppClone.GetComponent<PoisonPool>().nerfed = true;
            ppClone.GetComponent<ExplosionBehaviour>().nerfed = true;
            foreach (GameObject obj in playerStats.eatenEnemies) {
                obj.SetActive(true);
                obj.transform.position = new Vector3(Random.Range(transform.position.x-3f, transform.position.x+3f), Random.Range(transform.position.y-3f, transform.position.y+3f), 0 );
            }
        }
    }
}
