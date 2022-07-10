using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : MonoBehaviour
{
    private GameObject player;
    private float flightSpeed;
    private float checkBorder;
    // Start is called before the first frame update
    void Start()
    {
        checkBorder = 4f;
        flightSpeed = 3f;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < checkBorder)
        {
            if (player.transform.position.y < transform.position.y - 0.8f)
                transform.Translate(Vector2.down * flightSpeed * Time.deltaTime);
            else if (player.transform.position.y > transform.position.y - 0.4f)
            {
                transform.Translate(Vector2.up * flightSpeed * Time.deltaTime);
            }
            else
            {

            }
        }
    }
}
