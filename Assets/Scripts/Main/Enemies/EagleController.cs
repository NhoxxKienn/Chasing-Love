using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EagleController : MonoBehaviour
{
    private GameObject player;
    private GameManager gameManager;


    private float flightSpeed;
    private float checkBorder;



    // Instruction Text for first use
    public TextMeshProUGUI instructionText;


    // Start is called before the first frame update
    void Start()
    {
        checkBorder = 4f;
        flightSpeed = 3f;
        player = GameObject.Find("Player");

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (gameManager.hasShownEagleInstruction)
        {
            instructionText.gameObject.SetActive(false);
        }
        else
        {
            instructionText.gameObject.SetActive(true);
            gameManager.hasShownEagleInstruction = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < checkBorder)
        {
            if (player.transform.position.y < transform.position.y - 0.6f)
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
