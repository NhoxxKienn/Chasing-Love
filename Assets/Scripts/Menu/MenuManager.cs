using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    // Difficulties buttons
    [SerializeField] private GameObject[] buttons;
    private int currentIndex;
    private float waitTime = 0.25f;
    [SerializeField]private float tapTime = 0;
    [SerializeField]private int taps = 0;

    // Buttons sound effects
    [SerializeField] private AudioSource switchingSFX;
    [SerializeField] private AudioSource pressingSFX;

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.instance.currentScore = 0;
        highscoreText.text = "High Score: " + ScoreManager.instance.highScore;


        currentIndex = 2;
        buttons[currentIndex].GetComponent<Animator>().SetBool("isHighlighted", true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            taps++;
            if (taps == 1)
            {
                tapTime = Time.time;   
            }
        }

        if (taps >= 1 &&  (Time.time - tapTime) > waitTime )
        {
            buttons[currentIndex].GetComponent<Animator>().SetBool("isHighlighted", false);
            if (currentIndex < buttons.Length - 1) currentIndex++;
            else currentIndex = 0;
            buttons[currentIndex].GetComponent<Animator>().SetBool("isHighlighted", true);
            taps = 0;
            tapTime = 0;
            switchingSFX.Play();
        }

        if (taps > 1 && (Time.time - tapTime) <= waitTime)
        {
            pressingSFX.Play();
            ScoreManager.instance.difficulty = currentIndex + 1;
            buttons[currentIndex].GetComponent<Animator>().SetBool("isPressed", true);
            //Switch to main scene
            SceneManager.LoadScene(1);
        }

        // Quit if hit Esc
        if (Input.GetButtonDown("Cancel"))
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
