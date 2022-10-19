using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static GameManager;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private bool _inputEnabled = false;

    [SerializeField]
    private GameObject _ballPrefab = null;

    [SerializeField]
    private bool _verticalMovementEnabled = false;

    //private bool _movingUp = false;
    //private bool _movingDown = false;

    //[SerializeField]
    private float movementSpeed = 10f; //0.2f;

    private Vector3 _targetWorldPosition = new Vector3(-1,-1,-1);
    private Vector3 _initMouseWorldPosition;

    //private Vector2 _previousDelta = new Vector2();

    private Rigidbody2D rigidBody;
    //private bool _inWall = false;

    private Vector2 _previousDeltaMovement = Vector2.zero;
    //private Vector2 _normalizedCollision = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        EventHub.Instance.OnInputEnabled += OnInputEnabled;


        //rigidBody.velocity = new Vector2(10f, 0f);
    }

    private void OnInputEnabled(bool enabled)
    {
        _inputEnabled = enabled;
        SetInitMouseWorldPosition();
    }

    private void SetInitMouseWorldPosition()
    {
        var absoluteVector = Mouse.current.position.ReadValue();
        var screenV3 = new Vector3(absoluteVector.x, absoluteVector.y, 0);
        _initMouseWorldPosition = Camera.main.ScreenToWorldPoint(screenV3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLook(InputValue inputValue)
    {
        //if (!_inputEnabled)
        //{
        //    return;
        //}

        ////Debug.Log("Look");

        ////var deltaVector = inputValue.Get<Vector2>();
        //var absoluteVector = Mouse.current.position.ReadValue();
        //var screenV3 = new Vector3(absoluteVector.x, absoluteVector.y, 0);
        //_targetWorldPosition = Camera.main.ScreenToWorldPoint(screenV3) - _initMouseWorldPosition;

        //if (!_verticalMovementEnabled)
        //{
        //    _targetWorldPosition.y = transform.position.y;
        //}
    }

    public void OnFire(InputValue input)
    {
        if (!_inputEnabled)
        {
            return;
        }

        TrySereBall();

        //Debug.Log("Fire pressed");
    }

    private void TrySereBall()
    {
        if (GameManager.Instance.BallCount < 1)
        {
            return;
        }

        if (GameManager.Instance.Ball != null)
        {
            return;
        }

        Debug.Log("Serving ball");

        var ballPosition = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2 + _ballPrefab.transform.localScale.y / 2 + 0.1f, 0);
        var ballObject = Instantiate(_ballPrefab, ballPosition, Quaternion.identity);
        GameManager.Instance.BallServed(ballObject);

        GameManager.Instance.BallCount--;
    }


    public void OnMove(InputValue inputValue)
    {
        ////Debug.Log($"MOVE");
        //var val = inputValue.Get();
        
        //// released
        //if (val == null)
        //{
        //    _movingUp = false;
        //    _movingDown = false;
        //    //Debug.Log("NULL GET");
        //}
        //// pressed
        //else
        //{
        //    var vector = (Vector2)val;
        //    if (vector.y > 0)
        //    {
        //        _movingUp = true;
        //        //Debug.Log("Moving up");
        //    }
        //    else if (vector.y < 0)
        //    {
        //        _movingDown = true;
        //        //Debug.Log("Moving down");
        //    }
        //    //Debug.Log($"Move IV {val.GetType()} [{val.ToString()}]");
        //}
    }

    private void FixedUpdate()
    {
        if (!_inputEnabled)
        {
            return;
        }


        if (GameManager.Instance.Ball == null && GameManager.Instance.GameState == GameStateEnum.World)
        {
            TrySereBall();
        }

        //Debug.Log("Look");

        //var deltaVector = inputValue.Get<Vector2>();
        var absoluteVector = Mouse.current.position.ReadValue();
        var screenV3 = new Vector3(absoluteVector.x, absoluteVector.y, 0);
        _targetWorldPosition = Camera.main.ScreenToWorldPoint(screenV3) - _initMouseWorldPosition;

        if (!_verticalMovementEnabled)
        {
            _targetWorldPosition.y = transform.position.y;
        }

        if (_targetWorldPosition.x != -1 /*&& !_inWall*/)
        {
            var delta = (Vector2)_targetWorldPosition - rigidBody.position;
            

            if (!_verticalMovementEnabled)
            {
                //Debug.Log("NV");
                delta.y = 0;
            }

            //Debug.Log($"Delta {delta}");

            if (delta.magnitude > 1)
            {
                delta = delta.normalized;
            }

            //var normal = delta.normalized;
            //if ((_previousDelta - normal).magnitude < 0.1f)
            //{
            //    return;
            //}
            //_previousDelta = normal;


            //rigidBody.MovePosition(rigidBody.position + delta * movementSpeed * Time.fixedDeltaTime);
            rigidBody.velocity = delta * movementSpeed;
        }
    }
}
