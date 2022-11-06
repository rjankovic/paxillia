using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSign : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool _saveActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_saveActive)
        {
            return;
        }

        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }

        EventHub.Instance.DialogPause();

        DialogManager.Instance.StartYesNoDialog(new Dialog()
        {
            Messages = new List<Message>()
            {
                new Message() { Text = "Howdy! Welcome to the paddle vacation resort. Do you want to save your progress here and take a break?" }
            }
        },
        yesAction: () =>
        {
            GameManager.Instance.SaveGame();
            DialogManager.Instance.StartIngameDialog(new Dialog()
            {
                Messages = new List<Message>()
                {
                    new Message() { Text = "Game saved." }
                }
            });
            ActivateDelay();
            EventHub.Instance.DialogUnpause();
        },
        noAction: () =>
        {
            ActivateDelay();
            EventHub.Instance.DialogUnpause();
        });


    }

    private IEnumerator ActivateDelay()
    {
        _saveActive = false;
        yield return new WaitForSeconds(6f);
        _saveActive = true;
    }

    //private void OnSaveDialogClose(Dialog obj)
    //{
    //    Time.timeScale = 1;
    //    EventHub.Instance.OnDialogClose -= OnSaveDialogClose;
    //}
}
