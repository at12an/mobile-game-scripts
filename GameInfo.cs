using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameInfo : MonoBehaviour
{
    public int enemiesKilled;
    private float startTime;
    [SerializeField] private TMPro.TextMeshProUGUI score;
    [SerializeField] private TMPro.TextMeshProUGUI timer;
    [SerializeField] private TMPro.TextMeshProUGUI revives;
    [SerializeField] private TMPro.TextMeshProUGUI level;
    [SerializeField] private PlayerStats playerStats;
    public int goldGained;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        // Deal with score
        score.text = "Score: " + (enemiesKilled*100).ToString();

        // Deal with time
        TimeSpan t = TimeSpan.FromSeconds((int)(Time.time-startTime));
        String time = t.Minutes.ToString() + ":";
        if (t.Minutes < 10) {
            time = "0" + time;
        }
        if (t.Seconds < 10) {
            time = time + "0";
        }
        time = time + t.Seconds.ToString();
        timer.text = time;
        level.text = playerStats.level.ToString();
    }
}
