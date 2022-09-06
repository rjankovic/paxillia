using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;

    [SerializeField]
    private GameObject player;
    

    private bool movingRight = false;

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
                new Message() { Text = "Here you are in your room, bouncing a ball off the wall." },
                new Message() { Text = "It's the best thing ever! You could do that all day. In fact, you do just that, most days."},
                new Message() { Text = "You play with your ball in your room, in the kitchen, in the garden, in the outhouse...everywhere."},
                new Message() { Text = "And why not? Those who are not playing with balls right now clearly don't know what a great time it is." },
                new Message() { Text = "And from a metaphysical perspective..." }
            }
        });

        EventHub.Instance.OnDialogClose += IntroDialog_End;
        
        //DialogManager.Instance.OnDialogEnd += IntroDialog_End;
        // Code to execute after the delay
    }

    private void IntroDialog_End(Dialog dialog)
    {
        EventHub.Instance.OnDialogClose -= IntroDialog_End;
        movingRight = true;
        EventHub.Instance.OnBallBumOffPlayer += BounceOffToWindow;
    }

    private void BounceOffToWindow()
    {
        Debug.Log("Bounce off to window");

        var rigidbody = ball.GetComponent<Rigidbody2D>();
        var velocity = rigidbody.velocity;

        rigidbody.velocity = MathUtils.RotateVector(velocity, 0.7f);
        
        EventHub.Instance.OnBallBumOffPlayer -= BounceOffToWindow;
    }

    private void FixedUpdate()
    {
        if (movingRight)
        {
            if (player.transform.position.x >= -0.2f)
            {
                movingRight = false;
            }
            else
            {
                player.transform.Translate(new Vector3(0.03f, 0, 0));
            }
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
