using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHub : MonoBehaviour
{

    public static EventHub Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public event Action OnBallBumpOffWall;

    public event Action OnBallBumOffPlayer;

    public event Action<Dialog> OnDialogOpen;

    public event Action<Dialog> OnDialogClose;

    public event Action<Dialog> OnIngameDialogOpen;

    public event Action<Dialog> OnIngameDialogClose;

    public event Action OnWindowBroken;

    public event Action<int> OnBallCountUpdate;

    public event Action<bool> OnInputEnabled;

    public event Action<GameObject> OnBallServed;

    public event Action<GameObject> OnBallLost;

    public event Action OnBallPassed;

    public event Action OnPaused;
    public event Action OnUnpaused;

    public event Action OnDialogPaused;
    public event Action OnDialogUnpaused;

    public event Action<GameObjectSaveState> OnWorldSaveStateUpdated;

    public event Action<Vector2> OnCameraFocus;

    //private bool _inputEnabled;

    public event Action OnBallServeRequest;

    //public event Action OnLevelLoaded;

    public void BallBumpOffWall()
    {
        if(OnBallBumpOffWall != null)
            OnBallBumpOffWall();
    }

    public void BallBumpOffPlayer()
    {
        if (OnBallBumOffPlayer != null)
            OnBallBumOffPlayer();
    }

    public void DialogOpen(Dialog dialog)
    {
        if (OnDialogOpen != null)
            OnDialogOpen(dialog);
    }

    public void DialogClose(Dialog dialog)
    {
        if (OnDialogClose != null)
            OnDialogClose(dialog);
    }

    public void IngameDialogOpen(Dialog dialog)
    {
        if (OnIngameDialogOpen != null)
            OnIngameDialogOpen(dialog);
    }

    public void IngameDialogClose(Dialog dialog)
    {
        if (OnIngameDialogClose != null)
            OnIngameDialogClose(dialog);
    }

    public void WindowBroken()
    {
        if (OnWindowBroken != null)
            OnWindowBroken();
    }

    public void BallCountUpdate(int ballCount)
    {
        if (OnBallCountUpdate != null)
            OnBallCountUpdate(ballCount);
    }

    public void InputEnabled(bool enabled = true)
    {
        if (OnInputEnabled != null)
            OnInputEnabled(enabled);
    }

    public void BallServed(GameObject ball)
    {
        if (OnBallServed != null)
            OnBallServed(ball);
    }

    public void BallLost(GameObject ball)
    {
        if (OnBallLost != null)
            OnBallLost(ball);
    }

    public void BallPassed()
    {
        if (OnBallPassed != null)
            OnBallPassed();
    }

    public void Pause()
    {
        if (OnPaused != null)
            OnPaused();
    }

    public void Unpause()
    {
        if (OnUnpaused != null)
            OnUnpaused();
    }

    public void DialogPause()
    {
        if (OnDialogPaused != null)
            OnDialogPaused();
    }

    public void DialogUnpause()
    {
        if (OnDialogUnpaused != null)
            OnDialogUnpaused();
    }


    public void WordlSaveStateUpdate(GameObjectSaveState saveState)
    {
        Debug.Log($"Save state of {saveState.ObjectName}");
        if (OnWorldSaveStateUpdated != null)
            OnWorldSaveStateUpdated(saveState);
    }

    public void BallServeRequest()
    {
        if (OnBallServeRequest != null)
            OnBallServeRequest();
    }

    public void CameraFocus(Vector2 position)
    {
        if (OnCameraFocus != null)
            CameraFocus(position);
    }


    //public void LevelLoaded()
    //{
    //    if (OnLevelLoaded != null)
    //        OnLevelLoaded();
    //}
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
