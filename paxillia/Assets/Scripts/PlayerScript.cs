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


        //var deltaPosition = new Vector2(worldPosition.x - transform.position.x, 0);
        ////Debug.Log($"DX {deltaPosition.x}");

        //RaycastHit2D[] result = new RaycastHit2D[10];

        //// find any collisions that can occur when moving from transform.position to worldPosition on the X axis - check the rectangle between the original and new position (including)
        //var collisionRectCenter = new Vector2((worldPosition.x + transform.position.x) / 2, transform.position.y);
        //// the size is [deltaX + paddle width; paddle height]
        //var collisionRectangleSize = new Vector2(Mathf.Abs(worldPosition.x - transform.position.x) + transform.localScale.x, transform.localScale.y);

        //// filter the collisions to layer 6 - Walls
        //var cf = new ContactFilter2D();
        //cf.layerMask = new LayerMask();
        //cf.layerMask.value = 6;

        //// returns the number of boxcast hits
        //var cast = Physics2D.BoxCast(collisionRectCenter, collisionRectangleSize, 0, new Vector2(0,0), cf, result);
        //if (cast > 0)
        //{

        //    for (int i = 0; i < cast; i++)
        //    {
        //        //if (result[i].collider.gameObject.tag == "Player"
        //        //    || result[i].collider.gameObject.tag == "Ball")
        //        //{
        //        //    continue;
        //        //}

        //        if (result[i].collider.gameObject.tag != "Wall")
        //        {
        //            continue;
        //        }

        //        // skip the first physics collider
        //        //if (!result[i].collider.isTrigger)
        //        //{
        //        //    continue;
        //        //}

        //        // wall on the right and we're moving to the right
        //        if (result[i].point.x >= transform.position.x && deltaPosition.x > 0)
        //        {
        //            Debug.Log($"Block right - tX {worldPosition.x} dX {deltaPosition.x} CPX {result[i].point.x} COL {result[i].collider.gameObject.name}");
                    
        //            // cannot use this, because the collision point can be within the obstackle, not on the edge
        //            //deltaPosition.x = (result[i].point.x - transform.localScale.x / 2) - transform.position.x;

        //            // the target position is left from the obstackle by half of obstckle's width + half of paddle's width
        //            var targetPositionX = result[i].collider.gameObject.transform.position.x - result[i].collider.gameObject.transform.localScale.x / 2 - transform.localScale.x / 2;
        //            var candidatePosition = targetPositionX - transform.position.x;
        //            if (candidatePosition < deltaPosition.x)
        //            {
        //                deltaPosition.x = candidatePosition;
        //            }
        //        }
        //        // wall on the left and moving to the left
        //        else if (result[i].point.x <= transform.position.x && deltaPosition.x < 0)
        //        {
        //            Debug.Log($"Block left - tX {worldPosition.x} dX {deltaPosition.x} CPX {result[i].point.x} COL {result[i].collider.gameObject.name}");

        //            var targetPositionX = result[i].collider.gameObject.transform.position.x + result[i].collider.gameObject.transform.localScale.x / 2 + transform.localScale.x / 2;
        //            var candidatePosition = targetPositionX - transform.position.x;
        //            if (candidatePosition > deltaPosition.x)
        //            {
        //                deltaPosition.x = candidatePosition;
        //            }
        //        }
        //        //Debug.Log("Collision: " + result[i].collider.gameObject.name);
        //    }
        //}
        //transform.Translate(deltaPosition);
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
