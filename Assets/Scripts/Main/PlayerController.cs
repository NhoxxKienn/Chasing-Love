using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_jumpForce = 35f;

    private Rigidbody2D m_Rigidbody;
    private GameManager m_GameManager;

    // Gravity scale when jumping and falling
    [SerializeField] private float normalGravityScale = 10f;
    [SerializeField] private float fallingGravityScale = 40f;

    // Variable to determine if the game is over
    public bool gameOver = false;
    public bool isInvincible;

    // Healthbars
    [SerializeField] private GameObject[] healthBars;

    // Lives System
    private int playerLives;
    private int maxLives;

    // Determine if the player is on ground
    private bool onGround;

    // Variables to control the number of jumps
    public int maxJumpCount = 2;
    public int jumpCount;
    public bool isInterating;

    // Sound Effects
    [SerializeField] AudioSource jumpingSFX;
    [SerializeField] AudioSource landingSFX;
    [SerializeField] AudioSource hurtingSFX;
    [SerializeField] AudioSource collectingSFX;
    [SerializeField] AudioSource winningSFX;



    // Particles
    public GameObject runParticle;
    public GameObject jumpParticle;
    public GameObject fallParticle;

    // Animator
    private Animator animator;

    void Start()
    {
        Time.timeScale = 1;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_GameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();

        onGround = true;
        jumpCount = 0;
        gameOver = false;
        isInterating = false;
        isInvincible = false;
        if (ScoreManager.instance != null)
        {
            maxLives = 6 - ScoreManager.instance.difficulty;
        }
        else
        {
            maxLives = 3;
        }
        playerLives = maxLives;

        runParticle.SetActive(true);
        jumpParticle.SetActive(false);
        fallParticle.SetActive(false); 

        // Setup health bars
        foreach (GameObject healthbar in healthBars)
        {
            healthbar.SetActive(false);
        }
        healthBars[maxLives - 1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {   
        
        // Handle jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount && !gameOver && !isInterating)
        {
            // Reset the velocity before starting the next jump
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            jumpingSFX.Play();
            if (onGround)
            {
                jumpParticle.SetActive(true);
                runParticle.SetActive(false);
                onGround = false;
            }


        }
        
        // When jumping change to jump animation
        if (m_Rigidbody.velocity.y > 0)
        {
            m_Rigidbody.gravityScale = normalGravityScale;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        // When falling falls faster for better feels 
        else if (m_Rigidbody.velocity.y < 0)
        {
            m_Rigidbody.gravityScale = fallingGravityScale;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);

        }
        // Change back to running animation
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // When hit the ground, reset jump
        if (collision.gameObject.CompareTag("Ground"))
        {
            landingSFX.Play();
            fallParticle.SetActive(true);
            onGround = true;
            jumpCount = 0;
            runParticle.SetActive(true);
        }

        // If hit obstacle, died
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isInvincible)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                playerLives--;
                healthBars[maxLives - 1].GetComponent<HealthBar>().LifeDown();
                if (playerLives == 0)
                {
                    hurtingSFX.Play();
                    animator.SetBool("isDead", true);
                    gameOver = true;
                    Destroy(collision.gameObject);
                    Time.timeScale = 1;
                    StartCoroutine("TransitionToEndScene");
                }
                else
                {
                    hurtingSFX.Play();
                    animator.SetBool("isHurt", true);
                    Destroy(collision.gameObject);
                    isInvincible = true;
                    StartCoroutine("Invincible");
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectibles"))
        {
            collectingSFX.Play();
            m_GameManager.AddScore(10);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Live-Up"))
        {
            collectingSFX.Play();
            if (playerLives < maxLives)
            {
                playerLives++;
                healthBars[maxLives - 1].GetComponent<HealthBar>().LifeUp();
            } else 
            {
                m_GameManager.AddScore(10);
            }
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Parrot"))
        {
            m_GameManager.AddScore(500);
            gameOver = true;
            winningSFX.Play();
            StartCoroutine("TransitionToWinScene");

        }

        if (other.CompareTag("Interactables"))
        {
            isInterating = true;
        }

        if (other.CompareTag("Boss"))
        {
            if (isInvincible)
            {
                
            }
            else
            {
                playerLives--;
                healthBars[maxLives - 1].GetComponent<HealthBar>().LifeDown();
                if (playerLives == 0)
                {
                    hurtingSFX.Play();
                    animator.SetBool("isDead", true);
                    gameOver = true;
                    StartCoroutine("TransitionToEndScene");
                }
                else
                {
                    hurtingSFX.Play();
                    animator.SetBool("isHurt", true);
                    isInvincible = true;
                    StartCoroutine("Invincible");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactables"))
        {
            isInterating = false;
        }
    }

    IEnumerator TransitionToEndScene()
    {
        // Wait one second and load end scene
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(2);
    }

    IEnumerator TransitionToWinScene()
    {
        // Wait one second and load end scene
        yield return new WaitForSecondsRealtime(0.8f);
        SceneManager.LoadScene(3);
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("isHurt", false);
        isInvincible = false;
    }
}
