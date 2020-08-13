using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameOver;
    public GameObject levelSet;
    public GameObject GameModeOne;
    public GameObject GameModeTwo;
    public static bool IsGameMode = true;
    void Start()
    {
        if (GameController.IsGameJustOver)
        {
            menuPanel.SetActive(false);
            gameOver.SetActive(true);
        }
    }

    public void ButtonGameSettings()
    {
        levelSet.SetActive(true);
        menuPanel.SetActive(false);
        if (IsGameMode == true)
        {
            GameModeOne.GetComponent<Toggle>().isOn = true;
        }
        else if (IsGameMode == false)
        {
            GameModeTwo.GetComponent<Toggle>().isOn = true;
        }
    }


    public void BackToMenu()
    {
        GameModeIs();
        levelSet.SetActive(false);
        menuPanel.SetActive(true);
        gameOver.SetActive(false);
    }


    public void OnStartButtonClick()
    {
        if (IsGameMode == true)
        {
            SceneManager.LoadScene("last");
        }
        else if(IsGameMode == false)
        {
            SceneManager.LoadScene("ModeTwo");
        }
        ScoreController.lastScore = 0;
    }


    public void GameModeIs()
    {
        if (GameModeOne.GetComponent<Toggle>().isOn == true)
        {
            IsGameMode = true;
        } else { 
            IsGameMode = false; 
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
