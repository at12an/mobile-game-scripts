using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPool : MonoBehaviour
{
    [SerializeField] private GameObject poisonPoolPrefab;
    [SerializeField] private Transform offset;
    [SerializeField] private PlayerStats playerStats;
    public bool nerfed;

    private void SpawnPool() {
        GameObject ppclone = Instantiate(poisonPoolPrefab, offset.position, Quaternion.identity);
        ppclone.SetActive(true);
        if (nerfed) {
            ppclone.GetComponent<PoisonDamage>().nerfed = true;
        }
    }

    private void Kill() {
        Destroy(gameObject);
    }

    private void IncreaseSize() {
        if (nerfed) {
            transform.localScale *= 0.5f;
        }
        transform.localScale *= playerStats.explosionSizeMod;
    }
}
