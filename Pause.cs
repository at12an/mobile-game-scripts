using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Sprite pause;
    [SerializeField] private Sprite unpause;
    private bool paused;
    
    private void PauseGame() {
        if (!paused) {
            Time.timeScale = 0;
            gameObject.GetComponent<Image>().sprite = unpause;
            paused = true;
        } else {
            Time.timeScale = 1;
            gameObject.GetComponent<Image>().sprite = pause;
            paused = false;
        }
    }
}
