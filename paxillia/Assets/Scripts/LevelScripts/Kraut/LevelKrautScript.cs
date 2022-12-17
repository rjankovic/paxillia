using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelKrautScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SaveGame();
        EventHub.Instance.DialogPause();
        Debug.Log("Level kraut starting");

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
        {
            new Message() { Text = "Well, here we are, grandma's kabbages. Let's try not to manage them too much while we deal with the grass weed." },
            new Message() { Text = "Hey, that's Rolly hiding in the grass! Oh boy, you sure rolled way too far this time! I'll get you back, don't worry little one." },
        }
        });

        EventHub.Instance.OnDialogClose += OnIntroDialogClose;
        EventHub.Instance.OnKrautDestroyed += OnKrautDestroyedWait;
        EventHub.Instance.OnBallLost += OnBallLost;
        EventHub.Instance.OnRollyCollected += OnRollyCollected;
        EventHub.Instance.OnEnoughGrassDropped += OnEnoughGrassDropped;
    }

    private void OnEnoughGrassDropped()
    {
        if (GameManager.Instance.RollyCollected)
        {
            LevelWin();
            return;
        }

        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Character = Constants.CHAR_PAL, Text = "The grass is cut, but Rolly is still out in the field. I'm not leaving without him.", Duration = 7 }
            }
        });
    }

    private void OnRollyCollected()
    {
        if (GameManager.Instance.GrassWiped)
        {
            LevelWin();
            return;
        }
    }

    private void LevelWin()
    {
        EventHub.Instance.DialogPause();
        EventHub.Instance.LevelWon();

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = "Awesome! The weed is mostly gone, the cabbages are looking more-less fine and on top of that, you've got Rolly, the reason why you ended up here in the first place!" },
                new Message() { Text = "Let's go say goodbye to grandma and get back home." },
            }
        });

        GameManager.Instance.SaveOnLevelStart = true;

        EventHub.Instance.OnDialogClose += (x) =>
        {
            EventHub.Instance.DialogUnpause();
            EventHub.Instance.InputEnabled(true);
            GameManager.Instance.GotoLevel(GameManager.LevelEnum.Anim_Grandma2);
        };
    }

    private void OnKrautDestroyedWait()
    {
        StartCoroutine(OnKrautDestroyed());
    }

    private IEnumerator OnKrautDestroyed()
    {
        yield return new WaitForSeconds(0.5f);

        EventHub.Instance.InputEnabled(false);
        EventHub.Instance.DialogPause();

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
                    {
                        new Message() { Text = $"Oooh...a completely wasted cabbage. Grandma sure won't like the sight of that. What do I do now? Hmm..."},
                    }
        });

        EventHub.Instance.OnDialogClose += (x) =>
        {
            EventHub.Instance.InputEnabled(true);
            GameManager.Instance.LoadGame();
        };
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
                        new Message() { Text = $"Look at you - you went here to finally get Rolly and instead you just lost all your balls. No good. Have another go at it, pal."},
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
