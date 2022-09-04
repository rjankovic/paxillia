using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OpenIntroDialog());
    }

    IEnumerator OpenIntroDialog()
    {
        yield return new WaitForSeconds(3);

        Debug.Log("Starting dialog");
        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Text = "You are Pad, a paddle." }
            }
        });
        // Code to execute after the delay
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
