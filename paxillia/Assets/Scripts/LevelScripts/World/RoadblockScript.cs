using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadblockScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.RoadblockRemoved)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
