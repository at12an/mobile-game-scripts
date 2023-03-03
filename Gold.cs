using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] GameInfo gameInfo;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name.Contains("Player")) { 
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 10);
            Destroy(gameObject);
            gameInfo.goldGained += 1;
        }
    }
}
