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
                new Message() { Text = "Bone!" }
            }
        });
    }

    private void OnDialogClose(Dialog obj)
    {
        EventHub.Instance.OnDialogClose -= OnDialogClose;
        EventHub.Instance.DialogUnpause();
    }
}
