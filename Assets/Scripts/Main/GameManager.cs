using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    // UI
    public TextMeshProUGUI scoreText;
    public GameObject RedFlag; // Indicator shooting enemies 
    public GameObject GreenFlag; // Indicator for normal obstacles
    public GameObject YellowFlag; // Indicator for interactable wave
    public GameObject BlueFlag; // Indicator for moving enemies

    int numOfSpawns = 4;

    // Interactables
    int numOfInteractables = 2;
    public GameObject cannonPrefab;
    public GameObject bouncePadPrefab;

    // Collectibles
    int numOfCollectibles = 2;
    public GameObject beerPrefab;
    public GameObject liveUpPrefab;
     
    // Obstacles
    int numOfObstacles = 5;
    public GameObject spikePrefab;
    public GameObject bombPrefab;
    public GameObject eaglePrefab;
    public GameObject frogPrefab;
    public GameObject soldierPrefab;

    private float startDelay = 0;
    private float repeatRate = 4;

    // To check if the game is over
    private PlayerController playerController;

    // Keep track of the score
    private int score;


    // Start is called before the first frame update
    void Start()
    { 
        // Call spawn method automatically after an amount of time
        InvokeRepeating("SpawnObstacles", startDelay, repeatRate);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // Start score counting
        score = 0;
        StartCoroutine(UpdateScore());

        // Deactivate flags at the beginning
        RedFlag.SetActive(false);
        GreenFlag.SetActive(false);
        YellowFlag.SetActive(false);
        BlueFlag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // When game is over, save the current score
        if (playerController.gameOver)
        {
            if (ScoreManager.instance!= null)
                ScoreManager.instance.currentScore = score;
        }
    }

    void SpawnObstacles()
    {
        if (!playerController.gameOver)
        {
            // 75% Normal wave: 2 obstacles and 1 collectible
            if (Random.Range(0, 4) <= 2)
            {
                SpawnNormalWave();
            }
            // 25% Interactable wave: 1 interactables with 4 enemies
            else
            {
                SpawnInteractableWave();
            }
        }
    }

    // Add 1 point for every second and display it
    IEnumerator UpdateScore()
    {
        while (!playerController.gameOver)
        {
            AddScore(1);
            yield return new WaitForSeconds(1);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // Spawn wave of interactable item and obstacles that can only be overcomed by using the interactables
    private void SpawnInteractableWave()
    {
        int interactableIndex = Random.Range(0, numOfInteractables);
        int spawnPosX = 10;
        switch (interactableIndex)
        {
            case 0: // Cannon and soldiers
                callFlag(YellowFlag ,Instantiate(cannonPrefab, new Vector3(spawnPosX, -2.5f, 0), cannonPrefab.transform.rotation));
                for (int i = 0; i < numOfSpawns; i++)
                {
                    spawnPosX = 10 + 8 + (i * 1);
                    Instantiate(soldierPrefab, new Vector3(spawnPosX, -2, 0), soldierPrefab.transform.rotation);
                }
                break;
            case 1: // Bounce pad and bombs
                // First set of bounce pad and bomb;
                callFlag(YellowFlag, Instantiate(bouncePadPrefab, new Vector3(spawnPosX, -2.5f, 0), bouncePadPrefab.transform.rotation));
                spawnPosX = spawnPosX + 2;
                for (int i = 0; i < 3; i++)
                {
                    float spawnPosY = -i * 2  + 2;
                    Instantiate(bombPrefab, new Vector3(spawnPosX, spawnPosY, 0), bombPrefab.transform.rotation);
                }

                // Second set of bounce pad and bombs
                spawnPosX = 17; 
                Instantiate(bouncePadPrefab, new Vector3(spawnPosX, -2.5f, 0), bouncePadPrefab.transform.rotation);
                spawnPosX = spawnPosX + 2;
                for (int i = 0; i < 3; i++)
                {
                    float spawnPosY = -i * 2 + 2;
                    Instantiate(bombPrefab, new Vector3(spawnPosX, spawnPosY, 0), bombPrefab.transform.rotation);
                }
                break;
            default:
                break;
        }
    }

    // Spawn normal (non interactable wave of obstacles and collectables
    private void SpawnNormalWave()
    {
        for (int i = 0; i < numOfSpawns; i++)
        {
            int spawnPosX = 10 + (i * 4);
            int height = Random.Range(-2, 1);
            // Spawn obstacles
            if (i % 2 == 0)
            {
                int obstacleIndex = Random.Range(0, numOfObstacles);
                switch (obstacleIndex)
                {
                    case 0: // Spike
                        callFlag(GreenFlag, Instantiate(spikePrefab, new Vector3(spawnPosX, -3, 0), spikePrefab.transform.rotation));
                        break;
                    case 1: // Bomb
                        callFlag(GreenFlag, Instantiate(bombPrefab, new Vector3(spawnPosX, height, 0), bombPrefab.transform.rotation));
                        break;
                    case 2:
                        callFlag(BlueFlag, Instantiate(eaglePrefab, new Vector3(spawnPosX, height, 0), eaglePrefab.transform.rotation));
                        break;
                    case 3:
                        callFlag(BlueFlag, Instantiate(frogPrefab, new Vector3(spawnPosX, -2.66f, 0), frogPrefab.transform.rotation));
                        break;
                    case 4:
                        callFlag(RedFlag, Instantiate(soldierPrefab, new Vector3(spawnPosX, -2, 0), soldierPrefab.transform.rotation));
                        break;
                    default:
                        break;
                }
            }

            // Spawn Collectibles
            else
            {
                int collectibleIndex = Random.Range(0, numOfCollectibles);
                switch (collectibleIndex)
                {
                    case 0:
                        Instantiate(beerPrefab, new Vector3(spawnPosX, height, 0), bombPrefab.transform.rotation);
                        break;
                    case 1:
                        if (Random.Range(0, 4) < 3)
                        {
                            Instantiate(beerPrefab, new Vector3(spawnPosX, height, 0), bombPrefab.transform.rotation);
                        }
                        else // Only spawn live-up 25% of the time
                        {
                            Instantiate(liveUpPrefab, new Vector3(spawnPosX, height, 0), liveUpPrefab.transform.rotation);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Call the Flag and set the flag to watch the target
    private void callFlag(GameObject flag, GameObject target)
    {
        FlagController flagController = flag.GetComponent<FlagController>();

        flagController.targetObject = target;
        flag.SetActive(true);

    }
}
