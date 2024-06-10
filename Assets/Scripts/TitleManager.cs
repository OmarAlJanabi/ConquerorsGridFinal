using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class TitleManager : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameManager gameManager;
    public GameObject Difficulty;
    public GameObject MainMenu;
    public GameObject red;
    public GameObject blue;
    public GameObject current;
    public ScenesManager scenesManager;
    public string difficultyLevel;// Reference to ScenesManager script


    // Start is called before the first frame update
    void Start()
    {
        Difficulty.SetActive(false);
        red.SetActive(false);
        blue.SetActive(false);
        current.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf == false)
            {
                // Toggle the active state of the target object
                pauseMenu.SetActive(true);
            }
            else if (pauseMenu.activeSelf == true)
            {
                // Toggle the active state of the target object
                pauseMenu.SetActive(false);
            }

        }
    }

    public void Play()
    {
        Difficulty.SetActive(false);
        MainMenu.SetActive(false);
        red.SetActive(true);
        blue.SetActive(true);
        current.SetActive(true);

    }

    public void Quit()
    {
        UnityEngine.Application.Quit();

    }

    public void TitleScreen()
    {
        MainMenu.SetActive(true);
    }
    public void ShowDifficulty()
    {
        Difficulty.SetActive(true);
    }
    public void ShowHowToPlay()
    {
        Difficulty.SetActive(true);

    }
    public void SetDifficultyEasy()
    {
        Difficulty.SetActive(false);
        MainMenu.SetActive(false);
        scenesManager.isSinglePlayer = true;
        red.SetActive(true);
        blue.SetActive(true);
        current.SetActive(true);
        difficultyLevel = "Easy";
    }

    public void SetDifficultyMedium()
    {
        Difficulty.SetActive(false);
        MainMenu.SetActive(false);
        red.SetActive(true);
        blue.SetActive(true);
        current.SetActive(true);
        scenesManager.isSinglePlayer = true;

        difficultyLevel = "Medium";
    }

    public void SetDifficultyHard()
    {
        Difficulty.SetActive(false);
        MainMenu.SetActive(false);
        red.SetActive(true);
        blue.SetActive(true);
        current.SetActive(true);
        scenesManager.isSinglePlayer = true;

        difficultyLevel = "Hard";
    }
}