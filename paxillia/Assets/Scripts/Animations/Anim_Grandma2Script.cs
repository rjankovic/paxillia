using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Grandma2Script : MonoBehaviour
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
                new Message() { Character = Constants.CHAR_PAL, Text = "All done, grandma! The weed is gone and I found Rolly!", Duration = 5 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Wunderbar! Solch a gut boy, I'm proud. You deserve a treat for that.", Duration = 7 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "And - I prepared a little something for you here - sit down and eat, junge!", Duration = 7 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Ehm, grandma...I'm not hungry.", Duration = 5 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Not hungry? What's that supposed to mean?", Duration = 5 },
                new Message() { Character = Constants.CHAR_PAL, Text = "You see, I had an apple on the way here. I'm fine.", Duration = 7 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Apf...apfle? Like a Hase? Unsinn! You need energy to grow - look how thin you are!", Duration = 10 },
                new Message() { Character = Constants.CHAR_PAL, Text = "How about this, grandma? I'll go play with Rolly for a bit, and then when I'm hungry, I'll come back.", Duration = 10 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Oh ja, I know zis - last time you weren't hungry for 2 weeks straight, richtig?", Duration = 7 },
                new Message() { Character = Constants.CHAR_PAL, Text = "No, grandma, that's not how...well...don't be angry, remember your blood pressure.", Duration = 7 },
                new Message() { Character = Constants.CHAR_PAL, Text = "Anyways, see you later, grandma.", Duration = 5 },
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Oh, just you wait...!", Duration = 10 },
            }
        });

        EventHub.Instance.OnIngameDialogClose += IntroDialog_End;

        //DialogManager.Instance.OnDialogEnd += IntroDialog_End;
        // Code to execute after the delay
    }

    private void IntroDialog_End(Dialog dialog)
    {
        GameManager.Instance.SaveOnLevelStart = true;
        GameManager.Instance.GotoLevel(GameManager.LevelEnum.EscapeFromGrandma);
    }

}
