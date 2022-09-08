using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get => _instance; }
    private GameStateEnum _gameState = GameStateEnum.MainMenu;
    public GameStateEnum GameState { get => _gameState; }

    private int ballCount;

    public int BallCount
    {
        get => ballCount;
        set { ballCount = value; EventHub.Instance.BallCountUpdate(value); }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public enum AnimationEnum
    { 
        Intro
    }

    public enum LevelEnum
    {
        Home,
        Dog,
        AppleTree,
        GrandmasBalls,
        GrandmasKraut,
        EscapeFromGrandma
    }

    public enum GameStateEnum
    { 
        MainMenu,
        PauseMenu,
        Animation,
        Level,
        World
    }

    public void GoToLevel(LevelEnum level)
    { 
    
    }

    public void GoToAnimation(AnimationEnum animation)
    { 
    
    }

    public void GoToMainMenu()
    { 
    
    }

    public void GoToPauseMenu()
    { 
    
    }
}
