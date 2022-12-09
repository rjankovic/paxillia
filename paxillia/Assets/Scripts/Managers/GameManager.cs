using Assets.Scripts.Managers;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    [SerializeField]
    private GameObject _ballPrefab = null;

    private int ballCount = 3;
    //private int switch_on;
    private bool _inputEnabled;
    private string _savePath;

    private GameObject _player;
    private Rigidbody2D _playerRigidBody;
    private Vector2 _pausedBallVelocity = Vector2.zero;

    private Dialog _scheduledIngameDialog = null;

    public Vector2 WorldReturnPosition = Vector2.zero;

    public bool DialogPaused { get; private set; } = false;

    private Dictionary<string, GameObjectSaveState> _worldSaveStates = new Dictionary<string, GameObjectSaveState>();

    public GameObject Ball { get; private set; }

    public int BallEscapeCount { get; set; }
    public int BallEscapeTarget { get; set; }

    public int BallSpeed { get; set; } = 10; //3; // 10;

    private bool _dogLevelCompleted = false;
    private bool _treeLevelCompleted = false;

    public bool DogLevelCompleted { get => _dogLevelCompleted; set { _dogLevelCompleted = value; CheckRoadblock(); } }

    public bool TreeLevelCompleted { get => _treeLevelCompleted; set { _treeLevelCompleted = value; CheckRoadblock(); } }

    public bool RoadblockRemoved { get; set; }

    [SerializeField]
    private AudioSource _lostSound;

    [SerializeField]
    private AudioSource _winSound;

    [SerializeField]
    private AudioSource _appleSound;

    [SerializeField]
    private AudioSource _ballsCollectedSound;

    private void CheckRoadblock()
    {
        if (!RoadblockRemoved && (_dogLevelCompleted && _treeLevelCompleted))
        {
            RoadblockRemoved = true;
            EventHub.Instance.RoadblockRemoved();
            Debug.Log("Scheduling Gertruda reminder");
            _scheduledIngameDialog = new Dialog()
            {
                Messages = new List<Message>()
                {
                    new Message() { Character = Constants.CHAR_PAL, Text = "I should check up on grandma up north. Maybe she is back home already.", Duration = 10 }
                }
            };
        }
    }

    public bool LoadedLevelStart { get; private set; }

    public bool FirstTimeOutside { get; set; } = false;

    private int _appleCount = 0;
    public int AppleCount
    {
        get => _appleCount;
        set
        {
            bool drop = _appleCount > 0 && value == 0;
            _appleCount = value;
            if (drop)
            {
                Debug.Log("OUT OF APPLES");
                EventHub.Instance.AppleCountDownToZero();
            }
            Debug.Log("A " + AppleCount);
        }
    }

    public int BallCount
    {
        get => ballCount;
        set { ballCount = value; if (EventHub.Instance != null) EventHub.Instance.BallCountUpdate(value); }
    }

    private SaveState _saveStateAfterLoad;
    private bool _saveOnLevelStart = false;
    public bool SaveOnLevelStart { get => _saveOnLevelStart; set => _saveOnLevelStart = value; }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Object.Destroy(gameObject);
            return;
        }


        _instance = this;
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log("Save path: " + _savePath);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (_instance != null && _instance != this)
        {
            Object.Destroy(gameObject);
            return;
        }

        if (EventHub.Instance != null)
        {
            ReviveEventHub();
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }

    public void ReviveEventHub()
    {

        EventHub.Instance.OnInputEnabled -= EventHub_OnInputEnabled;
        EventHub.Instance.OnWorldSaveStateUpdated -= OnWorldSaveStateUpdated;


        EventHub.Instance.OnInputEnabled += EventHub_OnInputEnabled;
        EventHub.Instance.OnWorldSaveStateUpdated += OnWorldSaveStateUpdated;
        EventHub.Instance.OnPausedBallVelocity += (v) => _pausedBallVelocity = v;

        EventHub.Instance.OnDialogPaused += () => { Debug.Log("Dialog paused in GM"); DialogPaused = true; };
        EventHub.Instance.OnDialogUnpaused += () => DialogPaused = false;

        if (DogLevelCompleted)
        {
            Debug.Log("Dog level completed to EH");
            EventHub.Instance.DogLevelCompleted();
        }
        if (TreeLevelCompleted)
        {
            EventHub.Instance.TreeLevelCompleted();
        }
        if (RoadblockRemoved)
        {
            EventHub.Instance.RoadblockRemoved();
        }

        EventHub.Instance.OnLevelWon += LevelWon;
        EventHub.Instance.OnAppleDrop += OnAppleDrop;
    }

    private void OnAppleDrop()
    {
        _appleSound.Play();
    }

    private void OnWorldSaveStateUpdated(GameObjectSaveState obj)
    {
        _worldSaveStates[obj.ObjectName] = obj;
    }

    public void SetPlayer(GameObject gameObject)
    {
        //Debug.Log("Set player in GM");
        _player = gameObject;
        _playerRigidBody = _player.GetComponent<Rigidbody2D>();

        if (_saveStateAfterLoad != null && CurrentLevel == LevelEnum.World)
        {
            //Debug.Log("save state after load");
            //Debug.Log($"{_playerRigidBody}");

            if (_playerRigidBody != null && _saveStateAfterLoad.PositionX != 0f)
            {
                //Debug.Log($"setting position {_saveStateAfterLoad.PositionX} {_saveStateAfterLoad.PositionY}");
                _playerRigidBody.position = new Vector2(_saveStateAfterLoad.PositionX, _saveStateAfterLoad.PositionY);

                _saveStateAfterLoad.PositionX = 0f;
            }
            if (Ball == null && _saveStateAfterLoad.BallInGame)
            {
                var ballPosition = new Vector3(_saveStateAfterLoad.BallPositionX, _saveStateAfterLoad.BallPositionY, 0);
                var ballObject = Instantiate(_ballPrefab, ballPosition, Quaternion.identity);
                Ball = ballObject;
                var ballRigidbody = ballObject.GetComponent<Rigidbody2D>();
                ballRigidbody.velocity = new Vector2(_saveStateAfterLoad.BallVelocityX, _saveStateAfterLoad.BallVelocityY);

                EventHub.Instance.CameraFocus(new Vector2(_saveStateAfterLoad.BallPositionX, _saveStateAfterLoad.BallPositionY));
                Debug.Log($"Loaded ball to position\t {ballRigidbody.position}");
            }
            //_saveStateAfterLoad = null;
        }

        if (WorldReturnPosition != Vector2.zero && CurrentLevel == LevelEnum.World)
        {
            _playerRigidBody.position = WorldReturnPosition;
            WorldReturnPosition = Vector2.zero;
        }

        //Debug.Log("Getting player script");
        //var playerScript = gameObject.GetComponent<PlayerScript>();
        //Debug.Log(playerScript.GetType().FullName);
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
        get
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level1_Home":
                    return LevelEnum.Home;
                case "Level_Dog":
                    return LevelEnum.Dog;
                case "Level_AppleTree":
                    return LevelEnum.AppleTree;
                case "Level_Kraut":
                    return LevelEnum.GrandmasKraut;
                case "Level_Grandma":
                    return LevelEnum.EscapeFromGrandma;
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
            case LevelEnum.GrandmasKraut:
                return "Level_Kraut";
            case LevelEnum.EscapeFromGrandma:
                return "Level_Grandma";
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
        LoadedLevelStart = false;
        StartCoroutine(LoadLevel(levelName));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        crossFader.SetTrigger("Fadeout");

        yield return new WaitForSeconds(0.5f);

        _appleCount = 0;
        BallSpeed = 10;
        //DialogPaused = false;


        Debug.Log("Loading scene " + levelName);
        SceneManager.LoadScene(levelName);


        //crossFader.SetTrigger("Fadein");

        //if (SaveOnLevelStart)
        //{
        //    SaveGame();
        //    SaveOnLevelStart = false;
        //}

    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

        if (CurrentLevel == LevelEnum.World)
        {
            LoadWorldState();
        }

        crossFader.SetTrigger("Fadein");

        if (EventHub.Instance != null)
        {
            EventHub.Instance.ResetLevelAfterLoad();
        }

        //_gamePaused = false;
        //_pausePanel.SetActive(false);
        //if (_inputWasEnabled)
        //{
        //    EventHub.Instance.InputEnabled(true);
        //}
        Debug.Log("Unpause");
        EventHub.Instance.InputEnabled(true);
        Time.timeScale = 1;

        if(_scheduledIngameDialog != null)
        {
            Debug.Log("Starting scheduled dialog");
            DialogManager.Instance.StartIngameDialog(_scheduledIngameDialog);
            _scheduledIngameDialog = null;
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

        if (CurrentLevel != GameManager.LevelEnum.Home && CurrentLevel != GameManager.LevelEnum.IntroAnimation)
        {
            Debug.Log("Playing lost sound");
            _lostSound.Play();
        }
    }

    public bool LoadGame()
    {
        if (!File.Exists(_savePath))
        {
            return false;
        }

        Debug.Log($"Loading from {_savePath}");
        var saveData = File.ReadAllText(_savePath, Encoding.UTF8);
        var saveState = JsonConvert.DeserializeObject<SaveState>(saveData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
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
        var saveData = JsonConvert.SerializeObject(saveState, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented });
        File.WriteAllText(_savePath, saveData, Encoding.UTF8);

        return true;
    }

    private SaveState GetSaveState()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        SaveState saveState = new SaveState()
        {
            SavedWorldItems = _worldSaveStates.Select(x => x.Value).ToList(),
            Level = sceneName,
            BallCount = BallCount,
            WorldReturnPositionX = WorldReturnPosition.x,
            WorldReturnPositionY = WorldReturnPosition.y,
            DogLevelCompleted = DogLevelCompleted,
            TreeLevelCompleted = TreeLevelCompleted,
            RoadblockRemoved = RoadblockRemoved
        };

        if (_playerRigidBody != null)
        {
            saveState.PositionX = _playerRigidBody.position.x;
            saveState.PositionY = _playerRigidBody.position.y;
        }

        if (Ball != null)
        {
            var ballRigidBody = Ball.GetComponent<Rigidbody2D>();

            if (ballRigidBody != null)
            {
                saveState.BallInGame = true;
                saveState.BallPositionX = ballRigidBody.position.x;
                saveState.BallPositionY = ballRigidBody.position.y;

                if (ballRigidBody.velocity.magnitude > 0)
                {
                    saveState.BallVelocityX = ballRigidBody.velocity.x;
                    saveState.BallVelocityY = ballRigidBody.velocity.y;
                }
                else
                {
                    saveState.BallVelocityX = _pausedBallVelocity.x;
                    saveState.BallVelocityY = _pausedBallVelocity.y;
                }
            }
        }

        return saveState;
    }

    private void ApplySaveState(SaveState saveState)
    {
        _worldSaveStates = saveState.SavedWorldItems.ToDictionary(x => x.ObjectName, x => x);
        _saveStateAfterLoad = saveState;

        Debug.Log($"Loading level {saveState.Level}");

        DogLevelCompleted = saveState.DogLevelCompleted;
        TreeLevelCompleted = saveState.TreeLevelCompleted;
        RoadblockRemoved = saveState.RoadblockRemoved;

        LoadedLevelStart = true;
        StartCoroutine(LoadLevel(saveState.Level));
        BallCount = saveState.BallCount;
        WorldReturnPosition = new Vector2(saveState.WorldReturnPositionX, saveState.WorldReturnPositionY);

    }

    private void LoadWorldState()
    {
        var saveObjects = GameObject.FindGameObjectsWithTag("SaveState");

        foreach (GameObject saveObject in saveObjects)
        {
            SaveableWorldObject saveComponent = saveObject.GetComponent<SaveableWorldObject>();
            if (_worldSaveStates.ContainsKey(saveObject.name))
            {
                saveComponent.ApplySaveState(_worldSaveStates[saveObject.name]);
            }
        }
    }

    public void NewGameReset()
    {
        _worldSaveStates = new Dictionary<string, GameObjectSaveState>();
        DogLevelCompleted = false;
        TreeLevelCompleted = false;
        RoadblockRemoved = false;
    }

    private void LevelWon()
    {
        _winSound.Play();
    }

    public void BallsCollected()
    {
        _ballsCollectedSound.Play();
    }
}
