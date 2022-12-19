using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrandmaScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SaveGame();
        EventHub.Instance.DialogPause();
        Debug.Log("Level Grandma starting");

        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>()
        {
            new Message() { Character = Constants.CHAR_GRANDMA, Text = "Also, Paul, here komms the nutrition...!", Duration = 5 },
            new Message() { Character = Constants.CHAR_PAL, Text = "No, grandma, please have mercy...", Duration = 5 },
            new Message() { Character = Constants.CHAR_GRANDMA, Text = "Your nourishment is my honor!", Duration = 7 },
        }
        });

        EventHub.Instance.OnIngameDialogClose += OnIntroDialogPt1Close;
    }

    private void OnIntroDialogPt1Close(Dialog obj)
    {
        Debug.Log("Intro dialog PT 1 over");
        EventHub.Instance.OnIngameDialogClose -= OnIntroDialogPt1Close;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
        {
            new Message() { Text = "Avoid being overfed by grandma. Gently discourage her by throwing balls at her." }
        }
        });

        EventHub.Instance.OnDialogClose += OnIntroDialogClose;
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
