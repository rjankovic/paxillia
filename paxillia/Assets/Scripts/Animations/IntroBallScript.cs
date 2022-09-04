using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBallScript : MonoBehaviour
{
    // Start is called before the first frame update
    new Rigidbody2D rigidbody = null;
    //private Vector2 _velocity;

    [SerializeField] private float _ballSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, 1 * _ballSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
