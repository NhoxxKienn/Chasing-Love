using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    private float jumpSpeed = 20f;
    private float topBorder = 10f;
    [SerializeField] private GameObject actualBoss;
    [SerializeField] private ParticleSystem waterSproutParticle;
    [SerializeField] private AudioSource bossJumpSFX;
    private float timer = 2.0f; 

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        bossJumpSFX.Play();
        waterSproutParticle.Play();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (transform.position.y < topBorder)
            {
                transform.Translate(Vector2.up * jumpSpeed * Time.deltaTime);
            }
            else
            {
                gameManager.boss = Instantiate(actualBoss, transform.position, Quaternion.identity);
                gameManager.bossController = gameManager.boss.GetComponent<BossController>();
                Destroy(gameObject);
            }
        }
    }
}
