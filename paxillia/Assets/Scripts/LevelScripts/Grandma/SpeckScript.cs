using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeckScript : MonoBehaviour
{
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("TGT " + GameManager.Instance.PlayerRigidBody.position.ToString());
        direction = Vector3.MoveTowards(transform.position, GameManager.Instance.PlayerRigidBody.position, 0.3f) - transform.position;
        //Debug.Log("SRC " + transform.position.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != Constants.TAG_PLAYER)
        {
            return;
        }

        GameManager.Instance.CaloriesCounter += 50;
        Destroy(gameObject);
    }
}
