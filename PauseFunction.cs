using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFunction : MonoBehaviour
{
    public bool paused;

    // Update is called once per frame
    void Update()
    {
        if (paused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }
}
