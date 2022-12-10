using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GrassCount++;
        GameManager.Instance.TotalGrassCount++;
    }

    //private void OnDestroy()
    //{
    //    GameManager.Instance.AppleCount--;
    //}

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Apple Collision Enter");
        EventHub.Instance.GrassDrop();
        GameManager.Instance.GrassCount--;
        Destroy(gameObject);
    }
}
