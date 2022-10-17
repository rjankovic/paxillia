using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
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

        //EventHub.Instance.OnBallLost += Instance_OnBallLost;

        //EventHub.Instance.OnIngameDialogClose += IntroDialogClosed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
