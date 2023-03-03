using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarBehaviour : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Color low;
    [SerializeField] private Color high;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private bool still;

    public void SetHealth(float health, float maxHealth) {
        if (!gameObject.name.Contains("Player")) {
            slider.gameObject.SetActive(health < maxHealth);
        }
        slider.value = health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = high;
    }

    private void Update() {
        if (!still) {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        }
    }
}
