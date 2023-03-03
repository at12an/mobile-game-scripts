using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    [SerializeField] private GameObject enemy0;
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy4;
    [SerializeField] private bool boss;
    public bool stage2;

    public void SummonEnemy() {
        if (!stage2) {
            int rand = Random.Range(0,100);
            if (rand <= 30) {
                GameObject clone = Instantiate(enemy0, transform.position, Quaternion.identity);
                clone.SetActive(true);
            } else if (rand <= 60) {
                GameObject clone = Instantiate(enemy1, transform.position, Quaternion.identity);
                clone.SetActive(true);
            } else if (rand <= 90) {
                GameObject clone = Instantiate(enemy2, transform.position, Quaternion.identity);
                clone.SetActive(true);
            }
            gameObject.SetActive(false);
        } else {
            SummonEnemy2();
        }
    }

    public void SummonEnemy2() {
        GameObject clone = Instantiate(enemy4, transform.position, Quaternion.identity);
        clone.SetActive(true);
        gameObject.SetActive(false);
    }
}
