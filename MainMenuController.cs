using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private GameObject b1;
    [SerializeField] private GameObject b2;
    [SerializeField] private GameObject Loading;
    [SerializeField] private GameObject statCanvas;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject cosmetic;
    [SerializeField] private GameObject difficulty;

    public void PlayGame() {
        bg.SetActive(false);
        b1.SetActive(false);
        b2.SetActive(false);
        Loading.SetActive(true);
        difficulty.SetActive(false);
        StartCoroutine(LoadGame());
    }
    
    public IEnumerator LoadGame() {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadStats() {
        statCanvas.SetActive(true);
        menu.SetActive(false);
    }

    public void LoadMenu() {
        statCanvas.SetActive(false);
        menu.SetActive(true);
        cosmetic.SetActive(false);
    }

    public void LoadCosmetic() {
         menu.SetActive(false);
        cosmetic.SetActive(true);
    }

    public void LoadDifficulty() {
        difficulty.SetActive(true);
    }
}

