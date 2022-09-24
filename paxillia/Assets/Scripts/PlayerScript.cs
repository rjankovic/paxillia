using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private bool _inputEnabled = false;

    [SerializeField]
    private GameObject _ballPrefab = null;

    [SerializeField]
    private bool _verticalMovementEnabled = false;

    private bool _movingUp = false;
    private bool _movingDown = false;

    //[SerializeField]
    private float movementSpeed = 10f; //0.2f;

    private Vector3 _targetWorldPosition = new Vector3(-1,-1,-1);
    private Vector3 _initMouseWorldPosition;

    private Rigidbody2D rigidBody;
    //private bool _inWall = false;

    private Vector2 _previousDeltaMovement = Vector2.zero;
    private Vector2 _normalizedCollision = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        EventHub.Instance.OnInputEnabled += OnInputEnabled;
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
        if (!_inputEnabled)
        {
            return;
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
    }

    public void OnFire(InputValue input)
    {
        if (!_inputEnabled)
        {
            return;
        }

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

        //Debug.Log("Fire pressed");
    }

    public void OnMove(AxisEventData eventData)
    {
        Debug.Log($"Move {eventData.moveDir} [{eventData.moveVector}]");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Colliding {collision.collider.tag}");
        if (collision.collider.tag == "Wall")
        {
            //_inWall = true;
            rigidBody.velocity = Vector2.zero;
            _normalizedCollision = _previousDeltaMovement.normalized;
            //Debug.Log("NC " + _normalizedCollision.ToString());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log($"Exit colliding {collision.collider.tag}");
        
        //if (collision.collider.tag == "Wall")
        //{
        //    _inWall = false;
        //}
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
        if (_targetWorldPosition.x != -1 /*&& !_inWall*/)
        {
            var delta = (Vector2)_targetWorldPosition - rigidBody.position;
            if (!_verticalMovementEnabled)
            {
                //Debug.Log("NV");
                delta.y = 0;
            }

            if (delta.magnitude > 1)
            {
                delta = delta.normalized;
            }

            //var restrictedDelta = MathUtils.RestrictToMovementSpeed(delta, movementSpeed);
            
            if (_normalizedCollision == delta.normalized)
            {
                return;
            }
            else
            {
                _normalizedCollision = Vector2.zero;
            }
            
            if (delta == _previousDeltaMovement)
            {
                return;
            }

            //var moveToPosition = restrictedDelta + rigidBody.position;

            //Debug.Log("Movement changed to " + restrictedDelta.ToString());
            _previousDeltaMovement = delta;

            //Debug.Log("MV " + restrictedDelta.ToString());
            rigidBody.velocity = delta * movementSpeed;
            //rigidBody.MovePosition(restrictedDelta + rigidBody.position);
            //Debug.Log($"RB move [{restrictedDelta}]");
            
            //var deltaPosition = MathUtils.TryMove(transform.position, _targetWorldPosition, transform.localScale, movementSpeed);
            //transform.Translate(deltaPosition);
        }


        //if (_targetWorldPosition.x != -1)
        //{
        //    var deltaPosition = MathUtils.TryMove(transform.position, _targetWorldPosition, transform.localScale, movementSpeed);
        //    transform.Translate(deltaPosition);
        //}


        //if (_movingUp)
        //{
        //    var deltaPosition = new Vector2(0, movementSpeed); //MathUtils.TryMoveHorizontally(myPosition, targetPosition, transform.localScale);
        //    transform.Translate(deltaPosition);
        //}
        //else if (_movingDown)
        //{
        //    var deltaPosition = new Vector2(0, -1 * movementSpeed); //MathUtils.TryMoveHorizontally(myPosition, targetPosition, transform.localScale);
        //    transform.Translate(deltaPosition);
        //}
    }
}
