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
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Starting dialog");
        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Text = "You are Pal, a paddle." },
                new Message() { Text = "Here you are in your room, bouncing a ball off the wall." }
            }
        });
        // Code to execute after the delay
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
