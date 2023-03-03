using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    [SerializeField] private float RotateSpeed;
    [SerializeField] private float Radius;
    [SerializeField] private Transform center;
    private float angle;

    private void Update()
    {
        angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * Radius;
        Vector3 diff = new Vector3(offset.x, offset.y, 0);
        diff.Normalize();
 
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        transform.position = new Vector3(center.position.x + offset.x, center.position.y + offset.y, 0);
    }
}
