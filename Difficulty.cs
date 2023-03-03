using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Difficulty : MonoBehaviour
{
    [SerializeField] private int difficultyy;
    [SerializeField] private TextMeshProUGUI difficultyText;

    private void Awake() {
        difficultyy = 1;
        if (!PlayerPrefs.HasKey("scaleFactor")) {
            PlayerPrefs.SetFloat("scaleFactor",1f);
        }
    }

    private void Update()
    {
        difficultyText.text = difficultyy.ToString();
    }

    private void IncreaseDifficulty() {
        if (PlayerPrefs.HasKey("passedS" + difficultyy.ToString())) {
            difficultyy += 1;
            PlayerPrefs.SetFloat("scaleFactor", 1+ (difficultyy-1) *0.1f);
        }
    }

    private void DecreaseDifficulty() {
        if (difficultyy > 1) {
            difficultyy -= 1;
            PlayerPrefs.SetFloat("scaleFactor", 1+ (difficultyy-1) *0.1f);
        }
    }

}
