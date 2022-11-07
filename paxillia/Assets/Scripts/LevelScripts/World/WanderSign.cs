using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSign : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool _active = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_active)
        {
            return;
        }

        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }

        EventHub.Instance.DialogPause();

        EventHub.Instance.OnDialogClose += Unpause;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = $"Are you wondering what treasures await on this path?\nKeep looking ;)" }
            }
        });

        StartCoroutine(ActivateDelay());
    }

    void Unpause(Dialog d)
    {
        EventHub.Instance.DialogUnpause();
        EventHub.Instance.OnDialogClose -= Unpause;
    }


    private IEnumerator ActivateDelay()
    {
        _active = false;
        yield return new WaitForSeconds(6f);
        _active = true;
    }

}
