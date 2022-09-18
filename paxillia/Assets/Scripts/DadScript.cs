using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadScript : MonoBehaviour
{

    private GameObject ballObject = null;

    // Start is called before the first frame update
    void Start()
    {
        EventHub.Instance.OnBallServed += OnBallServed;
        EventHub.Instance.OnBallLost += OnBallLost;
        
    }

    private void OnBallServed(GameObject obj)
    {
        ballObject = obj;
    }

    private void OnBallLost(GameObject obj)
    {
        ballObject = null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
