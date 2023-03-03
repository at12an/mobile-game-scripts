using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarBehaviour : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Color high;
    [SerializeField] private Vector3 Offset;

    public void SetExp(float exp, float maxExp) {
        slider.value = exp;
        slider.maxValue = maxExp;
        slider.fillRect.GetComponentInChildren<Image>().color = high;
    }
}
