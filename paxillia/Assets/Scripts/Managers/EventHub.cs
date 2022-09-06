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


    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
