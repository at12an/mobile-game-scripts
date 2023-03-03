using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    // public static DamagePopup Create(Vector3 pos, int damage, GameObject damagePopup) {
    //     Transform dpTransform = Instantiate(damagePopup, pos, Quaternion.identity).transform;
    //     DamagePopup dp = dpTransform.GetComponent<DamagePopup>();
    //     dp.Setup(damage);
    //     return dp;
    // }

    [SerializeField] private TextMeshPro textMP;
    private float fadeTimer;
    private Color textColor;
    [SerializeField] private Color critColor;

    private void Awake() {
        textColor = textMP.color;
    }

    public void Setup(float damage, bool isCrit) {
        textMP.SetText(((int)damage).ToString());
        fadeTimer = 1f;
        if (isCrit) {
            textMP.fontSize = 13;
            textColor = critColor;
        }
        textMP.color = textColor;
    }

    private void Update() {
        transform.Translate(Vector3.up * Time.deltaTime, Space.World);

        fadeTimer -= Time.deltaTime;
        if (fadeTimer < 0) {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMP.color = textColor;
            if (textColor.a < 0) {
                fadeTimer = 1f;
                textColor.a = 255f;
                Destroy(gameObject);
            }
        }
    }

}
