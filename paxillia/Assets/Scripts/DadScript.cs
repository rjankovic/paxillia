using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadScript : MonoBehaviour
{

    //private GameObject ballObject = null;
    [SerializeField]
    private float movementSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //EventHub.Instance.OnBallServed += OnBallServed;
        //EventHub.Instance.OnBallLost += OnBallLost;
        
    }

    //private void OnBallServed(GameObject obj)
    //{
    //    ballObject = obj;
    //}

    //private void OnBallLost(GameObject obj)
    //{
    //    ballObject = null;
    //}


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 ballPosition;
        if (GameManager.Instance.Ball == null)
        {
            ballPosition = new Vector3(0,0,0);
        }
        else
        {
            //Debug.Log("moving to ball");
            ballPosition = GameManager.Instance.Ball.transform.position;
        }
        var myPosition = transform.position;
        if (ballPosition.x == myPosition.x)
        {
            return;
        }

        var targetPositionX = ballPosition.x;
        if (targetPositionX - myPosition.x < movementSpeed * -1)
        {
            targetPositionX = myPosition.x - movementSpeed;
        }
        else if (targetPositionX - myPosition.x > movementSpeed)
        {
            targetPositionX = myPosition.x + movementSpeed;
        }
        //Debug.Log($"Delta {targetPositionX - myPosition.x}");

        var targetPosition = new Vector3(targetPositionX, myPosition.y, myPosition.z);

        var deltaPosition = MathUtils.TryMoveHorizontally(myPosition, targetPosition, transform.localScale);
        transform.Translate(deltaPosition);
    }
}
