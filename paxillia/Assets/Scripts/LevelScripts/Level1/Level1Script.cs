using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Script : MonoBehaviour
{

    private int _currentEscapeCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SaveGame();
        StartCoroutine(ParentalDialog());
    }

    private IEnumerator ParentalDialog()
    {
        yield return new WaitForSeconds(0f);

        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Character = Constants.CHAR_DAD, Text = "Pal? You look weird...and why are you carrying those balls? What's going on?", Duration = 8 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Nothing dad, I was just playing with my ball... And it went out through the window, I'll get it.", Duration = 7 },
                new Message() { Character = Constants.CHAR_DAD, Text = "Oh, you should keep that window closed, I told you.", Duration = 5 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Yes, yes, it was closed.", Duration = 4 },
                new Message() { Character = Constants.CHAR_DAD, Text = "But then...?", Duration = 4 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Well, let's say ... now ... the window is ... very much open...", Duration = 7 },
                new Message() { Character = Constants.CHAR_DAD, Text = "What the... you little...!", Duration = 4 }
            }
        });

        EventHub.Instance.OnBallLost += Instance_OnBallLost;

        EventHub.Instance.OnIngameDialogClose += IntroDialogClosed;
    }

    private void Instance_OnBallLost(GameObject obj)
    {
        if (GameManager.Instance.BallCount > 0)
        {
            if (GameManager.Instance.BallEscapeCount > _currentEscapeCount)
            {
                if (GameManager.Instance.BallEscapeCount == 1)
                {
                    DialogManager.Instance.StartIngameDialog(new Dialog()
                    {
                        Messages = new List<Message>() {
                            new Message() { Character = Constants.CHAR_DAD, Text = "You just threw one ball through the window and now you're throwing another one out?? What's that?", Duration = 7 }
                        }
                    });
                }
                else if (GameManager.Instance.BallEscapeCount == 2)
                {
                    DialogManager.Instance.StartIngameDialog(new Dialog()
                    {
                        Messages = new List<Message>() {
                            new Message() { Character = Constants.CHAR_DAD, Text = "You trying make this household ballless?", Duration = 7 }
                        }
                    });
                }
            }

            _currentEscapeCount = GameManager.Instance.BallEscapeCount;
        }
        else // (GameManager.Instance.BallCount == 0)
        {
            if (GameManager.Instance.BallEscapeTarget <= GameManager.Instance.BallEscapeCount)
            {
                //Time.timeScale = 0;
                EventHub.Instance.InputEnabled(false);

                DialogManager.Instance.StartDialog(new Dialog()
                {
                    Messages = new List<Message>()
                    {
                        new Message() { Text = $"Great! You managed to escape with {GameManager.Instance.BallEscapeCount} balls! Now let's get out and find Rolly."}
                    }
                });

                GameManager.Instance.BallCount = GameManager.Instance.BallEscapeCount;
                GameManager.Instance.SaveOnLevelStart = true;

                EventHub.Instance.OnDialogClose += (x) =>
                {
                    //Time.timeScale = 1;
                    EventHub.Instance.InputEnabled(true);
                    GameManager.Instance.GotoLevel(GameManager.LevelEnum.World);
                };

            }
            else
            {
                //Time.timeScale = 0;
                EventHub.Instance.InputEnabled(false);

                DialogManager.Instance.StartDialog(new Dialog()
                {
                    Messages = new List<Message>()
                    {
                        new Message() { Text = $"Welp.. You managed to escape with {GameManager.Instance.BallEscapeCount} balls. You'll need at least {GameManager.Instance.BallEscapeTarget}. Try again."}
                    }
                });

                EventHub.Instance.OnDialogClose += (x) =>
                {
                    //Time.timeScale = 1;
                    EventHub.Instance.InputEnabled(true);
                    GameManager.Instance.LoadGame();
                };
            }
        }
    }

    
    private void IntroDialogClosed(Dialog obj)
    {
        EventHub.Instance.OnIngameDialogClose -= IntroDialogClosed;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>
            { 
                new Message() { Text = "You need to escape with at least two balls. Left click to serve a ball, mouse to move left / right. Get at least 2 of 3 balls out through the upper door of the hallway." }
            }
        });

        EventHub.Instance.OnDialogClose += GoalMessageClosed;
    }

    private void GoalMessageClosed(Dialog obj)
    {
        EventHub.Instance.OnDialogClose -= GoalMessageClosed;

        Debug.Log("Enabling input");
        EventHub.Instance.InputEnabled(true);
    }


    // You need to escepe with at least two balls. Press SPACE to serve a ball, arrows to move LEFT/RIGHT, UP/DOWN. Get your balls out through the upper door or the hallway.


    // Update is called once per frame
    void Update()
    {
        
    }
}
