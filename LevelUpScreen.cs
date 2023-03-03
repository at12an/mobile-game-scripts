using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour
{
    // [SerializeField] private GameObject levelScreen;
    [SerializeField] private Upgrades upgrades;
    // public FixedJoystick joystick;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    private List<int> attack = new List<int> {0,1};
    private List<int> hp = new List<int> {2,3};
    private List<int> crit = new List<int> {11,12};
    [SerializeField] private List<Sprite> cards;
    
    // Start is called before the first frame update
    public void StartLevelUp()
    {
        // Turn off ui
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        gameObject.SetActive(true);

        // Stop time
        Time.timeScale = 0;

        // Generate three random numbers and change buttons accordingly
        int id1 = GetValidId(100,100);
        int id2 = GetValidId(100,id1);
        int id3 = GetValidId(id2, id1);

        // Fill in description
        button1.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = upgrades.GetDescription(id1);
        button2.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = upgrades.GetDescription(id2);
        button3.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0].text = upgrades.GetDescription(id3);

        // Fill in title
        button1.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = upgrades.GetTitles(id1);
        button2.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = upgrades.GetTitles(id2);
        button3.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = upgrades.GetTitles(id3);

        // Remove and apply on click event
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => upgrades.ApplyUpgrade(id1));
        button2.onClick.AddListener(() => upgrades.ApplyUpgrade(id2));
        button3.onClick.AddListener(() => upgrades.ApplyUpgrade(id3));

        // Update image
        button1.GetComponent<Image>().sprite = cards[id1];
        button2.GetComponent<Image>().sprite = cards[id2];
        button3.GetComponent<Image>().sprite = cards[id3];
    }

    // Generate valid Id
    private int GetValidId(int id2, int id3) {
        bool run = true;
        int id = Random.Range(0,21);
        while (run) {
            id = Random.Range(0,21);
            run = false;
            if (playerStats.critChance >= 1 && (id == 11 || id == 12)) {
                run = true;
            } else if (playerStats.suckCouldown.GetValue() <= 1f && id == 6) {
                run = true;
            } else if (id == 15 || id == 13) {
                run = true;
            } else if (playerStats.currentHealth / playerStats.maxHealth >= 0.5 && id == 14) {
                run = true;
            } else if (attack.Contains(id) && (attack.Contains(id3) || attack.Contains(id2))) {
                run = true;
            } else if (hp.Contains(id) && (hp.Contains(id3) || hp.Contains(id2))) {
                run = true;
            } else if (crit.Contains(id) && (crit.Contains(id3) || crit.Contains(id2))) {
                run = true;
            } else if (id == id2 || id == id3) {
                run = true;
            } else if (id == 19 && playerStats.modelModifier > 1) {
                run = true;
            }
        }
        return id;
    }

}
