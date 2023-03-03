using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float speed;
    private float distance;  
    private bool right;
    
    private void Update()
    {
        // Angle towards player
        Vector3 targ = player.transform.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
        float anglee = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, anglee));

        // Move towards player
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        // Fix rotation
        if (player.transform.position.x - transform.position.x < 0 && !right) {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
            right = !right;
        } else if (player.transform.position.x - transform.position.x > 0 && right) {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
            right = !right;
        }

    }
}
