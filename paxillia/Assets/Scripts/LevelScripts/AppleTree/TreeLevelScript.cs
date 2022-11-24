using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SaveGame();
        EventHub.Instance.DialogPause();
        Debug.Log("Level tree starting");
        //EventHub.Instance.DialogUnpause();
        
        //Time.timeScale = 1;
        
        
        Debug.Log("Starting tree dialog");
        Debug.Log(GameManager.Instance.DialogPaused);
        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
        {
            new Message() { Text = "Approaching the tree, you see three of your friends - Billy, Jimmy and Willy - standing around it." },
            new Message() { Text = "These guys are not really the sharpest tools in the shed. In fact, said shed would probably burn down shortly after they entered." },
            new Message() { Text = "Snap, they noticed you. Too late to turn back now. Well, grin up; here we go..." },
            new Message() { Text = "Pal: \"Hey guys! What's up?\"" },
            new Message() { Text = "Billy: \"Paaal! Look lads, it's Pal and he's got balls!\"" },
            new Message() { Text = "Pal: \"Yeah, and I'd like to keep them, Bill!\"" },
            new Message() { Text = "Jimmy: \"See Pal - we're trying to...ehm...borrow some apples here, but we're too, well, paddly to climb the tree.\"" },
            new Message() { Text = "Jimmy: \"We really could use some balls to knock the apples down. Will you help?\"" },
            new Message() { Text = "Pal: \"Guys, this is the perfect plan... if the goal is to loose all your, pardon me, mine, balls! You will just throw a ball and the tree, you'll miss and the ball is gone!\"" },
            new Message() { Text = "Willy: \"Nah, we thought about it - we'll stand on each side of the tree, that way wherever the ball bounces off, we'll catch it!\"" },
            new Message() { Text = "Pal: \"Alright then, but you need to move exactly as I say, agreed?\"" },
            new Message() { Text = "Willy: \"Don't worry about it Pal, it will go swimmingly, as always!\"" },
            new Message() { Text = "Pal *quietly*: \"That's exactly what I worry about...\"" },
        }
        });

        
        EventHub.Instance.OnDialogClose += OnIntroDialogClose;

        EventHub.Instance.OnBallLost += OnBallLost;

        EventHub.Instance.OnAppleCountDownToZero += OnAppleCountDownToZero;
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
                        new Message() { Text = $"Jimmy: \"Snap! Pal's out of balls and the juiciest apples are still up there!\""},
                        new Message() { Text = $"Pal: \"See? This whole apple-borrowing enterprise was bollocks from the get-go!\""},
                        new Message() { Text = $"Billy: \"What do we do now?\""},
                        new Message() { Text = $"Jimmy: \"Willy, do you still have that thing of yours?\""},
                        new Message() { Text = $"Willy: \"My lucky poo on a stick?\""},
                        new Message() { Text = $"Jimmy: \"No, I mean that other thing, that pocket time travel thingy...\""},
                        new Message() { Text = $"Willy: \"Oh yes, I have the time machine here! Great idea, Jim - we can get back in time and get all the apples!\""},
                        new Message() { Text = $"Pal: \"W-w-whaat?? You built a time machine?\""},
                        new Message() { Text = $"Willy: \"Yup, well, it was an accident...\""},
                        new Message() { Text = $"Pal: \"William, you are the dumbest genius in the history of dumb geniuses, maybe ever!\""},
                        new Message() { Text = $"Willy: \"Don't get all methan-philological now, Pal! Get in the Clockcracker!\""},
                    }
        });

        EventHub.Instance.OnDialogClose += (x) =>
        {
            //Time.timeScale = 1;
            EventHub.Instance.InputEnabled(true);
            GameManager.Instance.LoadGame();
        };

        
    }

    private void OnAppleCountDownToZero()
    {
        EventHub.Instance.DialogPause();

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = "Jimmy: \"Awesome! All the apples are collected!\"" },
                new Message() { Text = "Willy: \"Thanks Pal! Come on, have an apple\"" },
                new Message() { Text = "Pal: \"Nom, nom... see you, guys!\"" },
            }
        });

        GameManager.Instance.SaveOnLevelStart = true;
        GameManager.Instance.TreeLevelCompleted = true;
        GameManager.Instance.BallCount++;

        EventHub.Instance.OnDialogClose += (x) =>
        {
            EventHub.Instance.DialogUnpause(); 
            EventHub.Instance.InputEnabled(true);
            GameManager.Instance.GotoLevel(GameManager.LevelEnum.World);
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
