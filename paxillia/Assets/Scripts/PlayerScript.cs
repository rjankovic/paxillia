using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private bool _inputEnabled = false;

    [SerializeField]
    private GameObject _ballPrefab = null;

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
}
