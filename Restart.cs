using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Restart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private GameObject nextStage;
    [SerializeField] private PauseFunction pauseFunction;

    public void ResetGame() {
        pauseFunction.paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseFunction.paused = false;
    }

    public void NextStage() {
        PlayerPrefs.SetFloat("scaleFactor", PlayerPrefs.GetFloat("scaleFactor") + 0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseFunction.paused = false;
    }

    public IEnumerator ResetTime() {
        yield return new WaitForSeconds(4f);
        pauseFunction.paused = false;
    }

    public void ToMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        pauseFunction.paused = false;
    }

    private void Update() {
        // Set score info
        if (PlayerPrefs.GetInt("highscore") < gameInfo.enemiesKilled * 100) {
            PlayerPrefs.SetInt("highscore", gameInfo.enemiesKilled * 100);
        }
        // Set next stage if stage passed
        if (PlayerPrefs.HasKey("passedS" + ((int)((PlayerPrefs.GetFloat("scaleFactor")-0.9f)/0.1f)).ToString())) {
            nextStage.SetActive(true);
        } else {
            nextStage.SetActive(false);
        }
        // Display score and gold earned
        text.text = "Current Score: " + (gameInfo.enemiesKilled*100).ToString() + "\nHighscore: " + PlayerPrefs.GetInt("highscore").ToString();
        text.text += "\nGold gained: " + gameInfo.goldGained.ToString();
    }
}
