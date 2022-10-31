using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("New game pressed");
        //SceneManager.LoadScene(1);
        GameManager.Instance.GotoLevel(GameManager.LevelEnum.IntroAnimation);
    }

    public void Continue()
    {
        Debug.Log("Continue pressed");
        GameManager.Instance.LoadGame();
    }

    public void Exit()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }
}
