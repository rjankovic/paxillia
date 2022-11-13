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

    private IEnumerator Jump()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Debug.Log("jump");
            rigidBody.AddForce(new Vector2(0f, 200f));
        }
    }
}
