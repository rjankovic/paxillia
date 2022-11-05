using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsCollectibleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Constants.TAG_BALL)
        {
            Destroy(gameObject);
            GameManager.Instance.BallCount += 3;
        }
    }
}
