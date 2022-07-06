using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Attributes of the Boss
    private int lives = 4;
    private float chargeSpeed = 20f;
    private bool chargeAttack = false;
    private float leftBorder = -2f;
    private Vector3 normalPos = new Vector3(5, -2.6f, 0);

    private float startDelay = 4;
    private float repeatRate = 5;

    private Rigidbody2D bossRB;
    private Animator bossAnimator;
    private PlayerController playerController;

    // Obstacles to Spawn
    public GameObject soldierPrefab;
    public GameObject bombPrefab;
    public GameObject spikePrefab;


    // Interactables to Spawn
    public GameObject cannonPrefab;
    public GameObject bouncePadPrefab;
    private Vector2 bouncePadSpawnPos = new Vector2(5, -2.5f);

    // Start is called before the first frame update.
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("AttackRoutine", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
    }

    void AttackRoutine()
    {
        if (!playerController.gameOver)
        {
            int index = Random.Range(0, 5);
            // 40%: Charge Attack
            if ( index < 2 )
            {
                bossAnimator.SetTrigger("Preparation");
                StartCoroutine(ChargePreparation());
                Instantiate(bouncePadPrefab, bouncePadSpawnPos, bouncePadPrefab.transform.rotation);

            }
            // 40% Spawn Obstacles
            else if (index >= 2 && index < 4)
            {
                bossAnimator.SetTrigger("SpawnObstacle");
                int numOfWaves = 5;
                float spawnPosX = 10;
                for (int i = 0; i < numOfWaves; i++)
                {
                    spawnPosX += 3;
                    int rand = Random.Range(0, 3);
                    for (int j = 0; j < 3; j++)
                    {
                        if (j != rand)
                        {
                            float spawnPosY = -i * 2 + 2;
                            Instantiate(bombPrefab, new Vector3(spawnPosX, spawnPosY, 0), bombPrefab.transform.rotation);
                        }
                    }
                }
                StartCoroutine(ReturnToIdle());
            }
            else // 20%: Summon Soldiers
            {
                bossAnimator.SetTrigger("Summon");
                float spawnPosX = 10;
                int numOfSpawns = 10;
                Instantiate(cannonPrefab, new Vector3(spawnPosX, -2.5f, 0), cannonPrefab.transform.rotation);
                for (int i = 0; i < numOfSpawns; i++)
                {
                    spawnPosX = 10 + 7 + (i * 0.5f);
                    Instantiate(soldierPrefab, new Vector3(spawnPosX, -2, 0), soldierPrefab.transform.rotation);
                }
                StartCoroutine (ReturnToIdle());
            }
        }
    }

    IEnumerator ChargePreparation()
    {
        yield return new WaitForSeconds(1);
        bossAnimator.SetTrigger("Charge");
        chargeAttack = true;
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(1);
        bossAnimator.SetTrigger("isIdle");
    }

    // The Boss charge forward and return back
    void Charge()
    {
        if (chargeAttack && transform.position.x > leftBorder)
        {
            transform.Translate(Vector2.left * chargeSpeed * Time.deltaTime);
        }
        else
        {
            chargeAttack = false;
            if (transform.position.x < normalPos.x)
            {
                transform.Translate(Vector2.right * chargeSpeed * Time.deltaTime);
            }
            else
            {
                bossAnimator.SetTrigger("isIdle");
            }
        }
    }
}
