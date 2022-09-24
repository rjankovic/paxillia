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

    [SerializeField]
    private float movementSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        EventHub.Instance.OnInputEnabled += OnInputEnabled;
    }

    private void OnInputEnabled(bool enabled)
    {
        _inputEnabled = enabled;
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
        var worldPosition = Camera.main.ScreenToWorldPoint(screenV3);

        var deltaPosition = MathUtils.TryMoveHorizontally(transform.position, worldPosition, transform.localScale);
        transform.Translate(deltaPosition);
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

    public void OnMove(InputValue inputValue)
    {
        //Debug.Log($"MOVE");
        var val = inputValue.Get();
        
        // released
        if (val == null)
        {
            _movingUp = false;
            _movingDown = false;
            //Debug.Log("NULL GET");
        }
        // pressed
        else
        {
            var vector = (Vector2)val;
            if (vector.y > 0)
            {
                _movingUp = true;
                //Debug.Log("Moving up");
            }
            else if (vector.y < 0)
            {
                _movingDown = true;
                //Debug.Log("Moving down");
            }
            //Debug.Log($"Move IV {val.GetType()} [{val.ToString()}]");
        }
    }

    private void FixedUpdate()
    {
        if (_movingUp)
        {
            var deltaPosition = new Vector2(0, movementSpeed); //MathUtils.TryMoveHorizontally(myPosition, targetPosition, transform.localScale);
            transform.Translate(deltaPosition);
        }
        else if (_movingDown)
        {
            var deltaPosition = new Vector2(0, -1 * movementSpeed); //MathUtils.TryMoveHorizontally(myPosition, targetPosition, transform.localScale);
            transform.Translate(deltaPosition);
        }
    }
}
