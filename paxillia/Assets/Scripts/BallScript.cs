using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    // Start is called before the first frame update
    new Rigidbody2D rigidbody = null;
    new SpriteRenderer renderer = null;
    //private Vector2 _velocity;

    //[SerializeField] private float _ballSpeed = 10;
    private int _ballLostDelay = 0; //5;

    private Vector2 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.velocity = new Vector2(0, 1 * GameManager.Instance.BallSpeed);
        }
        renderer = GetComponent<SpriteRenderer>();

        if (GameManager.Instance.CurrentLevel == GameManager.LevelEnum.World)
        {
            //Debug.Log("Ball in world - 5s lost delay");
            _ballLostDelay = 5;
        }
        //var sceneName = SceneManager.GetActiveScene().name;
        //Debug.Log($"scene {sceneName}");

        EventHub.Instance.OnDialogPaused += OnDialogPaused;
        EventHub.Instance.OnDialogUnpaused += OnDialogUnpaused;
    }

    
    private void OnDialogPaused()
    {
        if (gameObject == null)
        {
            return;
        }

        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }
        if (rigidbody == null)
        {
            return;
        }
        EventHub.Instance.PausedBallVelocity(rigidbody.velocity);
        rigidbody.velocity = Vector2.zero;
    }

    private void OnDialogUnpaused()
    {
        if (rigidbody != null)
        {
            rigidbody.velocity = _velocity;
        }
    }


    void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(WaitBallLost());
        }
    }

    private void OnBecameVisible()
    {
        
    }

    IEnumerator WaitBallLost()
    {
        yield return new WaitForSeconds(_ballLostDelay);
        if (!renderer.isVisible)
        {
            Debug.Log("Ball lost");
            GameManager.Instance.BallLost(gameObject);

            EventHub.Instance.OnDialogPaused -= OnDialogPaused;
            EventHub.Instance.OnDialogUnpaused -= OnDialogUnpaused;

            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude > 0)
        {
            _velocity = rigidbody.velocity;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Ball collision enter " + collision.collider.name);
        //if (collision.gameObject.tag != Constants.TAG_DOG)
        //{
        //    return;
        //}

        //if (collision.collider.name == "Player")
        if (collision.gameObject.tag == Constants.TAG_PLAYER)
        {
            
            BumpOffPlayer(collision);
            EventHub.Instance.BallBumpOffPlayer();
        }
    }

    private void BumpOffPlayer(Collision2D collision)
    {
        //Debug.Log("Calc bump");
        var collider = collision.contacts[0].collider;
        var collisionObject = collider.gameObject;
        //collisionObject.transform.rotation.

        //Debug.Log("VP " + _velocity);
        bool horizontalPaddle = collisionObject.transform.rotation.eulerAngles.z < 45;

        //Debug.Log($"Euler {collisionObject.transform.rotation.eulerAngles}");

        //if (!horizontalPaddle)
        //{
        //    Debug.Log("Vertical paddle");
        //}

        float relativePosition = 0f;
        float width = 0f;
        if (horizontalPaddle)
        {
            relativePosition = rigidbody.position.x - collisionObject.transform.position.x;
            width = collider.bounds.size.x; //collisionObject.  //collisionObject.transform.localScale.x;
        }
        else 
        {
            relativePosition = rigidbody.position.y - collisionObject.transform.position.y;
            width = collider.bounds.size.y; //collisionObject.  //collisionObject.transform.localScale.x;
        }
        //var collidingBody = collisionObject.GetComponent<BoxCollider2D>();

        //width = collider.bounds.size.x; //collisionObject.  //collisionObject.transform.localScale.x;
        //Debug.Log($"Bump width: {width}");
        var relativePoint = relativePosition / width;

        //Debug.Log($"RP {relativePoint}");

        //Debug.Log($"normal: {collision.contacts[0].normal}");
        var upDown = Mathf.Sign(collision.contacts[0].normal.y);
        var leftRight = Mathf.Sign(collision.contacts[0].normal.x);


        if (horizontalPaddle)
        {
            var normal = new Vector2(0, upDown); // collision.contacts[0].normal;

            var normalAdjusted = MathUtils.RotateVector(normal, -1 * relativePoint * upDown);

            var defaultVelocity = new Vector2(0, -1 * GameManager.Instance.BallSpeed * upDown);

            var reflection = Vector2.Reflect(defaultVelocity, normalAdjusted);

            rigidbody.velocity = reflection;
        }
        else
        {
            //Debug.Log("LR: " + leftRight + $" | RP: {relativePoint}");
            var normal = new Vector2(leftRight, 0); // collision.contacts[0].normal;

            var normalAdjusted = MathUtils.RotateVector(normal, relativePoint * leftRight);

            var defaultVelocity = new Vector2(-1 * GameManager.Instance.BallSpeed * leftRight, 0);

            var reflection = Vector2.Reflect(defaultVelocity, normalAdjusted);

            rigidbody.velocity = reflection;
        }

    }
}
