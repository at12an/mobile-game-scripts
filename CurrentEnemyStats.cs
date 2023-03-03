using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEnemyStats : MonoBehaviour
{
    public float atk;
    public float currentHealth;
    public float maxHealth;
    public float speed;

    private void Awake()
    {
        // Scale stats to stage
        if (!PlayerPrefs.HasKey("scaleFactor")) {
            PlayerPrefs.SetFloat("scaleFactor",1f);
        }
        atk *= PlayerPrefs.GetFloat("scaleFactor");
        currentHealth *= PlayerPrefs.GetFloat("scaleFactor");
        maxHealth *= PlayerPrefs.GetFloat("scaleFactor");
    }

    private void Update()
    {
        // Slowly scale stats
        maxHealth += (float)1 * Time.deltaTime * PlayerPrefs.GetFloat("scaleFactor");
        atk += (float)0.06 * Time.deltaTime * PlayerPrefs.GetFloat("scaleFactor");
        speed += (float)0.003 * Time.deltaTime;
    }
}
