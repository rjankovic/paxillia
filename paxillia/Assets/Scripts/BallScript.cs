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

    [SerializeField] private float _ballSpeed = 10;
    private int _ballLostDelay = 0; //5;

    private Vector2 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, 1 * _ballSpeed);
        renderer = GetComponent<SpriteRenderer>();

        if (GameManager.Instance.GameState == GameManager.GameStateEnum.World)
        {
            Debug.Log("Ball in world - 5s lost delay");
            _ballLostDelay = 5;
        }
        //var sceneName = SceneManager.GetActiveScene().name;
        //Debug.Log($"scene {sceneName}");
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
        if (collision.collider.name == "Player")
        {
            
            BumpOffPlayer(collision);
            EventHub.Instance.BallBumpOffPlayer();
        }
    }

    private void BumpOffPlayer(Collision2D collision)
    {
        var collider = collision.contacts[0].collider;
        var collisionObject = collider.gameObject;

        //Debug.Log("VP " + _velocity);
        var relativePosition = rigidbody.position.x - collisionObject.transform.position.x;
        //var collidingBody = collisionObject.GetComponent<BoxCollider2D>();

        var width = collider.bounds.size.x; //collisionObject.  //collisionObject.transform.localScale.x;
        Debug.Log($"Bump width: {width}");
        var relativePoint = relativePosition / width;


        //Debug.Log($"normal: {collision.contacts[0].normal}");
        var upDown = Mathf.Sign(collision.contacts[0].normal.y);
        
        var normal = new Vector2(0, upDown); // collision.contacts[0].normal;

        
        var normalAdjusted = MathUtils.RotateVector(normal, -1 * relativePoint * upDown);

        var defaultVelocity = new Vector2(0, -1 * _ballSpeed * upDown);
        
        var reflection = Vector2.Reflect(defaultVelocity, normalAdjusted);


        rigidbody.velocity = reflection;

    }
}
