using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDogScript : MonoBehaviour
{

    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        lastX = rigidBody.position.x;
        
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigidBody.AddForce(new Vector2(0f, -5f), ForceMode2D.Force);

    }

    private int movingDirection = -1;
    private float lastX = 0;

    private IEnumerator Jump()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            if (Math.Sign(rigidBody.position.x - lastX) != movingDirection)
            {
                movingDirection *= -1;
            }

            //Debug.Log("jump");
            lastX = rigidBody.position.x;
            rigidBody.AddForce(new Vector2(0, 220f));
            yield return new WaitForSeconds(0.3f);
            rigidBody.AddForce(new Vector2(200f * movingDirection, 0));
        }
    }
}
