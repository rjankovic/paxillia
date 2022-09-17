using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    // Start is called before the first frame update
    new Rigidbody2D rigidbody = null;
    //private Vector2 _velocity;

    [SerializeField] private float _ballSpeed = 5;

    private Vector2 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, 1 * _ballSpeed);
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
            EventHub.Instance.BallBumpOffPlayer();
            BumpOffPlayer(collision);
        }
    }

    private void BumpOffPlayer(Collision2D collision)
    {
        var collisionObject = collision.contacts[0].collider.gameObject;

        //Debug.Log("VP " + _velocity);
        var relativePosition = rigidbody.position.x - collisionObject.transform.position.x;
        var width = collisionObject.transform.localScale.x;
        var relativePoint = relativePosition / width;


        //Debug.Log($"normal: {collision.contacts[0].normal}");
        var normal = new Vector2(0, 1); // collision.contacts[0].normal;

        
        var normalAdjusted = MathUtils.RotateVector(normal, -1 * relativePoint);

        var defaultVelocity = new Vector2(0, -1 * _ballSpeed);
        
        var reflection = Vector2.Reflect(defaultVelocity, normalAdjusted);

        //var reflection = Vector2.Reflect(_velocity, normalAdjusted);

        //Debug.Log($"Relative point: {relativePoint}, normal {normal}, normalA {normalAdjusted} origVelocity: {_velocity} reflection: {reflection}");

        var perpendicular = new Vector2(0, _ballSpeed);
        //var angle = Vector2.SignedAngle(perpendicular, reflection);
        
        /*
        var maxAngle = 75;

        if (angle > maxAngle)
        {
            reflection = MathUtils.RotateVector(reflection, Mathf.Deg2Rad * (maxAngle - angle)).normalized * _ballSpeed;
        }
        if (angle < -1 * maxAngle)
        {
            reflection = MathUtils.RotateVector(reflection, Mathf.Deg2Rad * (-1 * maxAngle - angle)).normalized * _ballSpeed;
        }
        */
        rigidbody.velocity = reflection;

    }
}
