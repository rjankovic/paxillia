using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GertrudaScript : MonoBehaviour
{
    private const int MIN_POS = -14;
    private const int MAX_POS = 14;
    private const float MOVEMENT_SPEED = 0.01f;

    private int movementDirection = 0;
    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMovement()
    {
        movementDirection = Random.value >= 0.5 ? 1 : -1;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (movementDirection == 0)
        {
            return;
        }

        var targetPosition = transform.position.x + movementDirection * MOVEMENT_SPEED;

        if (targetPosition >= MAX_POS)
        {
            targetPosition = MAX_POS;
            movementDirection = -1;
        }
        if (targetPosition <= MIN_POS)
        {
            targetPosition = MIN_POS;
            movementDirection = 1;
        }

        transform.Translate(new Vector3(targetPosition - transform.position.x, 0, 0));

        var r = Random.value;
        var changeDirection = r < Time.fixedDeltaTime / 4f;

        if (changeDirection)
        {
            movementDirection *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }

        GameManager.Instance.GrandmaHealth -= 50;
    }
}
