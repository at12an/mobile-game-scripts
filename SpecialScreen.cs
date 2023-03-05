using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialScreen : MonoBehaviour
{
    [SerializeField] private Specials specials;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private PlayerStats playerStats;
    private int id1;
    private int id2;
    private int id3;
    private bool subsequent;
    [SerializeField] private TextMeshProUGUI subsequentText;
     [SerializeField] PauseFunction pauseFunction;

    public void StartSpecial()
    {
        // Remove Ui
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        pauseFunction.paused = true;

        // Get valid id
        id1 = GetValidId(100,100);
        id2 = GetValidId(id1,100);
        id3 = GetValidId(id1,id2);

        // Set title, description, functionality of buttons
        button1.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetDescription(id1);
        button2.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetDescription(id2);
        button3.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetDescription(id3);
        button1.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = specials.GetTitles(id1);
        button2.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = specials.GetTitles(id2);
        button3.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = specials.GetTitles(id3);
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => specials.ApplyUpgrade(id1));
        button2.onClick.AddListener(() => specials.ApplyUpgrade(id2));
        button3.onClick.AddListener(() => specials.ApplyUpgrade(id3));
    }

    public void SwitchText() {
        if (!subsequent) {
            button1.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetSubsequents(id1);
            button2.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetSubsequents(id2);
            button3.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetSubsequents(id3);
            subsequent = !subsequent;
            subsequentText.text = "Special";
        } else {
            button1.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetDescription(id1);
            button2.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetDescription(id2);
            button3.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = specials.GetDescription(id3);
            subsequent = !subsequent;
            subsequentText.text = "Subsequents";
        }
    }

    private int GetValidId(int id2, int id3) {
        bool run = true;
        int id = Random.Range(0,7);
        while (run) {
            id = Random.Range(0,7);
            run = false;
            if (id == id2 || id == id3) {
                run = true;
            } else if (playerStats.crystalProj && id == 0) {
                run = true; 
            } else if (playerStats.sporuptionLvl > 0 && id == 3) {
                run = true;
            } else if (id == 5) {
                run = true;
            }
        }
        return id;
    }
}
