using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    private int gasStage;
    private float timer;

    private void Start()
    {
        gasStage = 0;
        timer = Time.time;
    }

    private void KillSelf() {
        Destroy(gameObject);
    }

    private IEnumerator GasGrow() {
        yield return new WaitForSeconds(0.5f);
        if (gasStage < 5) {
            transform.localScale *= 1.25f;
            gasStage += 1;
            StartCoroutine(GasGrow());
        } else {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            PlayerStats playerStats = col.gameObject.GetComponent<PlayerStats>();
            playerStats.poisoned = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            PlayerStats playerStats = col.gameObject.GetComponent<PlayerStats>();
            playerStats.poisoned = false;
        }
    }
}
