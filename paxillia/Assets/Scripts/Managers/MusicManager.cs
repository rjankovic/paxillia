using Assets.Scripts.Managers;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    bool _startDone = false;

    [SerializeField] AudioSource _introMusic;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Object.Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _instance = this;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;


        /*
        Debug.Log(GameManager.Instance.CurrentLevel.ToString());
        if (GameManager.Instance.CurrentLevel == GameManager.LevelEnum.MainMenu && !_startDone)
        {
            Debug.Log("Music start");
        }
        _startDone = true;
         */


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

        
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        Debug.Log("Music manager: scene loaded");

        if (scene.name == "MainMenu" && !_startDone)
        {
            Debug.Log("Music manager: game started");
            _introMusic.Play();
        }

        _startDone = true;
    }
}
