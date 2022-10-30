using Assets.Scripts.Managers;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    private bool _inputEnabled;
    private string _savePath;

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
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (EventHub.Instance != null)
        {
            EventHub.Instance.OnInputEnabled += EventHub_OnInputEnabled;
        }
    }

    private void EventHub_OnInputEnabled(bool val)
    {
        _inputEnabled = val;
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
                case "Anim_Intro":
                    return GameStateEnum.Animation;
                default:
                    return GameStateEnum.Level;
            }
        }
    }

    public bool InputEnabled { get => _inputEnabled; }

    //public void GoToLevel(LevelEnum level)
    //{ 

    //}

    //public void GoToAnimation(AnimationEnum animation)
    //{ 

    //}

    public void GoToMainMenu()
    {
        GotoLevel(LevelEnum.MainMenu);
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

    public bool LoadGame()
    {
        if (!File.Exists(_savePath))
        {
            return false;
        }

        Debug.Log($"Loading from {_savePath}");
        var saveData = File.ReadAllText(_savePath, Encoding.UTF8);
        var saveState = JsonConvert.DeserializeObject<SaveState>(saveData);
        ApplySaveState(saveState);

        return true;
    }

    public bool SaveGame()
    {
        if (GameState != GameStateEnum.Level)
        {
            return false;
        }

        Debug.Log($"Saving to {_savePath}");
        var saveState = GetSaveState();
        var saveData = JsonConvert.SerializeObject(saveState);
        File.WriteAllText(_savePath, saveData, Encoding.UTF8);

        return true;
    }

    private SaveState GetSaveState()
    {
        return null;
    }

    private void ApplySaveState(SaveState saveState)
    { 
    
    }
}
