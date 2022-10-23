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

    [SerializeField]
    private Animator crossFader;

    private int ballCount = 3;
    private int switch_on;

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
        IntroAnimation,
        MainMenu,
        World,
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
                case "MainMenu":
                    return LevelEnum.MainMenu;
                case "Anim_Intro":
                    return LevelEnum.IntroAnimation;
                case "World":
                    return LevelEnum.World;
                default:
                    return LevelEnum.NA;
            }
        }
    }

    private string LevelEnumToScene(LevelEnum level)
    {
        switch (level)
        {
            case LevelEnum.Home:
                return "Level1_Home";
            case LevelEnum.Dog:
                return "Level_Dog";
            case LevelEnum.AppleTree:
                return "Level_AppleTree";
            //case LevelEnum.GrandmasBalls:
            //    break;
            //case LevelEnum.GrandmasKraut:
            //    break;
            //case LevelEnum.EscapeFromGrandma:
            //    break;
            case LevelEnum.IntroAnimation:
                return "Anim_Intro";
            case LevelEnum.MainMenu:
                return "MainMenu";
            case LevelEnum.World:
                return "World";
            case LevelEnum.NA:
                throw new KeyNotFoundException();
                //break;
        }
        throw new KeyNotFoundException();
    }

    public void GotoLevel(LevelEnum level)
    {
        var levelName = LevelEnumToScene(level);
        StartCoroutine(LoadLevel(levelName));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        crossFader.SetTrigger("Fadeout");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(levelName);
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

    //public void GoToLevel(LevelEnum level)
    //{ 
    
    //}

    //public void GoToAnimation(AnimationEnum animation)
    //{ 
    
    //}

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
