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
        yield return new WaitForSeconds(1f);

        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Text = "You made it outside, well done!", Duration = 3 },
                new Message() { Text = "Make sure you keep you ball in sight! You could lose it if it bounces too far from you.", Duration = 7 },
                new Message() { Text = "Since you have no hands, you can only use your balls to interact with the world.", Duration = 5 },
                //new Message() { Character = Constants.CHAR_PAL, Text = "Yes, yes, it was closed.", Duration = 2 },
                //new Message() { Character = Constants.CHAR_DAD, Text = "But then...?", Duration = 2 },
                //new Message() { Character = Constants.CHAR_PAL, Text = "Well, let's say ... now ... the window is ... very much open...", Duration = 5 },
                //new Message() { Character = Constants.CHAR_DAD, Text = "What the... you little...!", Duration = 2 }
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
