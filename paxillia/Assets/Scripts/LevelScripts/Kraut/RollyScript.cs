using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }
            
        GameManager.Instance.RollyCollected = true;
        GameManager.Instance.BallsCollectedSound();
        DialogManager.Instance.StartIngameDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Character = Constants.CHAR_PAL, Text = "Rolly! You're finally out of there! I'll get you back home as soon as we're done here!", Duration = 7 }
            }
        });
        EventHub.Instance.RollyCollected();
        Destroy(gameObject);
    }
}
