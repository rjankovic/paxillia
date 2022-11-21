using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelDogScript : MonoBehaviour
{
    public SpriteShapeController crackableWallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SaveGame();
        //EventHub.Instance.DialogPause();
        //EventHub.Instance.DialogUnpause();
        //EventHub.Instance.InputEnabled(true);

        Debug.Log("Level dog starting");

        GameManager.Instance.BallSpeed = 5;

        
        Debug.Log("Starting level dog dialog");
        Debug.Log(DialogManager.Instance);


        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
        {
            new Message() { Text = "Oh look, that little puppy apparently made one of his worse decisions when he jumped into that pit. Was probably running after one of those juicy bones on the sides of the pit." },
            new Message() { Text = "But how to help him? Let's think about it... wait, you're a paddle, you don't have a head, hm." },
            new Message() { Text = "But you've got balls! See the cracks in the bricks around the pit? Maybe you could grind them down and help the puppy make his way out somehow..." },
        }
        });

        //EventHub.Instance.DialogPause();

        EventHub.Instance.OnDialogClose += OnIntroDialogClose;

        Debug.Log("Started level dog dialog");

        EventHub.Instance.OnBallLost += OnBallLost;
    }

    private void OnBallLost(GameObject obj)
    {
        if (GameManager.Instance.BallCount > 0)
        {
            return;
        }

        EventHub.Instance.InputEnabled(false);

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
                    {
                        new Message() { Text = $"Hmm, the dog's still too deep in the pit and you are all out of balls.\nHow did that happen, buddy?\nLet's pretend it didn't :)"}
                    }
        });

        EventHub.Instance.OnDialogClose += (x) =>
        {
            //Time.timeScale = 1;
            EventHub.Instance.InputEnabled(true);
            GameManager.Instance.LoadGame();
        };
    }

    private void OnIntroDialogClose(Dialog obj)
    {
        Debug.Log("Intro dialog over");
        EventHub.Instance.DialogUnpause();
        EventHub.Instance.InputEnabled();
        EventHub.Instance.OnDialogClose -= OnIntroDialogClose;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
