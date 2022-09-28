using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float _movementSpeed = 1f;

    [SerializeField]
    private Transform _targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = new Vector3(_targetPosition.position.x, _targetPosition.position.y + 1f, -20f);
        transform.position = Vector3.Slerp(transform.position, newPos, _movementSpeed * Time.deltaTime);
    }
}
