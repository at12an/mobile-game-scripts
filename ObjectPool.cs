using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledExplosions = new List<GameObject>();

    private List<GameObject> pooledSporuptions = new List<GameObject>();

    private List<GameObject> pooledDP = new List<GameObject>();

    private List<GameObject> pooledExp = new List<GameObject>();


    private int explosionPoolSize = 60;
    // private int expPoolSize = 1000;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject sporuptionPrefab;
    [SerializeField] private GameObject dPPrefab;
    [SerializeField] private GameObject expPrefab;


    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start()
    {
        // Fill pools with respective objects
        for (int i = 0; i < explosionPoolSize ; i++) {
            GameObject obj = Instantiate(explosionPrefab);
            obj.SetActive(false);
            pooledExplosions.Add(obj);
        }

        for (int i = 0; i < explosionPoolSize ; i++) {
            GameObject obj = Instantiate(sporuptionPrefab);
            obj.SetActive(false);
            pooledSporuptions.Add(obj);
        }
        // for (int i = 0; i < expPoolSize ; i++) {
        //     GameObject obj = Instantiate(expPrefab);
        //     obj.SetActive(false);
        //     pooledExp.Add(obj);
        // }
    }

    public GameObject GetPooledExplosion() {
        for (int i = 0; i < pooledExplosions.Count; i++) {
            if (!pooledExplosions[i].activeInHierarchy) {
                return pooledExplosions[i];
            }
        }
        return null;
    }

    public GameObject GetPooledSporuption() {
        for (int i = 0; i < pooledSporuptions.Count; i++) {
            if (!pooledSporuptions[i].activeInHierarchy) {
                return pooledSporuptions[i];
            }
        }
        return null;
    }

    public GameObject GetPooledDP() {
        for (int i = 0; i < pooledDP.Count; i++) {
            if (!pooledDP[i].activeInHierarchy) {
                return pooledDP[i];
            }
        }
        return null;
    }

    public GameObject GetPooledExp() {
        for (int i = 0; i < pooledExp.Count; i++) {
            if (!pooledExp[i].activeInHierarchy) {
                return pooledExp[i];
            }
        }
        return null;
    }
}
