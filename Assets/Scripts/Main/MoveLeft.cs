using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private PlayerController player;
    public float m_Speed = 5f;

    private float leftBorder = -40f;

    // Start is called before the first frame update
    void Start()
    {
        player =  GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop when the game is over
        if (!player.gameOver)
        {
            transform.Translate(Vector2.left * m_Speed * Time.deltaTime);
        }

        // Destroy the object if it goes off bound
        if (transform.position.x < leftBorder)
        {
            Destroy(gameObject);
        }

    }
}
