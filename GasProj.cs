using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasProj : MonoBehaviour
{
    private float startY;
    [SerializeField] private GameObject gas;
    [SerializeField] private float speed;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        // Fall then explode after certain distance
        if (startY - transform.position.y <= 6) {
            transform.position = new Vector3(transform.position.x,transform.position.y - speed * Time.deltaTime, 0);
        } else {
            Explode();
        }
    }

    private void Explode() {
        GameObject newGas = Instantiate(gas, transform.position, Quaternion.identity);
        newGas.SetActive(true);
        Destroy(gameObject);
    }
}
