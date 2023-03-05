using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Sprite pause;
    [SerializeField] private Sprite unpause;
    [SerializeField] private PauseFunction pauseFunction;
    
    private void PauseGame() {
        if (pauseFunction.paused) {
            pauseFunction.paused = false;
            gameObject.GetComponent<Image>().sprite = unpause;
        } else {
            pauseFunction.paused = true;
            gameObject.GetComponent<Image>().sprite = pause;
        }
    }
}
