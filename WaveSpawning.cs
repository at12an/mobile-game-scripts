using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawning : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject waveEnemy0;
    [SerializeField] private GameObject waveEnemy1;
    [SerializeField] private GameObject waveEnemy2;
    [SerializeField] private GameObject waveEnemy0S;
    [SerializeField] private GameObject waveEnemy1S;
    [SerializeField] private GameObject waveEnemy2S;
    [SerializeField] private float interval;
    private Camera cam;

    void Update() {
        if (interval > 0.5f) {
            interval -= (float)0.005 * Time.deltaTime;
        }
    }

    void Start()
    {   
        cam = Camera.main;
        interval = 1f;
        StartCoroutine(spawnEnemy());
    }

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(interval);
        float ySize = cam.orthographicSize + 1;
        float xSize = cam.orthographicSize * cam.aspect + 1;
        bool run = true;
        float y = 0, x = 0;
        while (run) {
            y = Random.Range(-ySize, ySize);
            x = Random.Range(-xSize, xSize);
            if (!((x > -xSize + 1 && x < xSize - 1) && (y > -ySize + 1  && y < ySize - 1))) {
                run = false;
            }
        }
        int rand = Random.Range(0,100);
        if (rand < 32) {
            GameObject newEnemy = Instantiate(waveEnemy0, new Vector3(x + playerPos.position.x, y + playerPos.position.y, 0), Quaternion.identity);
            newEnemy.SetActive(true);
        } else if (rand < 64) {
            GameObject newEnemy = Instantiate(waveEnemy1, new Vector3(x + playerPos.position.x, y + playerPos.position.y, 0), Quaternion.identity);
            newEnemy.SetActive(true);
        } else if (rand < 96) {
            GameObject newEnemy = Instantiate(waveEnemy2, new Vector3(x + playerPos.position.x, y + playerPos.position.y, 0), Quaternion.identity);
            newEnemy.SetActive(true);
        } else if (rand < 97) {
            GameObject newEnemy = Instantiate(waveEnemy0S,new Vector3(x + playerPos.position.x, y + playerPos.position.y, 0), Quaternion.identity);
            newEnemy.SetActive(true);
        } else if (rand < 98) {
            GameObject newEnemy = Instantiate(waveEnemy1S,new Vector3(x + playerPos.position.x, y + playerPos.position.y, 0), Quaternion.identity);
            newEnemy.SetActive(true);
        } else if (rand < 99) {
            GameObject newEnemy = Instantiate(waveEnemy2S,new Vector3(x + playerPos.position.x, y + playerPos.position.y, 0), Quaternion.identity);
            newEnemy.SetActive(true);
        }
        StartCoroutine(spawnEnemy());
    }
}
