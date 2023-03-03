using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private float modifier = 1;

    public float GetValue() {
        return baseValue * modifier;
    }

    public void IncreaseModifier(float increase) {
        modifier += increase;
    }

    public void DecreaseModifier(float decrease) {
        modifier -= decrease;
    }
    public void SetBaseValue(float value) {
        baseValue = value;
    }
}
