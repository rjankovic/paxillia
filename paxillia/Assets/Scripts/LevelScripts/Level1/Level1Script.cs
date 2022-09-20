using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(ParentalDialog());
    }

    private IEnumerator ParentalDialog()
    {
        yield return new WaitForSeconds(0f);

        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Character = Constants.CHAR_DAD, Text = "Pal? You look weird...and why are you carrying those balls? What's going on?", Duration = 7 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Nothing dad, I was just playing with my ball... And it went out through the window, I'll get it.", Duration = 5 },
                new Message() { Character = Constants.CHAR_DAD, Text = "Oh, you should keep that window closed, I told you.", Duration = 5 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Yes, yes, it was closed.", Duration = 2 },
                new Message() { Character = Constants.CHAR_DAD, Text = "But then...?", Duration = 2 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Well, let's say ... now ... the window is ... very much open...", Duration = 5 },
                new Message() { Character = Constants.CHAR_DAD, Text = "What the... you little...!", Duration = 2 }
            }
        });

        EventHub.Instance.OnIngameDialogClose += IntroDialogClosed;
    }

    private void IntroDialogClosed(Dialog obj)
    {
        EventHub.Instance.OnIngameDialogClose -= IntroDialogClosed;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>
            { 
                new Message() { Text = "You need to escepe with at least two balls. Left click to serve a ball, mouse to move left / right. Get at least 2 of 3 balls out through the upper door of the hallway." }
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
