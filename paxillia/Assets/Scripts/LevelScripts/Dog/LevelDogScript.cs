using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelDogScript : MonoBehaviour
{
    public SpriteShapeController crackableWallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.BallSpeed = 5;

        EventHub.Instance.DialogPause();
        EventHub.Instance.OnDialogClose += OnIntroDialogClose;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
        {
            new Message() { Text = "Oh look, that little puppy apparently made one of his worse decisions when it jumped into that pit. Was probably running after one of those juicy bones on the sides of the pit." },
            new Message() { Text = "But how to help him? Let's think about it... wait, you're a paddle, you don't have a head, hm." },
            new Message() { Text = "But you've got balls! See the cracks in the bricks around the pit? Maybe you could grind them down and help the puppy make his way out somehow..." },
        }
        });

    }

    private void OnIntroDialogClose(Dialog obj)
    {
        EventHub.Instance.DialogUnpause();
        EventHub.Instance.OnDialogClose -= OnIntroDialogClose;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
