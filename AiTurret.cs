using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTurret : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject laser;
    [SerializeField] private Transform LaunchOffset;

    private Quaternion rotation;


    public float chargeUp;
    public bool charging;
    public bool shooting;
    public bool aim;

    private void Awake() {
    }

    private void Update() {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 7.5f) {
            if (!charging) {
                animator.SetBool("Charging", true);
                chargeUp = Time.time;
                charging = true;
            }
            if (Time.time - chargeUp >= 0.7f && !aim) {
                CalculateRotation();
                aim = true;
            }
            if (Time.time - chargeUp >= 1f && !shooting) {
                aim = false;
                laser.transform.position = CalculateCursorPos(7.5f);
                laser.transform.rotation = rotation;
                animator.SetBool("Shoot", true);
                laser.SetActive(true);
                shooting = true;
                chargeUp = Time.time;
            }
        } else {
            animator.SetBool("Charging", false);
            charging = false;
            aim = false;
        }

    }

    public void CalculateRotation() {
        rotation = Quaternion.LookRotation(Vector3.forward, (player.position - transform.position));
        rotation *= Quaternion.Euler(0, 0, 90);
    }

    private Vector3 CalculateCursorPos(float size) {
        float angle = rotation.eulerAngles.z * Mathf.PI/180;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        float z = 0;
        Vector3 suction_adjust = new Vector3(x,y,z);
        suction_adjust *= size;
        return LaunchOffset.position + suction_adjust;
    }
}
