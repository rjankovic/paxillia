using Assets.Scripts.Managers;
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
    bool _playingStarted = false;

    [SerializeField] AudioSource _introMusic;
    [SerializeField] AudioSource[] _music;
    [SerializeField] AudioSource[] _gerMusic;

    private enum MusicTheme { Neutral, Prussian };
    private MusicTheme musicTheme = MusicTheme.Neutral;

    private AudioSource currentMusic = null;

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

    private bool IsGerScene(string sceneName)
    {
        return sceneName.Contains("Grandma") || sceneName.Contains("Kraut");
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

        var newTheme = (IsGerScene(scene.name) ? MusicTheme.Prussian : MusicTheme.Neutral);

        if (newTheme != musicTheme)
        {
            Debug.Log($"Switching to {newTheme.ToString()} music");
            if (currentMusic != null)
            {
                if (currentMusic.isPlaying)
                {
                    currentMusic.Stop();
                }
            }
            _playingStarted = false;
            musicTheme = newTheme;   
        }

        if (scene.name != "MainMenu" && !_playingStarted)
        {
            Debug.Log("Music manager: BGM start");
            if (_introMusic.isPlaying)
            {
                IEnumerator fadeSound1 = AudioFadeOut.FadeOut(_introMusic, 2f);
                StartCoroutine(fadeSound1);
                StartCoroutine(PlayMusic(2f));
                //StopCoroutine(fadeSound1);
            }
            else
            {
                StartCoroutine(PlayMusic());
            }

            _playingStarted = true;
        }
    }

    private IEnumerator PlayMusic(float delay = 0)
    {
        var loopTheme = musicTheme;
        yield return new WaitForSeconds(delay);

        var count = _music.Length;
        List<int> l = new List<int>();
        for (int i = 0; i < count; i++)
        {
            l.Add(i);
        }

        var r = new System.Random();
        l.Shuffle(r);



        var count_g = _gerMusic.Length;
        List<int> l_g = new List<int>();
        for (int i = 0; i < count_g; i++)
        {
            l_g.Add(i);
        }

        var r2 = new System.Random();
        l_g.Shuffle(r2);


        int playIndex = 0;
        Debug.Log($"Music: {count} tracks, playing {l[playIndex]}");

        while (true && musicTheme == loopTheme)
        {
            var music = _music[l[playIndex]];
            if (loopTheme == MusicTheme.Prussian)
            {
                music = _gerMusic[l_g[playIndex]];
            }
            music.volume = 0.5f;
            currentMusic = music;
            music.Play();
            yield return new WaitForSeconds(music.clip.length + 5f);
            playIndex++;
            playIndex = playIndex % (loopTheme == MusicTheme.Neutral ? count : count_g);
        }
    }

    public static class AudioFadeOut
    {

        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list, System.Random random)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            int indexToSwap = random.Next(i + 1);
            (list[indexToSwap], list[i]) = (list[i], list[indexToSwap]);
        }
    }
}
