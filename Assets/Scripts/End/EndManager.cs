using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EndManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (ScoreManager.instance != null)
        {
            if (ScoreManager.instance.currentScore > ScoreManager.instance.highScore)
            {
                ScoreManager.instance.highScore = ScoreManager.instance.currentScore;
                scoreText.text = "New High Score: " + ScoreManager.instance.currentScore + "!!!";
            }
            else
            {
                scoreText.text = "Score: " + ScoreManager.instance.currentScore;
            }
            ScoreManager.instance.SaveHighScore();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //Switch to menu scene
            SceneManager.LoadScene(0);
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
