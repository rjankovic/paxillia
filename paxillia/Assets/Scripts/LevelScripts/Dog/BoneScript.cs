using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScript : MonoBehaviour
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
        if (collision.gameObject.tag != Constants.TAG_DOG)
        {
            return;
        }

        //Debug.Log("Ball collision");

        EventHub.Instance.DialogPause();

        EventHub.Instance.OnDialogClose += OnDialogClose;
        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = "Oh, that bone must be so delicious - the dog is absolutely besides himself!" },
                new Message() { Text = "Makes you think of Rolly. The dog has got his bone, but where is your ball? Maybe the doggy could sniff it out. You have the napkin with which you last cleaned Rolly, after all." },
                new Message() { Text = "The dog sniffs the napkin and starts to look around. Suddenly he starts running up north. You follow him hastily." },
                new Message() { Text = "You run up the corridor and eventually catch a glimpse of granny Gertruda's mansion. The dog comes to an abrupt halt, bares his teeth and barks once. Then he thinks better of it and runs back." },
                new Message() { Text = "Ah Rolly, you sure went far this time... Guess we'll have to pay grandma a visit...oh boy." },
            }
        });
    }

    private void OnDialogClose(Dialog obj)
    {
        EventHub.Instance.OnDialogClose -= OnDialogClose;
        EventHub.Instance.DialogUnpause();
    }
}
