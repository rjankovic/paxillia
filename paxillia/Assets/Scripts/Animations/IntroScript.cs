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

    [SerializeField]
    private GameObject door;

    [SerializeField]
    private Sprite openDoorSprite;


    private bool movingRight = false;
    private bool movingRightToLeave = false;
    private bool movingToBall = false;
    private int windowsToBreak = 1;
    private float exitSpeed = 0.05f;
    private bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.BallCount = 0;
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

        rigidbody.velocity = MathUtils.RotateVector(velocity, 2f);

        EventHub.Instance.OnBallBumOffPlayer -= BounceOffToWindow;

        EventHub.Instance.OnWindowBroken += WindowBroken;
    }

    private void WindowBroken()
    {
        windowsToBreak--;

        if (windowsToBreak > 0)
        {
            return;
        }
        EventHub.Instance.OnWindowBroken -= WindowBroken;

        DialogManager.Instance.StartDialog(new Dialog()
        {
            Messages = new List<Message>
            {
                new Message() { Text = "Whoopsie, too much thinking distracted you and that window paid the price..." },
                new Message() { Text = "The window be damned, but that was Rolly, your favorite ball! So round and bally...you have to get it back!" },
                new Message() { Text = "But first, let's get a few balls, just in case." }
            }
        });

        EventHub.Instance.OnDialogClose += LostBallDialogClosed;
    }

    private void LostBallDialogClosed(Dialog obj)
    {
        EventHub.Instance.OnDialogClose -= LostBallDialogClosed;

        StartCoroutine(TakeBalls());
    }


    IEnumerator TakeBalls()
    {
        yield return new WaitForSeconds(1.0f);
        movingToBall = true;
    }

    IEnumerator TakeBalls2()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.BallCount++;
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.BallCount++;
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.BallCount++;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitForSeconds(.5f);
        movingRightToLeave = true;
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.GotoLevel(GameManager.LevelEnum.Home);
    }

    private void FixedUpdate()
    {
        if (movingRight)
        {
            if (player.transform.position.x >= -0.1f)
            {
                movingRight = false;
            }
            else
            {
                player.transform.Translate(new Vector3(0.03f, 0, 0));
            }
        }

        if (movingToBall)
        {
            player.transform.Translate(new Vector3(-0.1f, 0, 0));
            if (player.transform.position.x <= -6f)
            {
                movingToBall = false;
                StartCoroutine(TakeBalls2());
            }
        }

        if (movingRightToLeave)
        {
            if (doorOpen == false && player.transform.position.x >= 1)
            {
                var doorRenderer = door.GetComponent<SpriteRenderer>();
                doorRenderer.sprite = openDoorSprite;
                door.transform.Translate(.45f, 0f, 0f);
                doorOpen = true;
            }

            player.transform.Translate(new Vector3(exitSpeed, 0, 0));
            exitSpeed += 0.01f;
            if (player.transform.position.x >= 20)
            {
                movingRightToLeave = false;
            }
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
