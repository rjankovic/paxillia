using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.SaveOnLevelStart)
        {
            GameManager.Instance.SaveGame();
            GameManager.Instance.SaveOnLevelStart = false;
        }


        StartCoroutine(WorldIntroDialog());

        EventHub.Instance.OnBallLost += OnBallLost;
    }

    

    private IEnumerator WorldIntroDialog()
    {
        yield return new WaitForSeconds(1f);

        EventHub.Instance.OnDialogClose += IntroDialogClosed;

        DialogManager.Instance.StartDialog(new Dialog()
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

    private void IntroDialogClosed(Dialog obj)
    {
        EventHub.Instance.OnDialogClose -= IntroDialogClosed;
        Debug.Log("Intro dialog closed");
        StartCoroutine(ServeNextBall(1));
    }

    private void OnBallLost(GameObject obj)
    {
        if (GameManager.Instance.BallCount > 0)
        {
            StartCoroutine(ServeNextBall());
            return;
        }
    }

    private IEnumerator ServeNextBall(float delay = 3)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Requesting a ball");
        EventHub.Instance.BallServeRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
