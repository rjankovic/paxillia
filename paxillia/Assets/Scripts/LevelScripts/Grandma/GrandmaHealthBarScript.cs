using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrandmaHealthBarScript : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GrandmaHealth = 1000;
        }

        EventHub.Instance.OnGrandmaHealthUpdated += OnGrandmaHealthUpdated;
    }

    private void OnGrandmaHealthUpdated()
    {
        SetValue(GameManager.Instance.GrandmaHealth);

        if (slider.value <= 0)
        {
            EventHub.Instance.DialogPause();
            EventHub.Instance.LevelWon();

            DialogManager.Instance.StartIngameDialog(new Dialog()
            {
                Messages = new List<Message>()
            {
                new Message() { Character = Constants.CHAR_GRANDMA, Text = "Himmelhergott, verdammte Anorexiebande... Tja.", Duration = 7 }
            }
            });

            EventHub.Instance.OnIngameDialogClose += OnIngameDialogClose;

            
        }
    }

    private void OnIngameDialogClose(Dialog obj)
    {
        EventHub.Instance.OnIngameDialogClose -= OnIngameDialogClose;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = "Grandma has finally given up on her feeding quest. Here's your chance! You dash to the front door, ping and bump your way through the garden." },
                new Message() { Text = "You shout out your gooood-bye, seee youuu later to grandma from a safe distance. You have escaped!" },
                new Message() { Text = "A few moments later, you stand in front of the window you broke earlier today. It will come in handy now. You and Rolly slip in effortlessly and coninue to play, where you left off." },
            }
        });

        EventHub.Instance.OnDialogClose += OnDialogClose;
    }

    private void OnDialogClose(Dialog obj)
    {
        EventHub.Instance.OnDialogClose -= OnDialogClose;

        EventHub.Instance.DialogUnpause();
        EventHub.Instance.InputEnabled(true);
        GameManager.Instance.GotoLevel(GameManager.LevelEnum.End);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int value)
    { 
        slider.value = value;
    }
}
