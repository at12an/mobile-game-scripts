using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        // Move with player
        transform.position = new Vector3(target.transform.position.x,target.transform.position.y, transform.position.z );
    }
}
