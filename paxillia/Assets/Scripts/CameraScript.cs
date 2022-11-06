using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float _movementSpeed = 2f;

    [SerializeField]
    private Transform _targetPosition;
    [SerializeField]
    private bool _cameraMoves;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(EventHub.Instance);
        EventHub.Instance.OnCameraFocus += Instance_OnCameraFocus;
    }

    private void Instance_OnCameraFocus(Vector2 obj)
    {
        transform.position = new Vector3(obj.x, obj.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (_cameraMoves)
        //{
        //    var newPos = new Vector3(_targetPosition.position.x, _targetPosition.position.y + 1f, -20f);
        //    transform.position = Vector3.Slerp(transform.position, newPos, _movementSpeed * Time.deltaTime);
        //}
#if UNITY_EDITOR
        if (_camera)
            ScaleViewport();
#endif
    }

    private void FixedUpdate()
    {
        if (_cameraMoves)
        {
            var newPos = new Vector3(_targetPosition.position.x, _targetPosition.position.y + 1f, -20f);
            transform.position = Vector3.Slerp(transform.position, newPos, _movementSpeed * Time.fixedDeltaTime);
        }
    }

    private Camera _camera;

    //[Tooltip("Set the target aspect ratio.")]
    //[SerializeField] 
    
    private float _targetAspectRatio = 16f/9f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (Application.isPlaying)
            ScaleViewport();
    }


    private void ScaleViewport()
    {
        // determine the game window's current aspect ratio
        var windowaspect = Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        var scaleheight = windowaspect / _targetAspectRatio;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1)
        {
            var rect = _camera.rect;

            rect.width = 1;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1 - scaleheight) / 2;

            _camera.rect = rect;
        }
        else // add pillarbox
        {
            var scalewidth = 1 / scaleheight;

            var rect = _camera.rect;

            rect.width = scalewidth;
            rect.height = 1;
            rect.x = (1 - scalewidth) / 2;
            rect.y = 0;

            _camera.rect = rect;
        }
    }
}
