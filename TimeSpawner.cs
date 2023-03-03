using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
// using Unity.Math;

public class TimeSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject waveEnemy0;
    [SerializeField] private GameObject waveEnemy0S;
    [SerializeField] private GameObject waveEnemy1;
    [SerializeField] private GameObject waveEnemy1S;
    [SerializeField] private GameObject waveEnemy2;
    [SerializeField] private GameObject waveEnemy2S;
    [SerializeField] private GameObject waveEnemy3;
    [SerializeField] private GameObject waveEnemy4;
    [SerializeField] private GameObject broccoli;
    [SerializeField] private GameObject bossEnemy;
    private float gameStart;
    [SerializeField] private GameObject randomSpawner;
    private Camera cam;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;

    void Start()
    {
        cam = Camera.main;
        gameStart = Time.time;
        // StartCoroutine(WaitThenStartSpawning(2.5f, waveEnemy3, 0));
        // StartCoroutine(WaitThenStartSpawningB(3f, waveEnemy0, 0));
        // 0, 90sec
        StartCoroutine(SpawnEnemyWithinTimeFrame(1.5f, waveEnemy0, Time.time, 90f));
        // 0S
        StartCoroutine(SpawnEnemyWithinTimeFrame(80f, waveEnemy0S, Time.time, 1f));
        // 0 1, 90sec
        StartCoroutine(WaitThenStartSpawning(4f, waveEnemy1, 90f));
        StartCoroutine(WaitThenStartSpawning(2f, waveEnemy0, 90f));
        // 1S
        StartCoroutine(SpawnEnemyWithinTimeFrame(170f, waveEnemy1S, Time.time, 1f));
        // 1 2, 90 sec
        StartCoroutine(WaitThenStartSpawning(2f, waveEnemy2, 180f));
        StartCoroutine(WaitThenStartSpawning(3f, waveEnemy1, 180f));
        // 2S
        StartCoroutine(SpawnEnemyWithinTimeFrame(210f, waveEnemy2S, Time.time, 1f));
        // 3 -1 90 sec
        StartCoroutine(WaitThenStartSpawning(6f, waveEnemy3, 270f));
        StartCoroutine(WaitThenStartSpawningB(2f, waveEnemy0, 270f));
        // 4 -1, 90 sec
        StartCoroutine(WaitThenStartSpawning(4f, waveEnemy4, 360f));
        StartCoroutine(WaitThenStartSpawningB(2f, waveEnemy0, 360f));
        // Spawn boss and broccoli
        StartCoroutine(SpawnBoss());
        StartCoroutine(WaitThenStartSpawningB(3f, waveEnemy0, 450f));
    }

    private IEnumerator SpawnBroccoli(float spawnInterval, GameObject enemy, float startTime, float timeFrame) {
        yield return new WaitForSeconds(spawnInterval);
        float x = Random.Range(bottomLeft.position.x, topRight.position.x);
        float y = Random.Range(bottomLeft.position.y, topRight.position.y);
        GameObject newEnemy = Instantiate(enemy, new Vector3(x,y,0), Quaternion.identity);
        newEnemy.SetActive(true);
        if (Time.time - startTime < timeFrame) {
            StartCoroutine(SpawnEnemyWithinTimeFrame(spawnInterval, enemy, startTime, timeFrame));
        }
    }

    private IEnumerator WaitThenStartSpawningB(float spawnInterval, GameObject enemy, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SpawnBroccoli(spawnInterval, enemy, Time.time, 10000f));
    }

    private IEnumerator SpawnEnemyWithinTimeFrame(float spawnInterval, GameObject enemy, float startTime, float timeFrame)
    {
        yield return new WaitForSeconds(spawnInterval);
        float ySize = cam.orthographicSize + 1;
        float xSize = cam.orthographicSize * cam.aspect + 1;
        float x,y;

        // Get x or y
        if (Random.Range(0,2) == 1) {
            // Get any valid x coordinate
            x = Random.Range(bottomLeft.position.x, topRight.position.x);
            // Get y coordinate outside frame
            // Check if player is close to top or bottom then spawn on other side
            if (Math.Abs(topRight.position.y - playerPos.position.y) > Math.Abs(bottomLeft.position.y - playerPos.position.y)) {
                y = Random.Range(topRight.position.y - 15, topRight.position.y - 10);
            } else {
                y = Random.Range(bottomLeft.position.y + 10, bottomLeft.position.y + 15);
            }
        } else {
            // Get any valid x coordinate
            y = Random.Range(bottomLeft.position.y, topRight.position.y);
            // Get y coordinate outside frame
            // Check if player is close to top or bottom then spawn on other side
            if (Math.Abs(topRight.position.x - playerPos.position.x) > Math.Abs(bottomLeft.position.x - playerPos.position.x)) {
                x = Random.Range(topRight.position.x - 10, topRight.position.x-5);
            } else {
                x = Random.Range(bottomLeft.position.x + 5, bottomLeft.position.x + 10);
            }
        }
        GameObject newEnemy = Instantiate(enemy, new Vector3(x,y,0), Quaternion.identity);
        newEnemy.SetActive(true);

        // Repeat spawning cycle
        if (Time.time - startTime < timeFrame) {
            StartCoroutine(SpawnEnemyWithinTimeFrame(spawnInterval, enemy, startTime, timeFrame));
        }
    }

    private IEnumerator WaitThenStartSpawning(float spawnInterval, GameObject enemy, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SpawnEnemyWithinTimeFrame(spawnInterval, enemy, Time.time, 90f));
    }

    private IEnumerator SwapSpawningScript() {
        yield return new WaitForSeconds(600f);
        randomSpawner.SetActive(true);
    }

    private IEnumerator SpawnBoss() {
        yield return new WaitForSeconds(450f);
        bossEnemy.SetActive(true);
    }
    
}
