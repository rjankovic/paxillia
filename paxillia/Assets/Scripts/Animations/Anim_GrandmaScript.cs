using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_GrandmaScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OpenIntroDialog());
    }

    IEnumerator OpenIntroDialog()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Starting dialog");
        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Paul!", Duration = 3 },
                new Message() { Character = Constants.CHAR_PAL, Text = "It's Pal, grandma, I've told you...", Duration = 7 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Na gut, Paul, doesn't messer...you kame to visit your old grossma? Gut boy!", Duration = 10 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Yes, well... you see, grandma, I was playing with my balls today and one of them went out the window...", Duration = 10 },
                new Message() { Character = Constants.CHAR_PAL, Text = "I've looked everywhere except here. Did you see Rolly by any chance? You know, that very ball-like ball.", Duration = 10 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Hmm, ja...I think somezing landed in mein Krautgarten, maybe it's your ball.", Duration = 10 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Oh great! Can I go have a look?", Duration = 5 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Sure, and weil you're there, could you also helf me a bit with the garten?", Duration = 10 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "You zee, mein Krautgarten is full of Unkraut!", Duration = 7 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Unkraut?", Duration = 3 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Weed, grass, you know...ze stuff you don't want in your Garten. You want to helf your grossma, don't you?", Duration = 10 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Sure thing grandma, I'll get right to it!", Duration = 5 },
            }
        });

        EventHub.Instance.OnIngameDialogClose += IntroDialog_End;

        //DialogManager.Instance.OnDialogEnd += IntroDialog_End;
        // Code to execute after the delay
    }

    private void IntroDialog_End(Dialog dialog)
    {
        GameManager.Instance.SaveOnLevelStart = true;
        GameManager.Instance.GotoLevel(GameManager.LevelEnum.GrandmasKraut);
    }

}
