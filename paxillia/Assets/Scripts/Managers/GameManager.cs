using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get => _instance; }
    //private GameStateEnum _gameState = GameStateEnum.MainMenu;
    //public GameStateEnum GameState { get => _gameState; }

    private int ballCount = 3;

    public GameObject Ball { get; private set; }

    public int BallEscapeCount { get; set; }
    public int BallEscapeTarget { get; set; }

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
        EscapeFromGrandma,
        NA
    }

    public enum GameStateEnum
    { 
        MainMenu,
        PauseMenu,
        Animation,
        Level,
        World,
        NA
    }

    public LevelEnum CurrentLevel
    {
        get {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level1_Home": 
                    return LevelEnum.Home;
                case "Level_Dog":
                    return LevelEnum.Dog;
                case "Level_AppleTree":
                    return LevelEnum.AppleTree;
                default:
                    return LevelEnum.NA;
            }
        }
    }

    public GameStateEnum GameState
    {
        get
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "MainMenu":
                    return GameStateEnum.MainMenu;
                case "WorldMap":
                    return GameStateEnum.World;
                default:
                    return GameStateEnum.NA;
            }
        }
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

    public void BallServed(GameObject ballObject)
    {
        Ball = ballObject;
        EventHub.Instance.BallServed(ballObject);
    }

    public void BallLost(GameObject ballObject)
    {
        Ball = null;
        EventHub.Instance.BallLost(ballObject);
    }
}
